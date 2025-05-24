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
        public int UsuarioId { get; set; }

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
    }
}

