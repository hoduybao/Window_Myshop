using MyShop.Configs;
using MyShop.Helpers;
using MyShop.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService
    {
        private static RestClient _client = RestSharpClient.getInstance();

        //private async void UpdateProductQuantities(List<ProductOfOrder> products, string accessToken)
        //{
        //    foreach (var productOfOrder in products)
        //    {
        //        var product = productOfOrder.Product;
        //        var updatedProduct = await ProductService.UpdateProduct(product, accessToken);

        //        if (updatedProduct != null)
        //        {
        //            // Cập nhật sản phẩm trong danh sách sản phẩm
        //            UpdateProductInList(products, updatedProduct);
        //        }
        //    }
        //}

        //private void UpdateProductInList(List<ProductOfOrder> products, Product updatedProduct)
        //{
        //    var productOfOrder = products.FirstOrDefault(p => p.Id == updatedProduct.Id);

        //    if (productOfOrder != null)
        //    {
        //        // Cập nhật số lượng sản phẩm trong danh sách đơn hàng
        //        productOfOrder.Amount = updatedProduct.Amount;
        //    }
        //}

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
                var request = new RestRequest("/orders/add?name=" + client.CustomerName + "&email=" + client.Email + "&address=" + client.Address);
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
    }
}
