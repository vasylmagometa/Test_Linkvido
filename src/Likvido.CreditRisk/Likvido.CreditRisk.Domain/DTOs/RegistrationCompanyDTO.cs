namespace Likvido.CreditRisk.Domain.DTOs
{
    public class RegistrationCompanyDTO : RegistrationDTO
    {
        public string RegistrationName { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string AddressTwo { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
