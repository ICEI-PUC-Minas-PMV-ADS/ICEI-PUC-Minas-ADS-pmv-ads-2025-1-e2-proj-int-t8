using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LanceCertoSQL.Models;

namespace LanceCertoSQL.Controllers
{
    public class PregoesController : Controller
    {
        private readonly AppDbContext _context;

        public PregoesController(AppDbContext context)
        {
            _context = context;
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome");
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusPregao)));
            return View();
        }

        // POST: Pregoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", pregao?.UsuarioId);
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

            var pregao = await _context.Pregoes.FindAsync(id);
            if (pregao == null)
            {
                return NotFound();
            }
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "Id", "Nome", pregao?.ImovelId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", pregao?.UsuarioId);
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusPregao)), pregao.Status);

            return View(pregao);
        }

        // POST: Pregoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImovelId,UsuarioId,ValorMinimo,DataInicio,DataFim,Status")] Pregao pregao)
        {
            if (id != pregao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pregao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
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
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "Id", "Nome", pregao?.ImovelId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", pregao?.UsuarioId);
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusPregao)), pregao.Status);
            return View(pregao);
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

        private bool PregaoExists(int id)
        {
            return _context.Pregoes.Any(e => e.Id == id);
        }
    }
}
