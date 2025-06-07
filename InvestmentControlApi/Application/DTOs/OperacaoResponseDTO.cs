using System;

public class OperacaoResponseDTO
{
    public int Id { get; set; }
    public string NomeUsuario { get; set; } = null!;
    public string CodigoAtivo { get; set; } = null!;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string TipoOperacao { get; set; } = null!;
    public decimal Corretagem { get; set; }
    public DateTime DataHora { get; set; }
    public decimal TotalOperacao { get; set; } // calculado: (Quantidade * PrecoUnitario) + Corretagem
}