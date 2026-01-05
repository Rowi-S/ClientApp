namespace ClientApp.Api.Dtos;

public record ClientSummaryDto(
    int Id,
    string FirstName,
    string LastName,
    int Age
);
