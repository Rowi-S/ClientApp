using System;

namespace ClientApp.Api.Models;

public class Client
{
    public int Id { get; set; }
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
    public int Age { get; set; }

    public ICollection<ClientActivity> ClientActivities { get; set; } = new List<ClientActivity>();




}
