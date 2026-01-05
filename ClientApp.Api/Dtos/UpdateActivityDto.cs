using System.ComponentModel.DataAnnotations;

namespace ClientApp.Api.Dtos;

public record UpdateActivityDto(
    [Required][StringLength(50)] string Name

);