using WebApplicationForm.Models;

namespace WebApplicationForm
{
    public interface IApplicationDetailsService
    {
        Task<ApplicationDetails> CreateAsync(ApplicationDetails details);
        Task<ApplicationDetails> GetByIdAsync(string id, string tenantId);
        Task UpdateAsync(string id, ApplicationDetails details, string tenantId);
        Task DeleteAsync(string id, string tenantId);
    }
}
