using Likvido.CreditRisk.Domain.Models.Credit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Services.Abstraction
{
    public interface ICreditService
    {
        Task<CreditPrivateData> GetCreditPrivateAsync(string registrationNumber);

        Task<List<CreditPrivateData>> GetCreditPrivatesAsync(List<string> registrationNumbers);

        Task<List<CreditPrivateData>> GetCreditPrivatesSearchAsync(string query);
    }
}
