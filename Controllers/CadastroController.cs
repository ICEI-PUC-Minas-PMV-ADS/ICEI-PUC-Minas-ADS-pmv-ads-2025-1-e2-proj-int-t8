using LanceCertoSQL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanceCertoSQL.Controllers
{
    public class CadastroController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CadastroUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
