using System;

namespace ClientApp.Api.Models;

public class Activity
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<ClientActivity> ClientActivities { get; set; } = new List<ClientActivity>();

}
