using System.ComponentModel.DataAnnotations;

namespace RestApiTutorial.DTOs;

public record CreateTicketDto(
    [Required] string Title,
    [Required] int ProjectId,
    string? Description
);
public record TicketDto(
    [Required] int TicketId,
    [Required] string Title,
    [Required] int ProjectId,
    string? Description
);

public record CreateProjectDto(
    [Required] string Title
);
public record ProjectDto(
    [Required] int ProjectId,
    [Required] string Title
);
