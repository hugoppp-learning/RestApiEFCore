using System.IO.Enumeration;
using DataStore.EF;
using DataStore.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestApiTutorial.Controllers;

[ApiController()]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private MyContext db;

    public TicketController(MyContext db)
    {
        this.db = db;
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetById()
    {
        return Ok(await db.Tickets.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Ticket? ticket = await db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        return Ok(ticket);
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] Ticket ticket)
    {
        if (db.Projects.Find(ticket.ProjectId) is null)
            return UnprocessableEntity($"Project with id {ticket.ProjectId} does not exist");

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { id = ticket.TicketId },
            ticket
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Ticket ticket)
    {
        if (id != ticket.TicketId)
            return UnprocessableEntity("Id doesn't match");

        db.Entry(ticket).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (db.Projects.Find(id) == null)
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Ticket? ticket = db.Tickets.Find(id);
        if (ticket is null)
            return NotFound();

        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync();
        return NoContent();
    }

}
