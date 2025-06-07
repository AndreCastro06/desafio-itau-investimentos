using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentControlApi.Domain.Entities
{
    [Table("cotacoes")]
    public class Cotacao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("ativo_id")]
        public int AtivoId { get; set; }

        [Column("preco_unitario", TypeName = "decimal(18,4)")]
        public decimal PrecoUnitario { get; set; }

        [Column("data_hora")]
        public DateTime DataHora { get; set; }

        [Column("preco_abertura", TypeName = "decimal(18,4)")]
        public decimal PrecoAbertura { get; set; }

        [Column("preco_maximo_dia", TypeName = "decimal(18,4)")]
        public decimal PrecoMaximoDia { get; set; }

        [Column("preco_minimo_dia", TypeName = "decimal(18,4)")]
        public decimal PrecoMinimoDia { get; set; }

        [Column("volume")]
        public long Volume { get; set; }

        [Column("pe", TypeName = "decimal(18,4)")]
        public decimal? PE { get; set; }

        [Column("eps", TypeName = "decimal(18,4)")]
        public decimal? EPS { get; set; }

        public Ativo? Ativo { get; set; }
    }
}