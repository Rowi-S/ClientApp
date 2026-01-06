namespace ClientApp.Api.Dtos;

public record ClientActivitySummaryDto
(
    int ClientId,
    int ActivityId,
    string ActivityName,
    int Rating,
    DateTime ParticipationDate,
    string? Notes
);

