using Kernel.Domain.Model.Entities;
using System;

namespace Empresa.Churras.Domain.Model.Entities
{
    public class Evento : EntityKeySeq
    {
        public string DonoDaCasa { get; set; }
        public string Tipo { get; set; }
        public DateTime Dia { get; set; }
        public string Periodo { get; set; }
        public string ColegasConfirmados { get; set; }
        public string OqueCadaUmVaiLevar { get; set; }
    }
}