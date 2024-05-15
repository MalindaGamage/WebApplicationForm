using WebApplicationForm.Models;
using Microsoft.Azure.Cosmos;
using System.Net;
using WebApplicationForm;

namespace ApplicationForm
{
    public class ApplicationDetailsService : IApplicationDetailsService
    {
        private readonly Container _container;

        public ApplicationDetailsService(CosmosClient client, string databaseName, string containerName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task<ApplicationDetails> CreateAsync(ApplicationDetails details)
        {
            details.Id = Guid.NewGuid().ToString(); // Assign a new GUID
            var response = await _container.CreateItemAsync(details, new PartitionKey(details.TenantId));
            return response.Resource;
        }

        public async Task<ApplicationDetails> GetByIdAsync(string id, string tenant)
        {
            try
            {
                var response = await _container.ReadItemAsync<ApplicationDetails>(id, new PartitionKey(tenant));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateAsync(string id, ApplicationDetails details, string tenant)
        {
            details.Id = id; // Ensure the ID is correct
            await _container.UpsertItemAsync(details, new PartitionKey(details.TenantId));
        }

        public async Task DeleteAsync(string id, string tenant)
        {
            await _container.DeleteItemAsync<ApplicationDetails>(id, new PartitionKey(tenant));
        }
    }
}