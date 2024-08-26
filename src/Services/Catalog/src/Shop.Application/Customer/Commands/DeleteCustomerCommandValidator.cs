using FluentValidation;

namespace Customers.Application.Customer.Commands;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}