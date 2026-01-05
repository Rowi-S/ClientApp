using System;
using ClientApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Api.Data;

public class ClientAppContext(DbContextOptions<ClientAppContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ClientActivity> ClientActivities => Set<ClientActivity>();
}
