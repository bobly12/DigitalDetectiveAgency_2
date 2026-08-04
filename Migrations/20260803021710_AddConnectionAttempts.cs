using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectionAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    FromType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    WasCorrect = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionAttempts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectionAttempts_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionAttempts_ApplicationUserId",
                table: "ConnectionAttempts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionAttempts_CaseId",
                table: "ConnectionAttempts",
                column: "CaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionAttempts");
        }
    }
}
