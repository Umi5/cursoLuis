using Curso.Bussiness.Features.Clients.Queries.GetAllClients;
using FastEndpoints;
using FastEndpoints.Swagger;

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

var app = builder.Build();

app.UseFastEndpoints()
    .UseSwaggerGen(uiConfig: opt =>
    {
        opt.DefaultModelsExpandDepth = -1;
    });

app.Run();
