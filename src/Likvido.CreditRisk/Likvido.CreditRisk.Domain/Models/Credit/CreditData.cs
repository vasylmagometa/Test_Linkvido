using Likvido.CreditRisk.Domain.Models.CompanyModels;

namespace Likvido.CreditRisk.Domain.Models.Credit
{
    public class CreditData
    {
        public string Registration { get; set; }

        public string DebtCollectionRecommendation { get; set; }

        public string Recommendation { get; set; }

        public string Description { get; set; }

        public Company CompanyData { get; set; }

        public Rating RatingData { get; set; }
    }
}
