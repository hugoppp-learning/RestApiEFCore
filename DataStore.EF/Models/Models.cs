using EntityFrameworkCore.Triggered;

namespace DataStore.EF.Models;

public class Ticket
{
    public int TicketId { get; set; }

    public int ProjectId { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public Project? Project { get; init; }

}

public class Project
{
    public int ProjectId { get; init; }

    public string Title { get; init; } = null!;

    public List<Ticket>? Tickets { get; init; }
}

class TicketInsertIdTrigger : IBeforeSaveTrigger<Ticket>
{
    private readonly MyContext _myContext;

    public TicketInsertIdTrigger(MyContext myContext)
    {
        _myContext = myContext;
    }

    public Task BeforeSave(ITriggerContext<Ticket> context, CancellationToken cancellationToken)
    {
        var insertedTicket = context.Entity;
        if (context.ChangeType == ChangeType.Added)
        {
            int max = _myContext.Tickets.Where(t => t.ProjectId == insertedTicket.ProjectId).Select(t => t.TicketId).DefaultIfEmpty().Max();
            insertedTicket.TicketId = max + 1;
        }

        return Task.CompletedTask;
    }
}
