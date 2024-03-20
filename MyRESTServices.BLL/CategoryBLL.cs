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

            _categoryData = categoryData;
            _mapper = mapper;
        }
        public async Task<bool> Delete(int id)
        {
            var category = await _categoryData.GetById(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            var deleted = await _categoryData.Delete(id);
            return deleted;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await _categoryData.GetAll();
            if (categories == null)
            {
                throw new Exception("No categories found");
            }
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            var category = await _categoryData.GetById(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetByName(string name)
        {
            var categories = await _categoryData.GetByName(name);
            if (categories == null)
            {
                throw new Exception("No categories found");
            }
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<int> GetCountCategories(string name)
        {
            var count = await _categoryData.GetCountCategories(name);
            return count;
        }

        public async Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            var categories = await _categoryData.GetWithPaging(pageNumber, pageSize, name);
            if (categories == null)
            {
                throw new Exception("No categories found");
            }
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<Task> Insert(CategoryCreateDTO entity)
        {
            var category = _mapper.Map<Category>(entity);
            await _categoryData.Insert(category);
            return Task.CompletedTask;
        }

        public async Task<Task> Update(CategoryUpdateDTO entity)
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
    }
}
