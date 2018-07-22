using System;

namespace Likvido.CreditRisk.Domain.DTOs
{
    public class RegistrationUserDTO
    {
        public Guid Id { get; set; }

        public string RegistrationNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string AddressTwo { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int NumberOfRegistratins { get; set; }
    }
}
