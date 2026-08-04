using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    public partial class DropInitialFourCases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete order matters because of Restrict FKs on Accusation and
            // SuspectElimination — these must go first, everything else
            // cascades once the Case rows themselves are deleted.

            migrationBuilder.Sql(@"
                DELETE FROM Accusations
                WHERE CaseId IN (1, 2, 3, 4);
            ");

            migrationBuilder.Sql(@"
                DELETE FROM SuspectEliminations
                WHERE CaseId IN (1, 2, 3, 4);
            ");

            // Cascades to: PlayerCases, Evidence, Suspects, Witnesses,
            // ClueConnections, CaseConnections, ConnectionAttempts
            migrationBuilder.Sql(@"
                DELETE FROM Cases
                WHERE Id IN (1, 2, 3, 4);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Deliberately irreversible — the original seed data for these
            // four cases is not preserved. Restore from a database backup
            // taken before this migration if you ever need it back.
        }
    }
}