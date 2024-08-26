using System;
using Customers.Domain.Entities.CustomerAggregate;

namespace Customers.Domain.Entities.CustomerAggregate.Events;

public class CustomerUpdatedEvent(
    Guid id,
    string firstName,
    string lastName,
    EGender gender,
    string email,
    DateTime dateOfBirth) : CustomerBaseEvent(id, firstName, lastName, gender, email, dateOfBirth)
{
}