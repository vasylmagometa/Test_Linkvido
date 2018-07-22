using System;

namespace Likvido.CreditRisk.Domain.ElasticSearchModels
{
    public class ElasticAnnualReportModelDTO
    {
        public string cvrNummer { get; set; }
        public string regNummer { get; set; }
        public bool omgoerelse { get; set; }
        public string sagsNummer { get; set; }
        public string offentliggoerelsestype { get; set; }
        public Regnskab regnskab { get; set; }
        public DateTime offentliggoerelsesTidspunkt { get; set; }
        public DateTime indlaesningsTidspunkt { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public Dokumenter[] dokumenter { get; set; }
        public string indlaesningsId { get; set; }
    }

    public class Regnskab
    {
        public Regnskabsperiode regnskabsperiode { get; set; }
    }

    public class Regnskabsperiode
    {
        public string startDato { get; set; }
        public string slutDato { get; set; }
    }

    public class Dokumenter
    {
        public string dokumentUrl { get; set; }
        public string dokumentMimeType { get; set; }
        public string dokumentType { get; set; }
    }
}
