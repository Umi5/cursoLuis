namespace Curso.Buisness.Features.Clients.Queries.GetAllClients;
public record GetAllClientsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}