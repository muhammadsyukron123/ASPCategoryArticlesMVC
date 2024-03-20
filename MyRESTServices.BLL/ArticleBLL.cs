using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.BLL
{
    public class ArticleBLL : IArticleBLL
    {
        private IArticleData _articleData;
        private IMapper _mapper;

        public ArticleBLL(IArticleData articleData, IMapper mapper)
        {
            _articleData = articleData;
            _mapper = mapper;
        }

        public async Task<bool> Delete(int id)
        {
            var deleted = await _articleData.Delete(id);
            return deleted;
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticleByCategory(int categoryId)
        {
            var articles = await _articleData.GetArticleByCategory(categoryId);
            if (articles == null)
            {
                throw new Exception("No articles found");
            }
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<ArticleDTO> GetArticleById(int id)
        {
            var article = await _articleData.GetById(id);
            if (article == null)
            {
                throw new Exception("Article not found");
            }
            return _mapper.Map<ArticleDTO>(article);
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticleWithCategory()
        {
            var articles = await _articleData.GetArticleWithCategory();
            if (articles == null)
            {
                throw new Exception("No articles found");
            }
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<ArticleDTO> Insert(ArticleCreateDTO article)
        {
            var articleInsert = _mapper.Map<Article>(article);
            var inserted = await _articleData.Insert(articleInsert);
            return _mapper.Map<ArticleDTO>(inserted);
        }

        public async Task<int> InsertWithIdentity(ArticleCreateDTO article)
        {
            var articleInsert = _mapper.Map<Article>(article);
            var inserted = await _articleData.InsertWithIdentity(articleInsert);
            return inserted;
        }

        public async Task<Task> Update(ArticleUpdateDTO article)
        {
            var articleUpdate = _mapper.Map<Article>(article);
            await _articleData.Update(articleUpdate);
            return Task.CompletedTask;
        }
    }
}
