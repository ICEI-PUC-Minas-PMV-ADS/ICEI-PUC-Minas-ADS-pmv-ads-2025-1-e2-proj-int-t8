using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanceCertoSQL.Models
{
    public enum StatusPregao
    {
        Pendente,
        Ativo,
        Finalizado,
        Cancelado
    }

    public class Pregao
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Imóvel")]
        public int ImovelId { get; set; }

        [ForeignKey("ImovelId")]
        public Imovel? Imovel { get; set; }

        [Required]
        [Display(Name = "Dono do Pregão")]
        public string UsuarioId { get; set; }  // Alterado de int para string

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Required]
        [Display(Name = "Valor Mínimo")]
        [DataType(DataType.Currency)]
        public decimal ValorMinimo { get; set; }

        [Required]
        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; }

        [Required]
        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; }

        [Required]
        public StatusPregao Status { get; set; }

        [Display(Name = "Usuário Vencedor")]
        public string? UsuarioVencedorId { get; set; }  // Alterado de int? para string?

        [ForeignKey("UsuarioVencedorId")]
        public Usuario? UsuarioVencedor { get; set; }

        [Display(Name = "Nome do Vencedor")]
        public string? NomeVencedor { get; set; }

        [NotMapped]
        public List<Lance> Lances { get; set; } = new List<Lance>();
    }
}



