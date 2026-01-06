using System;
using ClientApp.Api.Data;
using ClientApp.Api.Dtos;
using ClientApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Api.Endpoints;


public static class ClientEndpoints
{
    const string GetClientEndpointName = "GetClient";
    const string GetClientActivityEndpointName = "GetClientActivity";
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


        group.MapPut("/{id}", async (int id, UpdateClientDto updatedClient, ClientAppContext dbContext) =>
        {
            var existingClient = await dbContext.Clients.FindAsync(id);

            if (existingClient is null)
            {
                return Results.NotFound();
            }

            existingClient.FirstName = updatedClient.FirstName;
            existingClient.LastName = updatedClient.LastName;
            existingClient.Age = updatedClient.Age;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, ClientAppContext dbContext) =>
        {
            await dbContext.Clients
                .Where(client => client.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/{id}/activities", async (
            int id,
            CreateClientActivityDto createClientActivityDto,
            ClientAppContext dbContext) =>
    {
        var existingClient = await dbContext.Clients.FindAsync(id);
        if (existingClient is null)
            return Results.NotFound("Client not found");

        var existingActivity = await dbContext.Activities.FindAsync(createClientActivityDto.ActivityId);
        if (existingActivity is null)
            return Results.NotFound("Activity not found");

        var alreadyExists = await dbContext.ClientActivities
            .AnyAsync(ca => ca.ClientId == id && ca.ActivityId == createClientActivityDto.ActivityId);

        if (alreadyExists)
            return Results.Conflict("Client already has this activity");

        var clientActivity = new ClientActivity
        {
            ClientId = id,
            ActivityId = createClientActivityDto.ActivityId,
            Rating = createClientActivityDto.Rating,
            ParticipationDate = createClientActivityDto.ParticipationDate,
            Notes = createClientActivityDto.Notes
        };

        dbContext.ClientActivities.Add(clientActivity);
        await dbContext.SaveChangesAsync();

        var clientActivitySummaryDto = new ClientActivitySummaryDto(
            clientActivity.ClientId,
            clientActivity.ActivityId,
            clientActivity.Activity.Name,
            clientActivity.Rating,
            clientActivity.ParticipationDate,
            clientActivity.Notes
        );

        return Results.CreatedAtRoute(GetClientActivityEndpointName, new { clientId = clientActivitySummaryDto.ClientId, activityId = clientActivitySummaryDto.ActivityId }, clientActivitySummaryDto);



    });

        group.MapGet("/{clientId}/activities/{activityId}", async (
        int clientId,
        int activityId,
        ClientAppContext db) =>
    {
        var result = await db.ClientActivities
            .Where(ca => ca.ClientId == clientId && ca.ActivityId == activityId)
            .Include(ca => ca.Activity)
            .Select(ca => new ClientActivitySummaryDto(
                ca.ClientId,
                ca.ActivityId,
                ca.Activity.Name,
                ca.Rating,
                ca.ParticipationDate,
                ca.Notes
            ))
            .FirstOrDefaultAsync();

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    })
    .WithName(GetClientActivityEndpointName);


    }
}