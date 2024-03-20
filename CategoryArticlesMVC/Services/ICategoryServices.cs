using MyWebFormApp.BO;

namespace CategoryArticlesMVC.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
