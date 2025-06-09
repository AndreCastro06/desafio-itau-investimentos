using InvestmentControlApi.Application.Interfaces;
using InvestmentControlApi.Domain.Entities;
using InvestmentControlApi.Domain.Enums;
using InvestmentControlApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class PosicaoService : IPosicaoService
{
    private readonly InvestmentDbContext _context;
    private readonly IPrecoMedioService _precoMedioService;

    public PosicaoService(InvestmentDbContext context, IPrecoMedioService precoMedioService)
    {
        _context = context;
        _precoMedioService = precoMedioService;
    }

    public async Task AtualizarPosicoesAsync()
    {
        var operacoesAgrupadas = await _context.Operacoes
            .GroupBy(o => new { o.UsuarioId, o.AtivoId })
            .ToListAsync();

        foreach (var grupo in operacoesAgrupadas)
        {
            var operacoes = grupo.ToList();

            var quantidade = operacoes
                .Sum(o =>
                    o.TipoOperacao == TipoOperacao.Compra ? o.Quantidade :
                    o.TipoOperacao == TipoOperacao.Venda ? -o.Quantidade : 0
                );

            if (quantidade <= 0)
                continue;

            var precoMedio = _precoMedioService.CalcularPrecoMedio(operacoes);

            var ultimaCotacao = await _context.Cotacoes
                .Where(c => c.AtivoId == grupo.Key.AtivoId)
                .OrderByDescending(c => c.DataHora)
                .FirstOrDefaultAsync();

            if (ultimaCotacao == null)
                continue;

            var precoAtual = ultimaCotacao.PrecoUnitario;
            var pnl = (precoAtual - precoMedio) * quantidade;

            var posicaoExistente = await _context.Posicoes
                .FirstOrDefaultAsync(p => p.UsuarioId == grupo.Key.UsuarioId && p.AtivoId == grupo.Key.AtivoId);

            if (posicaoExistente != null)
            {
                posicaoExistente.Quantidade = quantidade;
                posicaoExistente.PrecoMedio = precoMedio;
                posicaoExistente.PrecoAtual = precoAtual;
                posicaoExistente.PL = pnl;
            }
            else
            {
                var novaPosicao = new Posicao
                {
                    UsuarioId = grupo.Key.UsuarioId,
                    AtivoId = grupo.Key.AtivoId,
                    Quantidade = quantidade,
                    PrecoMedio = precoMedio,
                    PrecoAtual = precoAtual,
                    PL = pnl
                };

                _context.Posicoes.Add(novaPosicao);
            }
        }

        await _context.SaveChangesAsync();
    }
}