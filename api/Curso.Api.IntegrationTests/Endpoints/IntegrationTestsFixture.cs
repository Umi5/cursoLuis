using Curso.Business.Data.DbContexts;
using Curso.Business.Data.Entities;
using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Curso.Api.IntegrationTests;

public class IntegrationTestsFixture : AppFixture<Program>
{
    private readonly string _testDatabaseId = Guid.NewGuid().ToString();

    protected override async Task SetupAsync()
    {
        // place one-time setup code here for every test class

        await using var scope = Services.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<CursoDbContext>();
        await db.Database.EnsureCreatedAsync();

        Client[] clients =
        [
            Curso
                .Business.Data.Entities.Client.CreateClient(
                    Guid.NewGuid(),
                    "John Doe",
                    "jhon@gmail.com",
                    DateTime.UtcNow
                )
                .Value,
            Curso
                .Business.Data.Entities.Client.CreateClient(
                    Guid.NewGuid(),
                    "Jane Doe",
                    "jane@gmail.com",
                    DateTime.UtcNow
                )
                .Value
        ];

        await db.Clients.AddRangeAsync(clients);
        await db.SaveChangesAsync();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // replace the real database with a test database
        services.RemoveAll<DbContextOptions<CursoDbContext>>();
        services.RemoveAll<CursoDbContext>();
        services.AddDbContext<CursoDbContext>(
            (sp, options) =>
            {
                var connectionString = sp.GetRequiredService<IConfiguration>()
                    .GetConnectionString("CursoDb")!;
                if (!connectionString.Contains("{:id}"))
                {
                    throw new ArgumentException(
                        "Test database name connection string must contain {:id} placeholder."
                    );
                }

                options.UseNpgsql(connectionString.Replace("{:id}", _testDatabaseId));
                options.UseSnakeCaseNamingConvention();
            }
        );
    }

    protected override async Task TearDownAsync()
    {
        // do cleanups here for every test class

        await using var scope = Services.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<CursoDbContext>();
        await db.Database.EnsureDeletedAsync();
    }

    public CursoDbContext GetDbContext()
    {
        return Services.GetRequiredService<CursoDbContext>();
    }
}
