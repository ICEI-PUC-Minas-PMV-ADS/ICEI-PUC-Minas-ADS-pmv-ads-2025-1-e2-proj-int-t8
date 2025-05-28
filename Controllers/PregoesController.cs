using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LanceCertoSQL.Models;
using Microsoft.AspNetCore.Identity;

namespace LanceCertoSQL.Controllers
{
    public class PregoesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public PregoesController(AppDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Pregoes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Pregoes.Include(p => p.Imovel).Include(p => p.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Pregoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregao = await _context.Pregoes
                .Include(p => p.Imovel)
                .Include(p => p.Usuario)
                .Include(p => p.Lances)
                    .ThenInclude(l => l.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pregao == null)
            {
                return NotFound();
            }

            return View(pregao);
        }

        // GET: Pregoes/Create
        public IActionResult Create()
        {
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "Id", "Nome");
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome");
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusPregao)));
            return View();
        }

        // POST: Pregoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImovelId,UsuarioId,ValorMinimo,DataInicio,DataFim,Status")] Pregao pregao)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                ViewData["ImovelId"] = new SelectList(_context.Imoveis, "Id", "Nome", pregao?.ImovelId);
                ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", pregao?.UsuarioId);
                ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusPregao)), pregao.Status);

                return View(pregao);
            }

            _context.Add(pregao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pregoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregao = await _context.Pregoes
                .Include(p => p.Imovel)
                .Include(p => p.Usuario)
                .Include(p => p.Lances)
                    .ThenInclude(l => l.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pregao == null)
            {
                return NotFound();
            }
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "Id", "Nome", pregao?.ImovelId);
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", pregao?.UsuarioId);
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusPregao)), pregao.Status);

            return View(pregao);
        }

        // POST: Pregoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pregao pregao)
        {
            if (id != pregao.Id)
            {
                Console.WriteLine("🔎 O id recebido não corresponde ao pregao.Id");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState inválido no Edit POST");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Erro de validação: " + error.ErrorMessage);
                }

                ViewData["ImovelId"] = new SelectList(_context.Imoveis, "Id", "Nome", pregao?.ImovelId);
                ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", pregao?.UsuarioId);
                ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusPregao)), pregao.Status);

                return View(pregao);
            }

            try
            {
                _context.Update(pregao);
                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Atualização realizada com sucesso.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("⚠️ Erro de concorrência: " + ex.Message);
                if (!PregaoExists(pregao.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Pregoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregao = await _context.Pregoes
                .Include(p => p.Imovel)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregao == null)
            {
                return NotFound();
            }

            return View(pregao);
        }

        // POST: Pregoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pregao = await _context.Pregoes.FindAsync(id);
            if (pregao != null)
            {
                _context.Pregoes.Remove(pregao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pregoes/Finalizar/5
        public async Task<IActionResult> Finalizar(int? id)
        {
            if (id == null) return NotFound();

            var pregao = await _context.Pregoes
                .Include(p => p.Lances)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pregao == null) return NotFound();

            var maiorLance = pregao.Lances.OrderByDescending(l => l.Valor).FirstOrDefault();

            if (maiorLance != null)
            {
                pregao.UsuarioVencedorId = maiorLance.UsuarioId;
                pregao.NomeVencedor = (await _userManager.FindByIdAsync(maiorLance.UsuarioId.ToString()))?.Nome;
                pregao.Status = StatusPregao.Finalizado;
            }
            else
            {
                pregao.UsuarioVencedorId = null;
                pregao.NomeVencedor = "Não houve vencedor";
                pregao.Status = StatusPregao.Finalizado;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = pregao.Id });
        }

        private bool PregaoExists(int id)
        {
            return _context.Pregoes.Any(e => e.Id == id);
        }
    }
}

