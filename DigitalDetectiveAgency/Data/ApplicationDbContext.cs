using DigitalDetectiveAgency.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Case> Cases { get; set; }
    public DbSet<PlayerCase> PlayerCases { get; set; }
    public DbSet<Evidence> EvidenceItems { get; set; }
    public DbSet<Suspect> Suspects { get; set; }
    public DbSet<Witness> Witnesses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // PlayerCase relationships
        builder.Entity<PlayerCase>()
            .HasOne(pc => pc.ApplicationUser)
            .WithMany()
            .HasForeignKey(pc => pc.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PlayerCase>()
            .HasOne(pc => pc.Case)
            .WithMany(c => c.PlayerCases)
            .HasForeignKey(pc => pc.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Evidence relationship
        builder.Entity<Evidence>()
            .HasOne(e => e.Case)
            .WithMany(c => c.EvidenceItems)
            .HasForeignKey(e => e.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Suspect relationship
        builder.Entity<Suspect>()
            .HasOne(s => s.Case)
            .WithMany(c => c.Suspects)
            .HasForeignKey(s => s.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Witness relationship
        builder.Entity<Witness>()
            .HasOne(w => w.Case)
            .WithMany(c => c.Witnesses)
            .HasForeignKey(w => w.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Cases
        builder.Entity<Case>().HasData(
            new Case
            {
                Id = 1,
                Title = "The Vanishing Violinist",
                Description = "A world-renowned violinist disappears moments before a sold-out concert. No signs of forced entry. No witnesses. Just an empty dressing room and a single dropped rose.",
                Difficulty = CaseDifficulty.Easy,
                IsPublished = true
            },
            new Case
            {
                Id = 2,
                Title = "Silence at Blackwood Manor",
                Description = "A wealthy family's annual reunion ends in tragedy when the patriarch is found dead in the locked study. Everyone had a motive. No one has an alibi.",
                Difficulty = CaseDifficulty.Medium,
                IsPublished = true
            }
        );

        // Seed Evidence
        builder.Entity<Evidence>().HasData(
            new Evidence
            {
                Id = 1,
                CaseId = 1,
                Name = "Dropped Rose",
                Description = "A single red rose found on the dressing room floor. The stem is freshly cut, not wilted — suggesting it was placed here recently, possibly during the disappearance itself.",
                ImageUrl = "/images/evidence/rose.jpg"
            },
            new Evidence
            {
                Id = 2,
                CaseId = 1,
                Name = "Torn Concert Program",
                Description = "A concert program torn roughly in half. The violinist's name has been circled in red ink, and a small phone number is scrawled in the margin.",
                ImageUrl = "/images/evidence/torn_program.jpg"
            },
            new Evidence
            {
                Id = 3,
                CaseId = 2,
                Name = "Broken Pocket Watch",
                Description = "An antique pocket watch, stopped at 9:47 PM. The glass is cracked, and the inside cover is engraved with initials that don't match anyone in the household.",
                ImageUrl = "/images/evidence/pocket_watch.jpg"
            }
        ); // <--- Added missing ");" here

        // Seed Suspects
        builder.Entity<Suspect>().HasData(
            new Suspect
            {
                Id = 1,
                CaseId = 1,
                Name = "Marcus Feld",
                Description = "The concert hall's stage manager. Nervous, evasive, avoids eye contact.",
                Motive = "Was recently fired by the violinist for stealing sheet music royalties.",
                Alibi = "Claims he was in the control booth the entire evening.",
                ImageUrl = "/images/suspects/marcus.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 2,
                CaseId = 1,
                Name = "Elena Vasquez",
                Description = "A rival violinist who lost the lead position to the victim last season.",
                Motive = "Publicly humiliated when passed over for the solo spot.",
                Alibi = "Says she was at a nearby cafe, but no one can confirm it.",
                ImageUrl = "/images/suspects/elena.jpg",
                IsGuilty = true
            }
        );

        // Seed Witnesses
        builder.Entity<Witness>().HasData(
            new Witness
            {
                Id = 1,
                CaseId = 1,
                Name = "Backstage Janitor",
                Statement = "I saw someone in a dark coat near the dressing rooms around 7:40 PM, but I didn't get a good look at their face.",
                ImageUrl = "/images/witnesses/janitor.jpg"
            },
            new Witness
            {
                Id = 2,
                CaseId = 1,
                Name = "Ticket Booth Clerk",
                Statement = "A woman matching Elena's description left in a hurry just before intermission. She seemed upset.",
                ImageUrl = "/images/witnesses/clerk.jpg"
            }
        );
    } // <--- Added missing closing brace for OnModelCreating
}