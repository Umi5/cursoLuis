using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace Curso.Business.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQuery : IRequest<Result<GetAllClientsResponse>>
{
    public GetAllClientsRequest Request { get; init; }
}

public record GetAllClientsRequest
{
    public string? Filter { get; init; }
    public string? OrderBy { get; init; }
    public string? OrderDirection { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}

public class GetAllClientsRequestValidator : Validator<GetAllClientsRequest>
{
    public GetAllClientsRequestValidator()
    {
        RuleFor(x => x.Filter)
            .MaximumLength(20)
            .When(x => x.Filter != null)
            .WithMessage("El filtro de nombre no puede tener más de 10 caracteres.")
            .MinimumLength(1)
            .When(x => x.Filter != null)
            .WithMessage("El filtro de nombre no puede estar vacío.");

        RuleFor(x => x.OrderDirection)
            .Must(x =>
                x!.Equals("asc", StringComparison.InvariantCultureIgnoreCase)
                || x.Contains("desc", StringComparison.InvariantCultureIgnoreCase)
            )
            .When(x => x.OrderBy != null);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("El tamaño de página debe ser mayor a 0.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor a 0.");
    }
}
