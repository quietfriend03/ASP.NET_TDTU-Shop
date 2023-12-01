using Microsoft.EntityFrameworkCore;
using TdtuProject.EF;
using TdtuProject.Models;
using TdtuProject.Models.DTO;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Repository.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext _context;

        public CategoryService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Status> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new Status { StatusCode = 1, Message = "Adding Category complete" };
        }

        public async Task DeleteAsync(Category category)
        {
            var result = await _context.Categories.FindAsync(category.CategoryId);
            if (result != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Status> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return new Status { StatusCode = 1, Message = "Updating Category complete" };
        }
    }
}
