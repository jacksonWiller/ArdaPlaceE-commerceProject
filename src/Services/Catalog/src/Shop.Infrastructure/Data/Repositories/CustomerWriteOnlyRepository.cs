using System;
using System.Threading.Tasks;
using Customers.Domain.Entities.CustomerAggregate;
using Customers.Domain.ValueObjects;
using Customers.Infrastructure.Data.Context;
using Customers.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure.Data.Repositories;

internal class CustomerWriteOnlyRepository(WriteDbContext context)
    : BaseWriteOnlyRepository<Customer, Guid>(context), ICustomerWriteOnlyRepository
{
    public async Task<bool> ExistsByEmailAsync(Email email) =>
        await Context.Customers
            .AsNoTracking()
            .AnyAsync(customer => customer.Email.Address == email.Address);

    public async Task<bool> ExistsByEmailAsync(Email email, Guid currentId) =>
        await Context.Customers
            .AsNoTracking()
            .AnyAsync(customer => customer.Email.Address == email.Address && customer.Id != currentId);
}