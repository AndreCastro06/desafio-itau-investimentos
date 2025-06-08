using InvestmentControlApi.Infrastructure.Data;
using InvestmentControlApi.Domain.Entities;
using InvestmentControlApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace InvestmentControlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperacoesController : ControllerBase
    {
        private readonly InvestmentDbContext _context;

        public OperacoesController(InvestmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operacao>>> GetTodas()
        {
            return await _context.Operacoes
                .Include(o => o.Usuario)
                .Include(o => o.Ativo)
                .ToListAsync();
        }

        [HttpGet("por-usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Operacao>>> GetPorUsuario(int usuarioId)
        {
            var operacoes = await _context.Operacoes
                .Where(o => o.UsuarioId == usuarioId)
                .Include(o => o.Ativo)
                .ToListAsync();

            return operacoes;
        }

        [HttpPost]
        public async Task<ActionResult<OperacaoResponseDTO>> PostOperacao(Operacao operacao)
        {
            var usuario = await _context.Usuarios.FindAsync(operacao.UsuarioId);
            var ativo = await _context.Ativos.FindAsync(operacao.AtivoId);

            if (usuario == null || ativo == null)
                return BadRequest("Usuário ou ativo não encontrado.");

            _context.Operacoes.Add(operacao);
            await _context.SaveChangesAsync();

            var response = new OperacaoResponseDTO
            {
                Id = operacao.Id,
                NomeUsuario = usuario.Nome,
                CodigoAtivo = ativo.Codigo,
                Quantidade = operacao.Quantidade,
                PrecoUnitario = operacao.PrecoUnitario,
                TipoOperacao = operacao.TipoOperacao.ToString(),
                Corretagem = operacao.Corretagem,
                DataHora = operacao.DataHora,
                TotalOperacao = operacao.Quantidade * operacao.PrecoUnitario + operacao.Corretagem
            };

            return CreatedAtAction(nameof(GetTodas), new { id = operacao.Id }, response);
        }
    }
}
