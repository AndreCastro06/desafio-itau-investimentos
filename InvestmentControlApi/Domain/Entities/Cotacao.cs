namespace InvestmentControlApi.Domain.Entities
{
    public class Cotacao
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }
        public Ativo Ativo { get; set; } = null!;

        public decimal Preco { get; set; }
        public DateTime Data { get; set; }
    }
}