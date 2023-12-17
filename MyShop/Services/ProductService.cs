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
    public class ProductService
    {
        private static RestClient _client = RestSharpClient.getInstance();

        public static async Task<List<Product>> GetProductList()
        {
            var request = new RestRequest("/product/getAll");
            var response = await _client.ExecuteGetAsync(request);
            var productListResponse = JsonConvert.DeserializeObject<ResponseData<List<Product>>>(response.Content);

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

        public static async Task<Product> GetProductById(int productId, string accessToken)
        {
            var request = new RestRequest("/product/" + productId);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await _client.ExecuteGetAsync(request);
            var product = JsonConvert.DeserializeObject<Respon1Data>(response.Content);

            if (response.IsSuccessful)
            {
                return product.Data; ;
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
                return null;
            }
        }

        public static async Task<Product> AddProduct(Product product, string accessToken)
        {
            try
            {
                // Tạo đối tượng JSON theo thông tin của sản phẩm
                var jsonBody = new
                {
                    // Id = product.Id,
                    name = product.Name,
                    price = product.Price,
                    category = product.Category.Id,
                    image = product.Image,
                    amount = product.Amount,
                    discount = product.Discount
                };

                // Tạo request mới
                var request = new RestRequest("/product/add");
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
                    // Xử lý kết quả nếu cần thiết
                    Respon1Data jsonResponse = JsonConvert.DeserializeObject<Respon1Data>(responseBody);

                    if (jsonResponse.Status == "ok")
                    {
                        Product newProduct = new Product
                        {
                            Name = jsonResponse.Data.Name,
                            Price = jsonResponse.Data.Price,
                            Category = jsonResponse.Data.Category,
                            Image = jsonResponse.Data.Image,
                            Amount = jsonResponse.Data.Amount,
                            Discount = jsonResponse.Data.Discount,

                        };
                        MessageBox.Show("Thêm sản phẩm thành công!");
                        return newProduct;
                    }
                    else
                    {
                        MessageBox.Show("Thêm sản phẩm thất bại!");
                        return null;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu cần thiết
                    Console.WriteLine($"Error adding product: {response.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                return null;
            }
        }

        //public static async Task<bool> UpdateProduct(Product product, string accessToken)
        //{
        //    try
        //    {
        //        // Tạo đối tượng JSON theo thông tin của sản phẩm
        //        var jsonBody = new
        //        {
        //            id = product.Id,
        //            name = product.Name,
        //            price = product.Price,
        //            category = product.Category.Id,
        //            image = product.Image,
        //            amount = product.Amount,
        //            discount = product.Discount
        //        };

        //        // Tạo request mới
        //        var request = new RestRequest("/product/update");
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddHeader("Authorization", $"Bearer {accessToken}");
        //        request.AddJsonBody(jsonBody);

        //        // Gửi yêu cầu POST
        //        var response = await _client.ExecutePostAsync(request);

        //        // Lấy nội dung từ phản hồi
        //        string responseBody = response.Content;
        //        Console.WriteLine(responseBody);

        //        // Kiểm tra xem yêu cầu có thành công không
        //        if (response.IsSuccessful)
        //        {
        //            // Xử lý kết quả nếu cần thiết
        //            Respon1Data jsonResponse = JsonConvert.DeserializeObject<Respon1Data>(responseBody);

        //            if (jsonResponse.Status == "ok")
        //            {
        //                //Product newProduct = new Product
        //                //{
        //                //    Name = jsonResponse.Data.Name,
        //                //    Price = jsonResponse.Data.Price,
        //                //    Category = jsonResponse.Data.Category,
        //                //    Image = jsonResponse.Data.Image,
        //                //    Amount = jsonResponse.Data.Amount,
        //                //    Discount = jsonResponse.Data.Discount,

        //                //};
        //                //MessageBox.Show("Cập nhật sản phẩm thành công!");
        //                return true;
        //            }
        //            else
        //            {
        //                //MessageBox.Show("Cập nhật sản phẩm thất bại!");
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            // Xử lý lỗi nếu cần thiết
        //            Console.WriteLine($"Error updating product: {response.ErrorMessage}");
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error updating product: {ex.Message}");
        //        return false;
        //    }
        //}

        public static async Task<Product> UpdateProduct(Product product, string accessToken)
        {
            try
            {
                // Tạo đối tượng JSON theo thông tin của sản phẩm
                var jsonBody = new
                {
                    id = product.Id,
                    name = product.Name,
                    price = product.Price,
                    category = product.Category.Id,
                    image = product.Image,
                    amount = product.Amount,
                    discount = product.Discount
                };

                // Tạo request mới
                var request = new RestRequest("/product/update");
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
                    // Xử lý kết quả nếu cần thiết
                    Respon1Data jsonResponse = JsonConvert.DeserializeObject<Respon1Data>(responseBody);

                    if (jsonResponse.Status == "ok")
                    {
                        Product newProduct = new Product
                        {
                            Name = jsonResponse.Data.Name,
                            Price = jsonResponse.Data.Price,
                            Category = jsonResponse.Data.Category,
                            Image = jsonResponse.Data.Image,
                            Amount = jsonResponse.Data.Amount,
                            Discount = jsonResponse.Data.Discount,

                        };
                        //MessageBox.Show("Cập nhật sản phẩm thành công!");
                        return newProduct;
                    }
                    else
                    {
                        //MessageBox.Show("Cập nhật sản phẩm thất bại!");
                        return null;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu cần thiết
                    Console.WriteLine($"Error updating product: {response.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> DeleteProduct(int productId, string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/product/" + productId, Method.Delete);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                // Gửi yêu cầu DELETE
                var response = await _client.ExecuteAsync(request);
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

        public static async Task<List<Product>> SearchProduct(string name, string accessToken)
        {
            var request = new RestRequest("/product/search?name=" + name);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await _client.ExecuteGetAsync(request);
            var productListResponse = JsonConvert.DeserializeObject<ResponseData<List<Product>>>(response.Content);

            if (response.IsSuccessful)
            {
                return productListResponse.Data;
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
                return null;
            }
        }
    }
}
