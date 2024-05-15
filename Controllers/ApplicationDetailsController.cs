using Microsoft.AspNetCore.Mvc;
using WebApplicationForm.Models;

namespace WebApplicationForm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationDetailsController : ControllerBase
    {
        private readonly IApplicationDetailsService _service;

        public ApplicationDetailsController(IApplicationDetailsService service)
        {
            _service = service;
        }

        // POST: /ApplicationDetails
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicationDetails details)
        {
            if (details == null)
            {
                return BadRequest("ApplicationDetails cannot be null");
            }

            details.Id = null; // Ensure Id is null to allow service to generate it

            var createdDetails = await _service.CreateAsync(details);
            return CreatedAtAction(nameof(GetById), new { id = createdDetails.Id, tenantId = createdDetails.TenantId }, createdDetails);
        }

        // GET: /ApplicationDetails/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] string tenantId)
        {
            var details = await _service.GetByIdAsync(id, tenantId);
            if (details == null)
            {
                return NotFound();
            }
            return Ok(details);
        }

        // PUT: /ApplicationDetails/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ApplicationDetails details, [FromQuery] string tenantId)
        {
            if (await _service.GetByIdAsync(id, tenantId) == null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, details, tenantId);
            return NoContent();
        }

        // DELETE: /ApplicationDetails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] string tenantId)
        {
            if (await _service.GetByIdAsync(id, tenantId) == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id, tenantId);
            return NoContent();
        }
    }
}
