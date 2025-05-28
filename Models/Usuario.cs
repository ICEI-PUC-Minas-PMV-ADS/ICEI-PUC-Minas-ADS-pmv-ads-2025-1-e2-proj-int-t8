using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LanceCertoSQL.Models
{
    public enum StatusConta
    {
        Comum = 0,
        Administrador = 1
    }

    public class Usuario : IdentityUser
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome do Usuário")]
        public string Nome { get; set; }

        [Display(Name = "Foto de Perfil")]
        public string? FotoPerfil { get; set; }

        [Required]
        [Display(Name = "Status da Conta")]
        public StatusConta Status { get; set; }

        [Display(Name = "CRECI")]
        public string? CRECI { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        // Relacionamento com imóveis
        public ICollection<Imovel>? Imoveis { get; set; }
    }
}



