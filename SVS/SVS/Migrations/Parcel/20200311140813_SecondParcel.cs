namespace SVS.Migrations.Parcel
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SecondParcel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                "Number",
                "Parcels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Number",
                "Parcels");
        }
    }
}