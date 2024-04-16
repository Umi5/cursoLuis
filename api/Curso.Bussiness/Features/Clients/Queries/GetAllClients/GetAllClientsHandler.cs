using Ardalis.Result;
using Curso.Bussiness.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Curso.Bussiness.Features.Clients.Queries.GetAllClients;

public class GetAllClientsHandler(CursoDbContext dbContext)
    : IRequestHandler<GetAllClientsQuery, Result<IReadOnlyCollection<GetAllClientsResponse>>>
{
    public async Task<Result<IReadOnlyCollection<GetAllClientsResponse>>> Handle(
        GetAllClientsQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var clients = await dbContext
                .Clients.AsNoTracking()
                .Where(x =>
                    query.Request.NameFilter == null
                    || x.Name.Contains(
                        query.Request.NameFilter
                    )
                )
                .Select(x => new GetAllClientsResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    BirthDate = x.BirthDate
                })
                .ToArrayAsync(cancellationToken);

            return clients;
        }
        catch (Exception ex)
        {
            return Result.CriticalError(ex.Message);
        }
    }
}
