using MyShop.Configs;
using MyShop.Helpers;
using MyShop.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop.Services
{
    public class ProductTypeService
    {
        private static RestClient _client = RestSharpClient.getInstance();
        public static async Task<List<ProductType>> GetProductTypeList(string accessToken)
        {
            var request = new RestRequest("/cate/getAll");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            var response = await _client.ExecuteGetAsync(request);

            var productListResponse = JsonConvert.DeserializeObject<ResponseData<List<ProductType>>>(response.Content);

            if (response.IsSuccessful)
            {
                return productListResponse.Data; ;
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
                return null;
            }
        }

        public static async Task<ProductType> AddProductType(ProductType type, string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/cate/add?name=" + type.Name);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                // Gửi yêu cầu POST
                var response = await _client.ExecutePostAsync(request);

                // Lấy nội dung từ phản hồi
                string responseBody = response.Content;
                Console.WriteLine(responseBody);

                // Kiểm tra xem yêu cầu có thành công không
                if (response.IsSuccessful)
                {
                    // Xử lý kết quả nếu cần thiết
                    Respon1ProductType jsonResponse = JsonConvert.DeserializeObject<Respon1ProductType>(responseBody);

                    if (jsonResponse.Status == "ok")
                    {
                        ProductType newType = new ProductType
                        {
                            Id = jsonResponse.Data.Id,
                            Name = jsonResponse.Data.Name
                        };
                        MessageBox.Show("Thêm loại sản phẩm mới thành công!");
                        return newType;
                    }
                    else
                    {
                        MessageBox.Show("Thêm loại sản phẩm mới thất bại!");
                        return null;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu cần thiết
                    Console.WriteLine($"Error adding category: {response.ErrorMessage}");
                    MessageBox.Show("Thêm loại sản phẩm mới thất bại!");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
                MessageBox.Show("Thêm loại sản phẩm mới thất bại!");
                return null;
            }
        }

        public static async Task<ProductType> UpdateProductType(ProductType type, string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/cate/update?categoryId=" + type.Id + "&name=" + type.Name);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                // Gửi yêu cầu POST
                var response = await _client.ExecutePutAsync(request);

                // Lấy nội dung từ phản hồi
                string responseBody = response.Content;
                Console.WriteLine(responseBody);

                // Kiểm tra xem yêu cầu có thành công không
                if (response.IsSuccessful)
                {
                    // Xử lý kết quả nếu cần thiết
                    Respon1ProductType jsonResponse = JsonConvert.DeserializeObject<Respon1ProductType>(responseBody);

                    if (jsonResponse.Status == "ok")
                    {
                        ProductType newType = new ProductType
                        {
                            Id = jsonResponse.Data.Id,
                            Name = jsonResponse.Data.Name
                        };
                        MessageBox.Show("Cập nhật loại sản phẩm mới thành công!");
                        return newType;
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật loại sản phẩm mới thất bại!");
                        return null;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu cần thiết
                    Console.WriteLine($"Error adding category: {response.ErrorMessage}");
                    MessageBox.Show("Cập nhật loại sản phẩm mới thất bại!");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
                MessageBox.Show("Cập nhật loại sản phẩm mới thất bại!");
                return null;
            }
        }

        public static async Task<bool> DeleteProductType(int typeId, string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/cate/delete/" + typeId);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                // Gửi yêu cầu DELETE
                var response = await _client.ExecutePostAsync(request);
                Response jsonResponse = JsonConvert.DeserializeObject<Response>(response.Content);
                // Kiểm tra xem yêu cầu có thành công không
                if (jsonResponse.status == "ok")
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
