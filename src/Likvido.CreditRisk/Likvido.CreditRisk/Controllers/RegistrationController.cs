using Likvido.CreditRisk.Domain.DTOs;
using Likvido.CreditRisk.Domain.Models.Registration;
using Likvido.CreditRisk.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class RegistrationController : Controller
    {
        private readonly IRegistrationService registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        [HttpPost]
        [Route("private")]
        [ProducesResponseType(typeof(IdDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePrivate([FromBody]RegistrationPrivateDTO createRegistration)
        {
            if (createRegistration == null)
            {
                return BadRequest(new { Message = "Invalid input data." });
            }
            var result = await this.registrationService.CreateRegistrationPrivateAsync(createRegistration);
            IdDTO idDTO = new IdDTO
            {
                Id = result.ToString()
            };

            return CreatedAtRoute("GetRegistration", new { id = result.ToString() }, idDTO);
        }

        [HttpPost]
        [Route("company")]
        [ProducesResponseType(typeof(IdDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCompany([FromBody]RegistrationCompanyDTO createRegistration)
        {
            if (createRegistration == null)
            {
                return BadRequest(new { Message = "Invalid input data." });
            }
            var result = await this.registrationService.CreateRegistrationCompanyAsync(createRegistration);
            IdDTO idDTO = new IdDTO
            {
                Id = result.ToString()
            };

            return CreatedAtRoute("GetRegistration", new { id = result.ToString() }, idDTO);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RegistrationDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<RegistrationPrivateDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<RegistrationCompanyDetailsDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBySpecification([FromQuery]RegistrationSpecification specification)
        {
            var result = await this.registrationService.GetRegistrationsBySpecificationAsync(specification);

            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetRegistration")]
        [ProducesResponseType(typeof(List<RegistrationDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<RegistrationPrivateDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<RegistrationCompanyDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([Required]Guid id)
        {
            var result = await this.registrationService.GetRegistrationByIdAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(List<RegistrationDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<RegistrationPrivateDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<RegistrationCompanyDetailsDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchRegistrations([Required]string query)
        {
            // TODO ask min query length
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { Message = "Search query can not be empty." });
            }
            var result = await this.registrationService.GetRegistrationsSearchAsync(query);

            return Ok(result);
        }

        [HttpPut("private/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePrivate(Guid id, [FromBody]RegistrationPrivateDTO registration)
        {
            // TODO ask what properties can be edited 
            await this.registrationService.UpdatePrivateRegistrationAsync(id, registration);

            return NoContent();
        }

        [HttpPut("company/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody]RegistrationCompanyDTO registration)
        {
            // TODO ask what properties can be edited 
            await this.registrationService.UpdateCompanyRegistrationAsync(id, registration);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([Required]Guid id)
        {
            await this.registrationService.DeleteRegistrationByIdAsync(id);

            return NoContent();
        }        
    }
}