using System.Collections.Generic;

namespace LanceCertoSQL.ViewModels
{
    public class PregaoListaViewModel
    {
        public List<PregaoItemViewModel> Leiloes { get; set; } = new List<PregaoItemViewModel>();

        // Filtros
        public string CidadeSelecionada { get; set; }
        public List<string> CidadesDisponiveis { get; set; } = new List<string>();

        public string FaixaValorSelecionada { get; set; }
        public List<string> FaixasValor { get; set; } = new List<string>
        {
            "<400000",      // Interpretação: até R$400.000
            "400000-900000",  // R$400.000 a R$900.000
            ">900000"       // Acima de R$900.000
        };


        public string DonoSelecionado { get; set; } // "Meus" ou "Todos"
        public string StatusSelecionado { get; set; } // Pendente, Ativo, Finalizado

        public class PregaoItemViewModel
        {
            public int Id { get; set; }
            public string ImovelNome { get; set; }
            public string Cidade { get; set; }
            public decimal ValorMinimo { get; set; }
            public string Status { get; set; }
            public string NomeDono { get; set; }
        }
    }
}
