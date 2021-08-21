using System.ComponentModel.DataAnnotations;

namespace Kernel.Domain.Model.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha inválida")]
        public string Senha { get; set; }
    }
}