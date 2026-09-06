using System.Security.Claims;
using ConsultaUVV.Data;
using ConsultaUVV.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsultaUVV.Controllers
{
    /// <summary>
    /// API REST simples para testar os endpoints de Consulta pelo Swagger/Postman,
    /// além das telas MVC. Usa a MESMA autenticação por cookie: faça login em
    /// /Conta/Login antes de chamar estes endpoints pelo navegador/Swagger.
    /// </summary>
    [ApiController]
    [Route("api/consultas")]
    [Authorize]
    public class ConsultasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConsultasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int UsuarioLogadoId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET /api/consultas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetAll()
        {
            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == UsuarioLogadoId)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return Ok(consultas);
        }

        // GET /api/consultas/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Consulta>> GetById(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);

            return consulta is null ? NotFound() : Ok(consulta);
        }

        // POST /api/consultas
        [HttpPost]
        public async Task<ActionResult<Consulta>> Create(Consulta consulta)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            consulta.Id = 0;
            consulta.UsuarioId = UsuarioLogadoId;

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, consulta);
        }

        // PUT /api/consultas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Consulta consultaAtualizada)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);

            if (consulta is null) return NotFound();

            consulta.Especialidade = consultaAtualizada.Especialidade;
            consulta.DataHora = consultaAtualizada.DataHora;
            consulta.Descricao = consultaAtualizada.Descricao;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/consultas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);

            if (consulta is null) return NotFound();

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
