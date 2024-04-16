using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace Curso.Business.Features.Clients.Queries.GetClientById;

public class GetClientByIdQuery : IRequest<Result<GetClientByIdResponse>>
{
    public GetClientByIdRequest Request { get; set; }
}

public record GetClientByIdRequest
{
    public Guid Id { get; set; }
}

public class GetClientByIdRequestValidator : Validator<GetClientByIdRequest>
{
    public GetClientByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
