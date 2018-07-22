using System.Collections.Generic;

namespace Likvido.CreditRisk.Domain.Models.Registration
{
    public class RegistrationSpecification
    {
        public List<string> Ids { get; set; } = new List<string>();

        public List<string> InvoiceIds { get; set; } = new List<string>();

        public List<string> DebtorIds { get; set; } = new List<string>();
    }
}
