using System.ComponentModel.DataAnnotations;

namespace ClientApp.Api.Dtos;

public record CreateClientDto(
    [Required][StringLength(50)] string FirstName,
    [Required][StringLength(50)] string LastName,
    [Range(1, 150)] int Age
);

