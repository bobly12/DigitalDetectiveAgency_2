using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddVictimNameToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VictimName",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 5,
                column: "VictimName",
                value: "Ramon \"Mon\" Padilla");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VictimName",
                table: "Cases");
        }
    }
}
