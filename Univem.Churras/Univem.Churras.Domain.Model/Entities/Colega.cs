using Kernel.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;
using Univem.Churras.Domain.Model.ValueObjects;

namespace Univem.Churras.Domain.Model.Entities
{
    public class Colega : EntityKeySeq, IAggregateRoot
    {
        [Required(ErrorMessage = "Nome do Colega é Obrigatório!")]
        [StringLength(50, ErrorMessage = "Tamanho máximo do nome é 50 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Endereço é Obrigatório!")]
        public Endereco Endereco { get; set; }

        public Colega()
        {
            Endereco = new Endereco();
        }

        public override string ToString() => $"[{Key}] {Nome}";
    }
}