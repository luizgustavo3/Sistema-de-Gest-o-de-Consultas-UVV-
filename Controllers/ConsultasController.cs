using System.Security.Claims;
using ConsultaUVV.Data;
using ConsultaUVV.Models;
using ConsultaUVV.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsultaUVV.Controllers
{
    [Authorize] // Só usuários autenticados acessam qualquer ação deste controller.
    public class ConsultasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultasController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int UsuarioLogadoId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /Consultas
        public async Task<IActionResult> Index()
        {
            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == UsuarioLogadoId)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return View(consultas);
        }

        // GET: /Consultas/Create
        public IActionResult Create()
        {
            return View(new ConsultaViewModel());
        }

        // POST: /Consultas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var consulta = new Consulta
            {
                Especialidade = model.Especialidade,
                DataHora = model.DataHora,
                Descricao = model.Descricao,
                UsuarioId = UsuarioLogadoId
            };

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Consulta agendada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Consultas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var consulta = await BuscarConsultaDoUsuario(id);
            if (consulta is null) return NotFound();

            var model = new ConsultaViewModel
            {
                Id = consulta.Id,
                Especialidade = consulta.Especialidade,
                DataHora = consulta.DataHora,
                Descricao = consulta.Descricao
            };

            return View(model);
        }

        // POST: /Consultas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConsultaViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var consulta = await BuscarConsultaDoUsuario(id);
            if (consulta is null) return NotFound();

            consulta.Especialidade = model.Especialidade;
            consulta.DataHora = model.DataHora;
            consulta.Descricao = model.Descricao;

            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Consulta atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Consultas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await BuscarConsultaDoUsuario(id);
            if (consulta is null) return NotFound();

            return View(consulta);
        }

        // POST: /Consultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await BuscarConsultaDoUsuario(id);
            if (consulta is null) return NotFound();

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Consulta excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // Garante que um usuário só acesse/edite/exclua as PRÓPRIAS consultas.
        private async Task<Consulta?> BuscarConsultaDoUsuario(int id)
        {
            return await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);
        }
    }
}
