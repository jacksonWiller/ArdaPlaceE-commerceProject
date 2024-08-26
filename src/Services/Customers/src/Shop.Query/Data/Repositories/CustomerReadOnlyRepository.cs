using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customers.Query.Abstractions;
using Customers.Query.Data.Repositories.Abstractions;
using Customers.Query.QueriesModel;
using MongoDB.Driver;

namespace Customers.Query.Data.Repositories;

internal class CustomerReadOnlyRepository(IReadDbContext readDbContext)
    : BaseReadOnlyRepository<CustomerQueryModel, Guid>(readDbContext), ICustomerReadOnlyRepository
{
    public async Task<IEnumerable<CustomerQueryModel>> GetAllAsync() =>
        await Collection
            .Find(Builders<CustomerQueryModel>.Filter.Empty)
            .SortBy(customer => customer.FirstName)
            .ThenBy(customer => customer.DateOfBirth)
            .ToListAsync();
}