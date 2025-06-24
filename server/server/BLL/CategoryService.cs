using Microsoft.Extensions.Logging;
using server.Bll.Interfaces;
using server.Dal.Interfaces;
using server.Models;

namespace server.Bll.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryDal _categoryDal;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryDal categoryDal, ILogger<CategoryService> logger)
        {
            _categoryDal = categoryDal;
            _logger = logger;
        }

        public async Task Add(Category category)
        {
            _logger.LogInformation($"Adding category: {category.Name}");
            try
            {
                await _categoryDal.Add(category);
                _logger.LogInformation($"Category {category.Name} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding category: {category.Name}");
                throw;
            }
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting category with id {id}");
            try
            {
                await _categoryDal.Delete(id);
                _logger.LogInformation($"Category with id {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category with id {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> Get()
        {
            _logger.LogInformation("Getting all categories");
            try
            {
                return await _categoryDal.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all categories");
                throw;
            }
        }

        public async Task<Category> Get(int id)
        {
            _logger.LogInformation($"Getting category with id {id}");
            try
            {
                return await _categoryDal.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting category with id {id}");
                throw;
            }
        }

        public async Task<bool> NameExist(string name)
        {
            _logger.LogInformation($"Checking if category name exists: {name}");
            try
            {
                return await _categoryDal.NameExist(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if category name exists: {name}");
                throw;
            }
        }

        public async Task Update(int id, Category category)
        {
            _logger.LogInformation($"Updating category with id {id}");
            try
            {
                await _categoryDal.Update(id, category);
                _logger.LogInformation($"Category with id {id} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category with id {id}");
                throw;
            }
        }
    }
}
