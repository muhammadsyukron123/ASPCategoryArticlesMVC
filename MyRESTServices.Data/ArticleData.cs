using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.Data
{
    public class ArticleData : IArticleData
    {
        private AppDbContext _context;
        public ArticleData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var articleDelete = await _context.Articles.FindAsync(id);
                if (articleDelete == null)
                {
                    throw new Exception("Article not found");
                }
                _context.Articles.Remove(articleDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while deleting article", ex);
            }
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            try
            {
                var articles = await _context.Articles.ToListAsync();
                if (articles == null)
                {
                    throw new Exception("No articles found");
                }
                return articles;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while getting articles", ex);
            }
        }

        public async Task<IEnumerable<Article>> GetArticleByCategory(int categoryId)
        {
            try
            {
                var articles = await _context.Articles.Include(c => c.Category).Where(a => a.CategoryId == categoryId).ToListAsync();
                if (articles == null)
                {
                    throw new Exception("No articles found");
                }
                return articles;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while getting articles by category", ex);
            }
        }

        public async Task<IEnumerable<Article>> GetArticleWithCategory()
        {
            try
            {
                var articles = await _context.Articles.Include(a => a.Category).ToListAsync();
                if (articles == null)
                {
                    throw new Exception("No articles found");
                }
                return articles;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while getting articles with category", ex);
            }
        }

        public async Task<Article> GetById(int id)
        {
            try
            {
                var article = await _context.Articles.Include(c => c.Category).FirstOrDefaultAsync(x => x.ArticleId == id);
                if (article == null)
                {
                    throw new Exception("Article not found");
                }
                return article;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while getting article by ID", ex);
            }
        }

        public async Task<Article> Insert(Article entity)
        {
            try
            {
                _context.Articles.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while inserting article", ex);
            }
        }

        public async Task<Task> InsertArticleWithCategory(Article article)
        {
            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while inserting article", ex);
            }
        }

        public async Task<int> InsertWithIdentity(Article article)
        {
            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return article.ArticleId;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while inserting article", ex);
            }
        }

        public async Task<Article> Update(Article entity)
        {
            try
            {
                var existingArticle = await _context.Articles.FindAsync(entity.ArticleId);
                if (existingArticle == null)
                {
                    throw new Exception("Article not found");
                }
                _context.Entry(existingArticle).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new ArgumentException("Error occurred while updating article", ex);
            }
        }
    }
}
