using Microsoft.Azure.Cosmos;
using WebApplicationForm.Models;
using WebApplicationForm;

namespace ApplicationForm
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseName;
        private readonly Dictionary<string, Container> _containers;

        public CosmosDbService(CosmosClient cosmosClient, string databaseName, Dictionary<string, string> containerNames)
        {
            _cosmosClient = cosmosClient;
            _databaseName = databaseName;
            _containers = containerNames.ToDictionary(
                pair => pair.Key,
                pair => cosmosClient.GetContainer(databaseName, pair.Key));
        }

        public Container GetContainer(string containerName)
        {
            if (!_containers.ContainsKey(containerName))
                throw new ArgumentException($"Container {containerName} is not configured.");
            return _containers[containerName];
        }

        public async Task AddQuestionAsync(Question question)
        {
            var container = GetContainer("Question");
            await container.CreateItemAsync(question, new PartitionKey(question.Type));
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync(string query, IDictionary<string, object> parameters = null)
        {
            var container = GetContainer("Question");
            var queryDefinition = new QueryDefinition(query);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    queryDefinition.WithParameter(param.Key, param.Value);
                }
            }

            var queryIterator = container.GetItemQueryIterator<Question>(queryDefinition);
            List<Question> results = new List<Question>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                results.AddRange(response.Resource);
            }
            return results;
        }

        public async Task<Question> GetQuestionAsync(string id)
        {
            var container = GetContainer("Question");
            try
            {
                // Retrieve the question's type to use as the partition key
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id);
                var queryIterator = container.GetItemQueryIterator<Question>(queryDefinition);
                var questions = await queryIterator.ReadNextAsync();
                var question = questions.FirstOrDefault();
                if (question == null)
                {
                    return null;
                }
                var response = await container.ReadItemAsync<Question>(id, new PartitionKey(question.Type));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateQuestionAsync(string id, Question question)
        {
            var container = GetContainer("Question");
            await container.UpsertItemAsync(question, new PartitionKey(question.Type));
        }

        public async Task DeleteQuestionAsync(string id)
        {
            var container = GetContainer("Question");
            var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id);
            var queryIterator = container.GetItemQueryIterator<Question>(queryDefinition);
            var questions = await queryIterator.ReadNextAsync();
            var question = questions.FirstOrDefault();
            if (question != null)
            {
                await container.DeleteItemAsync<Question>(id, new PartitionKey(question.Type));
            }
        }
    }
}