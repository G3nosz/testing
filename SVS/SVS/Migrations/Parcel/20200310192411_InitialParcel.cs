namespace SVS.Migrations.Parcel
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialParcel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Receiver",
                table => new
                {
                    ID          = table.Column<string>(),
                    Name        = table.Column<string>(),
                    Address     = table.Column<string>(),
                    Email       = table.Column<string>(),
                    PhoneNumber = table.Column<string>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receiver", x => x.ID);
                });

            migrationBuilder.CreateTable(
                "Sender",
                table => new
                {
                    ID          = table.Column<string>(),
                    Name        = table.Column<string>(),
                    Address     = table.Column<string>(),
                    Email       = table.Column<string>(),
                    PhoneNumber = table.Column<string>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sender", x => x.ID);
                });

            migrationBuilder.CreateTable(
                "Parcels",
                table => new
                {
                    ID         = table.Column<string>(),
                    Weight     = table.Column<float>(),
                    Length     = table.Column<float>(),
                    Width      = table.Column<float>(),
                    Height     = table.Column<float>(),
                    Fragile    = table.Column<bool>(),
                    Status     = table.Column<string>(nullable: true),
                    Driver     = table.Column<string>(nullable: true),
                    ReceiverID = table.Column<string>(nullable: true),
                    SenderID   = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.ID);
                    table.ForeignKey(
                        "FK_Parcels_Receiver_ReceiverID",
                        x => x.ReceiverID,
                        "Receiver",
                        "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Parcels_Sender_SenderID",
                        x => x.SenderID,
                        "Sender",
                        "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Parcels_ReceiverID",
                "Parcels",
                "ReceiverID");

            migrationBuilder.CreateIndex(
                "IX_Parcels_SenderID",
                "Parcels",
                "SenderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Parcels");

            migrationBuilder.DropTable(
                "Receiver");

            migrationBuilder.DropTable(
                "Sender");
        }
    }
}