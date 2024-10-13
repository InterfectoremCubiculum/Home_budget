using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home_budget_library.Migrations
{
    /// <inheritdoc />
    public partial class Init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_UserID",
                table: "Incomes",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users_UserID",
                table: "Incomes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users_UserID",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_UserID",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Expenses");
        }
    }
}
