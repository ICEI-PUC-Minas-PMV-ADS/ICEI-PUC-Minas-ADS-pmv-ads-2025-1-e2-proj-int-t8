using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LanceCertoSQL.Models;
using Microsoft.AspNetCore.Identity;
using LanceCertoSQL.ViewModels;

namespace LanceCertoSQL.Controllers
{
    public class ImoveisController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IWebHostEnvironment _env;


        public ImoveisController(AppDbContext context, UserManager<Usuario> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
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
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome");
            return View();
        }

        // POST: Imoveis/Create
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
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", imovel.UsuarioId);

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
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", imovel.UsuarioId);
            return View(imovel);
        }

        // POST: Imoveis/Edit/5
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
            ViewData["UsuarioId"] = new SelectList(_userManager.Users.ToList(), "Id", "Nome", imovel.UsuarioId);
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

        //ACTION REGISTRAR IMOVEIS
        [HttpGet]
        public IActionResult RegistrarImovel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarImovel(ImovelCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _userManager.GetUserAsync(User);

            string? fotoPath = null;
            if (model.FotoUpload != null && model.FotoUpload.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.FotoUpload.FileName)}";
                var uploads = Path.Combine(_env.WebRootPath, "img", "imoveis");

                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FotoUpload.CopyToAsync(stream);
                }

                fotoPath = $"/img/imoveis/{fileName}";
            }

            var imovel = new Imovel
            {
                Nome = model.Nome,
                Cidade = model.Cidade,
                ValorEstimado = model.ValorEstimado,
                Descricao = model.Descricao,
                Foto = fotoPath,
                UsuarioId = usuario.Id
            };

            _context.Imoveis.Add(imovel);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home"); // Ou outro destino apropriado
        }

        //Action BuscarImoveis
        [HttpGet]
        public async Task<IActionResult> BuscarImoveis()
        {
            var model = new BuscarImovelViewModel
            {
                UsuariosDisponiveis = await _context.Users.ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> BuscarImoveis(BuscarImovelViewModel model)
        {
            var query = _context.Imoveis.Include(i => i.Usuario).AsQueryable();

            if (!string.IsNullOrEmpty(model.Cidade))
                query = query.Where(i => i.Cidade.Contains(model.Cidade));

            if (!string.IsNullOrEmpty(model.FaixaValor))
            {
                switch (model.FaixaValor)
                {
                    case "Até R$ 100.000":
                        query = query.Where(i => i.ValorEstimado <= 100000);
                        break;
                    case "R$ 100.000 - R$ 500.000":
                        query = query.Where(i => i.ValorEstimado > 100000 && i.ValorEstimado <= 500000);
                        break;
                    case "Acima de R$ 500.000":
                        query = query.Where(i => i.ValorEstimado > 500000);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(model.UsuarioId))
                query = query.Where(i => i.UsuarioId == model.UsuarioId);

            var resultados = await query.ToListAsync();

            var leiloesAtivos = await _context.Pregoes
                .Where(l => l.Status == StatusPregao.Ativo)
                .Select(l => l.ImovelId)
                .ToListAsync();


            ViewBag.LeiloesAtivos = leiloesAtivos;

            // ⚠️ Monta o ViewModel completo de volta para a View
            model.Resultados = resultados;

            // Repopula listas de apoio se necessário
            model.FaixasValor = new List<string>
            {
            "Até R$ 100.000",
            "R$ 100.000 - R$ 500.000",
            "Acima de R$ 500.000"
            };

            model.UsuariosDisponiveis = await _context.Users
                .Select(u => new Usuario { Id = u.Id, Nome = u.Nome })
                .ToListAsync();

            return View(model);
        }



        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.Id == id);
        }
    }
}
