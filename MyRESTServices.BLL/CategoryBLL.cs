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
    public class CategoryBLL : ICategoryBLL
    {
        private ICategoryData _categoryData;
        private IMapper _mapper;

        public CategoryBLL(ICategoryData categoryData, IMapper mapper)
        {
            try
            {
                _categoryData = categoryData;
                _mapper = mapper;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while initializing CategoryBLL: " + ex.Message);
                throw;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var category = await _categoryData.GetById(id);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                var deleted = await _categoryData.Delete(id);
                return deleted;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while deleting the category: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            try
            {
                var categories = await _categoryData.GetAll();
                if (categories == null)
                {
                    throw new Exception("No categories found");
                }
                return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting all categories: " + ex.Message);
                throw;
            }
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            try
            {
                var category = await _categoryData.GetById(id);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting category by ID: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetByName(string name)
        {
            try
            {
                var categories = await _categoryData.GetByName(name);
                if (categories == null)
                {
                    throw new Exception("No categories found");
                }
                return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting categories by name: " + ex.Message);
                throw;
            }
        }

        public async Task<int> GetCountCategories(string name)
        {
            try
            {
                var count = await _categoryData.GetCountCategories(name);
                return count;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting the count of categories: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            try
            {
                var categories = await _categoryData.GetWithPaging(pageNumber, pageSize, name);
                if (categories == null)
                {
                    throw new Exception("No categories found");
                }
                return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting categories with paging: " + ex.Message);
                throw;
            }
        }

        public async Task<Task> Insert(CategoryCreateDTO entity)
        {
            try
            {
                var category = _mapper.Map<Category>(entity);
                await _categoryData.Insert(category);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while inserting the category: " + ex.Message);
                throw;
            }
        }

        public async Task<Task> Update(CategoryUpdateDTO entity)
        {
            try
            {
                var existingCategory = await _categoryData.GetById(entity.CategoryID);
                if (existingCategory == null)
                {
                    throw new Exception("Category not found");
                }
                _mapper.Map(entity, existingCategory);
                await _categoryData.Update(existingCategory);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while updating the category: " + ex.Message);
                throw;
            }
        }
    }
}
