# WebThuCung-TMDT
1. Điều kiện cần thiết
Trước khi chạy code, bạn cần cài đặt các công cụ sau:
•	.NET SDK 8.0: Tải và cài đặt từ https://dotnet.microsoft.com/en-us/download/dotnet/8.0
•	Visual Studio Code: Tải và cài đặt từ https://code.visualstudio.com/
•	MySQL Server & MySQL Workbench: Tải và cài đặt từ https://dev.mysql.com/downloads/workbench/
2. Clone repository từ GitHub
Mở Terminal hoặc Command Prompt và chạy lệnh sau để tải mã nguồn:
 git clone https://github.com/nghiadarealest/WebThuCung-TMDT.git
 cd WebThuCung-TMDT

3. Mở dự án trong Visual Studio Code
Sau khi tải mã nguồn về, mở VS Code và mở thư mục dự án:
Mở VS Code, chọn File > Open Folder và chọn thư mục dự án.
4. Kết nối CSDL MySQL Workbench với dự án
1.	Mở MySQL Workbench và thiết lập kết nối:
2.	Mở MySQL Workbench và nhấp vào + (New Connection).
3.	Điền thông tin kết nối:
4.	Connection Name: Đặt tên tùy ý.
5.	Hostname: localhost
6.	Port: 3306
7.	Username: root (hoặc tài khoản MySQL của bạn)
8.	Password: Nhập mật khẩu của MySQL.
9.	Nhấp vào Test Connection để kiểm tra.
10.	Chọn File > Open SQL Script, sau đó chạy file .sql để import dữ liệu vào MySQL.
11.	Mở file appsettings.json và chỉnh sửa phần ConnectionStrings:
"ConnectionStrings": {
   "DefaultConnection": "Server=localhost;Database=my_existing_database;User=root;Password=yourpassword;"
}
5. Chạy dự án 
1.	Mở Terminal trong VS Code:
Nhấn Ctrl + ~ (hoặc vào View > Terminal).
2.	Build dự án:
dotnet build
3.	Chạy dự án:
dotnet watch 
