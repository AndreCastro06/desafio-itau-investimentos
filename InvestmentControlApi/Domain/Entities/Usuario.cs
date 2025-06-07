using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentControlApi.Domain.Entities
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome", TypeName = "varchar(255)")]
        public string? Nome { get; set; }

        [Column("email", TypeName = "varchar(255)")]
        public string? Email { get; set; }

        [Column("corretagem", TypeName = "decimal(5,2)")]
        public decimal PercentualCorretagem { get; set; }

        public ICollection<Operacao> Operacoes  { get; set; } = new List<Operacao>();
        public ICollection<Posicao> Posicoes    { get; set; } = new List<Posicao>();
    }
}