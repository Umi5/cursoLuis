using MediatR;

namespace Curso.Buisness.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQuery : IRequest<GetAllClientsResponse>
{
    public GetAllClientsRequest Request { get; init; }
}

public record GetAllClientsRequest
{
    public string NameFilter { get; init; }
}