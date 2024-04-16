using Ardalis.Result.AspNetCore;
using Curso.Business.Features.Clients.Queries.GetClientById;
using FastEndpoints;
using MediatR;

namespace Curso.Api.Endpoints.Clients;

public class GetClientByIdEndpoint(ISender mediator)
    : Endpoint<GetClientByIdRequest, GetClientByIdResponse>
{
    public override void Configure()
    {
        Get("/api/clients/getClientById");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetClientByIdRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetClientByIdQuery { Request = req },
            cancellationToken: ct
        );

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
