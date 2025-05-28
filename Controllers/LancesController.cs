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
    public class LancesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public LancesController(AppDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Lances
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Lances.Include(l => l.Pregao).Include(l => l.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Lances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lance = await _context.Lances
                .Include(l => l.Pregao)
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lance == null)
            {
                return NotFound();
            }

            return View(lance);
        }

        // GET: Lances/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome");
            ViewData["PregaoId"] = new SelectList(_context.Pregoes.Include(p => p.Imovel)
                .Select(p => new { p.Id, NomePregao = p.Imovel.Nome }), "Id", "NomePregao");
            return View();
        }

        // POST: Lances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,PregaoId,Valor,DataHora")] Lance lance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", lance.UsuarioId);
            ViewData["PregaoId"] = new SelectList(_context.Pregoes.Include(p => p.Imovel)
                .Select(p => new { p.Id, NomePregao = p.Imovel.Nome }), "Id", "NomePregao", lance.PregaoId);
            return View(lance);
        }

        // GET: Lances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lance = await _context.Lances.FindAsync(id);
            if (lance == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", lance.UsuarioId);
            ViewData["PregaoId"] = new SelectList(_context.Pregoes.Include(p => p.Imovel)
                .Select(p => new { p.Id, NomePregao = p.Imovel.Nome }), "Id", "NomePregao", lance.PregaoId);
            return View(lance);
        }

        // POST: Lances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,PregaoId,Valor,DataHora")] Lance lance)
        {
            if (id != lance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanceExists(lance.Id))
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
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", lance.UsuarioId);
            ViewData["PregaoId"] = new SelectList(_context.Pregoes.Include(p => p.Imovel)
                .Select(p => new { p.Id, NomePregao = p.Imovel.Nome }), "Id", "NomePregao", lance.PregaoId);
            return View(lance);
        }

        // GET: Lances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lance = await _context.Lances
                .Include(l => l.Pregao)
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lance == null)
            {
                return NotFound();
            }

            return View(lance);
        }

        // POST: Lances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lance = await _context.Lances.FindAsync(id);
            if (lance != null)
            {
                _context.Lances.Remove(lance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LanceExists(int id)
        {
            return _context.Lances.Any(e => e.Id == id);
        }
    }
}
