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
    public class OrderService
    {
        private static RestClient _client = RestSharpClient.getInstance();

        public static async Task<bool> UpdateProductAmount(int productId, int newAmount, string accessToken)
        {
            // Lấy thông tin sản phẩm cần cập nhật
            Product productToUpdate = await ProductService.GetProductById(productId, accessToken);

            if (productToUpdate != null)
            {
                // Cập nhật số lượng sản phẩm mới
                productToUpdate.Amount = newAmount;

                // Gọi hàm cập nhật sản phẩm
                Product updatedProduct = await ProductService.UpdateProduct(productToUpdate, accessToken);

                // Kiểm tra xem cập nhật có thành công không
                return updatedProduct != null;
            }

            return false;
        }

        public static async Task<Order> AddOrder(Client client, OrderRequest orderRequest, string accessToken)
        {
            try
            {
                // Tạo đối tượng JSON theo thông tin của đơn hàng
                var jsonBody = new
                {
                    phone = client.PhoneNumber,
                    productIds = orderRequest.ProductIds,
                    quantities = orderRequest.Quantities,
                    price = orderRequest.Prices
                };

                // Tạo request mới
                var request = new RestRequest("/orders/add?name=" + client.Name + "&email=" + client.Email + "&address=" + client.Address);
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
                    Response1Order apiResponse = JsonConvert.DeserializeObject<Response1Order>(responseBody);

                    if (apiResponse.Status == "success")
                    {
                        //MessageBox.Show("Thêm đơn hàng thành công!");
                        return apiResponse.Data;
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

        public static async Task<OrderResponse> GetOrderListByPage(int currentPage, int pageSize, string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/orders/getByPage?page=" + currentPage + "&size=" + pageSize);
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                // Gửi yêu cầu POST
                var response = await _client.ExecuteGetAsync(request);

                // Lấy nội dung từ phản hồi
                string responseBody = response.Content;
                Console.WriteLine(responseBody);

                // Kiểm tra xem yêu cầu có thành công không
                if (response.IsSuccessful)
                {
                    // Xử lý kết quả nếu cần thiết
                    ResponseListOrder apiResponse = JsonConvert.DeserializeObject<ResponseListOrder>(responseBody);

                    if (apiResponse.message == "Success")
                    {
                        //MessageBox.Show("Thêm đơn hàng thành công!");
                        return apiResponse.data;
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

        public static async Task<bool> DeleteOrder(int OrderId, string accessToken)
        {
            try
            {
                // Tạo request mới
                var request = new RestRequest("/orders/delete/" + OrderId, Method.Post);
                // request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                // Gửi yêu cầu DELETE
                var response = await _client.ExecuteAsync(request);
                Response jsonResponse = JsonConvert.DeserializeObject<Response>(response.Content);
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

        public static async Task<List<Order>> SearchOrderByDate(DateTime dateStart, DateTime dateEnd, string accessToken)
        {
            try
            {
                RestRequest request = new RestRequest("/orders/search?startDate=" + dateStart.ToString("yyyy-MM-ddTHH:mm:ss") + "&endDate=" + dateEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                var response = await _client.ExecuteGetAsync(request);

                if (response.IsSuccessful)
                {
                    var jsonResponse = JsonConvert.DeserializeObject<ResponseListOrderSearchcs>(response.Content);

                    if (jsonResponse.status == "success")
                    {

                        return jsonResponse.data;
                    }
                    else
                    {
                        // Xử lý lỗi nếu có
                        Console.WriteLine($"Error: {jsonResponse.message}");
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
        }

        public static async Task<Order> GetOrderById(int orderId, string accessToken)
        {
            var request = new RestRequest("/orders/" + orderId);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await _client.ExecuteGetAsync(request);
            var order = JsonConvert.DeserializeObject<Response1Order>(response.Content);

            if (response.IsSuccessful)
            {
                return order.Data;
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
                return null;
            }
        }

        //public static async Task<Order> UpdateOrder(Order product, string accessToken)
        //{
        //    try
        //    {
        //        // Tạo đối tượng JSON theo thông tin của sản phẩm
        //        var jsonBody = new
        //        {
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
        //                Product newProduct = new Product
        //                {
        //                    Id = jsonResponse.Data.Id,
        //                    Name = jsonResponse.Data.Name,
        //                    Price = jsonResponse.Data.Price,
        //                    Category = jsonResponse.Data.Category,
        //                    Image = jsonResponse.Data.Image,
        //                    Amount = jsonResponse.Data.Amount,
        //                    Discount = jsonResponse.Data.Discount
        //                };
        //                //MessageBox.Show("Cập nhật sản phẩm thành công!");
        //                return newProduct;
        //            }
        //            else
        //            {
        //                //MessageBox.Show("Cập nhật sản phẩm thất bại!");
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            // Xử lý lỗi nếu cần thiết
        //            Console.WriteLine($"Error updating product: {response.ErrorMessage}");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error updating product: {ex.Message}");
        //        return null;
        //    }
        //}
    }
}
