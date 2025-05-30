using System;
using System.Collections.Generic;

namespace LanceCertoSQL.ViewModels
{
    public class LeilaoAndamentoViewModel
    {
        public int LeilaoId { get; set; }
        public string ImovelNome { get; set; }
        public string ImovelDescricao { get; set; }
        public string ImovelCidade { get; set; }
        public decimal ValorMinimo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string StatusLeilao { get; set; }

        public string NomeVendedor { get; set; }
        public bool UsuarioAtualEhVendedor { get; set; }

        public List<LanceViewModel> Lances { get; set; } = new List<LanceViewModel>();
        public string NomeVencedor { get; set; }
        public decimal? ValorVencedor { get; set; }

        public class LanceViewModel
        {
            public string NomeUsuario { get; set; }
            public decimal Valor { get; set; }
            public DateTime DataHora { get; set; }
        }
    }
}