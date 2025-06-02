using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LanceCertoSQL.ViewModels
{
    public class EditarPerfilUsuarioViewModel
    {
        [Required]
        public string Nome { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        public string? Estado { get; set; }

        public string? CRECI { get; set; }

        public IFormFile? FotoPerfil { get; set; }

        [DataType(DataType.Password)]
        public string? NovaSenha { get; set; }

        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "A senha de confirmação não confere.")]
        public string? ConfirmacaoSenha { get; set; }
    }
}
