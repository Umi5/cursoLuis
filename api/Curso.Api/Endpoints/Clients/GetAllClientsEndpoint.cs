using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Curso.Api.Endpoints.Clients;
public class GetAllClientsEndpoint 
    : Endpoint<GetAllClientsRequest, IEnumerable<GetAllClientsResponse>>
{
  public override void Configure()
  {
    Get("/api/clients/GetAllClients");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetAllClientsRequest request, CancellationToken ct)
  {

    GetAllClientsResponse[] response = [
        new GetAllClientsResponse {Id = Guid.NewGuid(), Name = "Manu"},
        new GetAllClientsResponse {Id = Guid.NewGuid(), Name = "Luis"},
    ];
    await SendOkAsync(response.Where(x => 
        request.NameFilter == null || x.Name.Contains(request.NameFilter, StringComparison.InvariantCultureIgnoreCase)));    
  }
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

public class GetAllClientsRequest{
    public string? NameFilter { get; set; }
}

public class GetAllClientsResponse {
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
}