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
                .GroupBy(o => new { o.UsuarioId, o.Usuario.Nome })
                .Select(g => new
                {
                    UsuarioId = g.Key.UsuarioId,
                    Usuario = g.Key.Nome,
                    TotalCorretagem = g.Sum(o => o.Corretagem)
                })
                .OrderByDescending(x => x.TotalCorretagem)
                .ToListAsync();

            return Ok(corretagemTotal);
        }

        [HttpGet("posicoes-total")]
        public async Task<IActionResult> GetValorTotalCarteira()
        {
            var total = await _context.Posicoes
                .SumAsync(p => p.Quantidade * p.PrecoAtual);

            return Ok(total);
        }

        [HttpGet("preco-medio")]
        public async Task<ActionResult<IEnumerable<PrecoMedioDTO>>> GetPrecoMedioPorUsuarioEAtivo()
        {
            var precosMedios = await _context.Operacoes
                .Include(o => o.Usuario)
                .Include(o => o.Ativo)
                .Where(o => o.TipoOperacao == TipoOperacao.Compra)
                .GroupBy(o => new { o.UsuarioId, o.Usuario.Nome, o.AtivoId, o.Ativo.Codigo })
                .Select(g => new PrecoMedioDTO
                {
                    Usuario = g.Key.Nome,
                    Ativo = g.Key.Codigo,
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
                .GroupBy(p => new { p.UsuarioId, p.Usuario.Nome })
                .Select(g => new
                {
                    Usuario = g.Key.Nome,
                    ValorTotalPL = g.Sum(p => p.PL)
                })
                .OrderByDescending(x => x.ValorTotalPL)
                .Take(10)
                .ToListAsync();

            return Ok(topUsuarios);
        }

        [HttpGet("posicoes-usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPosicoesPorUsuario(int usuarioId)
        {
            var posicoes = await _context.Posicoes
                .Include(p => p.Ativo)
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => new
                {
                    Ativo = p.Ativo.Codigo,
                    Quantidade = p.Quantidade,
                    PrecoMedio = p.PrecoMedio,
                    PrecoAtual = p.PrecoAtual,
                    PL = p.PL
                })
                .ToListAsync();

            return Ok(posicoes);
        }

        [HttpGet("resumo-financeiro/{usuarioId}")]
        public async Task<ActionResult<ResumoFinanceiroDTO>> GetResumoFinanceiro(int usuarioId)
        {
            var posicoes = await _context.Posicoes
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();

            if (posicoes == null || posicoes.Count == 0)
                return NotFound();

            var totalInvestido = posicoes.Sum(p => p.Quantidade * p.PrecoMedio);
            var carteiraAtual = posicoes.Sum(p => p.Quantidade * p.PrecoAtual);
            var lucroPrejuizo = carteiraAtual - totalInvestido;

            var resumo = new ResumoFinanceiroDTO
            {
                TotalInvestido = Math.Round(totalInvestido, 2),
                CarteiraAtual = Math.Round(carteiraAtual, 2),
                LucroPrejuizo = Math.Round(lucroPrejuizo, 2)
            };

            return Ok(resumo);
        }
    }
}