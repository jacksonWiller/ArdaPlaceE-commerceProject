using System;
using Ardalis.Result;
using Customers.Query.QueriesModel;
using MediatR;

namespace Customers.Query.Application.Customer.Queries;

public class GetCustomerByIdQuery(Guid id) : IRequest<Result<CustomerQueryModel>>
{
    public Guid Id { get; } = id;
}