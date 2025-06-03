using System;
using System.ComponentModel.DataAnnotations;

namespace LanceCertoSQL.ViewModels
{
    public class CadastroUsuarioViewModel
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [Display(Name = "Nome completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        public string CEP { get; set; }

        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }

        [Display(Name = "Número")]
        public string Numero { get; set; }

        public string Complemento { get; set; }

        [Display(Name = "CRECI (se aplicável)")]
        [DataType(DataType.Text)]
        public string? Creci { get; set; }

        public Guid? Id { get; set; }
        public string Status { get; set; } = "Usuário Comum";
    }
}
