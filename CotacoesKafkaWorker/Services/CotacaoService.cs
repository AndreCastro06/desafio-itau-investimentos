using CotacoesKafkaWorker.DTOs;
using CotacoesKafkaWorker.Interfaces;
using InvestmentControlApi.Domain.Entities;
using InvestmentControlApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CotacoesKafkaWorker.Services
{
    public class CotacaoService : ICotacaoService
    {
        private readonly InvestmentDbContext _context;

        public CotacaoService(InvestmentDbContext context)
        {
            _context = context;
        }

        public async Task SalvarCotacaoAsync(CotacaoKafkaDTO cotacaoDto)
        {
            var ativo = await _context.Ativos
                .FirstOrDefaultAsync(a => a.Codigo == cotacaoDto.Ativo);

            if (ativo == null)
            {
                ativo = new Ativo
                {
                    Codigo = cotacaoDto.Ativo,
                    Nome = cotacaoDto.Ativo
                };

                _context.Ativos.Add(ativo);
                await _context.SaveChangesAsync();
            }

            var existe = await _context.Cotacoes
                .AnyAsync(c => c.AtivoId == ativo.Id && c.DataHora == cotacaoDto.DataHora);

            if (existe)
                return;

            var novaCotacao = new Cotacao
            {
                AtivoId = ativo.Id,
                PrecoUnitario = cotacaoDto.Preco,
                DataHora = cotacaoDto.DataHora
            };

            _context.Cotacoes.Add(novaCotacao);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CotacaoExisteAsync(string ativo, DateTime dataHora)
        {
            return await _context.Cotacoes
                .Include(c => c.Ativo)
                .AnyAsync(c => c.Ativo.Codigo == ativo && c.DataHora == dataHora);
        }
    }
}
