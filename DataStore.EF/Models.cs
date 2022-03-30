namespace DataStore.EF;

public class Ticket
{
    public int TicketId { get; init; }

    public int ProjectId { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public Project? Project { get; init; }

}

public class Project
{
    public int ProjectId { get; init; }

    public string? Title { get; init; }

    public List<Ticket>? Tickets { get; init; }
}
