using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YMS.Migrations
{
    /// <inheritdoc />
    public partial class addingInvoiceStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceStatus",
                table: "Invoices",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceId",
                table: "Invoices",
                column: "InvoiceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceStatus",
                table: "Invoices");
        }
    }
}
