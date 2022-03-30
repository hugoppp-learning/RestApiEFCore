using Core.Models;
using Microsoft.AspNetCore.Mvc;
using RestApiTutorial.Services;

namespace RestApiTutorial.Controllers;

[ApiController()]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private ITicketService _tickets;

    public TicketController(ITicketService tickets)
    {
        _tickets = tickets;
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _tickets.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Ticket? ticket = await _tickets.GetById(id);
        if (ticket is null)
            return NotFound();

        return Ok(ticket);
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] Ticket ticket)
    {
        if (await _tickets.Create(ticket))
        {
            return CreatedAtAction(nameof(GetAll),
                new { id = ticket.TicketId },
                ticket
            );
        }
        else
            return UnprocessableEntity($"Project with id {ticket.ProjectId} does not exist");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Ticket ticket)
    {
        if (id != ticket.TicketId)
            return UnprocessableEntity("Id doesn't match");

        if (!await _tickets.Update(ticket))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _tickets.Delete(id))
            return NotFound();

        return NoContent();
    }

}
