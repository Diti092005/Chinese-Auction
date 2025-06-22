using server.Models;

namespace server.BLL.Intefaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> Get();
        public Task<Category> Get(int id);
        public Task Add(Category category);
        public Task Update(int id, Category category);
        public Task Delete(int id);
        public Task<bool> NameExist(string name);
    }
}
