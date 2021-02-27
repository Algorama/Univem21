using System.ComponentModel;

namespace Univem.Churras.Domain.Model.Enums
{
    public enum TipoEvento
    {
        Churras, 
        Pizza, 
        Lanche, 

        [Description("Happy Hour")]
        HappyHour,

        Outros
    }
}