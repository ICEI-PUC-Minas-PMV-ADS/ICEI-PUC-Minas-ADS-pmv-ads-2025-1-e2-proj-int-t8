using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LanceCertoSQL.Models;
using Microsoft.AspNetCore.Authorization;
using LanceCertoSQL.ViewModels;

namespace LanceCertoSQL.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuariosController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        public IActionResult Create()
        {
            return RedirectToAction("Index", "Cadastro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Email,Nome,FotoPerfil,Status,CRECI,DataNascimento,Estado")] Usuario usuario, string Password)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(usuario, Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(usuario);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,Nome,FotoPerfil,Status,CRECI,DataNascimento,Estado")] Usuario usuario)
        {
            if (id != usuario.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                    return NotFound();

                existingUser.UserName = usuario.UserName;
                existingUser.Email = usuario.Email;
                existingUser.Nome = usuario.Nome;
                existingUser.FotoPerfil = usuario.FotoPerfil;
                existingUser.Status = usuario.Status;
                existingUser.CRECI = usuario.CRECI;
                existingUser.DataNascimento = usuario.DataNascimento;
                existingUser.Estado = usuario.Estado;

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(usuario);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                var result = await _userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(usuario);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> PerfilUsuario()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
                return RedirectToAction("Login", "Account");

            var model = new UsuarioPerfilViewModel
            {
                Nome = usuario.Nome,
                UserName = usuario.UserName,
                FotoPerfil = usuario.FotoPerfil,
                DataNascimento = usuario.DataNascimento,
                Estado = usuario.Estado,
                Status = usuario.Status,
                CRECI = usuario.CRECI,

                CEP = usuario.CEP,
                Cidade = usuario.Cidade,
                Bairro = usuario.Bairro,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Complemento = usuario.Complemento
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> EditarPerfilUsuario()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditarPerfilUsuarioViewModel
            {
                Nome = usuario.Nome,
                DataNascimento = usuario.DataNascimento,
                Estado = usuario.Estado,
                Cidade = usuario.Cidade,
                Bairro = usuario.Bairro,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Complemento = usuario.Complemento,
                CEP = usuario.CEP,
                CRECI = usuario.CRECI
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditarPerfilUsuario(EditarPerfilUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            usuario.Nome = model.Nome ?? usuario.Nome;
            usuario.DataNascimento = model.DataNascimento ?? usuario.DataNascimento;
            usuario.Estado = string.IsNullOrWhiteSpace(model.Estado) ? usuario.Estado : model.Estado;
            usuario.Cidade = string.IsNullOrWhiteSpace(model.Cidade) ? usuario.Cidade : model.Cidade;
            usuario.Bairro = string.IsNullOrWhiteSpace(model.Bairro) ? usuario.Bairro : model.Bairro;
            usuario.Rua = string.IsNullOrWhiteSpace(model.Rua) ? usuario.Rua : model.Rua;
            usuario.Numero = string.IsNullOrWhiteSpace(model.Numero) ? usuario.Numero : model.Numero;
            usuario.Complemento = string.IsNullOrWhiteSpace(model.Complemento) ? usuario.Complemento : model.Complemento;
            usuario.CEP = string.IsNullOrWhiteSpace(model.CEP) ? usuario.CEP : model.CEP;
            usuario.CRECI = string.IsNullOrWhiteSpace(model.CRECI) ? usuario.CRECI : model.CRECI;

            if (model.FotoPerfil != null)
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "perfis");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.FotoPerfil.FileName);
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FotoPerfil.CopyToAsync(stream);
                }

                usuario.FotoPerfil = "/img/perfis/" + fileName;
            }

            if (!string.IsNullOrEmpty(model.NovaSenha))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var result = await _userManager.ResetPasswordAsync(usuario, token, model.NovaSenha);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            var updateResult = await _userManager.UpdateAsync(usuario);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            return RedirectToAction("PerfilUsuario");
        }



        private bool UsuarioExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}

