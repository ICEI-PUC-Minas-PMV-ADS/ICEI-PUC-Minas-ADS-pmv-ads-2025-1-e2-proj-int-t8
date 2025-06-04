using System;
using System.ComponentModel.DataAnnotations;
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
        public string? CRECI { get; set; }
        public string? CEP { get; set; }
        public string? Cidade { get; set; }
        public string? Bairro { get; set; }
        public string? Rua { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }

        public StatusConta Status { get; set; }
    }
}
