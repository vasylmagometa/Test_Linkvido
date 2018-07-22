using Likvido.CreditRisk.Domain.Enums;
using System;

namespace Likvido.CreditRisk.Domain.Models.CompanyModels
{
    public class Company
    {
        public bool AnnualReportAvailable { get; set; }
        public string VAT { get; set; }
        public string OfficialName { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }

        public string[] ContactInformation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }


        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public string Employees { get; set; }

        public int IndustryCode { get; set; }
        public string IndustryCodeDescription { get; set; }
        public int? IndustrySecondaryCode { get; set; }
        public string IndustryCodeSecondaryDescription { get; set; }

        public string CompanySituation { get; set; }
        public string CompanyTypeDescription { get; set; }

        public string Creditstartdate { get; set; }
        public int? Creditstatus { get; set; }
        public bool CreditBankrupt { get; set; }
        public bool CompanyStopped { get; set; }
        public bool CompanyDissolved { get; set; }

        public int Version { get; set; }

        public string EmployeesDescription
        {
            get
            {
                var employeeStrFormat = Employees.Replace("ANTAL_", string.Empty).Replace("_", "-");

                if (string.IsNullOrWhiteSpace(employeeStrFormat) ||
                    employeeStrFormat == "null" ||
                    employeeStrFormat == "0" ||
                    employeeStrFormat == "1" ||
                    employeeStrFormat == "0-0" ||
                    employeeStrFormat == "1-1")
                {
                    return "1";
                }

                return employeeStrFormat;
            }
        }

        public int EmployeesNumberForDataModel
        {
            get
            {
                var employeeStrFormat = Employees.Replace("ANTAL_", string.Empty).Replace("_", "-");

                if (string.IsNullOrWhiteSpace(employeeStrFormat) ||
                    employeeStrFormat == "null" ||
                    employeeStrFormat == "0" ||
                    employeeStrFormat == "1" ||
                    employeeStrFormat == "0-0" ||
                    employeeStrFormat == "1-1")
                {
                    return 1;
                }

                var numbers = employeeStrFormat.Split('-');

                int firstEmployeeCount;
                if (numbers.Length > 0 && int.TryParse(numbers[0], out firstEmployeeCount))
                {
                    return firstEmployeeCount;
                }

                return 0;
            }
        }

        public CompanyType Type
        {
            get
            {
                switch (this.CompanyTypeDescription)
                {
                    case "Enkeltmandsvirksomhed":
                        return CompanyType.Personal;
                    case "Iværksætterselskab":
                        return CompanyType.IVS;
                    case "Anpartsselskab":
                        return CompanyType.APS;
                    case "Aktieselskab":
                        return CompanyType.AS;
                    case "Interessentskab":
                        return CompanyType.IS;
                    case "InteressKommanditselskabentskab":
                        return CompanyType.KS;
                    case "SE-selskab":
                        return CompanyType.SPE;
                    case "Selskab med begrænset ansvar":
                        return CompanyType.SMBA;
                    default:
                        return CompanyType.Personal;
                }
            }
        }

        public DateTime? DateStarted
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Startdate) || this.Startdate.ToLower() == "null")
                {
                    return null;
                }

                var date = this.Startdate.Replace("-", "/").Replace(" ", "");

                DateTime parseDate;
                if (DateTime.TryParse(date, out parseDate))
                {
                    return parseDate;
                }

                return null;
            }
        }

        public bool CompanyActive
        {
            get
            {
                if (this.CreditBankrupt || this.CompanyStopped || this.CompanyDissolved)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
