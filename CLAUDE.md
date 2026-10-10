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

Everything targets `net10.0` with EF Core 10. `global.json` pins the .NET 10 SDK and selects Microsoft.Testing.Platform as the test runner. The solution is `src/aska.core.slnx`.

```sh
dotnet build src/aska.core.slnx
dotnet test --solution src/aska.core.slnx
dotnet test --project src/Aska.Core.EntityStorage.Ef.Sqlite.Tests
dotnet test --project src/Aska.Core.EntityStorage.Ef.MariaDb.Tests --filter-method "*Command_OnUpdateCommand_ShouldQueryChangedData"
```

The MariaDB and PostgreSQL tests start their own database containers with Testcontainers, so Docker must be running. Nothing else needs to be set up. The SQLite tests use a temporary database file.

Build settings shared by all projects live in `Directory.Build.props`, which sets nullable reference types, implicit usings and package metadata. Package versions live in `Directory.Packages.props` through central package management, so `.csproj` files reference packages without versions. `.editorconfig` enforces file-scoped namespaces and flags unused usings. The build has zero warnings, so keep it that way.

## Architecture

```
Abstractions        IQuery, ICommand family, specifications, IEntityStorageReader/Writer, type discovery
  Ef                EntityStorageContext base, no-tracking queries, EF commands, AddEfEntityStorage()
    Ef.MariaDb      AutoDiscovery*Context, connection string builder, UseMariaDb<TContext>()
    Ef.PostgreSql   same shape, UsePostgresql<TContext>()
    Ef.Sqlite       same shape, UseSqlite<TContext>()
```

Every project is prefixed `Aska.Core.EntityStorage.`. The three provider projects are near copies of each other, so a change to one usually belongs in all three.

The MariaDB provider is `Microting.EntityFrameworkCore.MySql`, a maintained fork of Pomelo that has EF Core 10 support. Pomelo itself has no EF Core 10 release. The API is Pomelo's, so `UseMySql` needs a server version. `AutoDiscoveryMariaDbContext` detects it from the server once per connection string, or takes a fixed version through its protected constructor. Pass a fixed version wherever no server is reachable, such as in a design-time factory.

### Registration

`src/Aska.Core.EntityStorage.DemoApp/src/Program.cs` is the reference setup:

```csharp
builder.Services.AddEfEntityStorage(storage =>
{
    storage.UseMariaDb<DemoMariaDbContext>()
        .WithEntitiesAutoDiscovery<IMariaDbEntity>("Aska.Core.EntityStorage.DemoApp")
        .WithConnectionString(MariaDbConnectionString.Create().WithServer("localhost")...);
    storage.UsePostgresql<DemoPsqlContext>()...;
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
- **The library never applies migrations.** `IEntityStorageContextInitializer.InitializeAsync()` calls each context's `InitializeAsync`, which does nothing by default. The demo contexts override it to run `Database.MigrateAsync()`. Migrations live in the consuming project, one folder per context, with an `IDesignTimeDbContextFactory` for `dotnet ef`.

### Demo app and migrations

The demo needs MariaDB and PostgreSQL. `compose.yaml` at the repo root provides both.

```sh
docker compose up -d --wait
dotnet run --project src/Aska.Core.EntityStorage.DemoApp
docker compose down
```

`dotnet-ef` is a local tool pinned in `dotnet-tools.json`, so run `dotnet tool restore` once. To add a demo migration, name the context and its output folder:

```sh
dotnet ef migrations add <Name> -p src/Aska.Core.EntityStorage.DemoApp -c DemoMariaDbContext -o Migrations/MariaDb
```

### Tests

`Aska.Core.EntityStorage.Ef.Tests` is a class library of shared test bases, not a runnable test project.

- `EntityStorageCrudTests<TFixture, TContext>` holds every test as a `[Fact]`. Each provider test project declares a sealed subclass and inherits all of them, so a new test added to the base runs on every provider.
- `EntityStorageFixture<TContext>` starts the database, wires storage through the real `AddEfEntityStorage` path, and creates the schema with `EnsureCreated`. Test projects therefore have no migrations.
- A provider test project contains only its fixture, which starts the database, plus a small context and the sealed test class.

## CI and packaging

`.github/workflows/ci.yml` builds, tests and packs on pushes to `master` and `develop`, on pull requests, and on tags. Pushing a tag also publishes the packages to nuget.org, which needs the `NUGET_API_KEY` repository secret.

Package versions come from git tags through MinVer, so never set `<Version>` in a project. A tag such as `1.2.0` produces version `1.2.0`. Commits after a tag get a pre-release version based on that tag. Only the five library projects set `IsPackable` to `true`.
