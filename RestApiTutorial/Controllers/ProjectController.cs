using Core.Models;
using Microsoft.AspNetCore.Mvc;
using RestApiTutorial.Services;

namespace RestApiTutorial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{

    private IProjectService _projectService;
    private ITicketService _ticketService;

    public ProjectController(IProjectService projectService, ITicketService ticketService)
    {
        _projectService = projectService;
        _ticketService = ticketService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _projectService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Project? project = await _projectService.GetById(id);
        if (project is null)
            return NotFound();

        return Ok(project);
    }

    [HttpGet("{pid}/tickets")]
    public async Task<IActionResult> GetProjectTickets(int pId, [FromQuery] int? tId)
    {
        if (tId is null)
        {
            List<Ticket> tickets = await _ticketService.GetTicketsInProject(pId);
            if (!tickets.Any())
            {
                if (await _projectService.GetById(pId) is null)
                    return NotFound();
            }

            return Ok(tickets);
        }

        return Ok($"getting ticket {tId} of project {pId}");
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        await _projectService.Add(project);

        return CreatedAtAction(nameof(GetById), new { id = project.ProjectId }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Project project)
    {
        if (id != project.ProjectId)
            return UnprocessableEntity("Id doesn't match");

        if (await _projectService.Update(project))
            return NoContent();
        else
            return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _projectService.Delete(id))
            return NoContent();
        else
            return NotFound();
    }


}
