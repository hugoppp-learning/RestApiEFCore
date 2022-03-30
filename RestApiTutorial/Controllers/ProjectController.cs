using AutoMapper;
using DataStore.EF;
using DataStore.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiTutorial.DTOs;

namespace RestApiTutorial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private MyContext db;
    private readonly IMapper _mapper;

    public ProjectController(MyContext db, IMapper mapper)
    {
        this.db = db;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        List<Project> projects = await db.Projects.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Project? project = await db.Projects.FindAsync(id);
        if (project is null)
            return NotFound();

        return Ok(_mapper.Map<ProjectDto>(project));
    }

    [HttpGet("{pid}/tickets")]
    public async Task<IActionResult> GetProjectTickets(int pId, [FromQuery] int? tId)
    {
        if (tId is null)
        {
            List<Ticket> tickets = await db.Tickets.Where(t => t.ProjectId == pId).ToListAsync();
            if (!tickets.Any())
            {
                if (db.Projects.Find(pId) is null)
                    return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<TicketDto>>(tickets));
        }

        return Ok($"getting ticket {tId} of project {pId}");
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
    {
        Project project = _mapper.Map<Project>(createProjectDto);
        db.Projects.Add(project);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = project.ProjectId }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectDto projectDto)
    {
        if (id != projectDto.ProjectId)
            return UnprocessableEntity("Id doesn't match");

        Project project = _mapper.Map<Project>(projectDto);
        db.Entry(project).State = EntityState.Modified;
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
        Project? project = db.Projects.Find(id);
        if (project is null)
            return NotFound();

        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return NoContent();
    }


}
