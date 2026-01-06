using System.ComponentModel.DataAnnotations;

namespace ClientApp.Api.Dtos;

public record CreateClientActivityDto(
    [Required] int ActivityId,
    [Required] int Rating,
    [Required] DateTime ParticipationDate,
    string? Notes
);
