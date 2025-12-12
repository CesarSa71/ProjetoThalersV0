using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Projeto1.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaPessoas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Canal = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CodigoCliente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DataAdesao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CpfCnpj = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Nome = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RazaoSocial = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Endereco = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Cidade = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Cep = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RamoAtividade = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Celular = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CpfAgenteComercial = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NomeAgenteComercial = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Column21 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StatusDocumentacao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FaixaRenda = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OrigemRenda = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ComentarioOrigemRenda = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FaixaPatrimonio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}

