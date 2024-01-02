using MyShop.Configs;
using MyShop.Helpers;
using MyShop.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class ClientService
    {
        private static RestClient _client = RestSharpClient.getInstance();

        public static async Task<List<Client>> GetCustomerList(string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/client");
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                // Gửi yêu cầu GET
                var response = await _client.ExecuteGetAsync(request);

                // Lấy nội dung từ phản hồi
                string responseBody = response.Content;
                Console.WriteLine(responseBody);

                // Kiểm tra xem yêu cầu có thành công không
                if (response.IsSuccessful)
                {
                    // Xử lý kết quả nếu cần thiết
                    var customerListResponse = JsonConvert.DeserializeObject<ResponseListCustomer>(response.Content);

                    if (customerListResponse.status == "ok")
                    {
                        //MessageBox.Show("Thêm đơn hàng thành công!");
                        return customerListResponse.data;
                    }
                    else
                    {
                        //MessageBox.Show("Thêm đơn hàng thất bại!");
                        return null;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu cần thiết
                    Console.WriteLine($"Error adding order: {response.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> UpdateClient(Client client, string accessToken)
        {
            try
            {
                // Tạo request mới
                var jsonBody = new
                {
                    Id = client.Id,
                    Name = client.Name,
                    Address = client.Address,
                    PhoneNumber = client.PhoneNumber,
                    Email = client.Email
                };
                var request = new RestRequest("/client");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AddJsonBody(jsonBody);

                // Gửi yêu cầu POST
                var response = await _client.ExecutePostAsync(request);

                // Lấy nội dung từ phản hồi
                string responseBody = response.Content;
                Console.WriteLine(responseBody);

                // Kiểm tra xem yêu cầu có thành công không
                if (response.IsSuccessful)
                {
                    //// Xử lý kết quả nếu cần thiết
                    //ResponseClientUpdate jsonResponse = JsonConvert.DeserializeObject<ResponseClientUpdate>(responseBody);

                    //Client newClient = new Client
                    //{
                    //    Id = jsonResponse.Client.Id,
                    //    Name = jsonResponse.Client.Name,
                    //    Address = jsonResponse.Client.Address,
                    //    PhoneNumber = jsonResponse.Client.PhoneNumber,
                    //    Email = jsonResponse.Client.Email
                    //};
                    //MessageBox.Show("Cập nhật khách hàng thành công!");
                    return true;

                }
                else
                {
                    // Xử lý lỗi nếu cần thiết
                    // Console.WriteLine($"Error updating client: {response.ErrorMessage}");
                    //MessageBox.Show("Cập nhật khách hàng thất bại!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error updating client: {ex.Message}");
                //MessageBox.Show("Cập nhật khách hàng thất bại!");
                return false;
            }
        }

        public static async Task<bool> DeleteClient(int clientID, string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/client/delete?id=" + clientID, Method.Post);
                // request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                // Gửi yêu cầu DELETE
                var response = await _client.ExecuteAsync(request);
                ResponseClient jsonResponse = JsonConvert.DeserializeObject<ResponseClient>(response.Content);
                // Kiểm tra xem yêu cầu có thành công không
                if (jsonResponse.status == "success")
                {
                    // Xử lý kết quả nếu cần thiết
                    return true;
                }
                else
                {
                    // Xử lý lỗi nếu cần thiết
                    Console.WriteLine($"Error deleting product: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần thiết
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false;
            }
        }
    }
}
