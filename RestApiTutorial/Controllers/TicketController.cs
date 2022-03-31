using AutoMapper;
using DataStore.EF;
using DataStore.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiTutorial.DTOs;

namespace RestApiTutorial.Controllers;

[ApiController()]
[Route("api/projects/{projectId}/tickets")]
public class TicketController : ControllerBase
{
    private MyContext db;
    private readonly IMapper _mapper;

    public TicketController(MyContext db, IMapper mapper)
    {
        this.db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int projectId)
    {
        List<Ticket> tickets = await db.Tickets.Where(t => t.ProjectId == projectId).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<TicketDto>>(tickets));
    }

    [HttpGet("{ticketId}")]
    public async Task<IActionResult> GetById(int projectId, int ticketId)
    {
        Ticket? ticket = await db.Tickets.FindAsync(projectId, ticketId);
        if (ticket is null)
            return NotFound();

        return Ok(_mapper.Map<TicketDto>(ticket));
    }


    [HttpPost]
    public async Task<IActionResult> Create(int projectId, [FromBody] CreateTicketDto ticketDto)
    {
        if (projectId != ticketDto.ProjectId)
            return UnprocessableEntity();

        if (!await db.Projects.AnyAsync(p => p.ProjectId == ticketDto.ProjectId))
            return UnprocessableEntity($"Project with id {ticketDto.ProjectId} does not exist");

        Ticket ticket = _mapper.Map<Ticket>(ticketDto);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { ticket.TicketId, projectId },
            _mapper.Map<TicketDto>(ticket)
        );
    }

    [HttpPut("{ticketId}")]
    public async Task<IActionResult> Update(int projectId, int ticketId, [FromBody] TicketDto ticketDto)
    {
        if (ticketId != ticketDto.TicketId || projectId != ticketDto.ProjectId)
            return UnprocessableEntity();

        db.Entry(_mapper.Map<Ticket>(ticketDto)).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception)
        {
            if (!await db.Projects.AnyAsync(p => p.ProjectId == ticketId))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{ticketId}")]
    public async Task<IActionResult> Delete(int projectId, int ticketId)
    {
        Ticket? ticket = db.Tickets.Find(projectId, ticketId);
        if (ticket is null)
            return NotFound();

        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync();
        return NoContent();
    }

}
