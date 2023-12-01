using Microsoft.EntityFrameworkCore;
using TdtuProject.EF;
using TdtuProject.Models;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Repository.Implementation
{
    public class CheckoutService : ICheckoutService
    {
        private readonly DatabaseContext _context;

        public CheckoutService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CheckoutViewModel>> GetAllAsync()
        {
            return await _context.CheckoutViewModel.ToListAsync();
        }
    }
}
