using AutoMapper;
using DataStore.EF;
using Microsoft.EntityFrameworkCore;
using RestApiTutorial.DTOs;
using Ticket = DataStore.EF.Ticket;

namespace RestApiTutorial.Services;

public interface ITicketService
{
    Task<List<Ticket>> GetTicketsInProject(int pId);
    Task<List<Ticket>> GetAll();
    ValueTask<Ticket?> GetById(int id);
    Task<Ticket> Create(CreateTicketDto createTicketDto);
    Task<bool> Update(TicketDto ticketDto);
    Task<bool> Delete(int id);
}

public class TicketService : ITicketService
{
    private MyContext db;
    private IMapper _mapper;

    public TicketService(MyContext db, IMapper mapper)
    {
        this.db = db;
        _mapper = mapper;
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

    public async Task<Ticket> Create(CreateTicketDto createTicketDto)
    {
        var ticket = _mapper.Map<Ticket>(createTicketDto);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();
        return ticket;
    }

    public async Task<bool> Update(TicketDto ticketDto)
    {
        var ticket = _mapper.Map<Ticket>(ticketDto);
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
