namespace CotacoesKafkaWorker.DTOs
{
    public class CotacaoKafkaDTO
    {
        public string Ativo { get; set; } = string.Empty;   
        public decimal Preco { get; set; }
        public DateTime DataHora { get; set; }
    }
}