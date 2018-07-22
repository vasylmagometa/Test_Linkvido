using Likvido.CreditRisk.Domain.Enums;
using System;

namespace Likvido.CreditRisk.Domain.Entities.Registration
{
    public class Registration : ISearchable
    {
        public Guid Id { get; set; }

        public RegistrationSystemType RegistrationSystemId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateDeleted { get; set; }

        public Guid InvoiceId { get; set; }

        public int DeptorId { get; set; }

        [SearchProperty]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public DateTime DeletionDate { get; set; }

        [SearchProperty]
        public string Foundation { get; set; }

        [SearchProperty]
        public string AdministratorName { get; set; }

        [SearchProperty]
        public string AdministratorReference { get; set; }

        [SearchProperty]
        public string AdministratorPhone { get; set; }

        [SearchProperty]
        public string CreditorName { get; set; }

        [SearchProperty]
        public string CreditorPhone { get; set; }

        [SearchProperty]
        public string CreditorReference { get; set; }

        public Guid? CompanyDataId { get; set; }

        public Guid? PrivateDataId { get; set; }

        [NavigationSearchProperty("Address")]
        public RegistrationUser PrivateData { get; set; }

        [NavigationSearchProperty("Address")]
        public RegistrationCompany CompanyData { get; set; }
    }
}
