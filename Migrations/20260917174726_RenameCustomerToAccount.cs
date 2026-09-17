using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artway.Migrations
{
    /// <inheritdoc />
    public partial class RenameCustomerToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Accounts"
                );

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Accounts",
                newName: "AccountId"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Accounts",
                newName: "CustomerId"
                );

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Customers"
                );
        }
    }
}
