
using MyRESTServices.Domain;
using System.Collections.Generic;

namespace MyRESTServices.Data.Interfaces
{
    public interface IArticleData : ICrudData<Article>
    {
        Task<IEnumerable<Article>> GetArticleWithCategory();
        Task<IEnumerable<Article>> GetArticleByCategory(int categoryId);
        Task<int> InsertWithIdentity(Article article);
        Task<Task> InsertArticleWithCategory(Article article);
    }
}
