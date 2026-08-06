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
 
    public DbSet<ConnectionAttempt> ConnectionAttempts { get; set; }
    
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
            .OnDelete(DeleteBehavior.Restrict);

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
            .OnDelete(DeleteBehavior.Restrict);

        // ============================
        // SEED CASES
        // ============================
        // ============================
        // SEED CASE 5: THE LAST JEEPNEY TO MALATE
        // ============================
        builder.Entity<Case>().HasData(
            new Case
            {
                Id = 5,
                Title = "The Last Jeepney to Malate",
                Description = "Jeepney operator Ramon \"Mon\" Padilla is found dead behind his terminal office in Malate, struck once with a tire iron. No forced entry, no witnesses to the act itself. His ledger of quiet arrangements colorum payments, a personal loan, and a land sale he was hiding from his partners gives half the neighborhood a reason to want him gone. Only one of them was actually there.",
                Difficulty = CaseDifficulty.Hard,
                IsPublished = true,
                VictimName = "Ramon \"Mon\" Padilla",
                VictimOccupation = "Jeepney Operator",
                Location = "Malate, Manila"
            }
        );

        // ============================
        // SEED EVIDENCE (Case 5)
        // ============================
        builder.Entity<Evidence>().HasData(
            new Evidence
            {
                Id = 34,
                CaseId = 5,
                Name = "Tire Iron",
                Description = "The murder weapon, wiped clean of prints. A faint smear on the handle doesn't match anything at the terminal it tests as floor wax, the kind used at the barangay hall.",
                ImageUrl = "/images/case_1/evidence/tire_iron.jpg"
            },
            new Evidence
            {
                Id = 35,
                CaseId = 5,
                Name = "Burnt Ledger Page",
                Description = "Most of Mon's ledger was burned, but a carbon-copy undersheet survived. One legible line reads '...facilitation fee RV...' before the char damage cuts it off.",
                ImageUrl = "/images/case_1/evidence/burnt_ledger.jpg"
            },
            new Evidence
            {
                Id = 36,
                CaseId = 5,
                Name = "Sari-Sari Store CCTV Still",
                Description = "A grainy frame from the store across the street, timestamped 11:20 PM. A figure in a barong-adjacent shirt common among barangay officials passes near the terminal. The face isn't visible.",
                ImageUrl = "/images/case_1/evidence/cctv_still.jpg"
            },
            new Evidence
            {
                Id = 37,
                CaseId = 5,
                Name = "Dindo's Payment Envelope",
                Description = "Found in Mon's desk drawer. Dindo's name and a partial amount are written on the front proof he was there, but nothing more.",
                ImageUrl = "/images/case_1/evidence/payment_envelope.jpg"
            },
            new Evidence
            {
                Id = 38,
                CaseId = 5,
                Name = "Alley Timestamp Photo",
                Description = "A neighbor's unrelated Facebook photo, taken in the alley behind the carinderia at 11:25 PM. Elena is visible in the background, taking out trash.",
                ImageUrl = "/images/case_1/evidence/alley_photo.jpg"
            },
            new Evidence
            {
                Id = 39,
                CaseId = 5,
                Name = "Text Exchange with Ferdie",
                Description = "A testy back-and-forth on Mon's phone about the renegotiated lot price. The exchange ends at 9:50 PM too early to place Ferdie at the scene.",
                ImageUrl = "/images/case_1/evidence/text_exchange.jpg"
            },
            new Evidence
            {
                Id = 40,
                CaseId = 5,
                Name = "Barangay Hall Logbook",
                Description = "Kap. Rey signed out at 10:50 PM for 'patrol.' There's no sign-back-in time until well after midnight an unaccounted gap that covers the time of the murder.",
                ImageUrl = "/images/case_1/evidence/barangay_logbook.jpg"
            },
            new Evidence
            {
                Id = 41,
                CaseId = 5,
                Name = "Partial Shoeprint",
                Description = "A faint rubber-sandal print in the dust near the body. Common brand, not unique on its own but it carries the same floor wax residue found on the tire iron.",
                ImageUrl = "/images/case_1/evidence/shoeprint.jpg"
            },
            new Evidence
            {
                Id = 42,
                CaseId = 5,
                Name = "Hospital Shift Log",
                Description = "A time-stamped nursing shift record placing Grace at a Quezon City hospital from before 9 PM until well past midnight. Locked-in, third-party verified.",
                ImageUrl = "/images/case_1/evidence/hospital_log.jpg"
            }
        );

        // ============================
        // SEED SUSPECTS (Case 5)
        // ============================
        builder.Entity<Suspect>().HasData(
            new Suspect
            {
                Id = 23,
                CaseId = 5,
                Name = "Elena Padilla",
                Description = "Mon's estranged wife, 48. Runs a small carinderia two blocks from the terminal.",
                Motive = "Mon was finalizing the terminal lot sale without telling her she'd get nothing from it despite twenty years of marriage; the annulment was never finalized.",
                Alibi = "Says she was closing up the carinderia until past midnight, with her helper Baby there the whole time preparing for the next day. Mentions a gas delivery Baby signed for around 11 PM.",
                ImageUrl = "/images/case_1/suspects/elena_padilla.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 24,
                CaseId = 5,
                Name = "Dindo Reyes",
                Description = "One of Mon's four drivers, 34. Deep in personal debt to Mon, on top of his fleet obligations.",
                Motive = "Owed Mon \u20b145,000 personally; Mon had started deducting it aggressively from his daily boundary, leaving little for his family.",
                Alibi = "Says he dropped his jeep off at the terminal around 11:15 PM and left immediately the terminal was already dark, so he never went inside, just left his keys in the drop box.",
                ImageUrl = "/images/case_1/suspects/dindo_reyes.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 25,
                CaseId = 5,
                Name = "Architect Ferdie Bautista",
                Description = "Local representative for the developer negotiating to buy the terminal lot, 45.",
                Motive = "The sale was stalling after Mon demanded a higher price at the last minute. A collapsed deal would cost Ferdie his commission and standing with the developer.",
                Alibi = "Says he was at a client dinner in Makati until 10:30 PM, then went straight home to Para\u00f1aque naming the restaurant and his dinner companion without hesitation.",
                ImageUrl = "/images/case_1/suspects/ferdie_bautista.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 26,
                CaseId = 5,
                Name = "Grace Padilla-Sison",
                Description = "Mon's estranged daughter from a prior relationship, 26. Works as a nurse, rarely spoke to her father.",
                Motive = "Recently reconnected with Mon, who reportedly intended to leave her a share of the terminal lot cutting into what Elena or the drivers might expect.",
                Alibi = "Says she was on shift at a hospital in Quezon City until past midnight.",
                ImageUrl = "/images/case_1/suspects/grace_sison.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 27,
                CaseId = 5,
                Name = "Kap. Rey Villamor",
                Description = "Barangay captain, 58. Has known Mon for thirty years and quietly brokered the terminal lot deal for a personal cut.",
                Motive = "Stood to lose a facilitation fee he'd already spent if Mon backed out of the sale after deciding to cut Grace in and shrink Rey's share.",
                Alibi = "Says he was doing late paperwork alone at the barangay hall no one to corroborate it, but insists the logbook proves he was there.",
                ImageUrl = "/images/case_1/suspects/rey_villamor.jpg",
                IsGuilty = true
            }
        );

        // ============================
        // SEED WITNESSES (Case 5)
        // ============================
        builder.Entity<Witness>().HasData(
            new Witness
            {
                Id = 19,
                CaseId = 5,
                Name = "Baby (Carinderia Helper)",
                Statement = "Naglagay pa kami ng sinigang mix para bukas, tapos may dumating na gas delivery mga alas-onse. Si Aling Elena, saglit lang siyang lumabas maglalabas ng basura sa eskinita. Hindi naman tumagal, ten minutes lang siguro.",
                ImageUrl = "/images/case_1/witnesses/baby.jpg"
            },
            new Witness
            {
                Id = 20,
                CaseId = 5,
                Name = "Aling Nena (Sari-Sari Store Owner)",
                Statement = "May CCTV kami dun sa labas, nakuhanan namin ng lumabas na parang naka-barong sa may terminal. Hindi ko masyadong nakita ang mukha. Alam mo, madalas naman dumaan si Kapitan dun kapag 'patrol' daw niya.",
                ImageUrl = "/images/case_1/witnesses/aling_nena.jpg"
            },
            new Witness
            {
                Id = 21,
                CaseId = 5,
                Name = "Jun (Fellow Driver)",
                Statement = "Bumalik si Dindo sa terminal after niya i-drop yung huling byahe niya. May ibinigay siyang envelope, sabi niya kay Mon daw. Mabilis lang siya, umalis din agad.",
                ImageUrl = "/images/case_1/witnesses/jun.jpg"
            },
            new Witness
            {
                Id = 22,
                CaseId = 5,
                Name = "Ferdie's Dinner Companion",
                Statement = "We were at the restaurant together until around 10:30 he left straight after, said he was heading home to Para\u00f1aque. I remember because he complained about the traffic before he even got in his car.",
                ImageUrl = "/images/case_1/witnesses/dinner_companion.jpg"
            }
        );

        // ============================
        // SEED CASE CONNECTIONS (ANSWER KEY) - Case 5
        // ============================
        builder.Entity<CaseConnection>().HasData(
            // Ties the murder weapon and physical trace to Rey
            new CaseConnection
            {
                Id = 17,
                CaseId = 5,
                FromType = "Evidence",
                FromId = 34, // Tire Iron
                ToType = "Suspect",
                ToId = 27,   // Rey
                Note = "The floor wax on the tire iron isn't from the terminal. It's from the barangay hall."
            },
            new CaseConnection
            {
                Id = 18,
                CaseId = 5,
                FromType = "Evidence",
                FromId = 35, // Burnt Ledger
                ToType = "Suspect",
                ToId = 27,   // Rey
                Note = "\"RV.\" The initials on the surviving carbon copy aren't a coincidence."
            },
            new CaseConnection
            {
                Id = 19,
                CaseId = 5,
                FromType = "Witness",
                FromId = 20, // Aling Nena
                ToType = "Suspect",
                ToId = 27,   // Rey
                Note = "The barong-shirt figure on Nena's CCTV walks Rey's usual patrol route."
            },
            new CaseConnection
            {
                Id = 20,
                CaseId = 5,
                FromType = "Evidence",
                FromId = 40, // Barangay Logbook
                ToType = "Suspect",
                ToId = 27,   // Rey
                Note = "No sign-in time. Whatever Rey was doing during that gap, it wasn't logged and it wasn't at the hall."
            },
            new CaseConnection
            {
                Id = 21,
                CaseId = 5,
                FromType = "Evidence",
                FromId = 41, // Shoeprint
                ToType = "Suspect",
                ToId = 27,   // Rey
                Note = "Same wax residue as the weapon. Same place, same night."
            },

            // Clearing connections innocent suspects
            new CaseConnection
            {
                Id = 22,
                CaseId = 5,
                FromType = "Witness",
                FromId = 19, // Baby
                ToType = "Suspect",
                ToId = 23,   // Elena
                Note = "Baby's account accounts for nearly all of Elena's time. The alley gap was real, but far too short."
            },
            new CaseConnection
            {
                Id = 23,
                CaseId = 5,
                FromType = "Witness",
                FromId = 22, // Ferdie's dinner companion
                ToType = "Suspect",
                ToId = 25,   // Ferdie
                Note = "The dinner alibi holds. Ferdie was nowhere near Malate that night."
            },
            new CaseConnection
            {
                Id = 24,
                CaseId = 5,
                FromType = "Witness",
                FromId = 21, // Jun
                ToType = "Suspect",
                ToId = 24,   // Dindo
                Note = "Dindo did go inside but only to leave a payment. He found nothing but an empty office and ran."
            },
            new CaseConnection
            {
                Id = 25,
                CaseId = 5,
                FromType = "Evidence",
                FromId = 42, // Hospital Shift Log
                ToType = "Suspect",
                ToId = 26,   // Grace
                Note = "Grace's shift log is locked in, third-party verified. She was never near Malate that night."
            }
        );
    }
}