using LanceCertoSQL.Models;
using LanceCertoSQL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LanceCertoSQL.Controllers
{
    public class CadastroController : Controller
    {
        private readonly UserManager<Usuario> _userManager;

        public CadastroController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CadastroUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = new Usuario
            {
                UserName = model.Username,
                Email = model.Email,
                Nome = model.NomeCompleto,
                DataNascimento = model.DataNascimento,
                Estado = model.Estado,
                CRECI = model.Creci,
                Status = StatusConta.Comum
            };

            var resultado = await _userManager.CreateAsync(usuario, model.Senha);

            if (resultado.Succeeded)
            {
                ViewBag.MensagemSucesso = "Usuário cadastrado com sucesso!";
                ViewBag.Redirecionar = true;
                return View();
            }

            TempData["MensagemErro"] = string.Join("<br>", resultado.Errors.Select(e => e.Description));


            return View(model);
        }
    }
}
