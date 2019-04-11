using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StrongStart.Migrations
{
    public partial class StrongStartDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    regionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    regionCode = table.Column<string>(nullable: true),
                    regionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.regionID);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    termID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    termName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.termID);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    siteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    siteName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    geoLat = table.Column<double>(nullable: false),
                    geoLng = table.Column<double>(nullable: false),
                    regionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.siteID);
                    table.ForeignKey(
                        name: "FK_Sites_Regions_regionID",
                        column: x => x.regionID,
                        principalTable: "Regions",
                        principalColumn: "regionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    trainerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<int>(nullable: false),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    regionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.trainerID);
                    table.ForeignKey(
                        name: "FK_Trainers_Regions_regionID",
                        column: x => x.regionID,
                        principalTable: "Regions",
                        principalColumn: "regionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    trainingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    siteID = table.Column<int>(nullable: false),
                    termID = table.Column<int>(nullable: false),
                    startTime = table.Column<DateTime>(nullable: false),
                    endTime = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    permit = table.Column<int>(nullable: false),
                    specInstructions = table.Column<string>(nullable: true),
                    part = table.Column<int>(nullable: false),
                    Capacity = table.Column<int>(nullable: false),
                    training_Status = table.Column<int>(nullable: false),
                    training_Progress_Status = table.Column<int>(nullable: false),
                    linkID = table.Column<int>(nullable: true),
                    trainingName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.trainingID);
                    table.ForeignKey(
                        name: "FK_Trainings_Sites_siteID",
                        column: x => x.siteID,
                        principalTable: "Sites",
                        principalColumn: "siteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trainings_Terms_termID",
                        column: x => x.termID,
                        principalTable: "Terms",
                        principalColumn: "termID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    volunteerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    infoID = table.Column<int>(nullable: false),
                    prefSchool = table.Column<int>(nullable: false),
                    siteID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.volunteerID);
                    table.ForeignKey(
                        name: "FK_Volunteers_Sites_siteID",
                        column: x => x.siteID,
                        principalTable: "Sites",
                        principalColumn: "siteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Training_Trainers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    trainingID = table.Column<int>(nullable: false),
                    trainerID = table.Column<int>(nullable: false),
                    becomeTrainer = table.Column<int>(nullable: false),
                    traineeStatus = table.Column<int>(nullable: false),
                    hasKit = table.Column<int>(nullable: false),
                    driveDistance = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training_Trainers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Training_Trainers_Trainers_trainerID",
                        column: x => x.trainerID,
                        principalTable: "Trainers",
                        principalColumn: "trainerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Training_Trainers_Trainings_trainingID",
                        column: x => x.trainingID,
                        principalTable: "Trainings",
                        principalColumn: "trainingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Training_Volunteers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    volunteerID = table.Column<string>(nullable: true),
                    trainingID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training_Volunteers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Training_Volunteers_Trainings_trainingID",
                        column: x => x.trainingID,
                        principalTable: "Trainings",
                        principalColumn: "trainingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "regionID", "regionCode", "regionName" },
                values: new object[,]
                {
                    { 1, "WR", "Waterloo Region" },
                    { 2, "WL", "Wellington Region" },
                    { 3, "PR", "Perth Region" }
                });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "termID", "termName" },
                values: new object[,]
                {
                    { 1, "F2019" },
                    { 2, "S2019" },
                    { 3, "F2020" }
                });

            migrationBuilder.InsertData(
                table: "Sites",
                columns: new[] { "siteID", "Address", "City", "Phone", "PostalCode", "Province", "geoLat", "geoLng", "regionID", "siteName" },
                values: new object[,]
                {
                    { 2, "299 Doon Valley", "Kitchener", "987456213", "N2G 4M4", "ON", 0.0, 0.0, 1, "Conestoga College" },
                    { 1, "406 Paisley Rd", "Guelph", "5198220675", "N1H 2R3", "ON", 0.0, 0.0, 2, "Paisley Road Public School" },
                    { 3, "905 Scotland St", "Fergus", "519843-2500", "N1M 1Y7", "ON", 0.0, 0.0, 2, "Centre Wellington District High School" },
                    { 4, "60 St Andrew St", "Stratford", "5192714500", "N5A 1A3", "ON", 0.0, 0.0, 3, "Stratford Central Secondary School" }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "trainerID", "Address", "City", "Country", "Email", "Phone", "Province", "Title", "firstName", "lastName", "regionID" },
                values: new object[,]
                {
                    { 1, "5 Rittenhouse Rd", "Waterloo", "Canada", "wangqiao921105@gmail.com", "123456789", "ON", 0, "qiao", "wang", 1 },
                    { 2, "10 Rittenhouse Rd", "Guelph", "Canada", "1@1.com", "123456789", "ON", 1, "mao", "qiu", 2 }
                });

            migrationBuilder.InsertData(
                table: "Trainings",
                columns: new[] { "trainingID", "Capacity", "Date", "endTime", "linkID", "part", "permit", "siteID", "specInstructions", "startTime", "termID", "trainingName", "training_Progress_Status", "training_Status" },
                values: new object[,]
                {
                    { 1, 30, new DateTime(2019, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1, 2, null, new DateTime(2019, 10, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, null, 0, 0 },
                    { 2, 40, new DateTime(2019, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 12, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1, 1, null, new DateTime(2019, 10, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, null, 1, 0 },
                    { 3, 20, new DateTime(2019, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1, 3, null, new DateTime(2019, 10, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), 2, null, 2, 1 },
                    { 5, 15, new DateTime(2019, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 11, 17, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1, 3, null, new DateTime(2019, 10, 11, 15, 0, 0, 0, DateTimeKind.Unspecified), 1, null, 3, 0 },
                    { 4, 10, new DateTime(2019, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 27, 15, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1, 4, null, new DateTime(2019, 10, 27, 13, 0, 0, 0, DateTimeKind.Unspecified), 1, null, 2, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sites_regionID",
                table: "Sites",
                column: "regionID");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_regionID",
                table: "Trainers",
                column: "regionID");

            migrationBuilder.CreateIndex(
                name: "IX_Training_Trainers_trainerID",
                table: "Training_Trainers",
                column: "trainerID");

            migrationBuilder.CreateIndex(
                name: "IX_Training_Trainers_trainingID",
                table: "Training_Trainers",
                column: "trainingID");

            migrationBuilder.CreateIndex(
                name: "IX_Training_Volunteers_trainingID",
                table: "Training_Volunteers",
                column: "trainingID");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_siteID",
                table: "Trainings",
                column: "siteID");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_termID",
                table: "Trainings",
                column: "termID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_siteID",
                table: "Volunteers",
                column: "siteID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Training_Trainers");

            migrationBuilder.DropTable(
                name: "Training_Volunteers");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
