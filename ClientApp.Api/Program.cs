
using ClientApp.Api.Data;
using ClientApp.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.AddClientAppDb();


var app = builder.Build();

app.MapClientsEndpoints();

app.MigrateDb();

app.Run();
