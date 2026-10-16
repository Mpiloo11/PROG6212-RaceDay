using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace RaceDay.Api;
public sealed class RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : DbContext(options)
{
    public DbSet<UserAccount> Users => Set<UserAccount>(); public DbSet<EventType> EventTypes => Set<EventType>(); public DbSet<RaceEvent> Events => Set<RaceEvent>(); public DbSet<Category> Categories => Set<Category>(); public DbSet<Enrolment> Enrolments => Set<Enrolment>(); public DbSet<RaceResult> Results => Set<RaceResult>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<UserAccount>().HasIndex(x => x.Email).IsUnique();
        b.Entity<RaceEvent>().HasOne(x => x.Organiser).WithMany().HasForeignKey(x => x.OrganiserId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<RaceEvent>().HasOne(x => x.EventType).WithMany(x => x.Events).HasForeignKey(x => x.EventTypeId);
        b.Entity<Category>().HasOne(x => x.RaceEvent).WithMany(x => x.Categories).HasForeignKey(x => x.RaceEventId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<Enrolment>().HasIndex(x => new { x.ParticipantId, x.RaceEventId }).IsUnique();
        b.Entity<Enrolment>().HasOne(x => x.Participant).WithMany().HasForeignKey(x => x.ParticipantId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Enrolment>().HasOne(x => x.RaceEvent).WithMany().HasForeignKey(x => x.RaceEventId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<RaceResult>().HasIndex(x => x.EnrolmentId).IsUnique();
        b.Entity<RaceResult>().HasOne(x => x.Enrolment).WithMany().HasForeignKey(x => x.EnrolmentId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<EventType>().HasData(new EventType { Id = 1, Name = "Run" }, new EventType { Id = 2, Name = "Walk" }, new EventType { Id = 3, Name = "Cycle" });
    }
}
