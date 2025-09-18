using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeaTime.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addStoreRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Address", "City", "Description", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "台北市信義區忠孝東路65號", "台北市", "好貴好貴的店租。", "台北信義店", "0912345678" },
                    { 2, "台北市大安區大安路一段11號", "台北市", "熱鬧哦。", "台北大安店", "0922222222" },
                    { 3, "台北市新莊區建中街989號", "新北市", "便宜的地方。", "新北建中店", "0933333333" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
