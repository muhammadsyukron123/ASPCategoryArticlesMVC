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
            try
            {
                var deleted = await _articleData.Delete(id);
                return deleted;
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("Error occurred while deleting article", ex);
            }
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticleByCategory(int categoryId)
        {
            try
            {
                var articles = await _articleData.GetArticleByCategory(categoryId);
                if (articles == null)
                {
                    throw new Exception("No articles found");
                }
                return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("Error occurred while getting articles by category", ex);
            }
        }

        public async Task<ArticleDTO> GetArticleById(int id)
        {
            try
            {
                var article = await _articleData.GetById(id);
                if (article == null)
                {
                    throw new Exception("Article not found");
                }
                return _mapper.Map<ArticleDTO>(article);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("Error occurred while getting article by ID", ex);
            }
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticleWithCategory()
        {
            try
            {
                var articles = await _articleData.GetArticleWithCategory();
                if (articles == null)
                {
                    throw new Exception("No articles found");
                }
                return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("Error occurred while getting articles with category", ex);
            }
        }

        public async Task<ArticleDTO> Insert(ArticleCreateDTO article)
        {
            try
            {
                var articleInsert = _mapper.Map<Article>(article);
                var inserted = await _articleData.Insert(articleInsert);
                return _mapper.Map<ArticleDTO>(inserted);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("Error occurred while inserting article", ex);
            }
        }

        public async Task<int> InsertWithIdentity(ArticleCreateDTO article)
        {
            try
            {
                var articleInsert = _mapper.Map<Article>(article);
                var inserted = await _articleData.InsertWithIdentity(articleInsert);
                return inserted;
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("Error occurred while inserting article with identity", ex);
            }
        }

        public async Task<ArticleDTO> Update(ArticleUpdateDTO article)
        {
            try
            {
                var existingArticle = await _articleData.GetById(article.ArticleID);
                if (existingArticle == null)
                {
                    throw new Exception("Article not found");
                }

                existingArticle.CategoryId = article.CategoryID;
                existingArticle.ArticleId = article.ArticleID;
                existingArticle.Title = article.Title;
                existingArticle.Details = article.Details;
                existingArticle.IsApproved = article.IsApproved;
                existingArticle.Pic = article.Pic;

                await _articleData.Update(existingArticle);
                return _mapper.Map<ArticleDTO>(existingArticle);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw new Exception("Error occurred while updating article", ex);
            }
        }
    }
}
