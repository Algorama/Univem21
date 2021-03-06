using Empresa.Churras.Domain.Model.Enums;
using Empresa.Churras.Domain.Model.ValueObjects;
using Kernel.Domain.Model.Entities;
using Kernel.Domain.Model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Empresa.Churras.Domain.Model.Entities
{
    public class Evento : EntityKeySeq, IAggregateRoot
    {
        public Colega DonoDaCasa { get; set; }
        public long DonoDaCasaKey { get; set; }

        [Required(ErrorMessage = "É necessário dar um Nome ao Evento")]
        [StringLength(100, ErrorMessage = "Nome do Evento deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }
        public TipoEvento Tipo { get; set; }
        public DateTime Dia { get; set; }
        public Periodo Periodo { get; set; }

        public List<EventoColegaConfirmado> ColegasConfirmados { get; set; }

        public Evento()
        {
            ColegasConfirmados = new List<EventoColegaConfirmado>();
        }

        public void ConfirmarPresenca(Colega colega, string vaiLevar)
        {
            if (string.IsNullOrWhiteSpace(vaiLevar))
                throw new ValidatorException("Você precisa informar o que vai Levar");

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

        public override string ToString() => $"#{Key}: - {Nome} na casa do {DonoDaCasa?.Nome} - {Dia:dd/MM/yyyy}";
    }

    public class EventoColegaConfirmado
    {
        public long ColegaKey { get; set; }
        public string ColegaNome { get; set; }
        public string VaiLevar { get; set; }

        public override string ToString() => $"Colega: #{ColegaKey}: {ColegaNome} confirmou e vai levar: {VaiLevar}";
    }
}