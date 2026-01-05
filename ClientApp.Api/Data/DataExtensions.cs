using System;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Api.Data;

public static class DataExtensions
{
    //automatically performs migrations when used in program.cs
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
        .GetRequiredService<ClientAppContext>();
        dbContext.Database.Migrate();

    }

    public static void AddClientAppDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("ClientApp");
        builder.Services.AddSqlite<ClientAppContext>(
            connString
            );
    }
}
