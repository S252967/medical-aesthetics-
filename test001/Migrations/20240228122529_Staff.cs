using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test001.Migrations
{
    /// <inheritdoc />
    public partial class Staff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie",
                table: "staffdata");

            migrationBuilder.AddPrimaryKey(
                name: "PK_staffdata",
                table: "staffdata",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_staffdata",
                table: "staffdata");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie",
                table: "staffdata",
                column: "Id");
        }
    }
}
