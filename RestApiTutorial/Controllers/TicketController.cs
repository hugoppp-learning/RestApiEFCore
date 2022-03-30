using AutoMapper;
using DataStore.EF;
using DataStore.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiTutorial.DTOs;

namespace RestApiTutorial.Controllers;

[ApiController()]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private MyContext db;
    private readonly IMapper _mapper;

    public TicketController(MyContext db, IMapper mapper)
    {
        this.db = db;
        _mapper = mapper;
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        List<Ticket> tickets = await db.Tickets.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<TicketDto>>(tickets));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Ticket? ticket = await db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        return Ok(_mapper.Map<TicketDto>(ticket));
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateTicketDto ticketDto)
    {
        if (db.Projects.Find(ticketDto.ProjectId) is null)
            return UnprocessableEntity($"Project with id {ticketDto.ProjectId} does not exist");

        Ticket ticket = _mapper.Map<Ticket>(ticketDto);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll),
            new { id = ticket.TicketId },
            _mapper.Map<TicketDto>(ticket)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticketDto)
    {
        if (id != ticketDto.TicketId)
            return UnprocessableEntity("Id doesn't match");

        db.Entry(_mapper.Map<Ticket>(ticketDto)).State = EntityState.Modified;
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
