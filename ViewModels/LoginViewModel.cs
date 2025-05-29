using System.ComponentModel.DataAnnotations;

namespace LanceCertoSQL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }
    }
}
