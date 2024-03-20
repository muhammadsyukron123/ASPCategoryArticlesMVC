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
    public class CategoryData : ICategoryData
    {
        private AppDbContext _context;

        public CategoryData(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<bool> Delete(int id)
        {
            var categoryDelete = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (categoryDelete == null)
            {
                throw new Exception("Category not found");
            }
            _context.Categories.Remove(categoryDelete);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories == null)
            {
                throw new Exception("No categories found");
            }
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return category;
        }

        public async Task<IEnumerable<Category>> GetByName(string name)
        {
            var categories = await _context.Categories.Where(c => c.CategoryName.Contains(name)).ToListAsync();
            if (categories == null)
            {
                throw new Exception("No categories found");
            }
            return categories;
        }

        public async Task<int> GetCountCategories(string name)
        {
            var count = await _context.Categories.Where(c => c.CategoryName.Contains(name)).CountAsync();
            return count;
        }

        public async Task<IEnumerable<Category>> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            var categories = await _context.Categories.Where(c => c.CategoryName.Contains(name))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            if (categories == null)
            {
                throw new Exception("No categories found");
            }
            return categories;
        }

        public async Task<Category> Insert(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> InsertWithIdentity(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task<Category> Update(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
