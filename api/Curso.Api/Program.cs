using Curso.Bussiness.Data.DbContexts;
using Curso.Bussiness.Features.Clients.Queries.GetAllClients;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddFastEndpoints()
    .SwaggerDocument(opt =>
    {
        opt.AutoTagPathSegmentIndex = 2;
        opt.ShortSchemaNames = true;
    });

builder.Services.AddMediatR(opt =>
    opt.RegisterServicesFromAssembly(typeof(GetAllClientsRequest).Assembly)
);

builder.Services.AddDbContext<CursoDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("CursoDb"));
    opt.UseSnakeCaseNamingConvention();
});

var app = builder.Build();

app.UseFastEndpoints()
    .UseSwaggerGen(uiConfig: opt =>
    {
        opt.DefaultModelsExpandDepth = -1;
    });

app.Run();

public partial class Program { }
