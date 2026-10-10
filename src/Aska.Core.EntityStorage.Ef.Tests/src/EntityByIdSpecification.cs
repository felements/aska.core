using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.EntityStorage.Ef.Tests;

public class EntityByIdSpecification(Guid id) : ExpressionSpecification<TestDbEntity>(entity => entity.Id == id);
