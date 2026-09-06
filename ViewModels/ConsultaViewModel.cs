using System.ComponentModel.DataAnnotations;

namespace ConsultaUVV.ViewModels
{
    public class ConsultaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a data e hora da consulta.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; } = DateTime.Now.AddDays(1);

        [StringLength(500)]
        public string? Descricao { get; set; }
    }
}
