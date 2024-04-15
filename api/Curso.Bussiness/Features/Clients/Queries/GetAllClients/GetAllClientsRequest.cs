using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace Curso.Bussiness.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQuery : IRequest<Result<IReadOnlyCollection<GetAllClientsResponse>>>
{
    public GetAllClientsRequest Request { get; init; }
}

public record GetAllClientsRequest
{
    public string? NameFilter { get; init; }
}

public class GetAllClientsRequestValidator : Validator<GetAllClientsRequest>
{
    public GetAllClientsRequestValidator()
    {
        RuleFor(x => x.NameFilter)
            .MaximumLength(10)
            .When(x => x.NameFilter != null)
            .WithMessage("El filtro de nombre no puede tener más de 10 caracteres.")
            .MinimumLength(1)
            .When(x => x.NameFilter != null)
            .WithMessage("El filtro de nombre no puede estar vacío.");
    }
}
