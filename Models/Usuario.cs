using System.ComponentModel.DataAnnotations;

namespace ConsultaUVV.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 120 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(180)]
        public string Email { get; set; } = string.Empty;

        // Armazena o HASH da senha (nunca a senha em texto puro).
        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Um usuário pode ter várias consultas.
        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
