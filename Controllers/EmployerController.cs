using Microsoft.AspNetCore.Mvc;
using WebApplicationForm.DTO;
using WebApplicationForm.Models;
using WebApplicationForm;

namespace ApplicationForm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployerController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public EmployerController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Content = questionDto.Content,
                Type = questionDto.Type,
                Options = questionDto.Options,
                MinValue = questionDto.MinValue,
                MaxValue = questionDto.MaxValue,
                Placeholder = questionDto.Placeholder
            };
            await _cosmosDbService.AddQuestionAsync(question);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
        }

        [HttpGet("GetQuestion/{id}")]
        public async Task<IActionResult> GetQuestion(string id)
        {
            var question = await _cosmosDbService.GetQuestionAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPut("UpdateQuestion/{id}")]
        public async Task<IActionResult> UpdateQuestion(string id, [FromBody] QuestionDto questionDto)
        {
            var question = await _cosmosDbService.GetQuestionAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            question.Content = questionDto.Content;
            question.Type = questionDto.Type;
            question.Options = questionDto.Options;
            question.MinValue = questionDto.MinValue;
            question.MaxValue = questionDto.MaxValue;
            question.Placeholder = questionDto.Placeholder;

            await _cosmosDbService.UpdateQuestionAsync(id, question);
            return NoContent();
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public async Task<IActionResult> DeleteQuestion(string id)
        {
            var question = await _cosmosDbService.GetQuestionAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            await _cosmosDbService.DeleteQuestionAsync(id);
            return NoContent();
        }
    }
}