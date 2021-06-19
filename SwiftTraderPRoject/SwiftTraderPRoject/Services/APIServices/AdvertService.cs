using Newtonsoft.Json;
using SwiftTraderPRoject.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwiftTraderPRoject.Services.APIServices
{
    public class AdvertService
    {
        HttpClient client;
        private static readonly string BaseAddress = "https://www.google.com/";
        private static readonly string URL = $"{BaseAddress}/api/AdvertModel";
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

        public async Task<IEnumerable<AdvertModel>> GetAllProducts()
        {
            client = await GetClient();
            string result = await client.GetStringAsync(URL);
            return JsonConvert.DeserializeObject<IEnumerable<AdvertModel>>(result);
        }

        public async Task<AdvertModel> AddProduct(string ad)
        {
            AdvertModel ads = new AdvertModel()
            {
                SwiftTraderAdVert = ad
            };

            client = await GetClient();
            var response = await client.PostAsync(URL, new StringContent(JsonConvert.SerializeObject(ads), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<AdvertModel>(await response.Content.ReadAsStringAsync());
        }

        public async Task<AdvertModel> GetProduct(string id)
        {
            client = await GetClient();
            string ads = await client.GetStringAsync(URL + id);
            return JsonConvert.DeserializeObject<AdvertModel>(ads);
        }

        public async Task RemoveUser(string id)
        {
            client = await GetClient();
            await client.DeleteAsync(URL + id);
        }
    }
}
