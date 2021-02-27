using Kernel.Domain.Model.Entities;
using Univem.Churras.Domain.Model.ValueObjects;

namespace Univem.Churras.Domain.Model.Entities
{
    public class Colega : EntityKeySeq, IAggregateRoot
    {
        public string Nome { get; set; }
        public Endereco Endereco { get; set; }
    }
}