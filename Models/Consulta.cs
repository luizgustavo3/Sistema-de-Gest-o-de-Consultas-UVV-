using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultaUVV.Models
{
    public class Consulta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a data e hora da consulta.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        // Chave estrangeira para o Usuário dono da consulta.
        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
    }
}
