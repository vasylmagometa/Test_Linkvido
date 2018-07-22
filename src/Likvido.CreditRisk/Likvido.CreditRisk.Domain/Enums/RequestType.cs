using System.ComponentModel;

namespace Likvido.CreditRisk.Domain.Enums
{
    public enum RequestType
    {
        [Description("Light")]
        Ligth = 1,

        [Description("Extended")]
        Extended = 2
    }
}
