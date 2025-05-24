using System.ComponentModel.DataAnnotations;

namespace LanceCertoSQL.Models
{
    public class Imovel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do imóvel é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome do Imóvel")]
        public string Nome { get; set; }

        [Display(Name = "Foto do Imóvel")]
        public string? Foto { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O valor estimado é obrigatório.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor Estimado")]
        public decimal ValorEstimado { get; set; }

        [Required(ErrorMessage = "É necessário selecionar o proprietário.")]
        [Display(Name = "Proprietário")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

    }
}

