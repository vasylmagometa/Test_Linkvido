using System.Collections.Generic;
using System.Threading.Tasks;
using Likvido.CreditRisk.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using System.ComponentModel.DataAnnotations;
using Likvido.CreditRisk.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Likvido.CreditRisk.Models;

namespace Likvido.CreditRisk.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {            
            this.companyService = companyService;
        }

        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(List<Company>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([Required]string query, [FromQuery]RequestType type = RequestType.Ligth)
        {
            // TODO ask min query length
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { Message = "Search query can not be empty." });
            }

            var result = await this.companyService.SearchCompaniesAsync(query, type);

            return Ok(result);
        }

        [HttpGet]
        [Route("find")]
        [ProducesResponseType(typeof(List<Company>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Find([Required]CollectionSpecification<string> specification, [FromQuery]RequestType type = RequestType.Ligth)
        {
            List<string> companyIds = specification.ResolveIds();
            if (!companyIds.Any())
            {
                return BadRequest(new { Message = "At least one company id required." });
            }

            var result = await this.companyService.FindCompaniesAsync(companyIds, type);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanyById([Required]string id, [FromQuery]RequestType type = RequestType.Ligth)
        {
            // TODO ask 8 chars length validation
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { Message = "Company id can not be empty." });
            }

            var result = await this.companyService.GetCompanyByIdAsync(id, type);

            return Ok(result);
        }

        [HttpGet]
        [Route("typeahead")]
        [ProducesResponseType(typeof(List<CompanyTypeahead>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Typeahead([Required]string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            {
                return BadRequest(new { Message = "Minimum query length is 3 characters" });
            }

            var result = await this.companyService.TypeaheadAsync(query);

            return Ok(result);
        }
    }
}
