using MyShop.Configs;
using MyShop.Helpers;
using MyShop.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class StatisticService
    {
        private static RestClient _client = RestSharpClient.getInstance();
        public static async Task<RevenueAndProfit> GetStatisticRevenue(string type, string accessToken, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            try
            {
                RestRequest request;
                if (type == "Date" && dateStart != null && dateEnd != null)
                {
                    request = new RestRequest("/statistics/getStatistic?type=date&dateStart=" + dateStart.Value.ToString("yyyy-MM-ddTHH:mm:ss") + "&dateEnd=" + dateEnd.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
                    request.AddHeader("Authorization", $"Bearer {accessToken}");
                }
                else
                {
                    request = new RestRequest("/statistics/getStatistic?type=" + type.ToLower());
                    request.AddHeader("Authorization", $"Bearer {accessToken}");
                }
                var response = await _client.ExecuteGetAsync(request);

                if (response.IsSuccessful)
                {
                    var jsonResponse = JsonConvert.DeserializeObject<ResponseStatistic>(response.Content);

                    if (jsonResponse.Status == "success")
                    {
                        // Trả về danh sách dữ liệu từ response
                        //var revenueData = new RevenueAndProfit
                        //{
                        //    Revenue = jsonResponse.Data[0].ToObject<List<double>>(),
                        //    Profit = jsonResponse.Data[1].ToObject<List<double>>(),
                        //    Labels = jsonResponse.Data[2].ToObject<List<string>>(),
                        //};
                        var revenueData = new RevenueAndProfit
                        {
                            Revenue = jsonResponse.Data[0].Select(item => Convert.ToDouble(item)).ToList(),
                            Profit = jsonResponse.Data[1].Select(item => Convert.ToDouble(item)).ToList(),
                            Labels = jsonResponse.Data[2].Select(item => Convert.ToString(item)).ToList(),
                        };
                        return revenueData;
                    }
                    else
                    {
                        // Xử lý lỗi nếu có
                        Console.WriteLine($"Error: {jsonResponse.Message}");
                        return null;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu có
                    Console.WriteLine($"Error: {response.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

            // Trả về null nếu có lỗi
            //return null;
        }

        public static async Task<ProductSold> GetStatisticSold(string type, string accessToken, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            try
            {
                RestRequest request;
                if (type == "Date" && dateStart != null && dateEnd != null)
                {
                    request = new RestRequest("/statistics/getProductSold?type=date&dateStart=" + dateStart.Value.ToString("yyyy-MM-ddTHH:mm:ss") + "&dateEnd=" + dateEnd.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
                    request.AddHeader("Authorization", $"Bearer {accessToken}");
                }
                else
                {
                    request = new RestRequest("/statistics/getProductSold?type=" + type.ToLower());
                    request.AddHeader("Authorization", $"Bearer {accessToken}");
                }
                var response = await _client.ExecuteGetAsync(request);

                if (response.IsSuccessful)
                {
                    var jsonResponse = JsonConvert.DeserializeObject<ResponseStatistic>(response.Content);

                    if (jsonResponse.Status == "success")
                    {
                        ////Trả về danh sách dữ liệu từ response
                        //var soldData = new ProductSold
                        //{
                        //    SoldQuantity = jsonResponse.Data[0].ToObject<List<int>>(),
                        //    RemainingQuantity = jsonResponse.Data[1].ToObject<List<int>>(),
                        //    ProductNames = jsonResponse.Data[2].ToObject<List<string>>(),
                        //};

                        var soldData = new ProductSold
                        {
                            //SoldQuantity = jsonResponse.Data[0].Select(item => Convert.ToInt32(item)).ToList(),
                            //RemainingQuantity = jsonResponse.Data[1].Select(item => Convert.ToInt32(item)).ToList(),
                            //ProductNames = jsonResponse.Data[2].Select(item => Convert.ToString(item)).ToList(),
                            SoldQuantity = jsonResponse.Data[0].Select(item => Convert.ToDouble(item)).ToList(),
                            RemainingQuantity = jsonResponse.Data[1].Select(item => Convert.ToDouble(item)).ToList(),
                            ProductNames = jsonResponse.Data[2].Select(item => Convert.ToString(item)).ToList()
                        };
                        return soldData;
                    }
                    else
                    {
                        // Xử lý lỗi nếu có
                        Console.WriteLine($"Error: {jsonResponse.Message}");
                        return null;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu có
                    Console.WriteLine($"Error: {response.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

            // Trả về null nếu có lỗi

        }
    }
}
