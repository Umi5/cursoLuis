namespace Curso.Business.Features.Clients.Queries.GetAllClients;

public record GetAllClientsResponse
{
    public record Client
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

    public IReadOnlyCollection<Client> Clients { get; init; }
    public int TotalRows { get; set; }
}
