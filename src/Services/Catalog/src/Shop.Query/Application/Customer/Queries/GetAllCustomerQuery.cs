using System.Collections.Generic;
using Ardalis.Result;
using Customers.Query.QueriesModel;
using MediatR;

namespace Customers.Query.Application.Customer.Queries;

public class GetAllCustomerQuery : IRequest<Result<IEnumerable<CustomerQueryModel>>>;