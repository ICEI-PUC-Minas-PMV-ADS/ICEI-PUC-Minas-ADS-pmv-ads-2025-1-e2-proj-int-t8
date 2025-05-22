using System.ComponentModel.DataAnnotations;

namespace LanceCertoSQL.Models
{
    public enum StatusConta
    {
        Comum = 0,
        Administrador = 1
    }

    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome do Usuário")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

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

        public ICollection<Imovel>? Imoveis { get; set; }
    }
}


