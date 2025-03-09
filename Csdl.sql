-- Tạo cơ sở dữ liệu và sử dụng
CREATE DATABASE handk;
USE handk;

-- Tạo các bảng (giữ nguyên cấu trúc như bạn đã cung cấp)
CREATE TABLE Role ( 
    id INT AUTO_INCREMENT PRIMARY KEY,  
    code VARCHAR(100) NOT NULL
);

CREATE TABLE Users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL,
    phone_number VARCHAR(20),
    address VARCHAR(50),
    role_id INT DEFAULT 1,
    is_deleted TINYINT(1) DEFAULT 0
);

CREATE TABLE Materials (
    material_id INT AUTO_INCREMENT PRIMARY KEY,
    material_name VARCHAR(50) NOT NULL,
    description TEXT,
    is_deleted TINYINT(1) DEFAULT 0
);

CREATE TABLE Categories (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(50) NOT NULL,
    description TEXT,
    is_deleted TINYINT(1) DEFAULT 0
);

CREATE TABLE Products (
    product_id VARCHAR(100) PRIMARY KEY,
    product_name VARCHAR(50) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    material_id INT,
    category_id INT,
    image_url VARCHAR(255),
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (material_id) REFERENCES Materials(material_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

CREATE TABLE ProductDetail (
    product_detail_iD INT PRIMARY KEY AUTO_INCREMENT,
    product_id VARCHAR(100) NOT NULL,
    tuoi NVARCHAR(255) NULL,
    nguon_goc NVARCHAR(255) NULL,
    suc_khoe NVARCHAR(255) NULL,
    tiem_phong NVARCHAR(255) NULL,
    gioi_tinh NVARCHAR(250) NULL,
    dac_diem NVARCHAR(500) NULL,
    van_chuyen NVARCHAR(255) NULL,
    tinh_trang NVARCHAR(255) NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

CREATE TABLE Orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    order_date DATETIME NOT NULL,
    status VARCHAR(50) NOT NULL,
    total_amount DECIMAL(10, 2) NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE Order_Items (
    order_item_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    product_name VARCHAR(50) NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id)
);

CREATE TABLE Suppliers (
    supplier_id INT AUTO_INCREMENT PRIMARY KEY,
    supplier_name VARCHAR(50) NOT NULL,
    contact_name VARCHAR(50),
    phone_number VARCHAR(20),
    address VARCHAR(50),
    email VARCHAR(50),
    is_deleted TINYINT(1) DEFAULT 0
);

CREATE TABLE Product_Supplier (
    ps_id INT AUTO_INCREMENT PRIMARY KEY,
    product_id VARCHAR(50) NOT NULL,
    supplier_id INT NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);

CREATE TABLE Inventory (
    inventory_id INT AUTO_INCREMENT PRIMARY KEY,
    product_id VARCHAR(50) NOT NULL,
    quantity_in_stock INT NOT NULL,
    last_updated DATETIME NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

CREATE TABLE Discounts (
    discount_id INT AUTO_INCREMENT PRIMARY KEY,
    discount_name VARCHAR(255) NOT NULL,
    discount_percent DECIMAL(5, 2) NOT NULL,
    start_date DATETIME NOT NULL,
    end_date DATETIME NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0
);

CREATE TABLE Product_Discount (
    pd_id INT AUTO_INCREMENT PRIMARY KEY,
    product_id VARCHAR(50) NOT NULL,
    discount_id INT NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (discount_id) REFERENCES Discounts(discount_id)
);

CREATE TABLE Payment_Methods (
    payment_method_id INT AUTO_INCREMENT PRIMARY KEY,
    payment_method_name VARCHAR(255) NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0
);

CREATE TABLE Order_Payments (
    order_payment_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    payment_method_id INT NOT NULL,
    amount_paid DECIMAL(10, 2) NOT NULL,
    payment_date DATETIME NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (payment_method_id) REFERENCES Payment_Methods(payment_method_id)
);

CREATE TABLE Shopping_Cart (
    cart_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    product_id VARCHAR(50) NOT NULL,
    quantity INT NOT NULL,
    added_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

CREATE TABLE Transaction_History (
    transaction_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    order_id INT NOT NULL,
    transaction_date DATETIME NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    payment_method_id INT NOT NULL,
    status VARCHAR(50) NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (payment_method_id) REFERENCES Payment_Methods(payment_method_id)
);

CREATE TABLE Reviews (
    review_id INT AUTO_INCREMENT PRIMARY KEY,
    product_id VARCHAR(50) NOT NULL,
    user_id INT NOT NULL,
    rating INT NOT NULL CHECK (rating >= 1 AND rating <= 5),
    comment TEXT,
    review_date DATETIME NOT NULL,
    is_deleted TINYINT(1) DEFAULT 0,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- Chèn dữ liệu mẫu (thay đổi nội dung để phù hợp với thú cưng)
INSERT INTO Role (id, code) VALUES (1, 'USER'), (2, 'ADMIN'), (3, 'STAFF');

INSERT INTO Users (username, password, email, phone_number, address, role_id, is_deleted) VALUES 
('admin', '123456', 'admin@example.com', '0912345678', '123 Pet Street', 2, 0),
('staff', '123456', 'staff@example.com', '0912345679', '456 Dog Lane', 3, 0),
('user1', '123456', 'user1@example.com', '0912345680', '789 Cat Road', 1, 0),
('user2', '123456', 'user2@example.com', '0912345681', '101 Puppy Ave', 1, 0),
('user3', '123456', 'user3@example.com', '0912345682', '202 Kitten Blvd', 1, 0);

INSERT INTO Materials (material_name, description, is_deleted) VALUES 
('Corgi', 'Giống chó nhỏ dễ thương, chân ngắn', 0), 
('Alaska', 'Giống chó lớn, lông dày ấm áp', 0), 
('Poodle', 'Giống chó thông minh, lông xoăn', 0), 
('Persian', 'Giống mèo lông dài, sang trọng', 0), 
('Maine Coon', 'Giống mèo lớn, thân thiện', 0);

INSERT INTO Categories (category_name, description, is_deleted) VALUES 
('Chó Cảnh', 'Các giống chó cảnh đẹp, dễ nuôi', 0),
('Mèo Cảnh', 'Các giống mèo cảnh độc đáo', 0),
('Chó Lớn', 'Các giống chó kích thước lớn', 0),
('Mèo Nhỏ', 'Các giống mèo nhỏ gọn, dễ thương', 0),
('Thú Cưng Lai', 'Thú cưng lai tạo độc đáo', 0);

-- Products (thú cưng)
INSERT INTO Products (product_id, product_name, description, price, material_id, category_id, image_url, is_deleted) VALUES
('P001', 'Corgi Puppy', 'Chó Corgi con dễ thương', 650.00, 1, 1, '/upload/corgi_puppy.jpg', 0),
('P002', 'Alaska Dog', 'Chó Alaska khỏe mạnh', 1600.00, 2, 3, '/upload/alaska_dog.jpg', 0),
('P003', 'Poodle Pup', 'Chó Poodle thông minh', 750.00, 3, 1, '/upload/poodle_pup.jpg', 0),
('P004', 'Persian Kitten', 'Mèo Persian lông dài', 950.00, 4, 2, '/upload/persian_kitten.jpg', 0),
('P005', 'Maine Coon Cat', 'Mèo Maine Coon thân thiện', 800.00, 5, 2, '/upload/maine_coon_cat.jpg', 0);

-- ProductDetail (chi tiết thú cưng)
INSERT INTO ProductDetail (product_id, tuoi, nguon_goc, suc_khoe, tiem_phong, gioi_tinh, dac_diem, van_chuyen, tinh_trang) VALUES 
('P001', '3 tháng', 'Việt Nam', 'Khỏe mạnh', 'Đã tiêm 2 mũi', 'Đực', 'Chân ngắn, lông mềm', 'Miễn phí nội thành', 'Còn hàng'),
('P002', '6 tháng', 'Nga', 'Tốt', 'Đã tiêm đầy đủ', 'Cái', 'Lông dày, khỏe khoắn', 'Tính phí', 'Còn hàng'),
('P003', '4 tháng', 'Việt Nam', 'Khỏe mạnh', 'Đã tiêm 1 mũi', 'Đực', 'Lông xoăn, thông minh', 'Miễn phí', 'Còn hàng'),
('P004', '2 tháng', 'Thái Lan', 'Khỏe mạnh', 'Chưa tiêm', 'Cái', 'Lông dài, mặt phẳng', 'Tính phí', 'Còn hàng'),
('P005', '5 tháng', 'Mỹ', 'Tốt', 'Đã tiêm đầy đủ', 'Đực', 'Kích thước lớn, hiền lành', 'Miễn phí', 'Còn hàng');

-- Orders (đơn hàng)
INSERT INTO Orders (user_id, order_date, status, total_amount, is_deleted) VALUES 
(1, '2024-12-01 10:30:00', 'Completed', 650.00, 0),
(2, '2024-12-01 11:45:00', 'Pending', 1600.00, 0),
(3, '2024-12-02 09:00:00', 'Shipped', 750.00, 0),
(4, '2024-12-02 15:30:00', 'Completed', 950.00, 0),
(5, '2024-12-03 14:00:00', 'Canceled', 800.00, 0);

-- Order_Items (chi tiết đơn hàng)
INSERT INTO Order_Items (order_id, product_name, quantity, price, is_deleted) VALUES 
(1, 'Corgi Puppy', 1, 650.00, 0),
(2, 'Alaska Dog', 1, 1600.00, 0),
(3, 'Poodle Pup', 1, 750.00, 0),
(4, 'Persian Kitten', 1, 950.00, 0),
(5, 'Maine Coon Cat', 1, 800.00, 0);

-- Suppliers (nhà cung cấp)
INSERT INTO Suppliers (supplier_name, contact_name, phone_number, address, email, is_deleted) VALUES
('Pet Haven', 'Nguyen Van A', '0912345678', '123 Pet Street', 'contact@pethaven.com', 0),
('Furry Friends', 'Tran Thi B', '0912345679', '456 Dog Lane', 'contact@furryfriends.com', 0),
('Cat Lovers', 'Le Van C', '0912345680', '789 Cat Road', 'contact@catlovers.com', 0),
('Puppy World', 'Pham Thi D', '0912345681', '101 Puppy Ave', 'contact@puppyworld.com', 0),
('Kitten Home', 'Hoang Van E', '0912345682', '202 Kitten Blvd', 'contact@kittenhome.com', 0);

-- Product_Supplier (liên kết sản phẩm và nhà cung cấp)
INSERT INTO Product_Supplier (product_id, supplier_id, is_deleted) VALUES
('P001', 1, 0), 
('P002', 2, 0), 
('P003', 3, 0), 
('P004', 4, 0), 
('P005', 5, 0);

-- Inventory (tồn kho)
INSERT INTO Inventory (product_id, quantity_in_stock, last_updated, is_deleted) VALUES
('P001', 10, '2024-12-01 10:00:00', 0),
('P002', 5, '2024-12-01 10:15:00', 0),
('P003', 8, '2024-12-01 10:30:00', 0),
('P004', 7, '2024-12-01 10:45:00', 0),
('P005', 6, '2024-12-01 11:00:00', 0);

-- Discounts (giảm giá)
INSERT INTO Discounts (discount_name, discount_percent, start_date, end_date, is_deleted) VALUES
('Pet Fest Sale', 10.00, '2024-12-01 00:00:00', '2024-12-31 23:59:59', 0),
('New Year Pet Promo', 15.00, '2024-12-25 00:00:00', '2025-01-05 23:59:59', 0),
('Summer Pet Deal', 20.00, '2024-06-01 00:00:00', '2024-06-30 23:59:59', 0),
('Pet Adoption Day', 25.00, '2024-11-01 00:00:00', '2024-11-01 23:59:59', 0),
('Winter Pet Discount', 5.00, '2024-12-01 00:00:00', '2024-12-15 23:59:59', 0);

-- Product_Discount (liên kết sản phẩm và giảm giá)
INSERT INTO Product_Discount (product_id, discount_id, is_deleted) VALUES
('P001', 1, 0), 
('P002', 2, 0), 
('P003', 3, 0), 
('P004', 4, 0), 
('P005', 5, 0);

-- Payment_Methods (phương thức thanh toán)
INSERT INTO Payment_Methods (payment_method_name, is_deleted) VALUES
('MoMo', 0),
('ZaloPay', 0),
('VNPay', 0),
('Bank Card', 0),
('COD', 0);

-- Order_Payments (thanh toán đơn hàng)
INSERT INTO Order_Payments (order_id, payment_method_id, amount_paid, payment_date, is_deleted) VALUES
(1, 1, 650.00, '2024-12-01 10:35:00', 0),
(2, 2, 1600.00, '2024-12-01 11:50:00', 0),
(3, 3, 750.00, '2024-12-02 09:05:00', 0),
(4, 4, 950.00, '2024-12-02 15:35:00', 0),
(5, 5, 800.00, '2024-12-03 14:05:00', 0);

-- Shopping_Cart (giỏ hàng)
INSERT INTO Shopping_Cart (user_id, product_id, quantity, added_at, is_deleted) VALUES
(1, 'P001', 1, '2024-12-01 09:00:00', 0),
(2, 'P002', 1, '2024-12-01 09:15:00', 0),
(3, 'P003', 1, '2024-12-02 10:00:00', 0),
(4, 'P004', 1, '2024-12-02 14:30:00', 0),
(5, 'P005', 1, '2024-12-03 13:45:00', 0);

-- Transaction_History (lịch sử giao dịch)
INSERT INTO Transaction_History (user_id, order_id, transaction_date, amount, payment_method_id, status, is_deleted) VALUES
(1, 1, '2024-12-01 10:30:00', 650.00, 1, 'Completed', 0),
(2, 2, '2024-12-01 11:45:00', 1600.00, 2, 'Pending', 0),
(3, 3, '2024-12-02 09:00:00', 750.00, 3, 'Completed', 0),
(4, 4, '2024-12-02 15:30:00', 950.00, 4, 'Completed', 0),
(5, 5, '2024-12-03 14:00:00', 800.00, 5, 'Canceled', 0);

-- Reviews (đánh giá)
INSERT INTO Reviews (product_id, user_id, rating, comment, review_date, is_deleted) VALUES
('P001', 1, 5, 'Corgi con rất dễ thương, khỏe mạnh!', '2024-12-01 11:00:00', 0),
('P002', 2, 4, 'Alaska đẹp nhưng vận chuyển hơi chậm.', '2024-12-01 12:00:00', 0),
('P003', 3, 5, 'Poodle thông minh, rất đáng tiền!', '2024-12-02 09:30:00', 0),
('P004', 4, 3, 'Mèo Persian đẹp nhưng giá hơi cao.', '2024-12-02 10:00:00', 0),
('P005', 5, 5, 'Maine Coon tuyệt vời, thân thiện!', '2024-12-03 14:00:00', 0);