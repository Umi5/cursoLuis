using Curso.Business.Data.DbContexts;
using Curso.Business.Features.Clients.Queries.GetAllClients;
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

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.AllowCredentials();
        policy.WithOrigins("http://localhost:4200");
    });
});

var app = builder.Build();

app.UseFastEndpoints()
    .UseSwaggerGen(uiConfig: opt =>
    {
        opt.DefaultModelsExpandDepth = -1;
    });

app.UseCors();

app.Run();

public partial class Program { }
