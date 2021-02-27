using Kernel.Domain.Model.ValueObjects;
using System;

namespace Univem.Churras.Domain.Model.ValueObjects
{
    public class Periodo : ValueObject<Periodo>
    {
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}