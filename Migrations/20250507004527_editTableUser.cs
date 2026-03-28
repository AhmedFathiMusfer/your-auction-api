using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace your_auction_api.Migrations
{
    /// <inheritdoc />
    public partial class editTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "state",
                table: "auctions",
                newName: "status");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "status",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "auctions",
                newName: "state");
        }
    }
}
