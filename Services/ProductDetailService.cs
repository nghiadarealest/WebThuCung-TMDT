using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using _123.Models;
using _123.Helpers;
namespace _123.Services
{
    public static class ProductDetailService
    {
        // Thêm thông tin chi tiết sản phẩm mới
        public static int CreateProductDetail(ProductDetail productDetail)
        {
            string query = @"INSERT INTO ProductDetail (product_id, tuoi, nguon_goc, suc_khoe, tiem_phong, gioi_tinh, dac_diem, van_chuyen, tinh_trang, is_deleted)
                             VALUES (@product_id, @tuoi, @nguon_goc, @suc_khoe, @tiem_phong, @gioi_tinh, @dac_diem, @van_chuyen, @tinh_trang, 0)";
            
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@product_id", MySqlDbType.VarChar) { Value = productDetail.ProductId },
                new MySqlParameter("@tuoi", MySqlDbType.VarChar) { Value = productDetail.NiTay },
                new MySqlParameter("@nguon_goc", MySqlDbType.VarChar) { Value = productDetail.KieuDang },
                new MySqlParameter("@suc_khoe", MySqlDbType.VarChar) { Value = productDetail.KieuVienChu },
                new MySqlParameter("@tiem_phong", MySqlDbType.VarChar) { Value = productDetail.KichThuocVienChu },
                new MySqlParameter("@gioi_tinh", MySqlDbType.VarChar) { Value = productDetail.GioiTinh },
                new MySqlParameter("@dac_diem", MySqlDbType.VarChar) { Value = productDetail.dac_diem },
                new MySqlParameter("@van_chuyen", MySqlDbType.VarChar) { Value = productDetail.MauKimLoai },
                new MySqlParameter("@tinh_trang", MySqlDbType.VarChar) { Value = productDetail.DaTam }
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
public static int CreateProductDetails(List<ProductDetail> productDetails)
{
    // Xây dựng câu lệnh SQL với nhiều giá trị để chèn nhiều chi tiết sản phẩm
    var query = @"INSERT INTO ProductDetail (product_id, tuoi, nguon_goc, suc_khoe, tiem_phong, gioi_tinh, dac_diem, van_chuyen, tinh_trang, is_deleted)
                  VALUES ";

    var parameters = new List<MySqlParameter>();
    var valueStrings = new List<string>();

    // Duyệt qua danh sách chi tiết sản phẩm và tạo các phần values cho câu lệnh SQL
    for (int i = 0; i < productDetails.Count; i++)
    {
        var productDetail = productDetails[i];

        valueStrings.Add(
            $"(@product_id{i}, @tuoi{i}, @nguon_goc{i}, @suc_khoe{i}, @tiem_phong{i}, @gioi_tinh{i}, @dac_diem{i}, @van_chuyen{i}, @tinh_trang{i}, 0)");

        parameters.Add(new MySqlParameter($"@product_id{i}", MySqlDbType.VarChar) { Value = productDetail.ProductId });
        parameters.Add(new MySqlParameter($"@tuoi{i}", MySqlDbType.VarChar) { Value = productDetail.NiTay });
        parameters.Add(new MySqlParameter($"@nguon_goc{i}", MySqlDbType.VarChar) { Value = productDetail.KieuDang });
        parameters.Add(new MySqlParameter($"@suc_khoe{i}", MySqlDbType.VarChar) { Value = productDetail.KieuVienChu });
        parameters.Add(new MySqlParameter($"@tiem_phong{i}", MySqlDbType.VarChar) { Value = productDetail.KichThuocVienChu });
        parameters.Add(new MySqlParameter($"@gioi_tinh{i}", MySqlDbType.VarChar) { Value = productDetail.GioiTinh });
        parameters.Add(new MySqlParameter($"@dac_diem{i}", MySqlDbType.VarChar) { Value = productDetail.dac_diem });
        parameters.Add(new MySqlParameter($"@van_chuyen{i}", MySqlDbType.VarChar) { Value = productDetail.MauKimLoai });
        parameters.Add(new MySqlParameter($"@tinh_trang{i}", MySqlDbType.VarChar) { Value = productDetail.DaTam });
    }

    // Kết hợp các phần values vào câu lệnh SQL
    query += string.Join(", ", valueStrings);

    // Thực thi câu lệnh SQL với các tham số
    return DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());
}

