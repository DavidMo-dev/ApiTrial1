using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTrial1.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADM_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADM_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ADM_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedByLog = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedByLog = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADM_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ADM_Users_ADM_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ADM_Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ADM_User_Accesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ADM_User_Id = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    CreateDate = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADM_User_Accesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ADM_User_Accesses_ADM_Users_ADM_User_Id",
                        column: x => x.ADM_User_Id,
                        principalTable: "ADM_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DCM_Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    File = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    UploadedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCM_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCM_Documents_ADM_Users_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "ADM_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADM_User_Accesses_ADM_User_Id",
                table: "ADM_User_Accesses",
                column: "ADM_User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ADM_Users_RoleId",
                table: "ADM_Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DCM_Documents_UploadedBy",
                table: "DCM_Documents",
                column: "UploadedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADM_User_Accesses");

            migrationBuilder.DropTable(
                name: "DCM_Documents");

            migrationBuilder.DropTable(
                name: "ADM_Users");

            migrationBuilder.DropTable(
                name: "ADM_Roles");
        }
    }
}
