using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddSuspectAndWitnessSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suspects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Motive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alibi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGuilty = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suspects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suspects_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Witnesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Witnesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Witnesses_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Suspects",
                columns: new[] { "Id", "Alibi", "CaseId", "Description", "ImageUrl", "IsGuilty", "Motive", "Name" },
                values: new object[,]
                {
                    { 1, "Claims he was in the control booth the entire evening.", 1, "The concert hall's stage manager. Nervous, evasive, avoids eye contact.", "/images/suspects/marcus.jpg", false, "Was recently fired by the violinist for stealing sheet music royalties.", "Marcus Feld" },
                    { 2, "Says she was at a nearby cafe, but no one can confirm it.", 1, "A rival violinist who lost the lead position to the victim last season.", "/images/suspects/elena.jpg", true, "Publicly humiliated when passed over for the solo spot.", "Elena Vasquez" }
                });

            migrationBuilder.InsertData(
                table: "Witnesses",
                columns: new[] { "Id", "CaseId", "ImageUrl", "Name", "Statement" },
                values: new object[,]
                {
                    { 1, 1, "/images/witnesses/janitor.jpg", "Backstage Janitor", "I saw someone in a dark coat near the dressing rooms around 7:40 PM, but I didn't get a good look at their face." },
                    { 2, 1, "/images/witnesses/clerk.jpg", "Ticket Booth Clerk", "A woman matching Elena's description left in a hurry just before intermission. She seemed upset." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suspects_CaseId",
                table: "Suspects",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Witnesses_CaseId",
                table: "Witnesses",
                column: "CaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suspects");

            migrationBuilder.DropTable(
                name: "Witnesses");
        }
    }
}
