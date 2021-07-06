using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace BGFolklore.Data.Migrations
{
    public partial class CreateTowns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Town",
                table: "PublicEvents");

            migrationBuilder.AddColumn<int>(
                name: "TownId",
                table: "PublicEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicEvents_TownId",
                table: "PublicEvents",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicEvents_Towns_TownId",
                table: "PublicEvents",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicEvents_Towns_TownId",
                table: "PublicEvents");

            migrationBuilder.DropTable(
                name: "Towns");

            migrationBuilder.DropIndex(
                name: "IX_PublicEvents_TownId",
                table: "PublicEvents");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "PublicEvents");

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "PublicEvents",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }
    }
}
