using System.ComponentModel.DataAnnotations;

namespace ClientApp.Api.Dtos;

public record CreateActivityDto(
    [Required][StringLength(50)] string Name
);




