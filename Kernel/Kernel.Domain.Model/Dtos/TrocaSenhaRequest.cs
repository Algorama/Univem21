using System.ComponentModel.DataAnnotations;

namespace Kernel.Domain.Model.Dtos
{
    public class TrocaSenhaRequest
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha inválida")]
        public string SenhaAntiga { get; set; }

        [Required(ErrorMessage = "Senha Nova inválida")]
        public string SenhaNova { get; set; }

        [Required(ErrorMessage = "Confirmação da Nova Senha inválida")]
        public string SenhaNovaConfirma { get; set; }
    }
}