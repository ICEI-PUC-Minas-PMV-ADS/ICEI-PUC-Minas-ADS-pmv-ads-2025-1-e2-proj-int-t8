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
    public class ImoveisController : Controller
    {
        private readonly AppDbContext _context;

        public ImoveisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Imoveis
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Imoveis.Include(i => i.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Imoveis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

            // GET: Imoveis/Create
            public IActionResult Create()
            {
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome");
                return View();
            }

            // POST: Imoveis/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("Id,Nome,Foto,Cidade,ValorEstimado,UsuarioId,Descricao")] Imovel imovel)
        {
            if (ModelState.IsValid)
                {
                    _context.Add(imovel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", imovel.UsuarioId);

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("Erro de validação: " + error.ErrorMessage);
            }

            return View(imovel);
            }

            // GET: Imoveis/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var imovel = await _context.Imoveis.FindAsync(id);
                if (imovel == null)
                {
                    return NotFound();
                }
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", imovel.UsuarioId);
                return View(imovel);
            }

            // POST: Imoveis/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Foto,Cidade,ValorEstimado,UsuarioId")] Imovel imovel)

            {
            if (id != imovel.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(imovel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ImovelExists(imovel.Id))
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
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", imovel.UsuarioId);
                return View(imovel);
            }

        // GET: Imoveis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // POST: Imoveis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel != null)
            {
                _context.Imoveis.Remove(imovel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.Id == id);
        }
    }
}
