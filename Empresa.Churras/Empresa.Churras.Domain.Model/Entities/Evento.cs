using Empresa.Churras.Domain.Model.ValueObjects;
using Kernel.Domain.Model.Entities;
using System;
using System.Collections.Generic;

namespace Empresa.Churras.Domain.Model.Entities
{
    public class Evento : EntityKeySeq
    {
        public Colega DonoDaCasa { get; set; }
        public long DonoDaCasaKey { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public DateTime Dia { get; set; }
        public Periodo Periodo { get; set; }
        public List<EventoColegaConfirmado> ColegasConfirmados { get; set; }
    }

    public class EventoColegaConfirmado
    {
        public long ColegaId { get; set; }
        public string ColegaNome { get; set; }
        public string VaiLevar { get; set; }
    }
}