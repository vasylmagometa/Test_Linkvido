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
    [Route("api/v{version:apiVersion}/credit/private")]
    [Produces("application/json")]
    public class PrivateCreditController : Controller
    {
        private readonly ICreditService creditService;

        public PrivateCreditController(ICreditService creditService)
        {
            this.creditService = creditService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CreditPrivateData), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanyCreditDataById([Required]string id)
        {
            // TODO ask 6 chars length validation
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { Message = "Personal id can not be empty." });
            }
            var result = await this.creditService.GetCreditPrivateAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("find")]
        [ProducesResponseType(typeof(List<CreditPrivateData>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindPrivateCreditData([Required]CollectionSpecification<string> specification)
        {
            List<string> personalIds = specification.ResolveIds();
            if (!personalIds.Any())
            {
                return BadRequest(new { Message = "At least one persomal id required." });
            }
            var result = await this.creditService.GetCreditPrivatesAsync(personalIds);

            return Ok(result);
        }

        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(List<CreditPrivateData>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchPrivateCreditData([Required]string query)
        {
            // TODO ask min query length
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { Message = "Search query can not be empty." });
            }
            var result = await this.creditService.GetCreditPrivatesSearchAsync(query);

            return Ok(result);
        }
    }
}
