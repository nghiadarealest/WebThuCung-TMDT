using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using _123.Helpers;
using _123.Models;
using MySql.Data.MySqlClient;

namespace _123.Services
{
    public static class ProductService
    {
        public static int CreateProduct(Product product)
        {
            string query = @"INSERT INTO Products (product_id, product_name, description, price, material_id, category_id, image_url, is_deleted)
                            VALUES (@product_id, @product_name, @description, @price, @material_id, @category_id, @image_url, 0)";
            Console.WriteLine(product.ProductId);
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@product_id", MySqlDbType.VarChar) { Value = product.ProductId },
                new MySqlParameter("@product_name", MySqlDbType.VarChar) { Value = product.ProductName },
                new MySqlParameter("@description", MySqlDbType.Text) { Value = product.Description ?? (object)DBNull.Value },
                new MySqlParameter("@price", MySqlDbType.Decimal) { Value = product.Price },
                new MySqlParameter("@material_id", MySqlDbType.Int32) { Value = product.MaterialId ?? (object)DBNull.Value },
                new MySqlParameter("@category_id", MySqlDbType.Int32) { Value = product.CategoryId ?? (object)DBNull.Value },
                new MySqlParameter("@image_url", MySqlDbType.VarChar) { Value = product.ImageUrl ?? (object)DBNull.Value }
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public static int CreateProducts(List<Product> products)
{
    // Xây dựng câu lệnh SQL với nhiều giá trị để chèn nhiều sản phẩm
    var query = @"INSERT INTO Products (product_id, product_name, description, price, material_id, category_id, image_url, is_deleted)
                  VALUES ";

    var parameters = new List<MySqlParameter>();
    var valueStrings = new List<string>();

    // Duyệt qua danh sách sản phẩm và tạo các phần values cho câu lệnh SQL
    for (int i = 0; i < products.Count; i++)
    {
        var product = products[i];

        valueStrings.Add(
            $"(@product_id{i}, @product_name{i}, @description{i}, @price{i}, @material_id{i}, @category_id{i}, @image_url{i}, 0)");

        parameters.Add(new MySqlParameter($"@product_id{i}", MySqlDbType.VarChar) { Value = product.ProductId });
        parameters.Add(new MySqlParameter($"@product_name{i}", MySqlDbType.VarChar) { Value = product.ProductName });
        parameters.Add(new MySqlParameter($"@description{i}", MySqlDbType.Text) { Value = product.Description ?? (object)DBNull.Value });
        parameters.Add(new MySqlParameter($"@price{i}", MySqlDbType.Decimal) { Value = product.Price });
        parameters.Add(new MySqlParameter($"@material_id{i}", MySqlDbType.Int32) { Value = product.MaterialId ?? (object)DBNull.Value });
        parameters.Add(new MySqlParameter($"@category_id{i}", MySqlDbType.Int32) { Value = product.CategoryId ?? (object)DBNull.Value });
        parameters.Add(new MySqlParameter($"@image_url{i}", MySqlDbType.VarChar) { Value = product.ImageUrl ?? (object)DBNull.Value });
    }

    // Kết hợp các phần values vào câu lệnh SQL
    query += string.Join(", ", valueStrings);

    // Thực thi câu lệnh SQL với các tham số
    return DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());
}


