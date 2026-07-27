using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddCase3SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cases",
                columns: new[] { "Id", "Description", "Difficulty", "IsPublished", "Title" },
                values: new object[] { 3, "The night of the town fiesta, sugar baron Ernesto Malinao is found dead in his study at the family's ancestral hacienda in Negros Occidental — hours before he was set to sign the papers selling generations of tenant land to a foreign conglomerate. The house was full of relatives, guests, and household staff. Everyone was watching the fireworks. No one saw a thing.", 2, true, "The Last Harvest at Hacienda Malinao" });

            migrationBuilder.InsertData(
                table: "CaseConnections",
                columns: new[] { "Id", "CaseId", "FromId", "FromType", "ToId", "ToType" },
                values: new object[,]
                {
                    { 4, 3, 9, "Evidence", 10, "Suspect" },
                    { 5, 3, 10, "Evidence", 10, "Suspect" },
                    { 6, 3, 8, "Witness", 10, "Suspect" }
                });

            migrationBuilder.InsertData(
                table: "EvidenceItems",
                columns: new[] { "Id", "CaseId", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 8, 3, "A common cane-cutting bolo, found half-buried near the chapel path. Nearly every household on the hacienda owns one identical to it.", "/images/evidence/bolo.jpg", "Bloodied Bolo" },
                    { 9, 3, "A single page torn from the sale agreement, the tenant-eviction clause circled hard enough to tear the paper. Found tucked behind the santo on the chapel altar.", "/images/evidence/contract_page.jpg", "Torn Land Sale Contract" },
                    { 10, 3, "A handwritten ledger tallying five months of unpaid wages owed to the cane workers, found in a nipa hut at the edge of the fields — the entries stop the day Ernesto died.", "/images/evidence/wage_ledger.jpg", "Ledger of Unpaid Sacada Wages" },
                    { 11, 3, "A short, cold note in Ernesto's handwriting: 'The allowance stops the day I sign. Don't make this harder than it needs to be.'", "/images/evidence/note_teresa.jpg", "Threatening Note to Teresa" },
                    { 12, 3, "An estate bank passbook with three withdrawals in Atty. Cruz's handwriting that were never logged in the official trust records.", "/images/evidence/passbook.jpg", "Bank Passbook with Shortages" },
                    { 13, 3, "An overdue-payment letter from a Manila casino, addressed to Ricardo, threatening 'further action' if the balance isn't settled by month's end.", "/images/evidence/casino_letter.jpg", "Casino Debt Collector Letter" },
                    { 14, 3, "An old land survey with a section re-measured in Loreta's handwriting, the margin noting 'this was never his to sell alone.'", "/images/evidence/survey.jpg", "Disputed Boundary Survey" },
                    { 15, 3, "William Tan's business card, a lower price scrawled on the back along with the words 'final — take it or we walk.'", "/images/evidence/business_card.jpg", "Business Card with Scribbled Offer" }
                });

            migrationBuilder.InsertData(
                table: "Suspects",
                columns: new[] { "Id", "Alibi", "CaseId", "Description", "ImageUrl", "IsGuilty", "Motive", "Name" },
                values: new object[,]
                {
                    { 6, "Says he was watching the fireworks with cousins the whole time. A few vaguely recall seeing him 'at some point' during the display.", 3, "Ernesto's estranged son, back from Manila after nearly a decade of silence — arriving just three days before his father's death.", "/images/suspects/ricardo.jpg", false, "Cut off financially years ago and only recently reconciled. With the land sale unsigned, he stood to inherit everything.", "Ricardo Malinao" },
                    { 7, "Claims she left before the fireworks even started. The gate guard's logbook has no exit time recorded for her that night.", 3, "Ernesto's longtime companion, kept quietly in a house at the edge of town for over a decade.", "/images/suspects/teresa.jpg", false, "Ernesto was ending their arrangement to remarry once the sale went through — cutting off her monthly allowance for good.", "Teresa Bautista" },
                    { 8, "Says he was drafting the final contract in the guest room all night. No one else can confirm he was there the whole time.", 3, "The family's lawyer and Ernesto's compadre of thirty years, handling the estate's finances and the pending sale.", "/images/suspects/cruz.jpg", false, "The incoming buyer's audit would have exposed years of quietly siphoned trust funds.", "Atty. Simeon Cruz" },
                    { 9, "Says she was arranging the fireworks display with the caterers. Staff remember her there for 'most of the evening.'", 3, "Ernesto's older sister, who ran the hacienda's day-to-day operations for thirty years while he lived abroad.", "/images/suspects/loreta.jpg", false, "Passed over in the will in favor of Ricardo, despite three decades of unpaid work keeping the estate afloat.", "Loreta Malinao" },
                    { 10, "Says he was in the chapel praying through the fireworks, as he does every fiesta. No one thought to check.", 3, "The hacienda's katiwala — overseer of the cane fields for over forty years. Quiet, devout, tends the small chapel on the property himself.", "/images/suspects/fidel.jpg", true, "The sale would evict the sacada families who'd worked the land for generations — families whose wages Ernesto had already quietly stopped paying months ago.", "Mang Fidel Cortez" },
                    { 11, "Says he was on a call with his office in Manila during the fireworks. Phone records confirm a call, but not that he was the one on it.", 3, "Representative for the conglomerate buying the land — flew in from Manila that morning to finalize the sale.", "/images/suspects/tan.jpg", false, "Ernesto tried to renegotiate the price at the last minute, threatening to walk away from the deal entirely.", "William Tan" }
                });

            migrationBuilder.InsertData(
                table: "Witnesses",
                columns: new[] { "Id", "CaseId", "ImageUrl", "Name", "Statement" },
                values: new object[,]
                {
                    { 5, 3, "/images/witnesses/nena.jpg", "Nena, the Kasambahay", "I saw someone pass by the study window right when the fireworks started, pero patalikod siya — back was turned, couldn't tell you who." },
                    { 6, 3, "/images/witnesses/tricycle_driver.jpg", "Tricycle Driver", "I drove Ricardo to town earlier that day. He was on the phone the whole ride, arguing with someone about a debt — sounded serious." },
                    { 7, 3, "/images/witnesses/councilor.jpg", "Town Councilor", "I saw Atty. Cruz slip out of the fiesta early, clutching a folder like his life depended on it. Didn't even say goodbye to anyone." },
                    { 8, 3, "/images/witnesses/farmhand.jpg", "Sacada Farmhand", "Nakita ko si Mang Fidel walking from the chapel toward the main house right before the fireworks ended, may dalang something wrapped in cloth. Sabi niya nagdadasal lang daw siya kanina, pero hindi galing sa kapilya papunta kundi papunta sa bahay." },
                    { 9, 3, "/images/witnesses/niece.jpg", "Ernesto's Niece", "I saw Tita Teresa and Tito Ernesto arguing near the garden earlier that evening, before the fireworks even started. She looked like she'd been crying." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
