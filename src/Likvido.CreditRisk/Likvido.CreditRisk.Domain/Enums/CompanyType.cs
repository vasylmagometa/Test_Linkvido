using System.ComponentModel;

namespace Likvido.CreditRisk.Domain.Enums
{
    public enum CompanyType
    {
        [Description("Enkeltmandsvirksomhed")]
        Personal = 0,

        [Description("Interessentskab")]
        IS = 1,

        [Description("Anpartsselskab")]
        APS = 2,

        [Description("Iværksætterselskab")]
        IVS = 3,

        [Description("Aktieselskab")]
        AS = 4,

        [Description("Selskab med begrænset ansvar")]
        SMBA = 5,

        [Description("Kommanditselskab")]
        KS = 6,

        [Description("SPE-Selskab")]
        SPE = 7
    }
}
