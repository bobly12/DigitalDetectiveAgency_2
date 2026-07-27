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
            },
            new Case
            {
                Id = 3,
                Title = "The Last Harvest at Hacienda Malinao",
                Description = "The night of the town fiesta, sugar baron Ernesto Malinao is found dead in his study at the family's ancestral hacienda in Negros Occidental — hours before he was set to sign the papers selling generations of tenant land to a foreign conglomerate. The house was full of relatives, guests, and household staff. Everyone was watching the fireworks. No one saw a thing.",
                Difficulty = CaseDifficulty.Hard,
                IsPublished = true
            },
            new Case
            {
                Id = 4,
                Title = "Undertow at Puerto Galera",
                Description = "The night before dive resort owner Ramon Aguilar was set to unveil a controversial expansion into Talipanan Cove — land the local fisherfolk have worked for generations and the Mangyan community considers ancestral ground — he's found dead at the bottom of his own boat dock. The town had gathered for the resort's investor showcase. Protesters had gathered outside the gate for weeks. Everyone had a reason to want the expansion stopped. Only one person had a reason to stop Ramon himself.",
                Difficulty = CaseDifficulty.Hard,
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
            },
            new Evidence
            {
                Id = 8,
                CaseId = 3,
                Name = "Bloodied Bolo",
                Description = "A common cane-cutting bolo, found half-buried near the chapel path. Nearly every household on the hacienda owns one identical to it.",
                ImageUrl = "/images/evidence/bolo.jpg"
            },
            new Evidence
            {
                Id = 9,
                CaseId = 3,
                Name = "Torn Land Sale Contract",
                Description = "A single page torn from the sale agreement, the tenant-eviction clause circled hard enough to tear the paper. Found tucked behind the santo on the chapel altar.",
                ImageUrl = "/images/evidence/contract_page.jpg"
            },
            new Evidence
            {
                Id = 10,
                CaseId = 3,
                Name = "Ledger of Unpaid Sacada Wages",
                Description = "A handwritten ledger tallying five months of unpaid wages owed to the cane workers, found in a nipa hut at the edge of the fields — the entries stop the day Ernesto died.",
                ImageUrl = "/images/evidence/wage_ledger.jpg"
            },
            new Evidence
            {
                Id = 11,
                CaseId = 3,
                Name = "Threatening Note to Teresa",
                Description = "A short, cold note in Ernesto's handwriting: 'The allowance stops the day I sign. Don't make this harder than it needs to be.'",
                ImageUrl = "/images/evidence/note_teresa.jpg"
            },
            new Evidence
            {
                Id = 12,
                CaseId = 3,
                Name = "Bank Passbook with Shortages",
                Description = "An estate bank passbook with three withdrawals in Atty. Cruz's handwriting that were never logged in the official trust records.",
                ImageUrl = "/images/evidence/passbook.jpg"
            },
            new Evidence
            {
                Id = 13,
                CaseId = 3,
                Name = "Casino Debt Collector Letter",
                Description = "An overdue-payment letter from a Manila casino, addressed to Ricardo, threatening 'further action' if the balance isn't settled by month's end.",
                ImageUrl = "/images/evidence/casino_letter.jpg"
            },
            new Evidence
            {
                Id = 14,
                CaseId = 3,
                Name = "Disputed Boundary Survey",
                Description = "An old land survey with a section re-measured in Loreta's handwriting, the margin noting 'this was never his to sell alone.'",
                ImageUrl = "/images/evidence/survey.jpg"
            },
            new Evidence
            {
                Id = 15,
                CaseId = 3,
                Name = "Business Card with Scribbled Offer",
                Description = "William Tan's business card, a lower price scrawled on the back along with the words 'final — take it or we walk.'",
                ImageUrl = "/images/evidence/business_card.jpg"
            },
            new Evidence
            {
                Id = 16,
                CaseId = 4,
                Name = "Diving Knife",
                Description = "A standard dive knife, the kind every instructor and boat crew on the property carries. No initials, no distinguishing marks.",
                ImageUrl = "/images/evidence/dive_knife.jpg"
            },
            new Evidence
            {
                Id = 17,
                CaseId = 4,
                Name = "Forged Permit Application",
                Description = "An environmental clearance form for the cove expansion, the approval signature traced rather than signed — and initialed 'N.A.' in the corner margin.",
                ImageUrl = "/images/evidence/forged_permit.jpg"
            },
            new Evidence
            {
                Id = 18,
                CaseId = 4,
                Name = "Diverted Maintenance Fund Ledger",
                Description = "A second, unofficial ledger found taped beneath the office desk drawer, tracking three years of maintenance budget quietly redirected to a personal account.",
                ImageUrl = "/images/evidence/fund_ledger.jpg"
            },
            new Evidence
            {
                Id = 19,
                CaseId = 4,
                Name = "Life Insurance Policy",
                Description = "A policy naming Cristina as sole beneficiary, dated two months after the wedding — and never disclosed to Ramon's children from his first marriage.",
                ImageUrl = "/images/evidence/insurance_policy.jpg"
            },
            new Evidence
            {
                Id = 20,
                CaseId = 4,
                Name = "Bribery Cash Envelope",
                Description = "An envelope of cash found in Mayor Villaruz's car, a sticky note attached reading 'para sa fast-track, huwag na banggitin.'",
                ImageUrl = "/images/evidence/bribe_envelope.jpg"
            },
            new Evidence
            {
                Id = 21,
                CaseId = 4,
                Name = "Sabotaged Compressor Photo",
                Description = "A phone photo of the resort's dive compressor with a cut hose — taken the same week Boyet was overheard threatening to 'make sure the showcase doesn't go smoothly.'",
                ImageUrl = "/images/evidence/compressor_photo.jpg"
            },
            new Evidence
            {
                Id = 22,
                CaseId = 4,
                Name = "Protest Flyer",
                Description = "A flyer from the fisherfolk association, hand-annotated in the margin: 'kung kailangan, gagamit tayo ng ibang paraan' — if necessary, we'll use other means.",
                ImageUrl = "/images/evidence/protest_flyer.jpg"
            },
            new Evidence
            {
                Id = 23,
                CaseId = 4,
                Name = "Burnt Petition Copy",
                Description = "A half-burned copy of the Mangyan community's ancestral domain petition, found in the resort office trash bin — someone tried to make it disappear before the investors saw it.",
                ImageUrl = "/images/evidence/burnt_petition.jpg"
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
            },
            new Suspect
            {
                Id = 6,
                CaseId = 3,
                Name = "Ricardo Malinao",
                Description = "Ernesto's estranged son, back from Manila after nearly a decade of silence — arriving just three days before his father's death.",
                Motive = "Cut off financially years ago and only recently reconciled. With the land sale unsigned, he stood to inherit everything.",
                Alibi = "Says he was watching the fireworks with cousins the whole time. A few vaguely recall seeing him 'at some point' during the display.",
                ImageUrl = "/images/suspects/ricardo.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 7,
                CaseId = 3,
                Name = "Teresa Bautista",
                Description = "Ernesto's longtime companion, kept quietly in a house at the edge of town for over a decade.",
                Motive = "Ernesto was ending their arrangement to remarry once the sale went through — cutting off her monthly allowance for good.",
                Alibi = "Claims she left before the fireworks even started. The gate guard's logbook has no exit time recorded for her that night.",
                ImageUrl = "/images/suspects/teresa.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 8,
                CaseId = 3,
                Name = "Atty. Simeon Cruz",
                Description = "The family's lawyer and Ernesto's compadre of thirty years, handling the estate's finances and the pending sale.",
                Motive = "The incoming buyer's audit would have exposed years of quietly siphoned trust funds.",
                Alibi = "Says he was drafting the final contract in the guest room all night. No one else can confirm he was there the whole time.",
                ImageUrl = "/images/suspects/cruz.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 9,
                CaseId = 3,
                Name = "Loreta Malinao",
                Description = "Ernesto's older sister, who ran the hacienda's day-to-day operations for thirty years while he lived abroad.",
                Motive = "Passed over in the will in favor of Ricardo, despite three decades of unpaid work keeping the estate afloat.",
                Alibi = "Says she was arranging the fireworks display with the caterers. Staff remember her there for 'most of the evening.'",
                ImageUrl = "/images/suspects/loreta.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 10,
                CaseId = 3,
                Name = "Mang Fidel Cortez",
                Description = "The hacienda's katiwala — overseer of the cane fields for over forty years. Quiet, devout, tends the small chapel on the property himself.",
                Motive = "The sale would evict the sacada families who'd worked the land for generations — families whose wages Ernesto had already quietly stopped paying months ago.",
                Alibi = "Says he was in the chapel praying through the fireworks, as he does every fiesta. No one thought to check.",
                ImageUrl = "/images/suspects/fidel.jpg",
                IsGuilty = true
            },
            new Suspect
            {
                Id = 11,
                CaseId = 3,
                Name = "William Tan",
                Description = "Representative for the conglomerate buying the land — flew in from Manila that morning to finalize the sale.",
                Motive = "Ernesto tried to renegotiate the price at the last minute, threatening to walk away from the deal entirely.",
                Alibi = "Says he was on a call with his office in Manila during the fireworks. Phone records confirm a call, but not that he was the one on it.",
                ImageUrl = "/images/suspects/tan.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 12,
                CaseId = 4,
                Name = "Nico Aguilar",
                Description = "Ramon's nephew and the resort's general manager, trusted with the books for the past six years.",
                Motive = "An outside audit tied to the expansion loan would have surfaced years of diverted maintenance funds and forged permit paperwork — all under his signature.",
                Alibi = "Says he was doing late inventory in the resort office alone. No one thought to question it — he's always the last one to leave.",
                ImageUrl = "/images/suspects/nico.jpg",
                IsGuilty = true
            },
            new Suspect
            {
                Id = 13,
                CaseId = 4,
                Name = "Cristina Aguilar",
                Description = "Ramon's second wife, twenty years his junior, married just three years ago.",
                Motive = "Sole beneficiary on a life insurance policy Ramon took out shortly after their wedding — one his first family never knew existed.",
                Alibi = "Claims she was greeting investors at the showcase all night. Several guests confirm seeing her — but not continuously.",
                ImageUrl = "/images/suspects/cristina.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 14,
                CaseId = 4,
                Name = "Boyet Ramos",
                Description = "Owner of a rival dive resort in Sabang, and Ramon's business rival for over a decade.",
                Motive = "Ramon's expansion would have driven half of Boyet's dive tourists to the new cove, likely closing his resort within a year.",
                Alibi = "Says his boat never left Sabang that night. The marina logbook backs him up — mostly; there's a 40-minute gap in the entries.",
                ImageUrl = "/images/suspects/boyet.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 15,
                CaseId = 4,
                Name = "Mayor Teodoro Villaruz",
                Description = "The town mayor, who fast-tracked the resort's expansion permits over the objections of his own municipal planning office.",
                Motive = "Ramon was the only person who could prove the permits were approved in exchange for a cut of the investor funding.",
                Alibi = "Says he left the showcase early with a headache. His driver confirms dropping him home, but not what time he arrived.",
                ImageUrl = "/images/suspects/villaruz.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 16,
                CaseId = 4,
                Name = "Datu Bandying",
                Description = "An elder of the local Mangyan community, who has spent months formally petitioning to have the cove recognized as ancestral domain.",
                Motive = "The expansion would pave over a burial ground his community has protected for generations — and Ramon had ignored every petition.",
                Alibi = "Says he was leading a prayer gathering with his community at the edge of the property when the fireworks started. Several members confirm it.",
                ImageUrl = "/images/suspects/bandying.jpg",
                IsGuilty = false
            },
            new Suspect
            {
                Id = 17,
                CaseId = 4,
                Name = "Fernando \"Ka Pandoy\" Reyes",
                Description = "Head of the local fisherfolk association, and the loudest voice at every protest outside the resort gate for the past two months.",
                Motive = "The cove is the fishing ground his association depends on. Losing it meant losing livelihoods for over thirty families.",
                Alibi = "Says he was leading the protest chant outside the gate when the fireworks went off — dozens of witnesses can place him there.",
                ImageUrl = "/images/suspects/pandoy.jpg",
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
            },
            new Witness
            {
                Id = 5,
                CaseId = 3,
                Name = "Nena, the Kasambahay",
                Statement = "I saw someone pass by the study window right when the fireworks started, pero patalikod siya — back was turned, couldn't tell you who.",
                ImageUrl = "/images/witnesses/nena.jpg"
            },
            new Witness
            {
                Id = 6,
                CaseId = 3,
                Name = "Tricycle Driver",
                Statement = "I drove Ricardo to town earlier that day. He was on the phone the whole ride, arguing with someone about a debt — sounded serious.",
                ImageUrl = "/images/witnesses/tricycle_driver.jpg"
            },
            new Witness
            {
                Id = 7,
                CaseId = 3,
                Name = "Town Councilor",
                Statement = "I saw Atty. Cruz slip out of the fiesta early, clutching a folder like his life depended on it. Didn't even say goodbye to anyone.",
                ImageUrl = "/images/witnesses/councilor.jpg"
            },
            new Witness
            {
                Id = 8,
                CaseId = 3,
                Name = "Sacada Farmhand",
                Statement = "Nakita ko si Mang Fidel walking from the chapel toward the main house right before the fireworks ended, may dalang something wrapped in cloth. Sabi niya nagdadasal lang daw siya kanina, pero hindi galing sa kapilya papunta kundi papunta sa bahay.",
                ImageUrl = "/images/witnesses/farmhand.jpg"
            },
            new Witness
            {
                Id = 9,
                CaseId = 3,
                Name = "Ernesto's Niece",
                Statement = "I saw Tita Teresa and Tito Ernesto arguing near the garden earlier that evening, before the fireworks even started. She looked like she'd been crying.",
                ImageUrl = "/images/witnesses/niece.jpg"
            },
            new Witness
            {
                Id = 10,
                CaseId = 4,
                Name = "Bangkero (Boat Captain)",
                Statement = "Merong nakita akong tao malapit sa dive shop that night, pero madilim — hindi ko masabi kung sino.",
                ImageUrl = "/images/witnesses/bangkero.jpg"
            },
            new Witness
            {
                Id = 11,
                CaseId = 4,
                Name = "Resort Front Desk Staff",
                Statement = "I overheard Cristina on the phone that night saying, 'after this, wala nang makakapigil sa 'kin.' Didn't think much of it at the time.",
                ImageUrl = "/images/witnesses/front_desk.jpg"
            },
            new Witness
            {
                Id = 12,
                CaseId = 4,
                Name = "Sabang Local",
                Statement = "I saw Boyet's boat idling near the resort's private dock that night, lights off. Left again after maybe ten minutes.",
                ImageUrl = "/images/witnesses/sabang_local.jpg"
            },
            new Witness
            {
                Id = 13,
                CaseId = 4,
                Name = "Resort Security Guard",
                Statement = "Si Nico lang ang nakita kong pumasok sa office nang mag-isa, late na late na. Sabi niya nagta-take lang daw siya ng inventory, pero nakita ko siyang may dalang shredder bag paglabas.",
                ImageUrl = "/images/witnesses/security_guard.jpg"
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

        // Seed the "answer keys" for Case 1, Case 3, and Case 4
        builder.Entity<CaseConnection>().HasData(
            new CaseConnection { Id = 1, CaseId = 1, FromType = "Evidence", FromId = 2, ToType = "Suspect", ToId = 2 },
            new CaseConnection { Id = 2, CaseId = 1, FromType = "Witness", FromId = 2, ToType = "Suspect", ToId = 2 },
            new CaseConnection { Id = 3, CaseId = 1, FromType = "Evidence", FromId = 7, ToType = "Suspect", ToId = 2 },
            new CaseConnection { Id = 4, CaseId = 3, FromType = "Evidence", FromId = 9, ToType = "Suspect", ToId = 10 },
            new CaseConnection { Id = 5, CaseId = 3, FromType = "Evidence", FromId = 10, ToType = "Suspect", ToId = 10 },
            new CaseConnection { Id = 6, CaseId = 3, FromType = "Witness", FromId = 8, ToType = "Suspect", ToId = 10 },
            new CaseConnection { Id = 7, CaseId = 4, FromType = "Evidence", FromId = 17, ToType = "Suspect", ToId = 12 },
            new CaseConnection { Id = 8, CaseId = 4, FromType = "Evidence", FromId = 18, ToType = "Suspect", ToId = 12 },
            new CaseConnection { Id = 9, CaseId = 4, FromType = "Witness", FromId = 13, ToType = "Suspect", ToId = 12 }
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