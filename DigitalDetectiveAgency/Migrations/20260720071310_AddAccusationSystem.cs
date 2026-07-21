using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class AddAccusationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accusations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    AccusedSuspectId = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accusations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accusations_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accusations_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accusations_Suspects_AccusedSuspectId",
                        column: x => x.AccusedSuspectId,
                        principalTable: "Suspects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    FromType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseConnections_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClueConnections",
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClueConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClueConnections_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClueConnections_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CaseConnections",
                columns: new[] { "Id", "CaseId", "FromId", "FromType", "ToId", "ToType" },
                values: new object[,]
                {
                    { 1, 1, 2, "Evidence", 2, "Suspect" },
                    { 2, 1, 2, "Witness", 2, "Suspect" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accusations_AccusedSuspectId",
                table: "Accusations",
                column: "AccusedSuspectId");

            migrationBuilder.CreateIndex(
                name: "IX_Accusations_ApplicationUserId",
                table: "Accusations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accusations_CaseId",
                table: "Accusations",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseConnections_CaseId",
                table: "CaseConnections",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClueConnections_ApplicationUserId",
                table: "ClueConnections",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClueConnections_CaseId",
                table: "ClueConnections",
                column: "CaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accusations");

            migrationBuilder.DropTable(
                name: "CaseConnections");

            migrationBuilder.DropTable(
                name: "ClueConnections");
        }
    }
}
