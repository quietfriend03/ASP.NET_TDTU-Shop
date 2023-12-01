using Microsoft.EntityFrameworkCore;
using TdtuProject.EF;
using TdtuProject.Models;
using TdtuProject.Models.DTO;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Repository.Implementation
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductService(DatabaseContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<Status> AddAsync(Product product)
        {
            SaveImage(product);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new Status { StatusCode = 1, Message = "Adding complete" };

        }

        public async Task DeleteAsync(Product product)
        {
            var result = await _context.Products.FindAsync(product.ProductId);
            if (result != null)
            {
                DeleteImage(product);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p=>p.Category).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Status> UpdateAsync(Product product)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Retrieve the existing product from the database
                    var existingProduct = await _context.Products.FindAsync(product.ProductId);

                    if (existingProduct == null)
                    {
                        return new Status { StatusCode = 0, Message = "Product not found" };
                    }

                    // Attach the existing product to the context
                    _context.Attach(existingProduct);

                    // Update only the specific fields you want
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.ProductPrice = product.ProductPrice;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.Category = product.Category;
                    SaveImage(product);
                    if(existingProduct.FileName == null)
                    {
                        existingProduct.FileName = product.FileName;
                    }
                   
                    // Save the changes to the database
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return new Status { StatusCode = 1, Message = "Updating complete" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log the exception and return an appropriate Status object.
                    // Example: _logger.LogError(ex, "Error updating the product");
                    return new Status { StatusCode = 0, Message = "Error updating the product" };
                }
            }
        }

        private void SaveImage(Product product)
        {
            if(product.ImageFile != null && product.ImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(fileStream);
                }
                product.FileName = uniqueFileName;
            }
        }

        private void DeleteImage(Product product)
        {
            if (!string.IsNullOrEmpty(product.FileName))
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                string filePath = Path.Combine(uploadsFolder, product.FileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            var products = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetSortedByPriceAsync(bool ascendingOrder)
        {
            return ascendingOrder
                ? await _context.Products.OrderBy(p => p.ProductPrice).ToListAsync()
                : await _context.Products.OrderByDescending(p => p.ProductPrice).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetRelatedProduct(int categoryId, int productId)
        {
            IEnumerable<Product> products = await _context.Products
                .Where(p => p.CategoryId == categoryId && p.ProductId != productId)
                .ToListAsync();
            return products;
        }
    }
}
