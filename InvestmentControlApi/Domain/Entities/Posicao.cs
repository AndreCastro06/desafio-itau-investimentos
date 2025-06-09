using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentControlApi.Domain.Entities
{
    [Table("posicoes")]
    public class Posicao
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

        [Column("preco_medio", TypeName = "decimal(18,4)")]
        public decimal PrecoMedio { get; set; }

        [Column("p_l", TypeName = "decimal(18,4)")]
        public decimal PL { get; set; }

        [Column("preco_atual", TypeName = "decimal(18,4)")]
        public decimal PrecoAtual { get; set; }

        public Usuario? Usuario { get; set; }
        public Ativo? Ativo { get; set; }
    }
}