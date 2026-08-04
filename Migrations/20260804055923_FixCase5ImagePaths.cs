using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetectiveAgency.Migrations
{
    /// <inheritdoc />
    public partial class FixCase5ImagePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 34,
                column: "ImageUrl",
                value: "/images/case_1/evidence/tire_iron.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 35,
                column: "ImageUrl",
                value: "/images/case_1/evidence/burnt_ledger.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 36,
                column: "ImageUrl",
                value: "/images/case_1/evidence/cctv_still.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 37,
                column: "ImageUrl",
                value: "/images/case_1/evidence/payment_envelope.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 38,
                column: "ImageUrl",
                value: "/images/case_1/evidence/alley_photo.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 39,
                column: "ImageUrl",
                value: "/images/case_1/evidence/text_exchange.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "ImageUrl",
                value: "/images/case_1/evidence/barangay_logbook.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 41,
                column: "ImageUrl",
                value: "/images/case_1/evidence/shoeprint.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 42,
                column: "ImageUrl",
                value: "/images/case_1/evidence/hospital_log.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 23,
                column: "ImageUrl",
                value: "/images/case_1/suspects/elena_padilla.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 24,
                column: "ImageUrl",
                value: "/images/case_1/suspects/dindo_reyes.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 25,
                column: "ImageUrl",
                value: "/images/case_1/suspects/ferdie_bautista.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 26,
                column: "ImageUrl",
                value: "/images/case_1/suspects/grace_sison.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 27,
                column: "ImageUrl",
                value: "/images/case_1/suspects/rey_villamor.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 19,
                column: "ImageUrl",
                value: "/images/case_1/witnesses/baby.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 20,
                column: "ImageUrl",
                value: "/images/case_1/witnesses/aling_nena.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 21,
                column: "ImageUrl",
                value: "/images/case_1/witnesses/jun.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 22,
                column: "ImageUrl",
                value: "/images/case_1/witnesses/dinner_companion.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 34,
                column: "ImageUrl",
                value: "/images/evidence/tire_iron.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 35,
                column: "ImageUrl",
                value: "/images/evidence/burnt_ledger.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 36,
                column: "ImageUrl",
                value: "/images/evidence/cctv_still.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 37,
                column: "ImageUrl",
                value: "/images/evidence/payment_envelope.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 38,
                column: "ImageUrl",
                value: "/images/evidence/alley_photo.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 39,
                column: "ImageUrl",
                value: "/images/evidence/text_exchange.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "ImageUrl",
                value: "/images/evidence/barangay_logbook.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 41,
                column: "ImageUrl",
                value: "/images/evidence/shoeprint.jpg");

            migrationBuilder.UpdateData(
                table: "EvidenceItems",
                keyColumn: "Id",
                keyValue: 42,
                column: "ImageUrl",
                value: "/images/evidence/hospital_log.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 23,
                column: "ImageUrl",
                value: "/images/suspects/elena_padilla.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 24,
                column: "ImageUrl",
                value: "/images/suspects/dindo_reyes.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 25,
                column: "ImageUrl",
                value: "/images/suspects/ferdie_bautista.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 26,
                column: "ImageUrl",
                value: "/images/suspects/grace_sison.jpg");

            migrationBuilder.UpdateData(
                table: "Suspects",
                keyColumn: "Id",
                keyValue: 27,
                column: "ImageUrl",
                value: "/images/suspects/rey_villamor.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 19,
                column: "ImageUrl",
                value: "/images/witnesses/baby.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 20,
                column: "ImageUrl",
                value: "/images/witnesses/aling_nena.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 21,
                column: "ImageUrl",
                value: "/images/witnesses/jun.jpg");

            migrationBuilder.UpdateData(
                table: "Witnesses",
                keyColumn: "Id",
                keyValue: 22,
                column: "ImageUrl",
                value: "/images/witnesses/dinner_companion.jpg");
        }
    }
}
