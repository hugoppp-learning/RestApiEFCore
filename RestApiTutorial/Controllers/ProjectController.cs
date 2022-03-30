using DataStore.EF;
using DataStore.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestApiTutorial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private MyContext db;

    public ProjectController(MyContext db)
    {
        this.db = db;
    }

    [HttpGet("all")]
    public async Task<IActionResult> Get()
    {
        return Ok(await db.Projects.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Project? project = await db.Projects.FindAsync(id);
        if (project is null)
            return NotFound();

        return Ok(project);
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

            return Ok(tickets);
        }

        return Ok($"getting ticket {tId} of project {pId}");
    }


    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = project.ProjectId }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Project project)
    {
        if (id != project.ProjectId)
            return UnprocessableEntity("Id doesn't match");

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
