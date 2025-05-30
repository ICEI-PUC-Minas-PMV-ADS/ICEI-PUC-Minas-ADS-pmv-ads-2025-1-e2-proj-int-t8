using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LanceCertoSQL.Models;
using LanceCertoSQL.ViewModels; // Vamos criar um ViewModel simples para o Login
using Microsoft.Data.SqlClient;


namespace LanceCertoSQL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Account/Login
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            // Você pode remover a ViewData["ReturnUrl"] se for desnecessária
            // ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            int maxAttempts = 3;
            int delayMilliseconds = 5000;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {
                        Console.WriteLine($"Tentativa {attempt}: Usuário '{model.UserName}' não encontrado.");
                        ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        Console.WriteLine($"Login bem-sucedido para o usuário: {model.UserName}");
                        // 🔥 Redireciona diretamente para a página de perfil do usuário
                        return RedirectToAction("PerfilUsuario", "Usuarios");
                    }
                    else
                    {
                        if (result.IsLockedOut)
                        {
                            Console.WriteLine($"Usuário '{model.UserName}' bloqueado por muitas tentativas.");
                        }
                        else if (result.IsNotAllowed)
                        {
                            Console.WriteLine($"Usuário '{model.UserName}' não está autorizado a fazer login.");
                        }
                        else if (result.RequiresTwoFactor)
                        {
                            Console.WriteLine($"Usuário '{model.UserName}' requer autenticação em dois fatores.");
                        }
                        else
                        {
                            Console.WriteLine($"Tentativa {attempt}: Senha incorreta para o usuário '{model.UserName}'.");
                        }

                        if (attempt == maxAttempts)
                        {
                            ModelState.AddModelError(string.Empty, "Falha ao fazer login. Verifique as credenciais.");
                            return View(model);
                        }
                        break;
                    }
                }
                catch (SqlException ex) when (ex.Number == 40613)
                {
                    if (attempt == maxAttempts)
                    {
                        ModelState.AddModelError(string.Empty, "Banco de dados não disponível. Tente novamente mais tarde.");
                        return View(model);
                    }
                    await Task.Delay(delayMilliseconds);
                }
            }

            return View(model);
        }


        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

