using System;
using System.Collections.Generic;
using LanceCertoSQL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LanceCertoSQL.ViewModels
{
    public class PregaoCreateViewModel
    {
        public int ImovelId { get; set; }
        public IEnumerable<SelectListItem>? ImoveisDisponiveis { get; set; }

        public decimal ValorMinimo { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
