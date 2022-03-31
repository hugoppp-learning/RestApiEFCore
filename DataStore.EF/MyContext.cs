using DataStore.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.EF;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }

    public MyContext()
    {
    }

    public DbSet<Project> Projects { get; set; } = null!;

    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .HasKey(ticket => new { ticket.ProjectId, ticket.TicketId });

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tickets)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Title = "project 1" },
            new Project { ProjectId = 2, Title = "project 2" }
        );

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket { ProjectId = 1, TicketId = 1, Title = "ticket 1", Description = "Lorem" },
            new Ticket { ProjectId = 1, TicketId = 2, Title = "ticket 2", Description = "ipsum" },
            new Ticket { ProjectId = 1, TicketId = 3, Title = "ticket 3" },
            new Ticket { ProjectId = 2, TicketId = 1, Title = "ticket 4" },
            new Ticket { ProjectId = 2, TicketId = 2, Title = "ticket 5" }
        );
    }
}
