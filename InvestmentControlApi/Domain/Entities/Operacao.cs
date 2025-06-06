using InvestmentControlApi.Domain.Enums;

namespace InvestmentControlApi.Domain.Entities
{
    public class Operacao
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int AtivoId { get; set; }
        public Ativo Ativo { get; set; } = null!;

        public TipoOperacao Tipo { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal ValorCorretagem { get; set; }

        public DateTime Data { get; set; }
    }
}