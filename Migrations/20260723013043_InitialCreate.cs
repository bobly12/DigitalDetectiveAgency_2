using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuspectEliminations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    SuspectId = table.Column<int>(type: "int", nullable: false),
                    EliminatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspectEliminations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuspectEliminations_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuspectEliminations_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuspectEliminations_Suspects_SuspectId",
                        column: x => x.SuspectId,
                        principalTable: "Suspects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuspectEliminations_ApplicationUserId",
                table: "SuspectEliminations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspectEliminations_CaseId",
                table: "SuspectEliminations",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspectEliminations_SuspectId",
                table: "SuspectEliminations",
                column: "SuspectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuspectEliminations");
        }
    }
}
