using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvestmentControlApi.Domain.Enums;

namespace InvestmentControlApi.Domain.Entities
{
    [Table("operacoes")]
    public class Operacao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("ativo_id")]
        public int AtivoId { get; set; }

        [Column("quantidade")]
        public int Quantidade { get; set; }

        [Column("preco_unitario", TypeName = "decimal(18,4)")]
        public decimal PrecoUnitario { get; set; }

        [Column("tipo_operacao", TypeName = "varchar(20)")]
        public TipoOperacao TipoOperacao { get; set; }

        [Column("corretagem", TypeName = "decimal(18,4)")]
        public decimal Corretagem { get; set; }

        [Column("data_hora")]
        public DateTime DataHora { get; set; }

        public Usuario? Usuario { get; set; }
        public Ativo? Ativo { get; set; }
    }
}