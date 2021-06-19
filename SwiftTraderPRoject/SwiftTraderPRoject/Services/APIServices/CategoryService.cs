using Newtonsoft.Json;
using SwiftTraderPRoject.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwiftTraderPRoject.Services.APIServices
{
    public class CategoryService
    {
        HttpClient client;
        private static readonly string BaseAddress = "https://www.google.com/";
        private static readonly string URL = $"{BaseAddress}/api/Category";
        private string authorizationKey;


        private async Task<HttpClient> GetClient()
        {
            client = new HttpClient();
            if (string.IsNullOrEmpty(authorizationKey))
            {
                authorizationKey = await client.GetStringAsync(URL + "login");
                authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
            }

            client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            client = await GetClient();
            string result = await client.GetStringAsync(URL);
            return JsonConvert.DeserializeObject<IEnumerable<Category>>(result);
        }

        public async Task<Category> AddCategory(string categoryName, string description, string imgUrl)
        {
            Category category = new Category()
            {
                CategoryName = categoryName,
                Description = description,
                ImgUrl = imgUrl
            };

            client = await GetClient();
            var response = await client.PostAsync(URL, new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Category> GetProduct(string id)
        {
            client = await GetClient();
            string cate = await client.GetStringAsync(URL + id);
            return JsonConvert.DeserializeObject<Category>(cate);
        }

        public async Task RemoveUser(string id)
        {
            client = await GetClient();
            await client.DeleteAsync(URL + id);
        }
    }
}
