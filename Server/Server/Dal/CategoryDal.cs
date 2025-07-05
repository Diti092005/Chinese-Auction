using Microsoft.EntityFrameworkCore;
using Server.Dal.Interfaces;
using Server.Models;
using Microsoft.Extensions.Logging;

namespace Server.Dal
{
    public class CategoryDal : ICategoryDal
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryDal> _logger;

        public CategoryDal(ApplicationDbContext context, ILogger<CategoryDal> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(Category category)
        {
            _logger.LogInformation($"Adding category: {category?.Name}");
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Category added successfully");
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting category with id {id}");
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found");
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
            // בדיקה אם יש מתנות לקטגוריה
            bool hasGifts = await _context.Gifts.AnyAsync(g => g.CategoryId == id);
            if (hasGifts)
            {
                _logger.LogWarning($"Cannot delete category {id} because there are gifts assigned to it");
                throw new InvalidOperationException($"Cannot delete category {id} because there are gifts assigned to it");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Category with id {id} deleted successfully");
        }

        public async Task<List<Category>> Get()
        {
            _logger.LogInformation("Getting all categories");
            var categories = await _context.Categories.ToListAsync();
            if (categories == null || !categories.Any())
            {
                _logger.LogWarning("No categories found");
                throw new InvalidOperationException("No categories found.");
            }
            _logger.LogInformation($"Returned {categories.Count} categories");
            return categories;
        }

        public async Task<Category> Get(int id)
        {
            _logger.LogInformation($"Getting category with id {id}");
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found");
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
            _logger.LogInformation($"Returned category with id {id}");
            return category;
        }

        public async Task<bool> NameExist(string name)
        {
            _logger.LogInformation($"Checking if category name exists: {name}");
            var exists = await _context.Categories.AnyAsync(c => c.Name == name);
            _logger.LogInformation($"Category name {name} exists: {exists}");
            return exists;
        }

        public async Task Update(int id, Category category)
        {
            _logger.LogInformation($"Updating category with id {id}");
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                _logger.LogWarning($"Category with ID {id} not found");
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            existingCategory.Name = category.Name;
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Category with id {id} updated successfully");
        }
    }
}
