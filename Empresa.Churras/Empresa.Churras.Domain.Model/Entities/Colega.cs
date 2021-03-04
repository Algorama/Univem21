using Empresa.Churras.Domain.Model.ValueObjects;
using Kernel.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace Empresa.Churras.Domain.Model.Entities
{
    public class Colega : EntityKeySeq, IAggregateRoot
    {
        [Required(ErrorMessage = "Nome é Obrigatório!")]
        public string Nome { get; set; }

        public Endereco Endereco { get; set; }
    }
}
