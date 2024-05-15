using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebApplicationForm.DTO;
using WebApplicationForm.Models;
using WebApplicationForm;

namespace ApplicationForm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly IApplicationDetailsService _applicationDetailsService;

        public CandidateController(ICosmosDbService cosmosDbService, IApplicationDetailsService applicationDetailsService)
        {
            _cosmosDbService = cosmosDbService;
            _applicationDetailsService = applicationDetailsService;
        }

        [HttpGet("GetQuestions")]
        public async Task<IActionResult> GetQuestions([FromQuery] string type)
        {
            string query;
            IDictionary<string, object> parameters = new ExpandoObject();

            if (string.IsNullOrEmpty(type))
            {
                query = "SELECT * FROM c";
            }
            else
            {
                query = "SELECT * FROM c WHERE c.type = @type";
                parameters["@type"] = type;
            }

            var questions = await _cosmosDbService.GetQuestionsAsync(query, parameters);
            return Ok(questions.Select(q => new QuestionDto
            {
                Content = q.Content,
                Type = q.Type,
                Options = q.Options,
                MinValue = q.MinValue,
                MaxValue = q.MaxValue,
                Placeholder = q.Placeholder
            }));
        }

        [HttpPost("SubmitResponses")]
        public async Task<IActionResult> SubmitResponses([FromBody] List<ResponseDto> responseDtos)
        {
            // Here, you would handle the responses, possibly storing them or processing them as needed
            return Ok();
        }

        [HttpPost("SubmitApplication")]
        public async Task<IActionResult> SubmitApplication([FromBody] ApplicationDetails applicationDetails)
        {
            var createdDetails = await _applicationDetailsService.CreateAsync(applicationDetails);
            return CreatedAtAction(nameof(GetApplicationById), new { id = createdDetails.Id, tenantId = createdDetails.TenantId }, createdDetails);
        }

        [HttpGet("GetApplicationById/{id}")]
        public async Task<IActionResult> GetApplicationById(string id, [FromQuery] string tenantId)
        {
            var application = await _applicationDetailsService.GetByIdAsync(id, tenantId);
            if (application == null)
            {
                return NotFound();
            }
            return Ok(application);
        }
    }
}