using Kernel.Domain.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Univem.Churras.Domain.Model.ValueObjects
{
    public class Endereco : ValueObject<Endereco>
    {
        [Required(ErrorMessage = "Descrição do Endereço é Obrigatória!")]
        public string Descricao { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }
}