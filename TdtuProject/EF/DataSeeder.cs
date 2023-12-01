using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using TdtuProject.Models;

namespace TdtuProject.EF
{
    public class DataSeeder
    {
        private readonly DatabaseContext _databaseContext;


        public DataSeeder(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void SeedData()
        {
            try
            {
                if (!_databaseContext.Categories.Any())
                {
                    _databaseContext.Categories.AddRange(
                        new Category { CategoryName = "Phone" },
                        new Category { CategoryName = "Tablet" }
                    );
                }

                if (!_databaseContext.Products.Any())
                {
                    _databaseContext.Products.AddRange(
                        new Product
                        {
                            ProductName = "Iphone 15",
                            ProductPrice = 799,
                            CategoryId = 1,
                            FileName = "iphone15.jpg"
                        },
                        new Product
                        {
                            ProductName = "Iphone 15 Pro Max",
                            ProductPrice = 999,
                            CategoryId = 1,
                            FileName = "iphone15promax.png"
                        },
                        new Product
                        {
                            ProductName = "Samsung Galaxy S10",
                            ProductPrice = 699,
                            CategoryId = 1,
                            FileName = "samsungs10.jpg"
                        },
                        new Product
                        {
                            ProductName = "Samsung Galaxy S23",
                            ProductPrice = 699,
                            CategoryId = 1,
                            FileName = "samsungs23.png"
                        },
                        new Product
                        {
                            ProductName = "Ipad Air 4",
                            ProductPrice = 699,
                            CategoryId = 2,
                            FileName = "ipadair4.png"
                        },
                        new Product
                        {
                            ProductName = "Ipad gen 10",
                            ProductPrice = 699,
                            CategoryId = 2,
                            FileName = "ipadgen10.jpg"
                        },
                        new Product
                        {
                            ProductName = "Ipad Mini 6",
                            ProductPrice = 699,
                            CategoryId = 2,
                            FileName = "ipadmini.jpg"
                        },
                        new Product
                        {
                            ProductName = "Ipad Pro 11",
                            ProductPrice = 699,
                            CategoryId = 2,
                            FileName = "ipadpro11.png"
                        }
                    );
                }

                _databaseContext.SaveChanges();
            } catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
                throw;
            }
        }
    }
}
