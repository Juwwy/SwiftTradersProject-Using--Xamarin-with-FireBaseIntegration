using Newtonsoft.Json;
using SwiftTraderPRoject.Models.RestModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SwiftTraderPRoject.Services.APIServices
{
    public class OrderService
    {
        HttpClient client;
        private static readonly string BaseAddress = "https://www.google.com/";
        private static readonly string URL = $"{BaseAddress}/api/Order";
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

       

        public async Task<IEnumerable<Order>> GetAllProducts()
        {
            client = await GetClient();
            string result = await client.GetStringAsync(URL);
            return JsonConvert.DeserializeObject<IEnumerable<Order>>(result);
        }

        public async Task<Order> AddOrder(string productId, string productName, decimal price, int quantity, string userId, string userName, decimal totalCost)
        {

            Order order = new Order()
            {
                ProductId = productId,
                ProductName = productName,
                Price = price,
                Quantity = quantity,
                UserId = userId,
                Username =userName,
                TotalCost = totalCost
            };

            client = await GetClient();
            var response = await client.PostAsync(URL, new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<Order>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Order> GetOrder(string id)
        {
            client = await GetClient();
            string order = await client.GetStringAsync(URL + id);
            return JsonConvert.DeserializeObject<Order>(order);
        }

        public async Task RemoveUser(string id)
        {
            client = await GetClient();
            await client.DeleteAsync(URL + id);
        }

        //public async Task<string> PlaceOrderAsync()
        //{
        //    var sqliteDB = DependencyService.Get<Models.ISqlite>().GetConnection();
        //    var record = sqliteDB.Table<Models.CartItem>().ToList();

        //    var orderId = Guid.NewGuid().ToString();
        //    var userName = Preferences.Get("Username", string.Empty);
        //    decimal totalCost = 0;

        //    foreach (var item in record)
        //    {
        //        Order data = new Order()
        //        {
                    
        //            OrderId = orderId,
        //            ProductName = item.ProductName,
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity,
        //            Price = item.Price,
        //        };

        //        totalCost += (item.Price * item.Quantity);
        //        await client.Child("OrderDetails").PostAsync(data);
        //    }

        //    Cost = totalCost;

        //    await client.Child("Orders").PostAsync(new Order()
        //    {
        //        OrderId = orderId,
        //        Username = userName,
        //        TotalCost = totalCost
        //    });

        //    return orderId;
        //}
    }
}
