using System;
using ClientApp.Api.Data;
using ClientApp.Api.Dtos;
using ClientApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Api.Endpoints;

//todo add update en delete endpoint.
public static class ClientEndpoints
{
    const string GetClientEndpointName = "GetClient";
    public static void MapClientsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/clients");




        group.MapGet("/", async (ClientAppContext dbContext)
        => await dbContext.Clients
                        .Select(client => new ClientSummaryDto(
                            client.Id,
                            client.FirstName,
                            client.LastName,
                            client.Age
                        ))
                        .AsNoTracking()
                        .ToListAsync());




        group.MapGet("/{id}", async (int id, ClientAppContext dbContext) =>
        {
            var client = await dbContext.Clients.FindAsync(id);
            return client is null ? Results.NotFound() : Results.Ok(
                new ClientSummaryDto(
                    client.Id,
                    client.FirstName,
                    client.LastName,
                    client.Age
                )
            );
        })
            .WithName(GetClientEndpointName);


        group.MapPost("/", async (CreateClientDto newClient, ClientAppContext dbContext) =>
        {
            Client client = new()
            {
                FirstName = newClient.FirstName,
                LastName = newClient.LastName,
                Age = newClient.Age,
            };

            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            ClientSummaryDto clientSummaryDto = new(
                client.Id,
                client.FirstName,
                client.LastName,
                client.Age
            );

            return Results.CreatedAtRoute(GetClientEndpointName, new { id = clientSummaryDto.Id }, clientSummaryDto);
        });

    }
}