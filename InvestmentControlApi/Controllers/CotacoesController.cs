using InvestmentControlApi.Infrastructure.Data;
using InvestmentControlApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentControlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotacoesController : ControllerBase
    {
        private readonly InvestmentDbContext _context;

        public CotacoesController(InvestmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cotacao>>> GetTodas()
        {
            return await _context.Cotacoes
                .Include(c => c.Ativo)
                .OrderByDescending(c => c.DataHora)
                .ToListAsync();
        }

        [HttpGet("ultimo/{ativoId}")]
        public async Task<ActionResult<Cotacao>> GetUltimaCotacao(int ativoId)
        {
            var cotacao = await _context.Cotacoes
                .Where(c => c.AtivoId == ativoId)
                .OrderByDescending(c => c.DataHora)
                .FirstOrDefaultAsync();

            if (cotacao == null)
                return NotFound();

            return cotacao;
        }

        [HttpPost]
        public async Task<ActionResult<Cotacao>> PostCotacao(Cotacao cotacao)
        {
            _context.Cotacoes.Add(cotacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUltimaCotacao), new { ativoId = cotacao.AtivoId }, cotacao);
        }
    }
}