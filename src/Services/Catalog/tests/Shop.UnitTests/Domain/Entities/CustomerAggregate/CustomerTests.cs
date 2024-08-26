using Bogus;
using Customers.Domain.Entities.CustomerAggregate;
using Customers.Domain.Entities.CustomerAggregate.Events;
using Customers.Domain.Factories;
using Customers.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Domain.Entities.CustomerAggregate;

[UnitTest]
public class CustomerTests
{
    [Fact]
    public void Should_CustomerCreatedEvent_WhenCreate()
    {
        // Arrange
        var customerFaker = new Faker<Customer>()
            .CustomInstantiator(faker => CustomerFactory.Create(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.PickRandom<EGender>(),
                faker.Person.Email,
                faker.Person.DateOfBirth));

        // Act
        var act = customerFaker.Generate();

        // Assert
        act.DomainEvents.Should()
            .NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainItemsAssignableTo<CustomerCreatedEvent>();
    }

    [Fact]
    public void Should_CustomerUpdatedEvent_WhenChangeEmail()
    {
        // Arrange
        var customerEntity = new Faker<Customer>()
            .CustomInstantiator(faker => CustomerFactory.Create(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.PickRandom<EGender>(),
                faker.Person.Email,
                faker.Person.DateOfBirth))
            .Generate();

        var email = new Faker<Email>()
            .CustomInstantiator(faker => Email.Create(faker.Person.Email).Value)
            .Generate();

        // Act
        customerEntity.ChangeEmail(email);

        // Assert
        customerEntity.DomainEvents.Should()
            .NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainItemsAssignableTo<CustomerUpdatedEvent>();
    }

    [Fact]
    public void Should_CustomerDeletedEvent_WhenDelete()
    {
        // Arrange
        var customerEntity = new Faker<Customer>()
            .CustomInstantiator(faker => CustomerFactory.Create(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.PickRandom<EGender>(),
                faker.Person.Email,
                faker.Person.DateOfBirth))
            .Generate();

        // Act
        customerEntity.Delete();

        // Assert
        customerEntity.DomainEvents.Should()
            .NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainItemsAssignableTo<CustomerDeletedEvent>();
    }
}