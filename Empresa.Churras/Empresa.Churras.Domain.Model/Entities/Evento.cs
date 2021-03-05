using Empresa.Churras.Domain.Model.Enums;
using Empresa.Churras.Domain.Model.ValueObjects;
using Kernel.Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Empresa.Churras.Domain.Model.Entities
{
    public class Evento : EntityKeySeq, IAggregateRoot
    {
        [Required(ErrorMessage = "Dono da Casa é Obrigatório!")]
        public Colega DonoDaCasa { get; set; }
        public long DonoDaCasaKey { get; set; }

        [Required(ErrorMessage = "É necessário dar um Nome ao Evento")]
        [StringLength(100, ErrorMessage = "Nome do Evento deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }
        public TipoEvento Tipo { get; set; }
        public DateTime Dia { get; set; }
        public Periodo Periodo { get; set; }

        public List<EventoColegaConfirmado> ColegasConfirmados { get; set; }

        public void ConfirmarPresenca(Colega colega)
        {
            if (ColegasConfirmados == null)
                ColegasConfirmados = new List<EventoColegaConfirmado>();

            var confirmacao = new EventoColegaConfirmado
            {
                ColegaKey = colega.Key,
                ColegaNome = colega.Nome
            };

            ColegasConfirmados.Add(confirmacao);
        }
        public void CancelarPresenca(Colega colega)
        {
            if (ColegasConfirmados == null) 
                return;

            ColegasConfirmados.RemoveAll(x => x.ColegaKey == colega.Key);
        }
    }

    public class EventoColegaConfirmado
    {
        public long ColegaKey { get; set; }
        public string ColegaNome { get; set; }
        public string VaiLevar { get; set; }
    }
}