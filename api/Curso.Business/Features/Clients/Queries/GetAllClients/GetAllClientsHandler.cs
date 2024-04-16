using System.Linq.Expressions;
using Ardalis.Result;
using Curso.Business.Data.DbContexts;
using Curso.Business.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Curso.Business.Features.Clients.Queries.GetAllClients;

public class GetAllClientsHandler(CursoDbContext dbContext)
    : IRequestHandler<GetAllClientsQuery, Result<GetAllClientsResponse>>
{
    public async Task<Result<GetAllClientsResponse>> Handle(
        GetAllClientsQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var filteredQueryResult = BuildQuery(query.Request);

            if (!filteredQueryResult.IsSuccess)
            {
                return Result.Invalid(filteredQueryResult.ValidationErrors.ToArray());
            }

            var clients = await filteredQueryResult
                .Value.Skip(query.Request.PageSize * (query.Request.PageNumber - 1))
                .Take(query.Request.PageSize)
                .Select(x => new GetAllClientsResponse.Client
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    BirthDate = x.BirthDate
                })
                .ToArrayAsync(cancellationToken);

            var totalRows = await dbContext.Clients.CountAsync(cancellationToken);

            return new GetAllClientsResponse { Clients = clients, TotalRows = totalRows };
        }
        catch (Exception ex)
        {
            return Result.CriticalError(ex.Message);
        }
    }

    private Result<IQueryable<Client>> BuildQuery(GetAllClientsRequest request)
    {
        var query = dbContext.Clients.AsNoTracking();

        if (request.Filter != null)
        {
            query = query.Where(x =>
                x.Name.ToLower().Contains(request.Filter.ToLower())
                || x.Email.ToLower().Contains(request.Filter.ToLower())
            );
        }

        Expression<Func<Client, object>> orderbyExpression = x => x.Name;

        if (request.OrderBy != null)
        {
            switch (request.OrderBy)
            {
                case "name":
                    orderbyExpression = x => x.Name;
                    break;
                case "email":
                    orderbyExpression = x => x.Email;
                    break;
                case "birthDate":
                    orderbyExpression = x => x.BirthDate;
                    break;
                default:
                    return Result.Invalid(
                        new ValidationError
                        {
                            Identifier = nameof(request.OrderBy),
                            ErrorMessage =
                                $"No se pueden ordenar clientes por el campo: {request.OrderBy}"
                        }
                    );
            }
            ;
        }

        query =
            request.OrderDirection?.ToLower()?.Equals("asc") ?? true
                ? query.OrderBy(orderbyExpression)
                : query.OrderByDescending(orderbyExpression);

        return Result.Success(query);
    }
}
