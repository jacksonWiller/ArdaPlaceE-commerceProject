using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customers.Query.Abstractions;
using Customers.Query.QueriesModel;

namespace Customers.Query.Data.Repositories.Abstractions;

public interface ICustomerReadOnlyRepository : IReadOnlyRepository<CustomerQueryModel, Guid>
{
    Task<IEnumerable<CustomerQueryModel>> GetAllAsync();
}