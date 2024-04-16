using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;

namespace Curso.Bussiness.Data.Entities;

public class Client
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Email { get; init; }
    public DateTime BirthDate { get; private set; }

    private Client() { }

    public static Result<Client> CreateClient(
        Guid id,
        string name,
        string email,
        DateTime birthDate
    )
    {
        var newClient = new Client
        {
            Id = id,
            Name = name,
            Email = email,
            BirthDate = birthDate
        };

        var validation = new ClientValidator().Validate(newClient);
        if (!validation.IsValid)
        {
            return Result.Invalid(validation.AsErrors());
        }

        return newClient;
    }
}

internal class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("El nombre no puede tener más de 50 caracteres.")
            .MinimumLength(4)
            .WithMessage("El nombre no puede tener menos de 4 caracteres.");

        RuleFor(x => x.Email).EmailAddress().WithMessage("El email no es válido.");
    }
}
