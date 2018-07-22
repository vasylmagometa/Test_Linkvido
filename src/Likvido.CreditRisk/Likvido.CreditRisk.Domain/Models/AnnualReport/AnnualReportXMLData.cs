namespace Likvido.CreditRisk.Domain.Models.AnnualReport
{
    public class AnnualReportXMLData
    {
        public string RegistrationNumber { get; set; }
        public decimal? GrossProfitLoss { get; set; }
        public decimal? ProfitLoss { get; set; }
        public decimal? CurrentAssets { get; set; }
        public decimal? Assets { get; set; }
        public decimal? Equity { get; set; }
    }
}
