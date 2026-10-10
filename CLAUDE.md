# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Purpose

Aska.Core.EntityStorage is a set of NuGet packages that give .NET applications a ready-made data-access layer over EF Core. Apps get a command/query API with expression specifications and generic create, update and delete commands through Microsoft DI, without writing repositories. Entity types are discovered by reflection and routed to the right database context, so one app can use several databases at once. MariaDB, PostgreSQL and SQLite are supported.

## Repository and branches

- The repo is `github.com/felements/aska.core`. An older GitLab copy is archived and no longer used.
- `develop` is the main line. Branch from it and target it with pull requests.
- `master` is the GitHub default branch but is far behind `develop`.
- Commit messages use conventional commits with a scope, for example `feat(storage):`, `tests(storage):`, `ci:`, `chore:`.

## Build and test

The solution is `src/aska.core.sln`. Libraries target `netstandard2.1`. The demo app and test runners target `netcoreapp3.1` with EF Core 3.1 packages.

```sh
dotnet build src/aska.core.sln
```

A modern SDK builds the solution. Running `netcoreapp3.1` assemblies without the 3.1 runtime installed needs `DOTNET_ROLL_FORWARD=Major`.

All runnable tests are integration tests against real databases. Start MariaDB on port 3306 and PostgreSQL on port 5432 from the repo root. Database, user and password are all `askaone`.

```sh
docker-compose -f docker-compose.db.yml up -d
```

Apply the test project's migrations, then run its tests. `dotnet-ef` 3.1 is the version CI uses. Set `DB_HOST` to override `localhost`.

```sh
dotnet ef database update -p src/Aska.Core.EntityStorage.Ef.MariaDb.Tests
DOTNET_ROLL_FORWARD=Major dotnet test src/Aska.Core.EntityStorage.Ef.MariaDb.Tests
DOTNET_ROLL_FORWARD=Major dotnet test src/Aska.Core.EntityStorage.Ef.MariaDb.Tests --filter "FullyQualifiedName~Command_OnUpdateCommand_ShouldQueryChangedData"
```

The PostgreSQL project, `Aska.Core.EntityStorage.Ef.PostgreSql.Tests`, works the same way. SQLite has no tests.

## Architecture

```
Abstractions        IQuery, ICommand family, specifications, IEntityStorageReader/Writer, type discovery
  Ef                EntityStorageContext base, no-tracking queries, EF commands, AddEfEntityStorage()
    Ef.MariaDb      AutoDiscovery*Context, connection string builder, UseMariaDb<TContext>()
    Ef.PostgreSql   same shape, UsePostgresql<TContext>()
    Ef.Sqlite       same shape, UseSqlite<TContext>()
```

Every project is prefixed `Aska.Core.EntityStorage.`. The three provider projects are near copies of each other, so a change to one usually belongs in all three. The SQLite project's namespace is `Aska.Core.Storage.Ef.Sqlite`, which does not match its project name.

### Registration

`Aska.Core.EntityStorage.DemoApp/src/Program.cs` is the reference setup:

```csharp
services.AddEfEntityStorage(opt =>
{
    opt.UseMariaDb<DemoMariaDbContext>()
        .WithEntitiesAutoDiscovery<IMariaDbEntity>("Aska.Core.EntityStorage.Demo", forceLoadAssemblies: true)
        .WithConnectionString(MariaDbConnectionString.Create().WithServer("localhost")...);
    opt.UsePostgresql<DemoPsqlContext>()...;
});
```

- `AddEfEntityStorage` registers the query and command factories and the open generic `IQuery<>`, `IQuery<,>` and command implementations.
- Each `Use*<TContext>()` registers the context under all of its storage interfaces.
- `WithEntitiesAutoDiscovery<TBaseEntity>` finds every exported type that implements `TBaseEntity` in assemblies whose name starts with the prefix. It registers a per-entity `IEntityStorageReader<T>` and `IEntityStorageWriter<T>` proxy bound to that context. The context also uses the same discovery to build its EF model, mapping each type to a table named after the class.

### How data flows

- **Entity routing works by marker interface.** Each context gets its own base entity type, such as `IMariaDbEntity` or `IPsqlEntity`. Queries and commands resolve `IEntityStorageReader<T>` or `IEntityStorageWriter<T>`, which is what routes an entity to its database. An entity that matches two contexts' marker types gets two registrations, and the last one wins.
- **Discovery only sees loaded assemblies.** Without `forceLoadAssemblies: true`, entity assemblies that haven't been loaded yet are silently skipped.
- **Queries are read-only.** `EfDatabaseNoTrackingQuery` applies `AsNoTracking()` and accepts only expression specifications. Get queries from `IQueryFactory.GetQuery<TEntity>()`, or `GetQuery<TEntity, TSpec>()` for a custom specification class.
- **Commands save immediately.** Each create, update or delete command calls `SaveAsync` itself. Single-entity commands return `true` only when exactly one row was affected. Bulk commands return the affected row count. Updating or deleting a missing entity throws `DbUpdateConcurrencyException`.
- **Migrations are never applied automatically.** `IEntityStorageContextInitializer.InitializeAsync()` calls each context's `InitializeAsync`. Contexts override it to check the database, as `DemoMariaDbContext` does by throwing when migrations are pending. Migrations live in the consuming project, one folder per context, with an `IDesignTimeDbContextFactory` for `dotnet ef`.

### Tests

`Aska.Core.EntityStorage.Ef.Tests` is a `netstandard2.1` library of shared test bases, not a runnable test project. `DbContextCrudOperationsTest<TContext>` holds the test logic as virtual methods. Each provider test project subclasses it and overrides every method with `[Fact]`. To add a test, write the virtual method in the base class, then add the `[Fact]` override in both provider test projects. The base uses its own `TestCommandFactory` and `TestQueryFactory` instead of DI, and it can see internal types through `InternalsVisibleTo`.

## CI and packaging

CI is defined in `.gitlab-ci.yml` and ran on a self-hosted GitLab instance. GitHub has no workflows. Builds run in Docker through `src/build.dockerfile`, with one job per project.

On `master` and `develop`, the pipeline computes a version with GitVersion, writes it into every `.csproj` with `src/.ci/set-version.sh`, and pushes each library to nuget.org. Package versions in the `.csproj` files are placeholders, so don't bump them by hand. A new library project needs its own build and publish jobs in `.gitlab-ci.yml`.
