using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTransfer.Api.Migrations
{
    /// <inheritdoc />
    public partial class Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Users_OwwnerId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_OwwnerId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "OwwnerId",
                table: "BankAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_OwnerId",
                table: "BankAccounts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Users_OwnerId",
                table: "BankAccounts",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Users_OwnerId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_OwnerId",
                table: "BankAccounts");

            migrationBuilder.AddColumn<int>(
                name: "OwwnerId",
                table: "BankAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_OwwnerId",
                table: "BankAccounts",
                column: "OwwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Users_OwwnerId",
                table: "BankAccounts",
                column: "OwwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
