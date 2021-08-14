using Kernel.Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Univem.Churras.Domain.Model.Enums;
using Univem.Churras.Domain.Model.ValueObjects;

namespace Univem.Churras.Domain.Model.Entities
{
    public class Evento : EntityKeySeq, IAggregateRoot
    {
        public Colega DonoDaCasa { get; set; }
        public long DonoDaCasaKey { get; set; }

        [Required(ErrorMessage = "Nome do Evento é Obrigatório!")]
        [StringLength(100, ErrorMessage = "Nome do Evento deve ter no máximo 100 caracteres!")]
        public string Nome { get; set; }
        public DateTime Dia { get; set; }
        public Periodo Periodo { get; set; }
        public TipoEvento Tipo { get; set; }
        public List<EventoColegaConfirmado> ColegasConfirmados { get; set; }

        public void ConfirmarPresenca(Colega colega, string vaiLevar = null)
        {
            if (ColegasConfirmados == null)
                ColegasConfirmados = new List<EventoColegaConfirmado>();

            var confirmacao = new EventoColegaConfirmado
            {
                ColegaKey = colega.Key,
                ColegaNome = colega.Nome,
                VaiLevar = vaiLevar
            };

            ColegasConfirmados.Add(confirmacao);
        }

        public void CancelarPresenca(Colega colega)
        {
            if (ColegasConfirmados == null)
                return;

            ColegasConfirmados.RemoveAll(x => x.ColegaKey == colega.Key);
        }

        public override string ToString() => $"[{Key}] {Nome}";
    }

    public class EventoColegaConfirmado
    {
        public long ColegaKey { get; set; }
        public string ColegaNome { get; set; }
        public string VaiLevar { get; set; }
    }
}
