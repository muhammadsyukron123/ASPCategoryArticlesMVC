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
            try
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
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while deleting category", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                if (categories == null)
                {
                    throw new Exception("No categories found");
                }
                return categories;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting categories", ex);
            }
        }

        public async Task<Category> GetById(int id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                return category;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting category by ID", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetByName(string name)
        {
            try
            {
                var categories = await _context.Categories.Where(c => c.CategoryName.Contains(name)).ToListAsync();
                if (categories == null)
                {
                    throw new Exception("No categories found");
                }
                return categories;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting categories by name", ex);
            }
        }

        public async Task<int> GetCountCategories(string name = "")
        {
            try
            {
                var count = await _context.Categories.Where(c => string.IsNullOrEmpty(name) || c.CategoryName.Contains(name)).CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting count of categories", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetWithPaging(int pageNumber, int pageSize, string name = "")
        {
            try
            {
                var categories = await _context.Categories.Where(c => string.IsNullOrEmpty(name) || c.CategoryName.Contains(name))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                if (categories == null)
                {
                    throw new Exception("No categories found");
                }
                return categories;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting categories with paging", ex);
            }
        }

        public async Task<Category> Insert(Category entity)
        {
            try
            {
                _context.Categories.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while inserting category", ex);
            }
        }

        public async Task<int> InsertWithIdentity(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return category.CategoryId;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while inserting category with identity", ex);
            }
        }


        public async Task<Category> Update(Category entity)
        {
            try
            {
                _context.Categories.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while updating category", ex);
            }
        }

        public async Task<int> CountCategories(string name ="")
        {
            try
            {
                var count = await _context.Categories.Where(c => string.IsNullOrEmpty(name) || c.CategoryName.Contains(name)).CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting count of categories", ex);
            }
        }
    }
}
