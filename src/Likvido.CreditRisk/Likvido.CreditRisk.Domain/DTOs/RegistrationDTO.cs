using Likvido.CreditRisk.Domain.Enums;
using System;

namespace Likvido.CreditRisk.Domain.DTOs
{
    public class RegistrationDTO
    {
        public RegistrationSystemType RegistrationSystemId { get; set; }        

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
    }
}
