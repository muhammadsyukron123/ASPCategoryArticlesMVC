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
            var articleDelete = await _context.Articles.FindAsync(id);
            if (articleDelete == null)
            {
                throw new Exception("Article not found");
            }
            _context.Articles.Remove(articleDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await _context.Articles.ToListAsync();
            if (articles == null)
            {
                throw new Exception("No articles found");
            }
            return articles;
        }

        public async Task<IEnumerable<Article>> GetArticleByCategory(int categoryId)
        {
            var articles = await _context.Articles.Where(a => a.CategoryId == categoryId).ToListAsync();
            if (articles == null)
            {
                throw new Exception("No articles found");
            }
            return articles;
        }

        public async Task<IEnumerable<Article>> GetArticleWithCategory()
        {
            var articles = await _context.Articles.Include(a => a.Category).ToListAsync();
            if (articles == null)
            {
                throw new Exception("No articles found");
            }
            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                throw new Exception("Article not found");
            }
            return article;
        }

        public async Task<Article> Insert(Article entity)
        {
            _context.Articles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Task> InsertArticleWithCategory(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<int> InsertWithIdentity(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article.ArticleId;
        }

        public async Task<Article> Update(Article entity)
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
    }
}
