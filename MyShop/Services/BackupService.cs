using ClosedXML.Excel;
using MyShop.Configs;
using MyShop.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop.Services
{
    public class BackupService
    {
        private static RestClient _client = RestSharpClient.getInstance();

        private static List<Product> ReadProductsFromExcel(string filePath)
        {
            var products = new List<Product>();
            try
            {
                // Mở file Excel
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheets.Worksheet(4);

                    // Lặp qua các dòng trong file Excel (bắt đầu từ dòng thứ 2 để tránh đọc tiêu đề)
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        // Đọc dữ liệu từ từng ô và tạo đối tượng Product
                        ProductType category = new ProductType
                        {
                            Id = int.Parse(row.Cell(5).Value.ToString()),
                        };
                        var product = new Product();
                        int id;
                        if (int.TryParse(row.Cell(1).Value.ToString(), out id))
                        {
                            product.Id = id;
                        }

                        product.Name = row.Cell(2).Value.ToString();
                        product.Image = row.Cell(3).Value.ToString();

                        double price;
                        if (double.TryParse(row.Cell(4).Value.ToString(), out price))
                        {
                            product.Price = price;
                        }

                        product.Category = category;

                        int amount;
                        if (int.TryParse(row.Cell(6).Value.ToString(), out amount))
                        {
                            product.Amount = amount;
                        }

                        double discount;
                        if (double.TryParse(row.Cell(7).Value.ToString(), out discount))
                        {
                            product.Discount = discount;
                        }

                        double profit;
                        if (double.TryParse(row.Cell(8).Value.ToString(), out profit))
                        {
                            product.Profit = profit;
                        }

                        products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading products from Excel: {ex.Message}");
                Console.WriteLine($"Error reading products from Excel: {ex.Message}");
                return null;
            }

            return products;
        }

        public static async Task<bool> ImportFile(string filePath)
        {
            try
            {
                var request = new RestRequest("/backup/import", Method.Post);
                request.AddFile("file", filePath);
                var response = await _client.ExecuteAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing products: {ex.Message}");
                return false;
            }
        }
        public static async Task<bool> ExportFile(string accessToken)
        {
            try
            {
                var request = new RestRequest("/backup/export", Method.Get);
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                // Assuming you are expecting a file content in the response, use 'await' to get the content
                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    // Assuming you want to save the response content to a file
                    var resultFilePath = "D:\\Nam_4\\uuuuu.xlsx";
                    File.WriteAllBytes(resultFilePath, response.RawBytes);

                    // Optionally, you can return the file path or the actual File object depending on your needs
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error exporting file. Status code: {response.StatusCode}, Content: {response.Content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting file: {ex.Message}");
                return false;
            }
        }
        //public static async Task<bool> ExportFile(string accessToken)
        //{
        //    try
        //    {
        //        var request = new RestRequest("/backup/export", Method.Post);
        //        request.AddHeader("Authorization", $"Bearer {accessToken}");
        //        var response = await _client.ExecuteAsync(request);
        //        if (response.IsSuccessful)
        //        {
        //            File Result = response.Content;
        //        }    
        //            return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error importing products: {ex.Message}");
        //        return false;
        //    }
        //}

        //public static async Task<List<Product>> ImportProductsFromExcel(string filePath, string accessToken)
        //{
        //    try
        //    {
        //        // Đọc dữ liệu từ file Excel
        //        var products = ReadProductsFromExcel(filePath);
        //        if (products == null)
        //        {

        //            return null;
        //        }
        //        // Gọi API để thêm sản phẩm từ danh sách đọc được
        //        foreach (var product in products)
        //        {
        //            await ProductService.AddProductNoImageFile(product, accessToken);

        //        }
        //        return products;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error importing products: {ex.Message}");
        //        return null;
        //    }
        //}

        //private static List<ProductType> ReadProductTypeFromExcel(string filePath)
        //{
        //    var productTypeList = new List<ProductType>();
        //    try
        //    {
        //        // Mở file Excel
        //        using (var workbook = new XLWorkbook(filePath))
        //        {
        //            var worksheet = workbook.Worksheets.Worksheet(1);

        //            // Lặp qua các dòng trong file Excel (bắt đầu từ dòng thứ 2 để tránh đọc tiêu đề)
        //            foreach (var row in worksheet.RowsUsed().Skip(1))
        //            {
        //                // Đọc dữ liệu từ từng ô và tạo đối tượng Product
        //                ProductType category = new ProductType();

        //                int id;
        //                if (int.TryParse(row.Cell(1).Value.ToString(), out id))
        //                {
        //                    category.Id = id;
        //                }

        //                category.Name = row.Cell(2).Value.ToString();

        //                productTypeList.Add(category);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error reading products from Excel: {ex.Message}");
        //    }

        //    return productTypeList;
        //}

        //public static async Task<List<ProductType>> ImportProductTypeFromExcel(string filePath, string accessToken)
        //{
        //    try
        //    {
        //        // Đọc dữ liệu từ file Excel
        //        var productType = ReadProductTypeFromExcel(filePath);

        //        // Gọi API để thêm loại sản phẩm từ danh sách đọc được
        //        foreach (var type in productType)
        //        {
        //            await ProductTypeService.AddProductType(type, accessToken);

        //        }
        //        return productType;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error importing products: {ex.Message}");
        //        return null;
        //    }
        //}
    }
}
