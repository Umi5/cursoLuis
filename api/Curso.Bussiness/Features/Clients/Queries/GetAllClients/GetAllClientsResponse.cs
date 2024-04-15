namespace Curso.Bussiness.Features.Clients.Queries.GetAllClients;

public record GetAllClientsResponse
{
    /// <summary>
    /// El identificador unico del cliente.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// El nombre del cliente.
    /// </summary>
    public string Name { get; init; }

    public string Email { get; init; }
    public DateTime BirthDate { get; init; }
}
