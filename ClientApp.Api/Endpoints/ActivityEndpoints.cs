using System;
using ClientApp.Api.Data;
using ClientApp.Api.Dtos;
using ClientApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Api.Endpoints;

public static class ActivityEndpoints
{
    const string GetActivityEndpointName = "GetActivity";
    public static void MapActivitiesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/activities");




        group.MapGet("/", async (ClientAppContext dbContext)
        => await dbContext.Activities
                        .Select(activity => new ActivitySummaryDto(
                            activity.Id,
                            activity.Name
                        ))
                        .AsNoTracking()
                        .ToListAsync());




        group.MapGet("/{id}", async (int id, ClientAppContext dbContext) =>
        {
            var activity = await dbContext.Activities.FindAsync(id);
            return activity is null ? Results.NotFound() : Results.Ok(
                new ActivitySummaryDto(
                    activity.Id,
                    activity.Name
                )
            );
        })
            .WithName(GetActivityEndpointName);


        group.MapPost("/", async (CreateActivityDto newActivity, ClientAppContext dbContext) =>
        {
            Activity activity = new()
            {
                Name = newActivity.Name
            };

            dbContext.Activities.Add(activity);
            await dbContext.SaveChangesAsync();

            ActivitySummaryDto activitySummaryDto = new(
                activity.Id,
                activity.Name
            );

            return Results.CreatedAtRoute(GetActivityEndpointName, new { id = activitySummaryDto.Id }, activitySummaryDto);
        });


        group.MapPut("/{id}", async (int id, UpdateActivityDto updatedActivityDto, ClientAppContext dbContext) =>
        {
            var existingActivity = await dbContext.Activities.FindAsync(id);

            if (existingActivity is null)
            {
                return Results.NotFound();
            }

            existingActivity.Name = updatedActivityDto.Name;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, ClientAppContext dbContext) =>
        {
            await dbContext.Activities
                .Where(activity => activity.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
