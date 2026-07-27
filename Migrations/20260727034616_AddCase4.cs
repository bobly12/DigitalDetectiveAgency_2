using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddCase4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cases",
                columns: new[] { "Id", "Description", "Difficulty", "IsPublished", "Title" },
                values: new object[] { 4, "The night before dive resort owner Ramon Aguilar was set to unveil a controversial expansion into Talipanan Cove — land the local fisherfolk have worked for generations and the Mangyan community considers ancestral ground — he's found dead at the bottom of his own boat dock. The town had gathered for the resort's investor showcase. Protesters had gathered outside the gate for weeks. Everyone had a reason to want the expansion stopped. Only one person had a reason to stop Ramon himself.", 2, true, "Undertow at Puerto Galera" });

            migrationBuilder.InsertData(
                table: "CaseConnections",
                columns: new[] { "Id", "CaseId", "FromId", "FromType", "ToId", "ToType" },
                values: new object[,]
                {
                    { 7, 4, 17, "Evidence", 12, "Suspect" },
                    { 8, 4, 18, "Evidence", 12, "Suspect" },
                    { 9, 4, 13, "Witness", 12, "Suspect" }
                });

            migrationBuilder.InsertData(
                table: "EvidenceItems",
                columns: new[] { "Id", "CaseId", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 16, 4, "A standard dive knife, the kind every instructor and boat crew on the property carries. No initials, no distinguishing marks.", "/images/evidence/dive_knife.jpg", "Diving Knife" },
                    { 17, 4, "An environmental clearance form for the cove expansion, the approval signature traced rather than signed — and initialed 'N.A.' in the corner margin.", "/images/evidence/forged_permit.jpg", "Forged Permit Application" },
                    { 18, 4, "A second, unofficial ledger found taped beneath the office desk drawer, tracking three years of maintenance budget quietly redirected to a personal account.", "/images/evidence/fund_ledger.jpg", "Diverted Maintenance Fund Ledger" },
                    { 19, 4, "A policy naming Cristina as sole beneficiary, dated two months after the wedding — and never disclosed to Ramon's children from his first marriage.", "/images/evidence/insurance_policy.jpg", "Life Insurance Policy" },
                    { 20, 4, "An envelope of cash found in Mayor Villaruz's car, a sticky note attached reading 'para sa fast-track, huwag na banggitin.'", "/images/evidence/bribe_envelope.jpg", "Bribery Cash Envelope" },
                    { 21, 4, "A phone photo of the resort's dive compressor with a cut hose — taken the same week Boyet was overheard threatening to 'make sure the showcase doesn't go smoothly.'", "/images/evidence/compressor_photo.jpg", "Sabotaged Compressor Photo" },
                    { 22, 4, "A flyer from the fisherfolk association, hand-annotated in the margin: 'kung kailangan, gagamit tayo ng ibang paraan' — if necessary, we'll use other means.", "/images/evidence/protest_flyer.jpg", "Protest Flyer" },
                    { 23, 4, "A half-burned copy of the Mangyan community's ancestral domain petition, found in the resort office trash bin — someone tried to make it disappear before the investors saw it.", "/images/evidence/burnt_petition.jpg", "Burnt Petition Copy" }
                });

            migrationBuilder.InsertData(
                table: "Suspects",
                columns: new[] { "Id", "Alibi", "CaseId", "Description", "ImageUrl", "IsGuilty", "Motive", "Name" },
                values: new object[,]
                {
                    { 12, "Says he was doing late inventory in the resort office alone. No one thought to question it — he's always the last one to leave.", 4, "Ramon's nephew and the resort's general manager, trusted with the books for the past six years.", "/images/suspects/nico.jpg", true, "An outside audit tied to the expansion loan would have surfaced years of diverted maintenance funds and forged permit paperwork — all under his signature.", "Nico Aguilar" },
                    { 13, "Claims she was greeting investors at the showcase all night. Several guests confirm seeing her — but not continuously.", 4, "Ramon's second wife, twenty years his junior, married just three years ago.", "/images/suspects/cristina.jpg", false, "Sole beneficiary on a life insurance policy Ramon took out shortly after their wedding — one his first family never knew existed.", "Cristina Aguilar" },
                    { 14, "Says his boat never left Sabang that night. The marina logbook backs him up — mostly; there's a 40-minute gap in the entries.", 4, "Owner of a rival dive resort in Sabang, and Ramon's business rival for over a decade.", "/images/suspects/boyet.jpg", false, "Ramon's expansion would have driven half of Boyet's dive tourists to the new cove, likely closing his resort within a year.", "Boyet Ramos" },
                    { 15, "Says he left the showcase early with a headache. His driver confirms dropping him home, but not what time he arrived.", 4, "The town mayor, who fast-tracked the resort's expansion permits over the objections of his own municipal planning office.", "/images/suspects/villaruz.jpg", false, "Ramon was the only person who could prove the permits were approved in exchange for a cut of the investor funding.", "Mayor Teodoro Villaruz" },
                    { 16, "Says he was leading a prayer gathering with his community at the edge of the property when the fireworks started. Several members confirm it.", 4, "An elder of the local Mangyan community, who has spent months formally petitioning to have the cove recognized as ancestral domain.", "/images/suspects/bandying.jpg", false, "The expansion would pave over a burial ground his community has protected for generations — and Ramon had ignored every petition.", "Datu Bandying" },
                    { 17, "Says he was leading the protest chant outside the gate when the fireworks went off — dozens of witnesses can place him there.", 4, "Head of the local fisherfolk association, and the loudest voice at every protest outside the resort gate for the past two months.", "/images/suspects/pandoy.jpg", false, "The cove is the fishing ground his association depends on. Losing it meant losing livelihoods for over thirty families.", "Fernando \"Ka Pandoy\" Reyes" }
                });

            migrationBuilder.InsertData(
                table: "Witnesses",
                columns: new[] { "Id", "CaseId", "ImageUrl", "Name", "Statement" },
                values: new object[,]
                {
                    { 10, 4, "/images/witnesses/bangkero.jpg", "Bangkero (Boat Captain)", "Merong nakita akong tao malapit sa dive shop that night, pero madilim — hindi ko masabi kung sino." },
                    { 11, 4, "/images/witnesses/front_desk.jpg", "Resort Front Desk Staff", "I overheard Cristina on the phone that night saying, 'after this, wala nang makakapigil sa 'kin.' Didn't think much of it at the time." },
                    { 12, 4, "/images/witnesses/sabang_local.jpg", "Sabang Local", "I saw Boyet's boat idling near the resort's private dock that night, lights off. Left again after maybe ten minutes." },
                    { 13, 4, "/images/witnesses/security_guard.jpg", "Resort Security Guard", "Si Nico lang ang nakita kong pumasok sa office nang mag-isa, late na late na. Sabi niya nagta-take lang daw siya ng inventory, pero nakita ko siyang may dalang shredder bag paglabas." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
