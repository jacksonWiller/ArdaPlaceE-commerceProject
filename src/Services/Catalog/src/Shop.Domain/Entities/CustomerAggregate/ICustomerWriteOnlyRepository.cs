using System;
using System.Threading.Tasks;
using Customers.Core.SharedKernel;
using Customers.Domain.ValueObjects;

namespace Customers.Domain.Entities.CustomerAggregate;

public interface ICustomerWriteOnlyRepository : IWriteOnlyRepository<Customer, Guid>
{
    /// <summary>
    /// Checks if a customer with the specified email already exists asynchronously.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <returns>True if a customer with the email exists, false otherwise.</returns>
    Task<bool> ExistsByEmailAsync(Email email);

    /// <summary>
    /// Checks if a customer with the specified email and current ID already exists asynchronously.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <param name="currentId">The current ID of the customer to exclude from the check.</param>
    /// <returns>True if a customer with the email and current ID exists, false otherwise.</returns>
    Task<bool> ExistsByEmailAsync(Email email, Guid currentId);
}