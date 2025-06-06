using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LanceCertoSQL.ViewModels
{
    public class ImovelCreateViewModel
    {
        [Required(ErrorMessage = "O nome do imóvel é obrigatório.")]
        [Display(Name = "Nome do Imóvel")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O valor estimado é obrigatório.")]
        [Display(Name = "Valor Estimado")]
        public decimal ValorEstimado { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Foto do Imóvel")]
        public IFormFile? FotoUpload { get; set; }
    }
}
