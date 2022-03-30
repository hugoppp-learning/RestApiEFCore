using AutoMapper;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using RestApiTutorial.DTOs;
using RestApiTutorial.Services;

namespace RestApiTutorial.Controllers;

[ApiController()]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private ITicketService _tickets;
    private readonly IMapper _mapper;

    public TicketController(ITicketService tickets, IMapper mapper)
    {
        _tickets = tickets;
        _mapper = mapper;
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok((await _tickets.GetAll()).Select(t => _mapper.Map<TicketDto>(t)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Ticket? ticket = await _tickets.GetById(id);
        if (ticket is null)
            return NotFound();

        return Ok(_mapper.Map<TicketDto>(ticket));
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateTicketDto createTicketDto)
    {
        Ticket ticket = await _tickets.Create(createTicketDto);
        return CreatedAtAction(nameof(GetAll),
            new { id = ticket.TicketId },
            _mapper.Map<TicketDto>(ticket)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticket)
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
