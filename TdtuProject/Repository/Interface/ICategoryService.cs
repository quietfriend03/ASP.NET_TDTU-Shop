using TdtuProject.Models;
using TdtuProject.Models.DTO;

namespace TdtuProject.Repository.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Status> AddAsync(Category product);
        Task<Status> UpdateAsync(Category product);
        Task DeleteAsync(Category product);
    }
}
