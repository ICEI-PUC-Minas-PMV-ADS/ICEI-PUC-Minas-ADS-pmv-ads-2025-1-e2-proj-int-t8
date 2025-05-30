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

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
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

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
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

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
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

        // 🔥 Aqui entra a Action de perfil exclusiva do usuário final
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
                CRECI = usuario.CRECI
            };

            return View(model);
        }

        private bool UsuarioExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}

