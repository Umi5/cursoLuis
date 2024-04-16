using Ardalis.Result.AspNetCore;
using Curso.Bussiness.Features.Clients.Queries.GetAllClients;
using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Curso.Api.Endpoints.Clients;

public class GetAllClientsEndpoint
    : Endpoint<GetAllClientsRequest, IEnumerable<GetAllClientsResponse>>
{
    private readonly ISender mediator;

    public GetAllClientsEndpoint(ISender mediator)
    {
        this.mediator = mediator;
    }

    public override void Configure()
    {
        Get("/api/clients/getAllClients");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllClientsRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetAllClientsQuery { Request = request },
            cancellationToken: ct
        );

        await SendResultAsync(result.ToMinimalApiResult());
    }
}

public class GetAllClientsEndpointSwagger : Summary<GetAllClientsEndpoint>
{
    public GetAllClientsEndpointSwagger()
    {
        Summary = "Este endpoint devuelve una lista de usuarios filtrados.";
        Description =
            "El endpoint pude filtrar por los nombres que contengan la cadena de texto opcional que se pasa por parámetro.";
        ExampleRequest = new GetAllClientsRequest { NameFilter = "uis" };
        Response<IEnumerable<GetAllClientsResponse>>(
            200,
            "Devuelve la lista filtrada correctamente.",
            example:
            [
                new GetAllClientsResponse { Id = Guid.NewGuid(), Name = "Manolo" },
                new GetAllClientsResponse { Id = Guid.NewGuid(), Name = "Cristina" }
            ]
        );
        Response<ErrorResponse>(
            400,
            "Ha ocurrido algún error de validacion.",
            example: new ErrorResponse
            {
                Message = "One or more errors occurred!",
                StatusCode = 400,
                Errors = new Dictionary<string, List<string>>
                {
                    ["nameFilter"] = ["El filtro de nombre no puede tener más de 10 caracteres."]
                }
            }
        );
    }
}
