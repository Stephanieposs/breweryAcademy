using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YMS.Migrations
{
    /// <inheritdoc />
    public partial class removedFieldFromCheckIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceReferenced",
                table: "CheckIns");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceReferenced",
                table: "CheckIns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
