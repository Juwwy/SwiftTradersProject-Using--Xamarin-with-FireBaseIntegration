using Newtonsoft.Json;
using SwiftTraderPRoject.Models.RestModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwiftTraderPRoject.Services.APIServices
{
    public class ProductService
    {
        HttpClient client;
        private static readonly string BaseAddress = "https://www.google.com/";
        private static readonly string URL = $"{BaseAddress}/api/Products";
        private string authorizationKey;
        public ObservableCollection<Products> MyProperty { get; set; }

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

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            client = await GetClient();
            string result = await client.GetStringAsync(URL);
            return JsonConvert.DeserializeObject<IEnumerable<Products>>(result);
        }

        public async Task<Products> AddProduct(string catId, string productName, string description, string imgUrl, decimal price)
        {
            Products product = new Products()
            {
                CatId = catId,
                ProductName = productName,
                Description = description,
                ImgUrl = imgUrl,
                Price = price
            };

            client = await GetClient();
            var response = await client.PostAsync(URL, new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<Products>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Products> GetProduct(string id)
        {
            client = await GetClient();
            string product = await client.GetStringAsync(URL + id);
            return JsonConvert.DeserializeObject<Products>(product);
        }

        public async Task RemoveUser(string id)
        {
            client = await GetClient();
            await client.DeleteAsync(URL + id);
        }

        public async Task<ObservableCollection<Products>> GetProductsByCategoryAsync(string id)
        {
            var itemCategory = new ObservableCollection<Products>();
            var Items = (await GetAllProducts()).Where(p => p.CatId == id).ToList();
            foreach (var item in Items)
            {
                itemCategory.Add(item);
            }

            return itemCategory;
        }

        public async Task<ObservableCollection<Products>> GetLatestProduct()
        {
            var latestItem = new ObservableCollection<Products>();
            var Items = (await GetAllProducts()).OrderByDescending(f => f.ProductName).Take(5);

            foreach (var item in Items)
            {
                latestItem.Add(item);
            }

            return latestItem;
        }
    }
}
