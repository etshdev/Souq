using Microsoft.EntityFrameworkCore.Migrations;

namespace Souq.DataAccessLayer.Migrations
{
    public partial class AddCartDetailes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_carts_products_ProductId",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_carts_ProductId",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "carts");

            migrationBuilder.CreateTable(
                name: "cartDetailes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productcount = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartDetailes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cartDetailes_carts_CartId",
                        column: x => x.CartId,
                        principalTable: "carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cartDetailes_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cartDetailes_CartId",
                table: "cartDetailes",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_cartDetailes_ProductId",
                table: "cartDetailes",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartDetailes");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_carts_ProductId",
                table: "carts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_carts_products_ProductId",
                table: "carts",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
