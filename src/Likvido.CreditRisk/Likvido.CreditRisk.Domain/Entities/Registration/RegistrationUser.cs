namespace Likvido.CreditRisk.Domain.Entities.Registration
{
    public class RegistrationUser : RegistrationData
    {
        [SearchProperty]
        public string RegistrationNumber { get; set; }

        [SearchProperty]
        public string FirstName { get; set; }

        [SearchProperty]
        public string LastName { get; set; }
    }
}