        // Lấy danh sách các chi tiết sản phẩm
        public static List<ProductDetail> GetProductDetails()
        {
            string query = @"SELECT pd.product_detail_id, pd.product_id, pd.tuoi, pd.nguon_goc, pd.suc_khoe, pd.tiem_phong, pd.gioi_tinh, 
                                    pd.dac_diem, pd.van_chuyen, pd.tinh_trang, pd.is_deleted, p.product_name
                             FROM ProductDetail pd
                             LEFT JOIN Products p ON p.product_id = pd.product_id
                             WHERE pd.is_deleted = 0";
            
            var productDetails = new List<ProductDetail>();

            try
            {
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    productDetails.Add(new ProductDetail
                    {
                        ProductDetailId = Convert.ToInt32(row["product_detail_id"]),
                        ProductId = row["product_id"].ToString(),
                        NiTay = row["tuoi"].ToString(),
                        KieuDang = row["nguon_goc"].ToString(),
                        KieuVienChu = row["suc_khoe"].ToString(),
                        KichThuocVienChu = row["tiem_phong"].ToString(),
                        GioiTinh = row["gioi_tinh"].ToString(),
                        dac_diem = row["dac_diem"].ToString(),
                        MauKimLoai = row["van_chuyen"].ToString(),
                        DaTam = row["tinh_trang"].ToString(),
                        IsDeleted = Convert.ToBoolean(row["is_deleted"]),
                        Product = row["product_name"] == DBNull.Value ? null : new Product
                        {
                            ProductId =  row["product_id"].ToString(),
                            ProductName = row["product_name"].ToString()
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách chi tiết sản phẩm: {ex.Message}");
                throw;
            }

            return productDetails;
        }

        // Lấy chi tiết sản phẩm theo ID
        public static ProductDetail GetProductDetailById(int productDetailId)
        {
            string query = @"SELECT product_detail_id, product_id, tuoi, nguon_goc, suc_khoe, tiem_phong, gioi_tinh, 
                                     dac_diem, van_chuyen, tinh_trang, is_deleted
                             FROM ProductDetail 
                             WHERE product_detail_id = @productDetailId AND is_deleted = 0";
            
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@productDetailId", MySqlDbType.Int32) { Value = productDetailId }
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                var row = result.Rows[0];
                return new ProductDetail
                {
                    ProductDetailId = Convert.ToInt32(row["product_detail_id"]),
                    ProductId = row["product_id"].ToString(),
                    NiTay = row["tuoi"].ToString(),
                    KieuDang = row["nguon_goc"].ToString(),
                    KieuVienChu = row["suc_khoe"].ToString(),
                    KichThuocVienChu = row["tiem_phong"].ToString(),
                    GioiTinh = row["gioi_tinh"].ToString(),
                    dac_diem = row["dac_diem"].ToString(),
                    MauKimLoai = row["van_chuyen"].ToString(),
                    DaTam = row["tinh_trang"].ToString(),
                    IsDeleted = Convert.ToBoolean(row["is_deleted"])
                };
            }

            return null;
        }

        // Cập nhật chi tiết sản phẩm
        public static int UpdateProductDetail(ProductDetail productDetail)
        {
            string query = @"UPDATE ProductDetail
                             SET tuoi = @tuoi,
                                 nguon_goc = @nguon_goc,
                                 suc_khoe = @suc_khoe,
                                 tiem_phong = @tiem_phong,
                                 gioi_tinh = @gioi_tinh,
                                 dac_diem = @dac_diem,
                                 van_chuyen = @van_chuyen,
                                 tinh_trang = @tinh_trang
                             WHERE product_detail_id = @product_detail_id AND is_deleted = 0";
    
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@product_detail_id", MySqlDbType.Int32) { Value = productDetail.ProductDetailId },
                new MySqlParameter("@tuoi", MySqlDbType.VarChar) { Value = productDetail.NiTay },
                new MySqlParameter("@nguon_goc", MySqlDbType.VarChar) { Value = productDetail.KieuDang },
                new MySqlParameter("@suc_khoe", MySqlDbType.VarChar) { Value = productDetail.KieuVienChu },
                new MySqlParameter("@tiem_phong", MySqlDbType.VarChar) { Value = productDetail.KichThuocVienChu },
                new MySqlParameter("@gioi_tinh", MySqlDbType.VarChar) { Value = productDetail.GioiTinh },
                new MySqlParameter("@dac_diem", MySqlDbType.VarChar) { Value = productDetail.dac_diem },
                new MySqlParameter("@van_chuyen", MySqlDbType.VarChar) { Value = productDetail.MauKimLoai },
                new MySqlParameter("@tinh_trang", MySqlDbType.VarChar) { Value = productDetail.DaTam }
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Xóa tạm thời chi tiết sản phẩm
        public static int DeleteProductDetail(int productDetailId)
        {
            string query = @"UPDATE ProductDetail
                             SET is_deleted = 1
                             WHERE product_detail_id = @product_detail_id AND is_deleted = 0";
    
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@product_detail_id", MySqlDbType.Int32) { Value = productDetailId }
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}