        public static List<Product> GetProducts()
        {
                    string query = @"
                        SELECT 
                            p.product_id, 
                            p.product_name, 
                            p.description, 
                            p.price, 
                            p.image_url, 
                            p.is_deleted, 
                            p.material_id, 
                            m.material_name, 
                            p.category_id, 
                            c.category_name
                        FROM Products p
                        LEFT JOIN Categories c ON p.category_id = c.category_id
                        LEFT JOIN Materials m ON p.material_id = m.material_id
                        WHERE p.is_deleted = 0";

            var products = new List<Product>();

            try
            {
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    products.Add(new Product
                    {
                        ProductId = row["product_id"].ToString(),
                        ProductName = row["product_name"].ToString(),
                        Description = row["description"]?.ToString(),
                        Price = Convert.ToDecimal(row["price"]),
                        MaterialId = row["material_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["material_id"]),
                        CategoryId = row["category_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["category_id"]),
                        ImageUrl = row["image_url"]?.ToString(),
                        IsDeleted = Convert.ToBoolean(row["is_deleted"]),
                        Material = row["material_name"] == DBNull.Value ? null : new Material
                        {
                            MaterialId = Convert.ToInt32(row["material_id"]),
                            MaterialName = row["material_name"].ToString()
                        },
                        Category = row["category_name"] == DBNull.Value ? null : new Category
                        {
                            CategoryId = Convert.ToInt32(row["category_id"]),
                            CategoryName = row["category_name"].ToString()
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
                throw;
            }

            return products;
        }

        public static Product GetProductById(string productId)
        {
            string query = "SELECT * FROM Products WHERE product_id = @product_id AND is_deleted = 0";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@product_id", MySqlDbType.VarChar) { Value = productId }
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                var row = result.Rows[0];
                return new Product
                {
                    ProductId = row["product_id"].ToString(),
                    ProductName = row["product_name"].ToString(),
                    Description = row["description"]?.ToString(),
                    Price = Convert.ToDecimal(row["price"]),
                    MaterialId = row["material_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["material_id"]),
                    CategoryId = row["category_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["category_id"]),
                    ImageUrl = row["image_url"]?.ToString(),
                    IsDeleted = Convert.ToBoolean(row["is_deleted"])
                };
            }

            return null;
        }

        public static int UpdateProduct(Product product)
        {
            string query = @"UPDATE Products
                            SET product_name = @product_name,
                                description = @description,
                                price = @price,
                                material_id = @material_id,
                                category_id = @category_id,
                                image_url = @image_url
                            WHERE product_id = @product_id AND is_deleted = 0";
            
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@product_id", MySqlDbType.VarChar) { Value = product.ProductId },
                new MySqlParameter("@product_name", MySqlDbType.VarChar) { Value = product.ProductName },
                new MySqlParameter("@description", MySqlDbType.Text) { Value = product.Description ?? (object)DBNull.Value },
                new MySqlParameter("@price", MySqlDbType.Decimal) { Value = product.Price },
                new MySqlParameter("@material_id", MySqlDbType.Int32) { Value = product.MaterialId ?? (object)DBNull.Value },
                new MySqlParameter("@category_id", MySqlDbType.Int32) { Value = product.CategoryId ?? (object)DBNull.Value },
                new MySqlParameter("@image_url", MySqlDbType.VarChar) { Value = product.ImageUrl ?? (object)DBNull.Value }
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteProduct(string productId)
        {
            string query = @"UPDATE Products
                            SET is_deleted = 1
                            WHERE product_id = @product_id AND is_deleted = 0";
            
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@product_id", MySqlDbType.VarChar) { Value = productId }
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
   

        public static List<dynamic> ViewGetProducts()
        {
            string query = @"
                        SELECT 
                            p.product_id, 
                            p.product_name, 
                            p.description, 
                            p.price, 
                            p.image_url, 
                            p.is_deleted, 
                            p.material_id, 
                            m.material_name, 
                            p.category_id, 
                            c.category_name,
                            pd.tuoi,
                            pd.tinh_trang,
                            pd.van_chuyen,
                            pd.dac_diem,
                            pd.gioi_tinh,
                            pd.tiem_phong,
                            pd.suc_khoe,
                            pd.nguon_goc,
                                                        i.quantity_in_stock
                        FROM Products p
                        LEFT JOIN Categories c ON p.category_id = c.category_id
                        LEFT JOIN Materials m ON p.material_id = m.material_id
                        LEFT JOIN ProductDetail pd ON pd.product_id = p.product_id
                                                LEFT JOIN Inventory i ON i.product_id = p.product_id
                        WHERE p.is_deleted = 0";

            var products = new List<dynamic>();

            try
            {
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    dynamic product = new ExpandoObject();
                    var productDict = (IDictionary<string, object>)product;

                    productDict["ProductId"] = row["product_id"].ToString();
                    productDict["ProductName"] = row["product_name"].ToString();
                    productDict["Description"] = row["description"]?.ToString();
                    productDict["Price"] = row["price"] == DBNull.Value ? null : Convert.ToDecimal(row["price"]);
                    productDict["MaterialId"] = row["material_id"] == DBNull.Value ? null : Convert.ToInt32(row["material_id"]);
                    productDict["CategoryId"] = row["category_id"] == DBNull.Value ? null : Convert.ToInt32(row["category_id"]);
                    productDict["ImageUrl"] = row["image_url"]?.ToString();
                    productDict["NiTay"] = row["tuoi"]?.ToString();
                    productDict["KieuDang"] = row["nguon_goc"]?.ToString();
                    productDict["KieuVienChu"] = row["suc_khoe"]?.ToString();
                    productDict["KichThuocVienChu"] = row["tiem_phong"]?.ToString();
                    productDict["GioiTinh"] = row["gioi_tinh"]?.ToString();
                    productDict["dac_diem"] = row["dac_diem"]?.ToString();
                    productDict["MauKimLoai"] = row["van_chuyen"]?.ToString();
                    productDict["DaTam"] = row["tinh_trang"]?.ToString();
                    productDict["QuantityInStock"] = row["quantity_in_stock"]?.ToString();
                    productDict["IsDeleted"] = row["is_deleted"] == DBNull.Value ? false : Convert.ToBoolean(row["is_deleted"]);
                    productDict["Material"] = new
                    {
                        MaterialId = Convert.ToInt32(row["material_id"]),
                        MaterialName = row["material_name"].ToString()
                    };
                    productDict["Category"] = new
                    {
                        CategoryId = Convert.ToInt32(row["category_id"]),
                        CategoryName = row["category_name"].ToString()
                    };

                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
                throw;
            }

            return products;
        }
   

        public static dynamic ViewGetProductById(string productId)
        {
            string query = @"
                        SELECT 
                            p.product_id, 
                            p.product_name, 
                            p.description, 
                            p.price, 
                            p.image_url, 
                            p.is_deleted, 
                            p.material_id, 
                            m.material_name, 
                            p.category_id, 
                            c.category_name,
                            pd.tuoi,
                            pd.tinh_trang,
                            pd.van_chuyen,
                            pd.dac_diem,
                            pd.gioi_tinh,
                            pd.tiem_phong,
                            pd.suc_khoe,
                            pd.nguon_goc,
                            i.quantity_in_stock,
                            dis.discount_percent
                        FROM Products p
                        LEFT JOIN Categories c ON p.category_id = c.category_id
                        LEFT JOIN Materials m ON p.material_id = m.material_id
                        LEFT JOIN ProductDetail pd ON pd.product_id = p.product_id
                        LEFT JOIN Inventory i ON i.product_id = p.product_id
                        LEFT JOIN Product_Discount pdis ON pdis.product_id = p.product_id
                        LEFT JOIN Discounts  dis ON dis.discount_id = pdis.discount_id 
                        WHERE p.is_deleted = 0 AND p.product_id = @ProductId";

            try
            {
                // Tạo tham số cho truy vấn
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@ProductId", MySqlDbType.VarChar) { Value = productId }
                };

                // Thực hiện truy vấn
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                // Kiểm tra nếu có dữ liệu trả về
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];

                    // Tạo đối tượng dynamic (ExpandoObject)
                    dynamic product = new ExpandoObject();
                    var productDict = (IDictionary<string, object>)product;

                    // Thêm các thuộc tính vào ExpandoObject
                    productDict["ProductId"] = row["product_id"].ToString();
                    productDict["ProductName"] = row["product_name"].ToString();
                    productDict["Description"] = row["description"]?.ToString();
                    productDict["Price"] = row["price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row["price"]);
                    productDict["MaterialId"] = row["material_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["material_id"]);
                    productDict["CategoryId"] = row["category_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["category_id"]);
                    productDict["ImageUrl"] = row["image_url"]?.ToString();
                    productDict["NiTay"] = row["tuoi"]?.ToString();
                    productDict["KieuDang"] = row["nguon_goc"]?.ToString();
                    productDict["KieuVienChu"] = row["suc_khoe"]?.ToString();
                    productDict["KichThuocVienChu"] = row["tiem_phong"]?.ToString();
                    productDict["GioiTinh"] = row["gioi_tinh"]?.ToString();
                    productDict["dac_diem"] = row["dac_diem"]?.ToString();
                    productDict["MauKimLoai"] = row["van_chuyen"]?.ToString();
                    productDict["DaTam"] = row["tinh_trang"]?.ToString();
                    productDict["QuantityInStock"] = row["quantity_in_stock"]?.ToString();
                    productDict["DiscountPercent"] = row["discount_percent"]?.ToString();
                    productDict["IsDeleted"] = row["is_deleted"] == DBNull.Value ? false : Convert.ToBoolean(row["is_deleted"]);

                    // Thêm Material và Category vào dưới dạng đối tượng
                    productDict["Material"] = new
                    {
                        MaterialId = row["material_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["material_id"]),
                        MaterialName = row["material_name"]?.ToString()
                    };

                    productDict["Category"] = new
                    {
                        CategoryId = row["category_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["category_id"]),
                        CategoryName = row["category_name"]?.ToString()
                    };

                    return product;
                }
                else
                {
                    Console.WriteLine("No product found with the specified product_id.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product by ID: {ex.Message}");
                throw;
            }
        }


        public static List<dynamic> ViewGetProductsByCateId(int categoryId)
{
    string query = @"
                SELECT 
                    p.product_id, 
                    p.product_name, 
                    p.description, 
                    p.price, 
                    p.image_url, 
                    p.is_deleted, 
                    p.material_id, 
                    m.material_name, 
                    p.category_id, 
                    c.category_name,
                    pd.tuoi,
                    pd.tinh_trang,
                    pd.van_chuyen,
                    pd.dac_diem,
                    pd.gioi_tinh,
                    pd.tiem_phong,
                    pd.suc_khoe,
                    pd.nguon_goc,
                            i.quantity_in_stock,
                            dis.discount_percent
                FROM Products p
                LEFT JOIN Categories c ON p.category_id = c.category_id
                LEFT JOIN Materials m ON p.material_id = m.material_id
                LEFT JOIN ProductDetail pd ON pd.product_id = p.product_id
                LEFT JOIN Inventory i ON i.product_id = p.product_id
                LEFT JOIN Product_Discount pdis ON pdis.product_id = p.product_id
                LEFT JOIN Discounts  dis ON dis.discount_id = pdis.discount_id 
                WHERE p.is_deleted = 0 AND p.category_id = @CategoryId";  // Thêm điều kiện lọc theo category_id

    var products = new List<dynamic>();

    try
    {
        // Tạo tham số cho truy vấn
        var parameters = new MySqlParameter[]
        {
            new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = categoryId }
        };

        // Thực hiện truy vấn
        DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

        // Duyệt qua các dòng dữ liệu trả về và chuyển thành đối tượng dynamic
        foreach (DataRow row in dataTable.Rows)
        {
            dynamic product = new ExpandoObject();
            var productDict = (IDictionary<string, object>)product;

            productDict["ProductId"] = row["product_id"].ToString();
            productDict["ProductName"] = row["product_name"].ToString();
            productDict["Description"] = row["description"]?.ToString();
            productDict["Price"] = row["price"] == DBNull.Value ? null : Convert.ToDecimal(row["price"]);
            productDict["MaterialId"] = row["material_id"] == DBNull.Value ? null : Convert.ToInt32(row["material_id"]);
            productDict["CategoryId"] = row["category_id"] == DBNull.Value ? null : Convert.ToInt32(row["category_id"]);
            productDict["ImageUrl"] = row["image_url"]?.ToString();
            productDict["NiTay"] = row["tuoi"]?.ToString();
            productDict["KieuDang"] = row["nguon_goc"]?.ToString();
            productDict["KieuVienChu"] = row["suc_khoe"]?.ToString();
            productDict["KichThuocVienChu"] = row["tiem_phong"]?.ToString();
            productDict["GioiTinh"] = row["gioi_tinh"]?.ToString();
            productDict["dac_diem"] = row["dac_diem"]?.ToString();
            productDict["MauKimLoai"] = row["van_chuyen"]?.ToString();
            productDict["DaTam"] = row["tinh_trang"]?.ToString();
                    productDict["QuantityInStock"] = row["quantity_in_stock"]?.ToString();
                    productDict["DiscountPercent"] = row["discount_percent"]?.ToString();
            productDict["IsDeleted"] = row["is_deleted"] == DBNull.Value ? false : Convert.ToBoolean(row["is_deleted"]);
            productDict["Material"] = new
            {
                MaterialId = Convert.ToInt32(row["material_id"]),
                MaterialName = row["material_name"].ToString()
            };
            productDict["Category"] = new
            {
                CategoryId = Convert.ToInt32(row["category_id"]),
                CategoryName = row["category_name"].ToString()
            };

            products.Add(product);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving products: {ex.Message}");
        throw;
    }

    return products;
}





    }
}
