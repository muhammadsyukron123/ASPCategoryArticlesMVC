using MyRESTServices.BLL.DTOs;
using System.Text;
using System.Text.Json;

namespace CategoryArticlesMVC.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public CategoryServices(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        private string BaseUrl()
        {
            return _configuration.GetSection("BaseUrl").Value;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var httpResponse = await _client.GetAsync(BaseUrl() + "/Categories");


            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve category");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (categories == null)
            {
                throw new Exception("No categories found");
            }

            return categories;
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            var httpResponse = await _client.GetAsync(BaseUrl() + "/Categories/" + id);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve category");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<CategoryDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (category == null)
            {
                throw new Exception("No category found");
            }

            return category;
        }

        public async Task<CategoryDTO> Insert(CategoryCreateDTO categoryCreateDTO)
        {
            var content = new StringContent(JsonSerializer.Serialize(categoryCreateDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(BaseUrl() + "/Categories", content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot insert category");
            }

            var createdContent = await httpResponse.Content.ReadAsStringAsync();
            var createdCategory = JsonSerializer.Deserialize<CategoryDTO>(createdContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (createdCategory == null)
            {
                throw new Exception("No category created");
            }

            return createdCategory;
        }

        public async Task<CategoryDTO> Update(CategoryUpdateDTO categoryUpdateDTO)
        {
            var content = new StringContent(JsonSerializer.Serialize(categoryUpdateDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(BaseUrl() + "/Categories/" + categoryUpdateDTO.CategoryID, content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot update category");
            }

            var updatedContent = await httpResponse.Content.ReadAsStringAsync();
            var updatedCategory = JsonSerializer.Deserialize<CategoryDTO>(updatedContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (updatedCategory == null)
            {
                throw new Exception("No category updated");
            }

            return updatedCategory;
        }

        public async Task<bool> Delete(int id)
        {
            var httpResponse = await _client.DeleteAsync(BaseUrl() + "/Categories/" + id);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot delete category");
            }

            return true;
        }

        //add
    }
}
