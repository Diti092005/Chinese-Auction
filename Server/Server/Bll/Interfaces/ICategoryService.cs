using Server.Models;
using Server.Models.DTO;

namespace Server.Bll.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> Get();
        public Task<Category> Get(int id);
        public Task<CategoryDTO> Add(CategoryDTO categoryDto);
        public Task Update(int id, CategoryDTO categoryDto);
        public Task Delete(int id);
        public Task<bool> NameExist(string name);
    }
}
