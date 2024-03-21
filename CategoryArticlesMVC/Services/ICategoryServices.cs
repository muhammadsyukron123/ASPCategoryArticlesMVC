
using MyRESTServices.BLL.DTOs;

namespace CategoryArticlesMVC.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDTO>> GetAll();
        Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber, int pageSize, string name);
        Task<int> GetCountCategories(string name);
        Task<CategoryDTO> GetById(int id);
        Task<CategoryDTO> Insert(CategoryCreateDTO categoryCreateDTO);
        Task<CategoryDTO> Update(CategoryUpdateDTO categoryUpdateDTO);
        
        Task<bool> Delete(int id);
    }
}
