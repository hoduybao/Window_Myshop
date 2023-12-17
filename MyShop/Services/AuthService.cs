using MyShop.Configs;
using MyShop.Helpers;
using MyShop.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class AuthService
    {
        private static RestClient _client = RestSharpClient.getInstance();

        public static async Task<bool> Login(Account account)
        {
            var request = new RestRequest("/auth/shopLogin");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(account);

            var response = await _client.ExecutePostAsync(request);

            Response jsonResponse = JsonConvert.DeserializeObject<Response>(response.Content);

            if (jsonResponse.status == "ok")
            {
                // using (var writer = new StreamWriter("D:\\data.json"))
                //string DocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //string currentDirectory = Environment.CurrentDirectory;
                string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine(appFolderPath);
                using (var writer = new StreamWriter(Path.Combine(appFolderPath, "data.json")))
                {
                    writer.Write(jsonResponse.data);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
