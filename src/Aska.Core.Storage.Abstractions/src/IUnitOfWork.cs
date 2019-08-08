using System;

namespace Aska.Core.Storage.Abstractions
{
    public interface IUnitOfWork : IDataAccessor, IDataReader, IDisposable
    {
    }
}