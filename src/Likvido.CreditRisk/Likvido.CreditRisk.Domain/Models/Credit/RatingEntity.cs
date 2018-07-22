namespace Likvido.CreditRisk.Domain.Models.Credit
{
    public class RatingEntity
    {
        public string Explaination { get; set; }

        public decimal? Value { get; set; }

        public string DataPoint { get; set; }

        // TODO ask maybe int  
        public string Impact { get; set; }

        public bool Include { get; set; } = true;
    }
}
