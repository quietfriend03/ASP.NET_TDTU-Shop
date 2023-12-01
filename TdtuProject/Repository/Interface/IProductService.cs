using TdtuProject.Models;
using TdtuProject.Models.DTO;

namespace TdtuProject.Repository.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Status> AddAsync(Product product);
        Task<Status> UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetSortedByPriceAsync(bool ascendingOrder);
        Task<IEnumerable<Product>> GetRelatedProduct(int categoryId, int productId);
    }
}
