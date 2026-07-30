using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class RestoreCase2AndSyncSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Insert Case 2 FIRST so Foreign Key constraints are satisfied
            migrationBuilder.InsertData(
                table: "Cases",
                columns: new[] { "Id", "Description", "Difficulty", "IsPublished", "Title" },
                values: new object[] { 2, "Community radio host Miguel Ramos is found dead moments after his live broadcast suddenly cuts off. Everyone blames the rolling blackout. But one small detail proves the station never actually lost power.", 1, true, "The Last Broadcast" });

            // 2. Now update CaseConnections that reference CaseId = 2
            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CaseId", "FromId", "ToId", "ToType" },
                values: new object[] { 2, 26, 25, "Evidence" });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CaseId", "FromId", "ToId", "ToType" },
                values: new object[] { 2, 24, 25, "Evidence" });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CaseId", "FromId", "FromType", "ToId" },
                values: new object[] { 2, 27, "Evidence", 22 });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CaseId", "FromId", "ToId" },
                values: new object[] { 2, 28, 22 });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CaseId", "FromId", "ToId" },
                values: new object[] { 2, 29, 22 });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CaseId", "FromId", "FromType", "ToId" },
                values: new object[] { 3, 9, "Evidence", 10 });

            migrationBuilder.InsertData(
                table: "CaseConnections",
                columns: new[] { "Id", "CaseId", "FromId", "FromType", "ToId", "ToType" },
                values: new object[,]
                {
                    { 10, 3, 10, "Evidence", 10, "Suspect" },
                    { 11, 3, 8, "Witness", 10, "Suspect" },
                    { 12, 4, 17, "Evidence", 12, "Suspect" },
                    { 13, 4, 18, "Evidence", 12, "Suspect" },
                    { 14, 4, 13, "Witness", 12, "Suspect" },
                    { 15, 4, 17, "Evidence", 18, "Evidence" },
                    { 16, 4, 18, "Evidence", 13, "Witness" }
                });

            migrationBuilder.UpdateData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "The night of the town fiesta, sugar baron Ernesto Malinao is found dead in his study at the family's ancestral hacienda in Negros Occidental hours before he was set to sign the papers selling generations of tenant land to a foreign conglomerate. The house was full of relatives, guests, and household staff. Everyone was watching the fireworks. No one saw a thing.");

            migrationBuilder.UpdateData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "The night before dive resort owner Ramon Aguilar was set to unveil a controversial expansion into Talipanan Cove land the local fisherfolk have worked for generations and the Mangyan community considers ancestral ground he's found dead at the bottom of his own boat dock. The town had gathered for the resort's investor showcase. Protesters had gathered outside the gate for weeks. Everyone had a reason to want the expansion stopped. Only one person had a reason to stop Ramon himself.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "A single red rose found on the dressing room floor. The stem is freshly cut, not wilted someone left it here recently.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "A printout of badge-scan entries. One badge registered to Elena Vasquez scanned into the dressing room corridor at 7:38 PM, twenty minutes before the violinist was last seen.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "A handwritten ledger tallying five months of unpaid wages owed to the cane workers, found in a nipa hut at the edge of the fields - the entries stop the day Ernesto died.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "William Tan's business card, a lower price scrawled on the back along with the words 'final take it or we walk.'");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "Description",
                value: "An environmental clearance form for the cove expansion, the approval signature traced rather than signed and initialed N.A. in the corner margin.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "Description",
                value: "A policy naming Cristina as sole beneficiary, dated two months after the wedding and never disclosed to Ramon's children from his first marriage.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "Description",
                value: "A phone photo of the resort's dive compressor with a cut hose taken the same week Boyet was overheard threatening to 'make sure the showcase doesn't go smoothly.'");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 22,
                column: "Description",
                value: "A flyer from the fisherfolk association, hand-annotated in the margin: 'kung kailangan, gagamit tayo ng ibang paraan' (if necessary, we'll use other means).");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "Description",
                value: "A half-burned copy of the Mangyan community's ancestral domain petition, found in the resort office trash bin someone tried to make it disappear before the investors saw it.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 3,
                column: "Alibi",
                value: "Says he was greeting donors in the lobby all evening mostly true, but he was unaccounted for a 15-minute window.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 4,
                column: "Alibi",
                value: "Claims she was home sick but no one can confirm it, and her phone was off all night.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 5,
                column: "Alibi",
                value: "Says he was at the bar the whole time bartender remembers seeing him, but not exactly when he left.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Ernesto's estranged son, back from Manila after nearly a decade of silence arriving just three days before his father's death.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 7,
                column: "Motive",
                value: "Ernesto was ending their arrangement to remarry once the sale went through cutting off her monthly allowance for good.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Motive" },
                values: new object[] { "The hacienda's katiwala overseer of the cane fields for over forty years. Quiet, devout, tends the small chapel on the property himself.", "The sale would evict the sacada families who'd worked the land for generations families whose wages Ernesto had already quietly stopped paying months ago." });

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "Representative for the conglomerate buying the land flew in from Manila that morning to finalize the sale.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Alibi", "Motive" },
                values: new object[] { "Says he was doing late inventory in the resort office alone. No one thought to question it he's always the last one to leave.", "An outside audit tied to the expansion loan would have surfaced years of diverted maintenance funds and forged permit paperwork - all under his signature." });

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Alibi", "Motive" },
                values: new object[] { "Claims she was greeting investors at the showcase all night. Several guests confirm seeing her but not continuously.", "Sole beneficiary on a life insurance policy Ramon took out shortly after their wedding one his first family never knew existed." });

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 14,
                column: "Alibi",
                value: "Says his boat never left Sabang that night. The marina logbook backs him up mostly; there's a 40-minute gap in the entries.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 16,
                column: "Motive",
                value: "The expansion would pave over a burial ground his community has protected for generations and Ramon had ignored every petition.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 17,
                column: "Alibi",
                value: "Says he was leading the protest chant outside the gate when the fireworks went off dozens of witnesses can place him there.");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 5,
                column: "Statement",
                value: "I saw someone pass by the study window right when the fireworks started, pero patalikod siya (back was turned), couldn't tell you who.");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 6,
                column: "Statement",
                value: "I drove Ricardo to town earlier that day. He was on the phone the whole ride, arguing with someone about a debt- sounded serious.");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 10,
                column: "Statement",
                value: "Merong nakita akong tao malapit sa dive shop that night, pero madilim hindi ko masabi kung sino.");

            migrationBuilder.InsertData(
                table: "EvidenceItems",
                columns: new[] { "Id", "CaseId", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 24, 2, "Miguel died from a single blunt force trauma to the back of the head. Estimated time of death: 9:17 PM.", "/images/evidence/autopsy_report.jpg", "Autopsy Report" },
                    { 25, 2, "The live broadcast ends abruptly at exactly 9:17 PM. No scream, no struggle-only sudden silence.", "/images/evidence/broadcast_recording.jpg", "Broadcast Recording" },
                    { 26, 2, "The backup generator automatically activated at 9:16 PM and supplied uninterrupted power until 9:19 PM.", "/images/evidence/generator_log.jpg", "Generator Log" },
                    { 27, 2, "A flashlight recovered from Marco's toolbox. Rainwater is mixed with fiberglass insulation dust matching the generator shed.", "/images/evidence/wet_flashlight.jpg", "Wet Flashlight" },
                    { 28, 2, "Several maintenance expenses were redirected into an unknown personal account over the past six months.", "/images/evidence/budget_spreadsheet.jpg", "Maintenance Budget Spreadsheet" },
                    { 29, 2, "Miguel recorded a message saying, 'Marco... we'll figure this out tomorrow. Your son shouldn't suffer because of this.' The recording was never sent.", "/images/evidence/voice_memo.jpg", "Unsent Voice Memo" },
                    { 30, 2, "The roof hatch was opened earlier that afternoon for routine antenna maintenance. No activity was recorded during the murder.", "/images/evidence/roof_access.jpg", "Roof Access Log" },
                    { 31, 2, "A receipt timestamped 9:12 PM showing Liza purchased two coffees several blocks away from the station.", "/images/evidence/coffee_receipt.jpg", "Coffee Shop Receipt" },
                    { 32, 2, "Reception attendance sheets reveal Tina forged employee time records several weeks earlier to help her younger brother keep his job.", "/images/evidence/attendance_log.jpg", "Attendance Log" },
                    { 33, 2, "A blurry CCTV frame captures a white delivery van leaving the station district after the murder. The license plate is unreadable, and investigators never identify the driver.", "/images/evidence/white_van.jpg", "White Van CCTV" }
                });

            migrationBuilder.InsertData(
                table: "Suspects",
                columns: new[] { "Id", "Alibi", "CaseId", "Description", "ImageUrl", "IsGuilty", "Motive", "Name" },
                values: new object[,]
                {
                    { 18, "Claims she left to buy coffee before the broadcast ended. A receipt supports her story.", 2, "The station producer. Organized every broadcast and managed the daily schedule.", "/images/suspects/liza.jpg", false, "Miguel planned to replace her after months of declining ratings.", "Liza Mendoza" },
                    { 19, "Says he spent the evening repairing speakers in Studio B.", 2, "Senior sound engineer responsible for maintaining Studio B.", "/images/suspects/noel.jpg", false, "Miguel accused him of stealing expensive recording equipment.", "Noel Santos" },
                    { 20, "Claims he was attending a charity dinner throughout the evening.", 2, "A local politician frequently criticized on Miguel's radio program.", "/images/suspects/reyes.jpg", false, "Miguel planned to expose corruption involving flood-relief funds.", "Councilor Adrian Reyes" },
                    { 21, "Claims she remained at the front desk until police arrived.", 2, "Receptionist responsible for visitor records and station access.", "/images/suspects/tina.jpg", false, "Miguel publicly embarrassed her brother during an investigative segment.", "Tina Flores" },
                    { 22, "Claims he spent the blackout inspecting the generator outside.", 2, "Station maintenance technician. Quiet, dependable, and the first to help investigators search the building.", "/images/suspects/marco.jpg", true, "He secretly diverted station maintenance funds to pay for his son's heart surgery and feared Miguel had discovered the missing money.", "Marco Villanueva" }
                });

            migrationBuilder.InsertData(
                table: "Witnesses",
                columns: new[] { "Id", "CaseId", "ImageUrl", "Name", "Statement" },
                values: new object[,]
                {
                    { 14, 2, "/images/witnesses/security_guard_radio.jpg", "Security Guard", "I heard something heavy hit the floor around 9:24 PM. I assumed a microphone stand had fallen." },
                    { 15, 2, "/images/witnesses/coffee_vendor.jpg", "Coffee Vendor", "The producer bought coffee just after nine. She looked rushed but never returned before the police arrived." },
                    { 16, 2, "/images/witnesses/janitor_radio.jpg", "Janitor", "I saw someone carrying a flashlight outside near the generator shed. It was raining too hard to recognize who it was." },
                    { 17, 2, "/images/witnesses/listener.jpg", "Regular Listener", "The broadcast suddenly stopped. Everyone online thought the neighborhood had lost power." },
                    { 18, 2, "/images/witnesses/delivery_rider.jpg", "Delivery Rider", "A white van left the station shortly after the police arrived. I couldn't read the plate because of the rain." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CaseId", "FromId", "ToId", "ToType" },
                values: new object[] { 3, 9, 10, "Suspect" });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CaseId", "FromId", "ToId", "ToType" },
                values: new object[] { 3, 10, 10, "Suspect" });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CaseId", "FromId", "FromType", "ToId" },
                values: new object[] { 3, 8, "Witness", 10 });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CaseId", "FromId", "ToId" },
                values: new object[] { 4, 17, 12 });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CaseId", "FromId", "ToId" },
                values: new object[] { 4, 18, 12 });

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CaseId", "FromId", "FromType", "ToId" },
                values: new object[] { 4, 13, "Witness", 12 });

            migrationBuilder.UpdateData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "The night of the town fiesta, sugar baron Ernesto Malinao is found dead in his study at the family's ancestral hacienda in Negros Occidental — hours before he was set to sign the papers selling generations of tenant land to a foreign conglomerate. The house was full of relatives, guests, and household staff. Everyone was watching the fireworks. No one saw a thing.");

            migrationBuilder.UpdateData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "The night before dive resort owner Ramon Aguilar was set to unveil a controversial expansion into Talipanan Cove — land the local fisherfolk have worked for generations and the Mangyan community considers ancestral ground — he's found dead at the bottom of his own boat dock. The town had gathered for the resort's investor showcase. Protesters had gathered outside the gate for weeks. Everyone had a reason to want the expansion stopped. Only one person had a reason to stop Ramon himself.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "A single red rose found on the dressing room floor. The stem is freshly cut, not wilted — someone left it here recently.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "A printout of badge-scan entries. One badge — registered to Elena Vasquez — scanned into the dressing room corridor at 7:38 PM, twenty minutes before the violinist was last seen.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "A handwritten ledger tallying five months of unpaid wages owed to the cane workers, found in a nipa hut at the edge of the fields — the entries stop the day Ernesto died.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "William Tan's business card, a lower price scrawled on the back along with the words 'final — take it or we walk.'");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "Description",
                value: "An environmental clearance form for the cove expansion, the approval signature traced rather than signed — and initialed 'N.A.' in the corner margin.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "Description",
                value: "A policy naming Cristina as sole beneficiary, dated two months after the wedding — and never disclosed to Ramon's children from his first marriage.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "Description",
                value: "A phone photo of the resort's dive compressor with a cut hose — taken the same week Boyet was overheard threatening to 'make sure the showcase doesn't go smoothly.'");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 22,
                column: "Description",
                value: "A flyer from the fisherfolk association, hand-annotated in the margin: 'kung kailangan, gagamit tayo ng ibang paraan' — if necessary, we'll use other means.");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "Description",
                value: "A half-burned copy of the Mangyan community's ancestral domain petition, found in the resort office trash bin — someone tried to make it disappear before the investors saw it.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 3,
                column: "Alibi",
                value: "Says he was greeting donors in the lobby all evening — mostly true, but he was unaccounted for a 15-minute window.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 4,
                column: "Alibi",
                value: "Claims she was home sick — but no one can confirm it, and her phone was off all night.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 5,
                column: "Alibi",
                value: "Says he was at the bar the whole time — bartender remembers seeing him, but not exactly when he left.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Ernesto's estranged son, back from Manila after nearly a decade of silence — arriving just three days before his father's death.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 7,
                column: "Motive",
                value: "Ernesto was ending their arrangement to remarry once the sale went through — cutting off her monthly allowance for good.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Motive" },
                values: new object[] { "The hacienda's katiwala — overseer of the cane fields for over forty years. Quiet, devout, tends the small chapel on the property himself.", "The sale would evict the sacada families who'd worked the land for generations — families whose wages Ernesto had already quietly stopped paying months ago." });

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "Representative for the conglomerate buying the land — flew in from Manila that morning to finalize the sale.");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Alibi", "Motive" },
                values: new object[] { "Says he was doing late inventory in the resort office alone. No one thought to question it — he's always the last one to leave.", "An outside audit tied to the expansion loan would have surfaced years of diverted maintenance funds and forged permit paperwork — all under his signature." });
        }
    }
}