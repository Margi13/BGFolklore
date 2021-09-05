using Microsoft.EntityFrameworkCore.Migrations;

namespace BGFolklore.Data.Migrations
{
    public partial class AddForeignKeyFromPublicEventToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "PublicEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PublicEvents_StatusId",
                table: "PublicEvents",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicEvents_Status_StatusId",
                table: "PublicEvents",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicEvents_Status_StatusId",
                table: "PublicEvents");

            migrationBuilder.DropIndex(
                name: "IX_PublicEvents_StatusId",
                table: "PublicEvents");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PublicEvents");
        }
    }
}
