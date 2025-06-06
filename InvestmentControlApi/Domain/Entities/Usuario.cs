namespace InvestmentControlApi.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nome { get; set; }

        public ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
    }
}