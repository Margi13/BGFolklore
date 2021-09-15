using Microsoft.EntityFrameworkCore.Migrations;

namespace BGFolklore.Data.Migrations
{
    public partial class RemoveDescriptionColumnsFromEthnoAreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagesDescription",
                table: "EthnographicAreas");

            migrationBuilder.DropColumn(
                name: "VideosDescription",
                table: "EthnographicAreas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagesDescription",
                table: "EthnographicAreas",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideosDescription",
                table: "EthnographicAreas",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
