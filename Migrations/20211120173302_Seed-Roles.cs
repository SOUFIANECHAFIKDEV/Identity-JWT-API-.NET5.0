using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityAPI.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "08a876a7-7323-4ee8-acbd-657718419736", "cc4d153e-163b-4825-b872-3c48e1bd3197", "superAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "99bb7555-3a87-48d8-837f-7ce98a717f39", "9ce7801c-77ed-4586-80e6-38f4400102cb", "admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "08a876a7-7323-4ee8-acbd-657718419736");

            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "99bb7555-3a87-48d8-837f-7ce98a717f39");
        }
    }
}
