using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace LanceCertoSQL.ViewModels
{
    public class EditarPerfilUsuarioViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Display(Name = "CRECI")]
        public string? CRECI { get; set; }

        [Display(Name = "CEP")]
        public string? CEP { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [Display(Name = "Rua / Avenida")]
        public string? Rua { get; set; }

        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Display(Name = "Nova Senha")]
        [DataType(DataType.Password)]
        public string? NovaSenha { get; set; }

        [Display(Name = "Confirmação da Senha")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        [DataType(DataType.Password)]
        public string? ConfirmacaoSenha { get; set; }

        [Display(Name = "Foto de Perfil")]
        public IFormFile? FotoPerfil { get; set; }
    }
}
