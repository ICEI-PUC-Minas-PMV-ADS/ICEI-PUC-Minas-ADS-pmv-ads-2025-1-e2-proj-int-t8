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
using Microsoft.AspNetCore.Authorization;

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
            return RedirectToAction("Andamento", "Pregoes", new { id = pregao.Id });
        }

        // GET: Pregao/Andamento/5
        [Authorize]
        public async Task<IActionResult> Andamento(int id)
        {
            var pregao = await _context.Pregoes
                .Include(p => p.Imovel)
                .Include(p => p.Usuario) // Vendedor
                .Include(p => p.Lances)
                    .ThenInclude(l => l.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pregao == null)
                return NotFound();

            var usuarioAtual = await _userManager.GetUserAsync(User);
            bool usuarioEhVendedor = pregao.UsuarioId == usuarioAtual.Id;

            // Pegando o maior lance (vencedor)
            var maiorLance = pregao.Lances.OrderByDescending(l => l.Valor).FirstOrDefault();

            var viewModel = new LeilaoAndamentoViewModel
            {
                LeilaoId = pregao.Id,
                ImovelNome = pregao.Imovel.Nome,
                ImovelDescricao = pregao.Imovel.Descricao,
                ImovelCidade = pregao.Imovel.Cidade,
                ValorMinimo = pregao.ValorMinimo,
                DataInicio = pregao.DataInicio,
                DataFim = pregao.DataFim,
                StatusLeilao = pregao.Status.ToString(),
                NomeVendedor = pregao.Usuario.Nome,
                UsuarioAtualEhVendedor = usuarioEhVendedor,
                Lances = pregao.Lances
                    .OrderByDescending(l => l.Valor)
                    .Select(l => new LeilaoAndamentoViewModel.LanceViewModel
                    {
                        NomeUsuario = l.Usuario.Nome,
                        Valor = l.Valor,
                        DataHora = l.DataHora
                    }).ToList(),
                NomeVencedor = pregao.NomeVencedor,  // Aqui pega do banco
                ValorVencedor = maiorLance?.Valor   // Se tiver vencedor, mostra valor
            };

            return View(viewModel);
        }

        //Action, dar Lance
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DarLance(int id, decimal valor)
        {
            var pregao = await _context.Pregoes
                .Include(p => p.Lances)
                .Include(p => p.Imovel)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pregao == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return Unauthorized();
            }

            // Validação: Valor mínimo e lance superior ao maior atual
            decimal maiorLance = pregao.Lances.Any() ? pregao.Lances.Max(l => l.Valor) : pregao.ValorMinimo;
            if (valor <= maiorLance)
            {
                ModelState.AddModelError("", $"O valor do lance deve ser maior que o lance atual: {maiorLance:C}");
                return RedirectToAction("Andamento", new { id });
            }

            var novoLance = new Lance
            {
                PregaoId = id,
                UsuarioId = usuario.Id,
                Valor = valor,
                DataHora = DateTime.Now
            };

            _context.Lances.Add(novoLance);
            await _context.SaveChangesAsync();

            return RedirectToAction("Andamento", new { id });
        }

        //GET: Registrar Leilão

        [Authorize]
        public async Task<IActionResult> RegistrarLeilao()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Pega os imóveis do usuário
            var imoveis = _context.Imoveis
                            .Where(i => i.UsuarioId == usuario.Id)
                            .Select(i => new SelectListItem
                            {
                                Value = i.Id.ToString(),
                                Text = i.Nome // Se não tem Nome, podemos usar "Imóvel ID #"
                            }).ToList();

            var model = new PregaoCreateViewModel
            {
                ImoveisDisponiveis = imoveis,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(7) // Padrão para 7 dias depois
            };

            return View(model);
        }

        //POST: Registrar leilão
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RegistrarLeilao(PregaoCreateViewModel model)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var pregao = new Pregao
                {
                    ImovelId = model.ImovelId,
                    UsuarioId = usuario.Id,
                    ValorMinimo = model.ValorMinimo,
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    Status = StatusPregao.Pendente
                };

                _context.Pregoes.Add(pregao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // Vai para a lista de leilões após cadastro
            }

            // Se der erro, recarrega os imóveis
            model.ImoveisDisponiveis = _context.Imoveis
                .Where(i => i.UsuarioId == usuario.Id)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Nome
                }).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Iniciar(int id)
        {
            var pregao = await _context.Pregoes.FindAsync(id);
            if (pregao == null)
                return NotFound();

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null || pregao.UsuarioId != usuario.Id)
                return Forbid(); // Garante que só o vendedor possa iniciar

            if (pregao.Status == StatusPregao.Pendente)
            {
                pregao.Status = StatusPregao.Ativo;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Andamento", new { id = pregao.Id });
        }


        private bool PregaoExists(int id)
        {
            return _context.Pregoes.Any(e => e.Id == id);
        }
    }
}

