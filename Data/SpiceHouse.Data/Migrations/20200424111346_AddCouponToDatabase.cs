namespace SpiceHouse.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddCouponToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CouponType = table.Column<string>(nullable: false),
                    ECouponType = table.Column<int>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    MinimumAmount = table.Column<decimal>(nullable: false),
                    Picture = table.Column<byte[]>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
