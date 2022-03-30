using Core.Models;
using DataStore.EF;
using Microsoft.EntityFrameworkCore;

namespace RestApiTutorial.Services;

public interface ITicketService
{
    Task<List<Ticket>> GetTicketsInProject(int pId);
    Task<List<Ticket>> GetAll();
    ValueTask<Ticket?> GetById(int id);
    Task<bool> Create(Ticket ticket);
    Task<bool> Update(Ticket ticket);
    Task<bool> Delete(int id);
}

public class TicketService : ITicketService
{
    private MyContext db;

    public TicketService(MyContext db)
    {
        this.db = db;
    }

    public Task<List<Ticket>> GetTicketsInProject(int pId)
    {
        return db.Tickets.Where(t => t.ProjectId == pId).ToListAsync();
    }

    public Task<List<Ticket>> GetAll()
    {
        return db.Tickets.ToListAsync();
    }

    public ValueTask<Ticket?> GetById(int id)
    {
        return db.Tickets.FindAsync(id);
    }

    public async Task<bool> Create(Ticket ticket)
    {
        if (db.Tickets.Find(ticket.TicketId) is null)
            return false;

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Ticket ticket)
    {
        db.Entry(ticket).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            if (db.Tickets.Find(ticket.TicketId) == null)
                return false;

            throw;
        }
    }

    public async Task<bool> Delete(int id)
    {
        Ticket? ticket = db.Tickets.Find(id);
        if (ticket is null)
            return false;

        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync();

        return true;
    }


}
