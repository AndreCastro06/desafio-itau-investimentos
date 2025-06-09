using InvestmentControlApi.Infrastructure.Data;
using InvestmentControlApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InvestmentControlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly InvestmentDbContext _context;

        public UsuariosController(InvestmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            return usuario;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<object>>> BuscarUsuarios([FromQuery] string nome)
        {
            var usuarios = await _context.Usuarios
                .Where(u => string.IsNullOrEmpty(nome) || u.Nome.Contains(nome))
                .Select(u => new { u.Id, u.Nome })
                .ToListAsync();

            return Ok(usuarios);
        }
    }
}