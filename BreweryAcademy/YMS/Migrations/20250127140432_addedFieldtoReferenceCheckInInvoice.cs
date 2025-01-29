using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YMS.Migrations
{
    /// <inheritdoc />
    public partial class addedFieldtoReferenceCheckInInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckIns_Invoices_InvoiceReferenced",
                table: "CheckIns");

            migrationBuilder.DropIndex(
                name: "IX_CheckIns_InvoiceReferenced",
                table: "CheckIns");

            migrationBuilder.AddColumn<int>(
                name: "CheckInId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CheckInId",
                table: "Invoices",
                column: "CheckInId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CheckIns_CheckInId",
                table: "Invoices",
                column: "CheckInId",
                principalTable: "CheckIns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CheckIns_CheckInId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CheckInId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CheckInId",
                table: "Invoices");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_InvoiceReferenced",
                table: "CheckIns",
                column: "InvoiceReferenced",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckIns_Invoices_InvoiceReferenced",
                table: "CheckIns",
                column: "InvoiceReferenced",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
