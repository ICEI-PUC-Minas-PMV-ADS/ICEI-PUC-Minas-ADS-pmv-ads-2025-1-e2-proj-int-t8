﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanceCertoSQL.Models
{
    public class Lance
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public string UsuarioId { get; set; }  // Alterado de int para string

        [ForeignKey(nameof(UsuarioId))]  // Mapeamento para a chave string
        public Usuario? Usuario { get; set; }

        [Required]
        [Display(Name = "Pregão")]
        public int PregaoId { get; set; }

        [ForeignKey(nameof(PregaoId))]
        public Pregao? Pregao { get; set; }

        [Required]
        [Display(Name = "Valor do Lance")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Required]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; } = DateTime.Now;
    }
}



