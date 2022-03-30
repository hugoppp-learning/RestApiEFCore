using System.Text.Json.Serialization;

namespace DataStore.EF.Models;

public class Ticket
{
    public int TicketId { get; init; }

    public int ProjectId { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    [JsonIgnore]
    public Project? Project { get; init; }

}

public class Project
{
    public int ProjectId { get; init; }

    public string? Title { get; init; }

    [JsonIgnore]
    public List<Ticket>? Tickets { get; init; }
}
