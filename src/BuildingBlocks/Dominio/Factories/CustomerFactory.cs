using System;
using System.Linq;
using Ardalis.Result;
using JdMarketplace.Core.Dominio.Entities.CustomerAggregate;
using JdMarketplace.Core.Dominio.ValueObjects;

namespace JdMarketplace.Core.Dominio.Factories;

public static class CustomerFactory
{
    public static Result<Customer> Create(
        string firstName,
        string lastName,
        EGender gender,
        string email,
        DateTime dateOfBirth)
    {
        var emailResult = EmailNew.Create(email);
        return !emailResult.IsSuccess
            ? Result<Customer>.Error(emailResult.Errors.ToArray())
            : Result<Customer>.Success(new Customer(firstName, lastName, gender, emailResult.Value, dateOfBirth));
    }

    public static Customer Create(string firstName, string lastName, EGender gender, EmailNew email, DateTime dateOfBirth)
        => new(firstName, lastName, gender, email, dateOfBirth);
}