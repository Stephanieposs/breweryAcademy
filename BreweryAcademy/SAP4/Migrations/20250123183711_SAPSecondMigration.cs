using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP4.Migrations
{
    /// <inheritdoc />
    public partial class SAPSecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceSAPs",
                table: "InvoiceSAPs");

            migrationBuilder.RenameTable(
                name: "InvoiceSAPs",
                newName: "InvoiceSAP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceSAP",
                table: "InvoiceSAP",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceSAP",
                table: "InvoiceSAP");

            migrationBuilder.RenameTable(
                name: "InvoiceSAP",
                newName: "InvoiceSAPs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceSAPs",
                table: "InvoiceSAPs",
                column: "Id");
        }
    }
}
