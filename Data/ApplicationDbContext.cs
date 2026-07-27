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
    public DbSet<ClueConnection> ClueConnections { get; set; }
    public DbSet<CaseConnection> CaseConnections { get; set; }
    public DbSet<Accusation> Accusations { get; set; }
    public DbSet<SuspectElimination> SuspectEliminations { get; set; }

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
                Description = "A single red rose found on the dressing room floor. The stem is freshly cut, not wilted — someone left it here recently.",
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
            },
            new Evidence
            {
                Id = 4,
                CaseId = 1,
                Name = "Insurance Ledger Page",
                Description = "A photocopied ledger page with several transactions circled and the word 'AUDIT' underlined twice. Found stuffed in a lobby trash can.",
                ImageUrl = "/images/evidence/ledger.jpg"
            },
            new Evidence
            {
                Id = 5,
                CaseId = 1,
                Name = "Handwritten Note",
                Description = "A crumpled note reading: 'We need to talk. I'm not done fighting for us.' No signature, but the handwriting is bold and impatient.",
                ImageUrl = "/images/evidence/note.jpg"
            },
            new Evidence
            {
                Id = 6,
                CaseId = 1,
                Name = "Voicemail Transcript",
                Description = "A printed transcript of a voicemail: 'You can't just cut me out after everything I've done for you. We'll see about that.' Timestamp: three days before the disappearance.",
                ImageUrl = "/images/evidence/voicemail.jpg"
            },
            new Evidence
            {
                Id = 7,
                CaseId = 1,
                Name = "Backstage Security Log",
                Description = "A printout of badge-scan entries. One badge — registered to Elena Vasquez — scanned into the dressing room corridor at 7:38 PM, twenty minutes before the violinist was last seen.",
                ImageUrl = "/images/evidence/security_log.jpg"
            }
        );

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
            },
            new Suspect
            {
                Id = 3,
                CaseId = 1,
                Name = "Victor Hale",
                Description = "The concert hall's owner and producer. Smooth talker, expensive suit, always working the room.",
                Motive = "The violinist had discovered irregularities in the hall's finances and threatened to go public before the show.",
                Alibi = "Says he was greeting donors in the lobby all evening — mostly true, but he was unaccounted for a 15-minute window.",
                ImageUrl = "/images/suspects/victor.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 4,
                CaseId = 1,
                Name = "Sophia Reyes",
                Description = "The violinist's longtime personal assistant. Quiet, meticulous, always one step behind her.",
                Motive = "Learned two days ago that she'd been written out of the violinist's will after years of unpaid overtime.",
                Alibi = "Claims she was home sick — but no one can confirm it, and her phone was off all night.",
                ImageUrl = "/images/suspects/sophia.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 5,
                CaseId = 1,
                Name = "Damian Cole",
                Description = "The violinist's ex-fiancé. Showed up uninvited backstage an hour before the show.",
                Motive = "Still bitter over their public breakup; was overheard arguing with her in the hallway.",
                Alibi = "Says he was at the bar the whole time — bartender remembers seeing him, but not exactly when he left.",
                ImageUrl = "/images/suspects/damian.jpg",
                IsGuilty = false
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
            },
            new Witness
            {
                Id = 3,
                CaseId = 1,
                Name = "Sound Technician",
                Statement = "I overheard Victor Hale on the phone saying something like 'we need to make the numbers disappear before anyone audits this.' Didn't think much of it at the time.",
                ImageUrl = "/images/witnesses/sound_tech.jpg"
            },
            new Witness
            {
                Id = 4,
                CaseId = 1,
                Name = "Valet",
                Statement = "Damian Cole's car peeled out of the lot around 9 PM, tires screeching. Looked like he was in a real hurry to leave.",
                ImageUrl = "/images/witnesses/valet.jpg"
            }
        );

        // ClueConnection relationships
        builder.Entity<ClueConnection>()
            .HasOne(cc => cc.ApplicationUser)
            .WithMany()
            .HasForeignKey(cc => cc.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ClueConnection>()
            .HasOne(cc => cc.Case)
            .WithMany(c => c.ClueConnections)
            .HasForeignKey(cc => cc.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CaseConnection>()
            .HasOne(cc => cc.Case)
            .WithMany(c => c.CaseConnections)
            .HasForeignKey(cc => cc.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed the "answer key" for Case 1 (The Vanishing Violinist)
        builder.Entity<CaseConnection>().HasData(
            new CaseConnection { Id = 1, CaseId = 1, FromType = "Evidence", FromId = 2, ToType = "Suspect", ToId = 2 },
            new CaseConnection { Id = 2, CaseId = 1, FromType = "Witness", FromId = 2, ToType = "Suspect", ToId = 2 },
            new CaseConnection { Id = 3, CaseId = 1, FromType = "Evidence", FromId = 7, ToType = "Suspect", ToId = 2 }
        );

        // Accusation relationships
        builder.Entity<Accusation>()
            .HasOne(a => a.ApplicationUser)
            .WithMany()
            .HasForeignKey(a => a.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Accusation>()
            .HasOne(a => a.Case)
            .WithMany(c => c.Accusations)
            .HasForeignKey(a => a.CaseId)
            .OnDelete(DeleteBehavior.Restrict); // avoid multiple cascade paths

        builder.Entity<Accusation>()
            .HasOne(a => a.AccusedSuspect)
            .WithMany()
            .HasForeignKey(a => a.AccusedSuspectId)
            .OnDelete(DeleteBehavior.Restrict);

        // SuspectElimination relationships
        builder.Entity<SuspectElimination>()
            .HasOne(se => se.ApplicationUser)
            .WithMany()
            .HasForeignKey(se => se.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SuspectElimination>()
            .HasOne(se => se.Case)
            .WithMany(c => c.SuspectEliminations)
            .HasForeignKey(se => se.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SuspectElimination>()
            .HasOne(se => se.Suspect)
            .WithMany()
            .HasForeignKey(se => se.SuspectId)
            .OnDelete(DeleteBehavior.Restrict); // avoid multiple cascade paths
    }
}