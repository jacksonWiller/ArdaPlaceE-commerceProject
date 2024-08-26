using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Customers.Application.Customer.Commands;
using Customers.Core.SharedKernel;
using Customers.Domain.Entities.CustomerAggregate;
using Customers.Domain.Factories;
using Customers.Infrastructure.Data;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shop.Application.Customer.Commands;
using Shop.Application.Customer.Handlers;
using Shop.Infrastructure.Data.Repositories;
using Shop.UnitTests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Application.Customer.Handlers;

[UnitTest]
public class DeleteCustomerCommandHandlerTests(EfSqliteFixture fixture) : IClassFixture<EfSqliteFixture>
{
    private readonly EfSqliteFixture _fixture = fixture;
    private readonly DeleteCustomerCommandValidator _validator = new();

    [Fact]
    public async Task Delete_ValidCustomerId_ShouldReturnsSuccessResult()
    {
        // Arrange
        var customer = new Faker<Customers.Domain.Entities.CustomerAggregate.Customer>()
            .CustomInstantiator(faker => CustomerFactory.Create(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.PickRandom<EGender>(),
                faker.Person.Email,
                faker.Person.DateOfBirth))
            .Generate();

        var repository = new CustomerWriteOnlyRepository(_fixture.Context);
        repository.Add(customer);

        await _fixture.Context.SaveChangesAsync();
        _fixture.Context.ChangeTracker.Clear();

        var unitOfWork = new UnitOfWork(
            _fixture.Context,
            Substitute.For<IEventStoreRepository>(),
            Substitute.For<IMediator>(),
            Substitute.For<ILogger<UnitOfWork>>());

        var handler = new DeleteCustomerCommandHandler(
            _validator,
            new CustomerWriteOnlyRepository(_fixture.Context),
            unitOfWork);

        var command = new DeleteCustomerCommand(customer.Id);

        // Act
        var act = await handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.IsSuccess.Should().BeTrue();
        act.SuccessMessage.Should().Be("Successfully removed!");
    }

    [Fact]
    public async Task Delete_NotFoundCustomer_ShouldReturnsFailResult()
    {
        // Arrange
        var command = new DeleteCustomerCommand(Guid.NewGuid());

        var handler = new DeleteCustomerCommandHandler(
            _validator,
            new CustomerWriteOnlyRepository(_fixture.Context),
            Substitute.For<IUnitOfWork>());

        // Act
        var act = await handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.IsSuccess.Should().BeFalse();
        act.Errors.Should()
            .NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.Contain(errorMessage => errorMessage == $"No customer found by Id: {command.Id}");
    }

    [Fact]
    public async Task Delete_InvalidCommand_ShouldReturnsFailResult()
    {
        // Arrange
        var handler = new DeleteCustomerCommandHandler(
            _validator,
            Substitute.For<ICustomerWriteOnlyRepository>(),
            Substitute.For<IUnitOfWork>());

        // Act
        var act = await handler.Handle(new DeleteCustomerCommand(Guid.Empty), CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.IsSuccess.Should().BeFalse();
        act.ValidationErrors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
    }
}