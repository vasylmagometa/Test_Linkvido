using Likvido.CreditRisk.Domain.Models.Credit;
using Likvido.CreditRisk.Models;
using Likvido.CreditRisk.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/credit/company")]
    [Produces("application/json")]
    public class CompanyCreditController : Controller
    {
        private readonly ICompanyService companyService;

        public CompanyCreditController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CreditData), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanyCreditDataById([Required]string id)
        {
            // TODO ask 8 chars length validation
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { Message = "Company id can not be empty." });
            }

            var result = await this.companyService.GetCompanyCreditDataByIdAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("find")]
        [ProducesResponseType(typeof(List<CreditData>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindCompaniesCreditData([Required]CollectionSpecification<string> specification)
        {
            List<string> companyIds = specification.ResolveIds();
            if (!companyIds.Any())
            {
                return BadRequest(new { Message = "At least one company id required." });
            }

            var result = await this.companyService.FindCompaniesCreditDataAsync(companyIds);

            return Ok(result);
        }
    }
}
