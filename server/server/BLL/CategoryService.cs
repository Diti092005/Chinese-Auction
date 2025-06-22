using server.BLL.Intefaces;
using server.DAL.intefaces;
using server.Models;

namespace server.BLL
{
    public class CategoryService : ICategoryService
    {
        private ICategoryDal _categoryDal;
        public CategoryService(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task Add(Category category)
        {
            await _categoryDal.Add(category);
        }

        public async Task Delete(int id)
        {
            await _categoryDal.Delete(id);
        }

        public async Task<IEnumerable<Category>> Get()
        {
            return await _categoryDal.Get();
        }

        public async Task<Category> Get(int id)
        {
            return await _categoryDal.Get(id);
        }

        public async Task<bool> NameExist(string name)
        {
            return await _categoryDal.NameExist(name);
        }

        public async Task Update(int id, Category category)
        {
            await _categoryDal.Update(id, category);
        }
    }
}
