using System;
using JdMarketplace.Core.SharedKernel;

namespace JdMarketplace.Core.Data
{
    public interface IRepository<T> : IDisposable where T : Dominio.Entities.IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}