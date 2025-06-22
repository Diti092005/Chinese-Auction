using server.Models;

namespace server.DAL.intefaces
{
    public interface ICategoryDal
    {
        public Task<List<Category>> Get();
        public Task<Category> Get(int id);
        public Task Add(Category category);
        public Task Update(int id, Category category);
        public Task Delete(int id);
        public Task<bool> NameExist(string name);
    }
}
