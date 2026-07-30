using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cases",
                columns: new[] { "Id", "Description", "Difficulty", "IsPublished", "Title" },
                values: new object[] { 2, "A wealthy family's annual reunion ends in tragedy when the patriarch is found dead in the locked study. Everyone had a motive. No one has an alibi.", 1, true, "Silence at Blackwood Manor" });

            migrationBuilder.InsertData(
                table: "EvidenceItems",
                columns: new[] { "Id", "CaseId", "Description", "ImageUrl", "Name" },
                values: new object[] { 3, 2, "An antique pocket watch, stopped at 9:47 PM. The glass is cracked, and the inside cover is engraved with initials that don't match anyone in the household.", "/images/evidence/pocket_watch.jpg", "Broken Pocket Watch" });
        }
    }
}
