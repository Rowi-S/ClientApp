using System;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Api.Models;

[PrimaryKey(nameof(ClientId), nameof(ActivityId))]
public class ClientActivity
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    public int Rating { get; set; }
    public DateTime ParticipationDate { get; set; }
    public string? Notes { get; set; }

}
