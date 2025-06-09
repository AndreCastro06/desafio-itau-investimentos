using InvestmentControlApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InvestmentControlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PosicoesController : ControllerBase
    {
        private readonly IPosicaoService _posicaoService;

        public PosicoesController(IPosicaoService posicaoService)
        {
            _posicaoService = posicaoService;
        }

        [HttpPost("atualizar")]
        public async Task<IActionResult> Atualizar()
        {
            await _posicaoService.AtualizarPosicoesAsync();
            return Ok("Posicoes atualizadas com sucesso.");
        }
    }
}