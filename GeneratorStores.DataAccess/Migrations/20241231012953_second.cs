using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeneratorStores.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "CategoryProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_CategoryId1",
                table: "CategoryProducts",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_Categories_CategoryId1",
                table: "CategoryProducts",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_Categories_CategoryId1",
                table: "CategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_CategoryId1",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "CategoryProducts");
        }
    }
}
