using Likvido.CreditRisk.Domain.ElasticSearchModels;
using Likvido.CreditRisk.Domain.Models.AnnualReport;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Utils.Exensions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Likvido.CreditRisk.Services.Factory
{
    public class CompanyFactory : ICompanyFactory
    {
        public Company GetBaseCompany(ElasticCompanyModelDTO model)
        {
            var company = new Company();

            FillBaseCompany(company, model);
            
            return company;
        }

        public ExtendedCompany CreateExtendedCompany(ElasticCompanyModelDTO model, AnnualReportXMLData annualReport)
        {
            var company = new ExtendedCompany();
            FillBaseCompany(company, model);

            if (annualReport == null)
            {
                company.AnnualReportAvailable = false;

                return company;
            }

            company.Equity = annualReport.Equity;
            company.Assets = annualReport.Assets;
            company.CurrentAssets = annualReport.CurrentAssets;
            company.GrossProfitLoss = annualReport.GrossProfitLoss;
            company.ProfitLoss = annualReport.ProfitLoss;
            company.AnnualReportAvailable = true;

            return company;
        }

        private void FillBaseCompany<T>(T company, ElasticCompanyModelDTO model) where T : Company
        {
            company.VAT = model.Vrvirksomhed.cvrNummer;
            company.OfficialName = model.Data.nyesteNavn.navn;
            company.Address = string.Format("{0} {1} {2} {3} {4}",
                model.Data.nyesteBeliggenhedsadresse.vejnavn,
                model.Data.nyesteBeliggenhedsadresse.husnummerFra,
                model.Data.nyesteBeliggenhedsadresse.etage ?? string.Empty,
                model.Data.nyesteBeliggenhedsadresse.bogstavFra ?? string.Empty,
                model.Data.nyesteBeliggenhedsadresse.sidedoer ?? string.Empty);
            company.City = model.Data.nyesteBeliggenhedsadresse.postdistrikt;
            company.Zipcode = model.Data.nyesteBeliggenhedsadresse.postnummer.ToString();
            company.IndustryCode = model.Data.nyesteHovedbranche != null ?
                int.Parse(model.Data.nyesteHovedbranche.branchekode) :
                0;
            company.IndustryCodeDescription = model.Data.nyesteHovedbranche != null ?
                model.Data.nyesteHovedbranche.branchetekst :
                string.Empty;
            company.IndustrySecondaryCode = model.Data.nyesteBibranche1 != null ?
                    int.Parse(model.Data.nyesteBibranche1.branchekode) :
                    0;
            company.IndustryCodeSecondaryDescription = model.Data.nyesteBibranche1 != null ?
                    model.Data.nyesteBibranche1.branchetekst :
                    string.Empty;
            company.Startdate = model.Data.stiftelsesDato;
            company.CompanySituation = model.Data.sammensatStatus.ToLower().First().ToString().ToUpper() + model.Data.sammensatStatus.ToLower().Substring(1);
            company.Employees = model.Data?.nyesteKvartalsbeskaeftigelse?.intervalKodeAntalAnsatte ?? string.Empty;
            company.CompanyTypeDescription = model.Data.nyesteVirksomhedsform.langBeskrivelse;
            company.CompanyStopped = model.Data.sammensatStatus.Contains("OPHØRT", StringComparison.InvariantCultureIgnoreCase);
            company.CompanyDissolved = model.Data.sammensatStatus.Contains("OPLØST", StringComparison.InvariantCultureIgnoreCase);
            company.CreditBankrupt = model.Data.sammensatStatus.Contains("KONKURS", StringComparison.InvariantCultureIgnoreCase);

            SetContactInformation(model, company);
        }

        private static void SetContactInformation(ElasticCompanyModelDTO model, Company newModel)
        {
            newModel.ContactInformation = model.Data.nyesteKontaktoplysninger;

            // PHONE
            var phoneToUse = ParseContactDataBasedOnRegex(@"(?<!\d)\d{8}(?!\d)", model.Data.nyesteKontaktoplysninger);
            newModel.Phone = phoneToUse;

            // EMAIL
            var emailToUse = ParseContactDataBasedOnRegex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", model.Data.nyesteKontaktoplysninger); ;
            newModel.Email = emailToUse;
        }

        private static string ParseContactDataBasedOnRegex(string regex, string[] contacts)
        {
            var result = string.Empty;
            foreach (var contact in contacts)
            {
                var results = Regex.Matches(contact, regex, RegexOptions.ECMAScript)
                 .GetEnumerator();                    

                if (results.MoveNext())
                {
                    result = ((Match)results.Current).Value;
                    break;
                }
            }
            return result;
        }
    }
}
