using System;

namespace InvestmentControlApi.Application.DTOs
{
    public class PrecoMedioDTO
    {
        public string Usuario { get; set; } = string.Empty;
        public string Ativo { get; set; } = string.Empty;
        public decimal PrecoMedio { get; set; }
    }
}