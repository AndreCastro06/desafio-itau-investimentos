using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentControlApi.Domain.Entities
{
    [Table("ativos")]
    public class Ativo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("codigo", TypeName = "varchar(10)")]
        public string? Codigo { get; set; }

        [Column("nome", TypeName = "varchar(255)")]
        public string? Nome { get; set; }

        public ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
        public ICollection<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
        public ICollection<Posicao> Posicoes { get; set; } = new List<Posicao>();
    }
}