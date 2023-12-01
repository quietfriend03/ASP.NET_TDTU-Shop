using TdtuProject.Models;

namespace TdtuProject.Repository.Interface
{
    public interface ICheckoutService
    {
        Task<IEnumerable<CheckoutViewModel>> GetAllAsync();
    }
}
