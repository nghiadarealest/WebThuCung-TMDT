﻿﻿using _123.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _123.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Category(string category)
        {
            switch (category.ToLower())
            {
                case "cho":
                    ViewData["Title"] = "Chó Cảnh";
                    break;
                case "meo":
                    ViewData["Title"] = "Mèo Cảnh";
                    break;
                case "trangsuc":
                    ViewData["Title"] = "Trang sức";
                    break;
                case "kimcuong":
                    ViewData["Title"] = "Kim cương";
                    break;
                case "mens":
                    ViewData["Title"] = "Men's";
                    break;
                default:
                    return NotFound(); // Nếu không khớp, trả về lỗi 404
            }

            return View("Category"); // Sử dụng chung một View
        }

        public IActionResult ThongTin(string topic)
        {
            switch (topic?.ToLower())
            {
                case "chinhsachthuhoivadoitra":
                    return View("ChinhSachThuHoiVaDoiTra");
                case "huongdandosize":
                    return View("HuongDanDoSize");
                case "chinhsachthanhtoan":
                    return View("ChinhSachThanhToan");
                case "chinhsachbaomatthongtin":
                    return View("ChinhSachBaoMatThongTin");
                case "giavang":
                    return View("GiaVang");
                default:
                    return NotFound(); // Nếu không tìm thấy topic, trả về lỗi 404
            }
        }
        
        public IActionResult KhuyenMai()
        {
            return View(); // Trả về view "KhuyenMai"
        }


        public IActionResult CauChuyenPNP()
        {
            return View(); // Trả về view "CauChuyenPNP"
        }

        public IActionResult CuaHangHanDK()
        {
            return View(); // Trả về view "CuaHangHanDK"
        }

        public IActionResult ProductDetail()
    {
        return View();
    }

        public IActionResult orderPayment()
    {
        return View();
    }

        public IActionResult paymentSuccessful()
    {
        return View("paymentSuccessful");
    }
    public IActionResult Momopayment()
    {
        return View("Momopayment");
    }
    public IActionResult qrcodepayment()
    {
        return View("qrcodepayment");
    }
    public IActionResult VNPaypaymentCallBack()
    {
        return View("PaymentCallBack");
    }
    public IActionResult Vnpaypayment()
    {
        return View("Vnpaypayment");
    }

    public IActionResult Zalopayment()
    {
        return View("Zalopayment");
    }
    public IActionResult cart()
    {
        return View();
    }
    public IActionResult transactionHistory()
    {
        return View();
    }
    public IActionResult UserInfo()
    {
        return View();
    }

    public IActionResult YeuThich()
    {
        return View();
    }

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}