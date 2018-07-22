using Likvido.CreditRisk.Domain.Enums;
using System;

namespace Likvido.CreditRisk.Domain.DTOs
{
    public class RegistrationDetailsDTO
    {
        public Guid Id { get; set; }

        public RegistrationSystemType RegistrationSystemId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateDeleted { get; set; }

        public Guid InvoiceId { get; set; }

        public int DeptorId { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public DateTime DeletionDate { get; set; }

        public string Foundation { get; set; }

        public string AdministratorName { get; set; }

        public string AdministratorReference { get; set; }

        public string AdministratorPhone { get; set; }

        public string CreditorName { get; set; }

        public string CreditorPhone { get; set; }

        public string CreditorReference { get; set; }

        public Guid? CompanyDataId { get; set; }

        public Guid? PrivateDataId { get; set; }        
    }
}
