using System;
using System.Linq;
using Ardalis.Result;
using Customers.Domain.Entities.CustomerAggregate;
using Customers.Domain.ValueObjects;

namespace Customers.Domain.Factories;

public static class CustomerFactory
{
    public static Result<Customer> Create(
        string firstName,
        string lastName,
        EGender gender,
        string email,
        DateTime dateOfBirth)
    {
        var emailResult = Email.Create(email);
        return !emailResult.IsSuccess
            ? Result<Customer>.Error(emailResult.Errors.ToArray())
            : Result<Customer>.Success(new Customer(firstName, lastName, gender, emailResult.Value, dateOfBirth));
    }

    public static Customer Create(string firstName, string lastName, EGender gender, Email email, DateTime dateOfBirth)
        => new(firstName, lastName, gender, email, dateOfBirth);
}