using WebApplicationForm.Models;

namespace WebApplicationForm
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Question>> GetQuestionsAsync(string query, IDictionary<string, object> parameters = null);
        Task<Question> GetQuestionAsync(string id);
        Task AddQuestionAsync(Question question);
        Task UpdateQuestionAsync(string id, Question question);
        Task DeleteQuestionAsync(string id);
    }
}
