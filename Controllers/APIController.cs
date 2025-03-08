using _123.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using _123.Services;
using _123.Helpers;
using OfficeOpenXml;

namespace _123.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;
        private readonly ZaloPayService _zaloPayService;

        public ApiController(ILogger<ApiController> logger,ZaloPayService zaloPayService)
        {
            _logger = logger;
             _zaloPayService = zaloPayService;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class OrderDto
        {
            public Order order { get; set; }
            public List<OrderItem> orderItems { get; set; }
            public int userId  { get; set; }
        }

        public class Dashboard
        {
            public int Role1Count { get; set; }
            public int Role3Count { get; set; }
            public decimal TotalPrice { get; set; }

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest) {
            var res = AuthService.Login(loginRequest.Username, loginRequest.Password);

            if(res == null) {
                return NotFound(new ApiResponse<dynamic>(404, "Not Found", res));
            }  

            var response = new ApiResponse<dynamic>(200, "Success", res);
            return Ok(response);
            
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var result = UserService.GetUserRoleCounts();
             var dashboard = new Dashboard
            {
                Role1Count = result.role1Count,
                Role3Count = result.role3Count,
                TotalPrice = result.totalPrice
            };

            return Ok(dashboard);
        }


        [HttpGet("products/byId")]
        public IActionResult GetProductById([FromQuery] string id)
        {
            // Tạo một danh sách sản phẩm mẫu
            var products = ProductService.ViewGetProductById(id);

            // Kiểm tra nếu danh sách không rỗng
            if (products != null )
            {
              var response = new ApiResponse<dynamic>(200, "Success", products);
              return Ok(response);
            }
            else
            {
                // Nếu không có sản phẩm, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy sản phẩm nào."));
            }
        }

        [HttpGet("products/byCate")]
        public IActionResult GetProductByCate([FromQuery] int id)
        {
            // Tạo một danh sách sản phẩm mẫu
            var products = ProductService.ViewGetProductsByCateId(id);

            // Kiểm tra nếu danh sách không rỗng
            if (products != null )
            {
              var response = new ApiResponse<dynamic>(200, "Success", products);
              return Ok(response);
            }
            else
            {
                // Nếu không có sản phẩm, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy sản phẩm nào."));
            }
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            // Tạo một danh sách sản phẩm mẫu
            var products = ProductService.ViewGetProducts();

            // Kiểm tra nếu danh sách không rỗng
            if (products != null && products.Any())
            {
              var response = new ApiResponse<dynamic>(200, "Success", products);
              return Ok(response);
            }
            else
            {
                // Nếu không có sản phẩm, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy sản phẩm nào."));
            }
        }

        [HttpGet("categories")]
        public IActionResult GetCategorys()
        {
            // Tạo một danh sách sản phẩm mẫu
            var cates = CategoryService.ViewGetCategories();

            // Kiểm tra nếu danh sách không rỗng
            if (cates != null )
            {
              var response = new ApiResponse<dynamic>(200, "Success", cates);
              return Ok(response);
            }
            else
            {
                // Nếu không có sản phẩm, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy danh mục."));
            }
        }

        [HttpGet("carts")]
        public IActionResult GetCarts([FromQuery] int id)
        {
            // Tạo một danh sách sản phẩm mẫu
            var carts = ShoppingCartService.GetCartItemsByUserId(id);

            // Kiểm tra nếu danh sách không rỗng
            if (carts != null )
            {
              var response = new ApiResponse<dynamic>(200, "Success", carts);
              return Ok(response);
            }
            else
            {
                // Nếu không có sản phẩm, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy rỏ hàng."));
            }
        }

        [HttpPost("cart")]
        public IActionResult AddorUpdateCart([FromBody] ShoppingCart cart)
        {
            // Tạo một danh sách sản phẩm mẫu
            var carts = ShoppingCartService.AddOrUpdateToCart(cart);

            // Kiểm tra nếu danh sách không rỗng
            if (carts != null )
            {
              var response = new ApiResponse<dynamic>(200, "Success", carts);
              return Ok(response);
            }
            else
            {
                // Nếu không có sản phẩm, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy rỏ hàng."));
            }
        }

        [HttpPost("carts")]
        public IActionResult AddorUpdateCarts([FromBody] ShoppingCart[] carts)
        {
            Console.WriteLine(carts);
            // Tạo một danh sách sản phẩm mẫu
            var mcarts = ShoppingCartService.AddOrUpdateToManyCart(carts);

            // Kiểm tra nếu danh sách không rỗng
            if (mcarts != null )
            {
              var response = new ApiResponse<dynamic>(200, "Success", mcarts);
              return Ok(response);
            }
            else
            {
                // Nếu không có sản phẩm, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy rỏ hàng."));
            }
        }

        [HttpDelete("cart")]
        public IActionResult RemoveFromCart([FromQuery] int cartId)
        {
            var result = ShoppingCartService.DeleteCartItem(cartId);
            if (result >0)
            {
                return Ok(new ApiResponse<dynamic>(200, "Sản phẩm đã được xóa khỏi giỏ hàng"));
            }
            else
            {
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy sản phẩm trong giỏ hàng"));
            }
        }

        [HttpPost("orders")]
        public IActionResult CreateOrder([FromBody] OrderDto orders)
        {
            // Tạo đơn hàng và lấy ID đơn hàng vừa tạo
            var result = OrderService.CreateOrder(orders.order);
            
            if (result > 0)
            {
                // Tạo các mục đơn hàng
                var orderItems = OrderItemService.CreateOrderItems(
                    orders.orderItems.Select(item => new OrderItem 
                    { 
                        OrderId = result, 
                        ProductName = item.ProductName, 
                        Quantity = item.Quantity, 
                        Price = item.Price
                    }).ToList());
                
                // Cập nhật giỏ hàng
                var shop = ShoppingCartService.UpdateCartToDeleteByUserId(orders.userId);
                
                // Trả lại response với order_id
                return Ok(new ApiResponse<dynamic>(200, "Tạo đơn hàng thành công", new { order_id = result }));
            }
            else
            {
                // Trả lại lỗi nếu tạo đơn hàng thất bại
                return NotFound(new ApiResponse<dynamic>(404, "Tạo đơn hàng thất bại"));
            }
        }

        [HttpGet("orders")]
        public IActionResult GetOrdersByUserId([FromQuery] int userId)
        {
            // Lấy danh sách đơn hàng theo userId
            var orders = OrderService.GetOrdersByUserId(userId);

            // Kiểm tra nếu danh sách không rỗng
            if (orders != null && orders.Count > 0)
            {
                var response = new ApiResponse<dynamic>(200, "Success", orders);
                return Ok(response);
            }
            else
            {
                // Nếu không có đơn hàng, trả về thông báo lỗi
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy đơn hàng."));
            }
        }
        [HttpPost("orders/cancel")]
        public IActionResult CancelOrder([FromQuery] int orderId)
        {
            // Gọi service để hủy đơn hàng
            var result = OrderService.CancelOrder(orderId);

            if (result > 0)
            {
                return Ok(new ApiResponse<dynamic>(200, "Đơn hàng đã được hủy thành công."));
            }
            else
            {
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy đơn hàng hoặc hủy thất bại."));
            }
        }
        [HttpPost("orders/update-status")]
        public IActionResult UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            if (request == null || request.OrderId <= 0)
            {
                return BadRequest(new ApiResponse<dynamic>(400, "Dữ liệu không hợp lệ."));
            }

            // Cập nhật trạng thái đơn hàng
            int result = OrderService.UpdateOrderStatus(request.OrderId, request.IsSuccess);

            if (result > 0)
            {
                return Ok(new ApiResponse<dynamic>(200, "Cập nhật trạng thái đơn hàng thành công."));
            }
            else
            {
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy đơn hàng hoặc cập nhật thất bại."));
            }
        }

        public class UpdateOrderStatusRequest
        {
            public int OrderId { get; set; }
            public bool IsSuccess { get; set; }
        }
        public class PaymentRequest
        {
            public decimal Amount { get; set; }
            public string Description { get; set; }
        }
        [HttpPost("zalo1")]
        public async Task<IActionResult> CreatePaymentZalo([FromBody] PaymentRequest request)
        {
            try
            {
                // Validate request
                // if (request == null || string.IsNullOrWhiteSpace(request.OrderId) || 
                //     request.Amount <= 0 || string.IsNullOrWhiteSpace(request.UserPhone))
                // {
                //     return BadRequest(new { message = "Dữ liệu không hợp lệ" });
                // }

                // Gọi dịch vụ thanh toán
                var result = await ZaloPayService.CreatePaymentRequestAsync(request.Amount, request.Description);

                // Trả về kết quả dưới dạng HTTP Response
                return Ok(new { message = "Thanh toán thành công", data = result });
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về mã lỗi HTTP với thông điệp lỗi
                return BadRequest(new { message = "Lỗi: " + ex.Message });
            }
        }
         [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            // Gọi dịch vụ để lấy thông tin người dùng theo ID
            var user = UserService.GetUserById(id);

            // Kiểm tra nếu người dùng tồn tại
            if (user != null)
            {
                var response = new ApiResponse<dynamic>(200, "Success", user);
                return Ok(response);
            }
            else
            {
                return NotFound(new ApiResponse<dynamic>(404, "Không tìm thấy người dùng."));
            }
        }
        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.user_id)
            {
                return BadRequest();
            }
            
            var result = UserService.UpdateUser(user);
            if (result > 0)
            {
                return NoContent(); // Cập nhật thành công
            }

            return NotFound(); // Không tìm thấy người dùng
        }

        [HttpPost("change-password/{id}")]
        public IActionResult ChangePassword(int id, [FromBody] string newPassword)
        {
            var result = UserService.ChangePassword(id, newPassword);
            if (result > 0)
            {
                return NoContent(); // Đổi mật khẩu thành công
            }

            return NotFound(); // Không tìm thấy người dùng
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = UserService.DeleteUser(id);
            if (result > 0)
            {
                return NoContent();
                 // Xóa thành công
            }

            return NotFound(); // Không tìm thấy người dùng
        }

        public List<Dictionary<string, object>> ReadExcelData(IFormFile file)
        {
            var data = new List<Dictionary<string, object>>();

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ hoặc trống.");
            }

            try
            {
                // Đọc file Excel vào bộ nhớ
                using var stream = new MemoryStream();
                file.CopyTo(stream);
                stream.Position = 0;

                // Sử dụng EPPlus để đọc dữ liệu từ file Excel
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0]; // Đọc sheet đầu tiên

                // Lấy số dòng và số cột trong sheet
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Đọc tiêu đề (header)
                var headers = new List<string>();
                for (int col = 1; col <= colCount; col++)
                {
                    headers.Add(worksheet.Cells[1, col].Text);
                }

                // Đọc dữ liệu từ dòng 2 trở đi
                for (int row = 2; row <= rowCount; row++)
                {
                    var rowData = new Dictionary<string, object>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        rowData[headers[col - 1]] = worksheet.Cells[row, col].Text;
                    }
                    data.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Đã xảy ra lỗi khi đọc file: {ex.Message}");
            }

            return data;
        }
        

        [HttpPost("upload-excel-category")]
        public async Task<IActionResult> UploadExcelCategory([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
    {
        return BadRequest("File không hợp lệ hoặc trống.");
    }

            try
            {
                // Gọi hàm ReadExcelData để đọc dữ liệu từ file
                var data = ReadExcelData(file);

                var categories = data.Select(d => new Category
                {
                    CategoryName = d.ContainsKey("category_name") ? d["category_name"]?.ToString() : null,
                    Description = d.ContainsKey("description") ? d["description"]?.ToString() : null,
                }).ToList();

                var result = CategoryService.CreateCategories(categories);
                if (result > 0)
                {
                    return Ok(new ApiResponse<dynamic>(200, "Tao thanh cong"));
                }
                else
                {
                    return NotFound(new ApiResponse<dynamic>(404, "Tao that bai"));
                } 
                    }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }


        [HttpPost("upload-excel-product")]
        public async Task<IActionResult> UploadExcelProduct([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ hoặc trống.");
            }

            try
            {
                // Gọi hàm ReadExcelData để đọc dữ liệu từ file
                var data = ReadExcelData(file);

                var products = data.Select(d => new Product
                {
                    ProductId = d.ContainsKey("product_id") ? d["product_id"]?.ToString() : null,
                    ProductName = d.ContainsKey("product_name") ? d["product_name"]?.ToString() : null,
                    Description = d.ContainsKey("description") ? d["description"]?.ToString() : null,
                    Price = d.ContainsKey("price") ? Convert.ToDecimal(d["price"]) : 0m,
                    MaterialId = d.ContainsKey("material_id") ? (int?)Convert.ToInt32(d["material_id"]) : null,
                    CategoryId = d.ContainsKey("category_id") ? (int?)Convert.ToInt32(d["category_id"]) : null,
                    ImageUrl = d.ContainsKey("image_url") ? d["image_url"]?.ToString() : null
                }).ToList();
                var result = ProductService.CreateProducts(products);
                if (result > 0)
                {
                    return Ok(new ApiResponse<dynamic>(200, "Tao thanh cong"));
                }
                else
                {
                    return NotFound(new ApiResponse<dynamic>(404, "Tao that bai"));
                } 
                    }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        [HttpPost("upload-excel-productdetail")]
        public async Task<IActionResult> UploadExcelProductDetail([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ hoặc trống.");
            }

            try
            {
                // Gọi hàm ReadExcelData để đọc dữ liệu từ file
                var data = ReadExcelData(file);

                var productDetails = data.Select(d => new ProductDetail
                {
                    ProductId = d.ContainsKey("product_id") ? d["product_id"]?.ToString() : null,
                    NiTay = d.ContainsKey("tuoi") ? d["tuoi"]?.ToString() : null,
                    KieuDang = d.ContainsKey("nguon_goc") ? d["nguon_goc"]?.ToString() : null,
                    KieuVienChu = d.ContainsKey("suc_khoe") ? d["suc_khoe"]?.ToString() : null,
                    KichThuocVienChu = d.ContainsKey("tiem_phong") ? d["tiem_phong"]?.ToString() : null,
                    GioiTinh = d.ContainsKey("gioi_tinh") ? d["gioi_tinh"]?.ToString() : null,
                    dac_diem = d.ContainsKey("dac_diem") ? d["dac_diem"]?.ToString() : null,
                    MauKimLoai = d.ContainsKey("van_chuyen") ? d["van_chuyen"]?.ToString() : null,
                    DaTam = d.ContainsKey("tinh_trang") ? d["tinh_trang"]?.ToString() : null
                }).ToList();

                // Gọi service để tạo chi tiết sản phẩm
                var result = ProductDetailService.CreateProductDetails(productDetails);
                if (result > 0)
                {
                    return Ok(new ApiResponse<dynamic>(200, "Tao thanh cong"));
                }
                else
                {
                    return NotFound(new ApiResponse<dynamic>(404, "Tao that bai"));
                } 
                    }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}