using System;
using System.Collections.Generic;
using System.Text;

namespace Likvido.CreditRisk.Domain.Entities.Registration
{
    public class RegistrationData : ISearchable
    {
        public Guid Id { get; set; }        

        [SearchProperty]
        public string Address { get; set; }

        public string AddressTwo { get; set; }

        [SearchProperty]
        public string ZipCode { get; set; }

        [SearchProperty]
        public string City { get; set; }

        public string State { get; set; }

        [SearchProperty]
        public string Country { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; }

        public RegistrationData()
        {
            this.Registrations = new List<Registration>();
        }
    }
}
