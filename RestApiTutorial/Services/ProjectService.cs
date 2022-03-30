using AutoMapper;
using DataStore.EF;
using Microsoft.EntityFrameworkCore;
using RestApiTutorial.DTOs;

namespace RestApiTutorial.Services;

public interface IProjectService
{
    Task<List<Project>> GetAll();
    ValueTask<Project?> GetById(int id);
    Task<Project> Add(CreateProjectDto createProjectDto);
    Task<bool> Update(ProjectDto projectDto);
    Task<bool> Delete(int id);
}

public class ProjectService : IProjectService
{

    private MyContext db;
    private IMapper _mapper;

    public ProjectService(MyContext db, IMapper mapper)
    {
        this.db = db;
        _mapper = mapper;
    }

    public Task<List<Project>> GetAll()
    {
        return db.Projects.ToListAsync();
    }

    public ValueTask<Project?> GetById(int id)
    {
        return db.Projects.FindAsync(id);
    }

    public async Task<Project> Add(CreateProjectDto createProjectDto)
    {
        var project = _mapper.Map<Project>(createProjectDto);
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return project;
    }

    public async Task<bool> Update(ProjectDto projectDto)
    {
        var project = _mapper.Map<Project>(projectDto);
        db.Entry(project).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            if (db.Projects.Find(project.ProjectId) == null)
                return false;

            throw;
        }
    }

    public async Task<bool> Delete(int id)
    {
        Project? project = db.Projects.Find(id);
        if (project is null)
            return false;

        db.Projects.Remove(project);
        await db.SaveChangesAsync();

        return true;
    }


}
