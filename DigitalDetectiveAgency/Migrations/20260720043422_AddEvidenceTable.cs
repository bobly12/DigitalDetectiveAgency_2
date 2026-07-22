using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddEvidenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvidenceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidenceItems_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EvidenceItems",
                columns: new[] { "Id", "CaseId", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, 1, "A single red rose found on the dressing room floor. The stem is freshly cut, not wilted — suggesting it was placed here recently, possibly during the disappearance itself.", "/images/evidence/rose.jpg", "Dropped Rose" },
                    { 2, 1, "A concert program torn roughly in half. The violinist's name has been circled in red ink, and a small phone number is scrawled in the margin.", "/images/evidence/torn_program.jpg", "Torn Concert Program" },
                    { 3, 2, "An antique pocket watch, stopped at 9:47 PM. The glass is cracked, and the inside cover is engraved with initials that don't match anyone in the household.", "/images/evidence/pocket_watch.jpg", "Broken Pocket Watch" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceItems_CaseId",
                table: "EvidenceItems",
                column: "CaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidenceItems");
        }
    }
}
