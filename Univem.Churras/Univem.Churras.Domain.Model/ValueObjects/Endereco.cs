using Kernel.Domain.Model.ValueObjects;

namespace Univem.Churras.Domain.Model.ValueObjects
{
    public class Endereco : ValueObject<Endereco>
    {
        public string Descricao { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }
}