using AutoMapper;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using RestApiTutorial.DTOs;
using RestApiTutorial.Services;

namespace RestApiTutorial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{

    private IProjectService _projectService;
    private ITicketService _ticketService;
    private IMapper _mapper;

    public ProjectController(IProjectService projectService, ITicketService ticketService, IMapper mapper)
    {
        _projectService = projectService;
        _ticketService = ticketService;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        List<Project> projects = await _projectService.GetAll();
        return Ok(projects.Select(p => _mapper.Map<ProjectDto>(p)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Project? project = await _projectService.GetById(id);
        if (project is null)
            return NotFound();

        return Ok(_mapper.Map<ProjectDto>(project));
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

            return Ok(tickets.Select(t => _mapper.Map<TicketDto>(t)));
        }

        return Ok($"getting ticket {tId} of project {pId}");
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
    {
        Project project = await _projectService.Add(createProjectDto);

        return CreatedAtAction(nameof(GetById), new { id = createProjectDto },
            _mapper.Map<ProjectDto>(project));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectDto projectDto)
    {
        if (id != projectDto.ProjectId)
            return UnprocessableEntity("Id doesn't match");

        if (await _projectService.Update(projectDto))
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
