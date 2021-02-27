﻿using Kernel.Domain.Model.ValueObjects;
using System;

namespace Univem.Churras.Domain.Model.ValueObjects
{
    public class Periodo : ValueObject<Periodo>
    {
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

        public int QuantoDuraEmHoras()
        {
            var duracao = Fim.Subtract(Inicio);
            return duracao.Hours;
        }

        public string QuantoFaltaParaComecar()
        {
            var duracao = Inicio.Subtract(DateTime.Now);
            return duracao.Days > 0
                ? $"Começa em {duracao.Days} dias e {duracao.Hours} horas"
                : $"Começa em {duracao.Hours} horas";
        }
    }
}