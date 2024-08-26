using System;
using Customers.Core.SharedKernel;

namespace Customers.Application.Customer.Responses;

public class CreatedCustomerResponse(Guid id) : IResponse
{
    public Guid Id { get; } = id;
}