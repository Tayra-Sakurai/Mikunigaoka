using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakaishi.Migrations
{
    /// <inheritdoc />
    public partial class AddedConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Balance",
                table: "Items",
                sql: "[Expense] > 0 AND [Income] = 0 OR [Expense] = 0 AND [Income] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Balance",
                table: "Items");
        }
    }
}
