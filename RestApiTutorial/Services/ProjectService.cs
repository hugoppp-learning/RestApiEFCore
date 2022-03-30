using Core.Models;
using DataStore.EF;
using Microsoft.EntityFrameworkCore;

namespace RestApiTutorial.Services;

public interface IProjectService
{
    Task<List<Project>> GetAll();
    ValueTask<Project?> GetById(int id);
    Task Add(Project project);
    Task<bool> Update(Project project);
    Task<bool> Delete(int id);
}

public class ProjectService : IProjectService
{

    private MyContext db;

    public ProjectService(MyContext db)
    {
        this.db = db;
    }

    public Task<List<Project>> GetAll()
    {
        return db.Projects.ToListAsync();
    }

    public ValueTask<Project?> GetById(int id)
    {
        return db.Projects.FindAsync(id);
    }

    public async Task Add(Project project)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync();
    }

    public async Task<bool> Update(Project project)
    {
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
