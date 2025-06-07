using InvestmentControlApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvestmentControlApi.Application.DTOs;
using InvestmentControlApi.Domain.Enums;

namespace InvestmentControlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly InvestmentDbContext _context;

        public RelatoriosController(InvestmentDbContext context)
        {
            _context = context;
        }

        [HttpGet("corretagem-total")]
        public async Task<ActionResult<IEnumerable<object>>> GetCorretagemTotalPorUsuario()
        {
            var corretagemTotal = await _context.Operacoes
                .Include(o => o.Usuario)
                .Where(o => o.Usuario != null)
                .GroupBy(o => new { o.UsuarioId, UsuarioNome = o.Usuario!.Nome })
                .Select(g => new
                {
                    Usuario = g.Key.UsuarioNome,
                    TotalCorretagem = g.Sum(o => o.Corretagem)
                })
                .OrderByDescending(x => x.TotalCorretagem)
                .ToListAsync();

            return Ok(corretagemTotal);
        }

        [HttpGet("preco-medio")]
        public async Task<ActionResult<IEnumerable<PrecoMedioDTO>>> GetPrecoMedioPorUsuarioEAtivo()
        {
            var precosMedios = await _context.Operacoes
                .Include(o => o.Usuario)
                .Include(o => o.Ativo)
                .Where(o => o.TipoOperacao == TipoOperacao.Compra && o.Usuario != null && o.Ativo != null)
                .GroupBy(o => new
                {
                    o.UsuarioId,
                    UsuarioNome = o.Usuario!.Nome,
                    o.AtivoId,
                    AtivoCodigo = o.Ativo!.Codigo
                })
                .Select(g => new PrecoMedioDTO
                {
                    Usuario = g.Key.UsuarioNome,
                    Ativo = g.Key.AtivoCodigo,
                    PrecoMedio = Math.Round(
                        g.Sum(x => x.Quantidade * x.PrecoUnitario) / g.Sum(x => x.Quantidade), 2
                    )
                })
                .OrderBy(x => x.Usuario)
                .ThenBy(x => x.Ativo)
                .ToListAsync();

            return Ok(precosMedios);
        }

        [HttpGet("top-posicoes")]
        public async Task<ActionResult<IEnumerable<object>>> GetTop10UsuariosPorPosicao()
        {
            var topUsuarios = await _context.Posicoes
                .Include(p => p.Usuario)
                .Where(p => p.Usuario != null)
                .GroupBy(p => new { p.UsuarioId, UsuarioNome = p.Usuario!.Nome })
                .Select(g => new
                {
                    Usuario = g.Key.UsuarioNome,
                    ValorTotalPL = g.Sum(p => p.PL)
                })
                .OrderByDescending(x => x.ValorTotalPL)
                .Take(10)
                .ToListAsync();

            return Ok(topUsuarios);
        }
    }
}