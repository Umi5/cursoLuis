using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints().SwaggerDocument(opt => {
    opt.AutoTagPathSegmentIndex = 2;
});

var app = builder.Build();


app.UseFastEndpoints().UseSwaggerGen(uiConfig: opt => {
    opt.DefaultModelsExpandDepth = -1;
});

app.Run();