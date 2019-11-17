using System;
using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    public class EntityByIdSpecification : ExpressionSpecification<TestDbEntity>
    {
        public EntityByIdSpecification(Guid id) : base(entity => entity.Id == id)
        {
        }
    }
}