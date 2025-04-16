using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeneratorStores.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class sseventh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_Products_ProductId1",
                table: "CategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_ProductId1",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "CategoryProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "CategoryProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_ProductId1",
                table: "CategoryProducts",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_Products_ProductId1",
                table: "CategoryProducts",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
