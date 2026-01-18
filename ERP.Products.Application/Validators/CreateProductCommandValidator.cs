using ERP.Products.Application.Commands;
using FluentValidation;

namespace ERP.Products.Application.Validators;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("CategoryId must be a valid GUID.");
    }
}
