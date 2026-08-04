using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseConnectionNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "CaseConnections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 1,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 2,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 3,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 4,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 5,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 6,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 7,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 8,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 9,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 10,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 11,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 12,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 13,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 14,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 15,
                column: "Note",
                value: null);

            migrationBuilder.UpdateData(
                table: "CaseConnections",
                keyColumn: "Id",
                keyValue: 16,
                column: "Note",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "CaseConnections");
        }
    }
}
