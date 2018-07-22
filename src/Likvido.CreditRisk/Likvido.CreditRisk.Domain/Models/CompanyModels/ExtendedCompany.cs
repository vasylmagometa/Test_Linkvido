namespace Likvido.CreditRisk.Domain.Models.CompanyModels
{
    public class ExtendedCompany : Company
    {
        public decimal? GrossProfitLoss { get; set; }

        public decimal? ProfitLoss { get; set; }

        public decimal? CurrentAssets { get; set; }

        public decimal? Assets { get; set; }

        public decimal? Equity { get; set; }
    }
}
