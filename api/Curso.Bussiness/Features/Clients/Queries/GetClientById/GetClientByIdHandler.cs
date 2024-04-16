using Ardalis.Result;
using Curso.Bussiness.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Curso.Bussiness.Features.Clients.Queries.GetClientById;

public class GetClientByIdHandler(CursoDbContext dbContext)
    : IRequestHandler<GetClientByIdQuery, Result<GetClientByIdResponse>>
{
    public async Task<Result<GetClientByIdResponse>> Handle(
        GetClientByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await dbContext
                .Clients.Select(x => new GetClientByIdResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    BirthDate = x.BirthDate
                })
                .SingleOrDefaultAsync(x => x.Id == query.Request.Id, cancellationToken);

            if (result == null)
            {
                return Result.NotFound(
                    $"The client with Id: {query.Request.Id} could not be found."
                );
            }

            return result;
        }
        catch (Exception ex)
        {
            return Result.CriticalError(ex.Message);
        }
    }
}
