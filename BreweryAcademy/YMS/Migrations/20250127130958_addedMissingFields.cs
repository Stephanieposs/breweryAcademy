using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YMS.Migrations
{
    /// <inheritdoc />
    public partial class addedMissingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoices_Invoice",
                table: "InvoiceItem");

            migrationBuilder.RenameColumn(
                name: "Invoice",
                table: "InvoiceItem",
                newName: "InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItem_Invoice",
                table: "InvoiceItem",
                newName: "IX_InvoiceItem_InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Invoices_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoices_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "InvoiceItem",
                newName: "Invoice");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItem_InvoiceId",
                table: "InvoiceItem",
                newName: "IX_InvoiceItem_Invoice");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Invoices_Invoice",
                table: "InvoiceItem",
                column: "Invoice",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
