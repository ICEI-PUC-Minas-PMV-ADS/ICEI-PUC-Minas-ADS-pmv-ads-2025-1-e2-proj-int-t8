using LanceCertoSQL.Models;

namespace LanceCertoSQL.ViewModels
{
    public class UsuarioPerfilViewModel
    {
        public string Nome { get; set; }
        public string UserName { get; set; }
        public string? FotoPerfil { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Estado { get; set; }
        public StatusConta Status { get; set; }
        public string? CRECI { get; set; }
    }
}
