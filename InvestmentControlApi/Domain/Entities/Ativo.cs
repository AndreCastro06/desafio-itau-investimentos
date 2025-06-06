namespace InvestmentControlApi.Domain.Entities
{
    public class Ativo
    {
        public int Id { get; set; }
        public required string Ticker { get; set; }

        public ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
        public ICollection<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
    }
}