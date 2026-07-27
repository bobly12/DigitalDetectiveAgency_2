using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCase1SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CaseConnections",
                columns: new[] { "Id", "CaseId", "FromId", "FromType", "ToId", "ToType" },
                values: new object[] { 3, 1, 7, "Evidence", 2, "Suspect" });

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "A single red rose found on the dressing room floor. The stem is freshly cut, not wilted — someone left it here recently.");

            migrationBuilder.InsertData(
                table: "EvidenceItems",
                columns: new[] { "Id", "CaseId", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 4, 1, "A photocopied ledger page with several transactions circled and the word 'AUDIT' underlined twice. Found stuffed in a lobby trash can.", "/images/evidence/ledger.jpg", "Insurance Ledger Page" },
                    { 5, 1, "A crumpled note reading: 'We need to talk. I'm not done fighting for us.' No signature, but the handwriting is bold and impatient.", "/images/evidence/note.jpg", "Handwritten Note" },
                    { 6, 1, "A printed transcript of a voicemail: 'You can't just cut me out after everything I've done for you. We'll see about that.' Timestamp: three days before the disappearance.", "/images/evidence/voicemail.jpg", "Voicemail Transcript" },
                    { 7, 1, "A printout of badge-scan entries. One badge — registered to Elena Vasquez — scanned into the dressing room corridor at 7:38 PM, twenty minutes before the violinist was last seen.", "/images/evidence/security_log.jpg", "Backstage Security Log" }
                });

            migrationBuilder.InsertData(
                table: "Suspects",
                columns: new[] { "Id", "Alibi", "CaseId", "Description", "ImageUrl", "IsGuilty", "Motive", "Name" },
                values: new object[,]
                {
                    { 3, "Says he was greeting donors in the lobby all evening — mostly true, but he was unaccounted for a 15-minute window.", 1, "The concert hall's owner and producer. Smooth talker, expensive suit, always working the room.", "/images/suspects/victor.jpg", false, "The violinist had discovered irregularities in the hall's finances and threatened to go public before the show.", "Victor Hale" },
                    { 4, "Claims she was home sick — but no one can confirm it, and her phone was off all night.", 1, "The violinist's longtime personal assistant. Quiet, meticulous, always one step behind her.", "/images/suspects/sophia.jpg", false, "Learned two days ago that she'd been written out of the violinist's will after years of unpaid overtime.", "Sophia Reyes" },
                    { 5, "Says he was at the bar the whole time — bartender remembers seeing him, but not exactly when he left.", 1, "The violinist's ex-fiancé. Showed up uninvited backstage an hour before the show.", "/images/suspects/damian.jpg", false, "Still bitter over their public breakup; was overheard arguing with her in the hallway.", "Damian Cole" }
                });

            migrationBuilder.InsertData(
                table: "Witnesses",
                columns: new[] { "Id", "CaseId", "ImageUrl", "Name", "Statement" },
                values: new object[,]
                {
                    { 3, 1, "/images/witnesses/sound_tech.jpg", "Sound Technician", "I overheard Victor Hale on the phone saying something like 'we need to make the numbers disappear before anyone audits this.' Didn't think much of it at the time." },
                    { 4, 1, "/images/witnesses/valet.jpg", "Valet", "Damian Cole's car peeled out of the lot around 9 PM, tires screeching. Looked like he was in a real hurry to leave." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "A single red rose found on the dressing room floor. The stem is freshly cut, not wilted — suggesting it was placed here recently, possibly during the disappearance itself.");
        }
    }
}
