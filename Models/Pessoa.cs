using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto1.Models;

[Table("Pessoas")]
public class Pessoa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(500)]
    [Column("canal")]
    public string? Canal { get; set; }

    [MaxLength(50)]
    [Column("codigocliente")]
    public string? CodigoCliente { get; set; }

    [Column("dataadesao")]
    public DateTime? DataAdesao { get; set; }

    [MaxLength(50)]
    [Column("status")]
    public string? Status { get; set; }

    [MaxLength(50)]
    [Column("cpfcnpj")]
    public string? CpfCnpj { get; set; }

    [MaxLength(500)]
    [Column("nome")]
    public string? Nome { get; set; }

    [MaxLength(500)]
    [Column("razaosocial")]
    public string? RazaoSocial { get; set; }

    [MaxLength(500)]
    [Column("endereco")]
    public string? Endereco { get; set; }

    [MaxLength(50)]
    [Column("numero")]
    public string? Numero { get; set; }

    [MaxLength(200)]
    [Column("complemento")]
    public string? Complemento { get; set; }

    [MaxLength(200)]
    [Column("bairro")]
    public string? Bairro { get; set; }

    [MaxLength(200)]
    [Column("cidade")]
    public string? Cidade { get; set; }

    [MaxLength(2)]
    [Column("estado")]
    public string? Estado { get; set; }

    [MaxLength(20)]
    [Column("cep")]
    public string? Cep { get; set; }

    [MaxLength(200)]
    [Column("ramoatividade")]
    public string? RamoAtividade { get; set; }

    [MaxLength(200)]
    [Column("email")]
    public string? Email { get; set; }

    [MaxLength(50)]
    [Column("celular")]
    public string? Celular { get; set; }

    [MaxLength(50)]
    [Column("telefone")]
    public string? Telefone { get; set; }

    [MaxLength(50)]
    [Column("cpfagentecomercial")]
    public string? CpfAgenteComercial { get; set; }

    [MaxLength(500)]
    [Column("nomeagentecomercial")]
    public string? NomeAgenteComercial { get; set; }

    [MaxLength(200)]
    [Column("column21")]
    public string? Column21 { get; set; }

    [MaxLength(100)]
    [Column("statusdocumentacao")]
    public string? StatusDocumentacao { get; set; }

    [MaxLength(100)]
    [Column("faixarenda")]
    public string? FaixaRenda { get; set; }

    [MaxLength(200)]
    [Column("origemrenda")]
    public string? OrigemRenda { get; set; }

    [MaxLength(1000)]
    [Column("comentarioorigemrenda")]
    public string? ComentarioOrigemRenda { get; set; }

    [MaxLength(100)]
    [Column("faixapatrimonio")]
    public string? FaixaPatrimonio { get; set; }
}