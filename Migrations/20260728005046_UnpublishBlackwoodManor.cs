using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class UnpublishBlackwoodManor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
migrationBuilder.Sql(
    "UPDATE Cases SET IsPublished = 0 WHERE Id = 2;"
);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
migrationBuilder.Sql(
    "UPDATE Cases SET IsPublished = 1 WHERE Id = 2;"
);
        }
    }
}
