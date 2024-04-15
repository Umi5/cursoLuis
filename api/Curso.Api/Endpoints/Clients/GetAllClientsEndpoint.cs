using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Curso.Api.Endpoints.Clients;
public class GetAllClientsEndpoint : EndpointWithoutRequest<IEnumerable<GetAllClientsResponse>>
{
  public override void Configure()
  {
    Get("/api/clients/GetAllClients");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await SendOkAsync([
        new GetAllClientsResponse {Id = Guid.NewGuid(), Name = "Manu"},
        new GetAllClientsResponse {Id = Guid.NewGuid(), Name = "Luis"},
    ]);
  }
}


public class GetAllClientsResponse {
    public Guid Id { get; set; }
    public string Name { get; set; }
}