using LanceCertoSQL.Models;

namespace LanceCertoSQL.ViewModels
{
    public class BuscarImovelViewModel
    {
        public string? Cidade { get; set; }
        public string? UsuarioId { get; set; }
        public string? FaixaValor { get; set; }
        public string? TemLeilaoAtivo { get; set; } // "Sim", "Não" ou null

        public List<Imovel> Resultados { get; set; } = new List<Imovel>();

        public List<string> FaixasValor { get; set; } = new List<string>
        {
            "<400000",
            "400000-900000",
            ">900000"
        };

        public List<Usuario> UsuariosDisponiveis { get; set; } = new List<Usuario>();
    }
}

