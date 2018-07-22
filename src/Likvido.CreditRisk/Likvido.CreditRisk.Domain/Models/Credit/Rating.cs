namespace Likvido.CreditRisk.Domain.Models.Credit
{
    public class Rating
    {
        public string RegistrationNumber { get; set; }

        public int SummaryRating { get; set; }

        public RatingEntity Equity { get; set; }

        public RatingEntity Result { get; set; }

        public RatingEntity Age { get; set; }

        public RatingEntity Employees { get; set; }

        public RatingEntity Type { get; set; }

        public RatingEntity Situation { get; set; }

        public bool Stopped { get; set; }
    }
}
