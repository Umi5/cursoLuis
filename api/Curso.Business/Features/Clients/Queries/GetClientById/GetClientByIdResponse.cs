namespace Curso.Business.Features.Clients.Queries.GetClientById;

public record GetClientByIdResponse
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
