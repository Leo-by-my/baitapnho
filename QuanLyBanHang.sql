-- Tạo database mới (Bạn có thể đổi tên 'QuanLyBanHang' thành tên bạn muốn)
CREATE DATABASE QuanLyBanHang;
GO

USE QuanLyBanHang;
GO

-- 1. BẢNG THÀNH PHỐ (Bảng cha - tạo trước)
CREATE TABLE ThanhPho (
    ThanhPho VARCHAR(10) PRIMARY KEY, -- Mã thành phố (VD: HN, HCM)
    TenThanhPho NVARCHAR(50) NOT NULL -- Tên thành phố có dấu
);

-- 2. BẢNG SẢN PHẨM (Bảng cha - tạo trước)
CREATE TABLE SanPham (
    MaSP VARCHAR(10) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    DonViTinh NVARCHAR(20),
    DonGia DECIMAL(18, 2), -- Kiểu số thực để lưu tiền tệ
    Hinh NVARCHAR(MAX) -- Lưu đường dẫn ảnh (hoặc dùng VARBINARY(MAX) nếu muốn lưu file ảnh trực tiếp)
);

-- 3. BẢNG NHÂN VIÊN (Bảng cha - tạo trước)
CREATE TABLE NhanVien (
    MaNV VARCHAR(10) PRIMARY KEY,
    Ho NVARCHAR(50),
    Ten NVARCHAR(50) NOT NULL,
    Nu BIT, -- Kiểu True/False (1: Nữ, 0: Nam)
    NgayNV DATE, -- Ngày vào làm
    DiaChi NVARCHAR(200),
    DienThoai VARCHAR(20),
    Hinh NVARCHAR(MAX)
);

-- 4. BẢNG KHÁCH HÀNG (Bảng con của Thành Phố)
CREATE TABLE KhachHang (
    MaKH VARCHAR(10) PRIMARY KEY,
    TenCty NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200),
    ThanhPho VARCHAR(10),
    DienThoai VARCHAR(20),
    -- Tạo khóa ngoại liên kết với bảng ThanhPho
    CONSTRAINT FK_KhachHang_ThanhPho FOREIGN KEY (ThanhPho) REFERENCES ThanhPho(ThanhPho)
);

-- 5. BẢNG HÓA ĐƠN (Bảng con của Khách Hàng và Nhân Viên)
CREATE TABLE HoaDon (
    MaHD VARCHAR(10) PRIMARY KEY,
    MaKH VARCHAR(10),
    MaNV VARCHAR(10),
    NgayLapHD DATE,
    NgayNhanHang DATE,
    -- Khóa ngoại
    CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_HoaDon_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- 6. BẢNG CHI TIẾT HÓA ĐƠN (Bảng con của Hóa Đơn và Sản Phẩm)
CREATE TABLE ChiTietHoaDon (
    MaHD VARCHAR(10),
    MaSP VARCHAR(10),
    SoLuong INT,
    -- Khóa chính kép (Một hóa đơn có nhiều sản phẩm, một sản phẩm nằm trong nhiều hóa đơn)
    PRIMARY KEY (MaHD, MaSP),
    -- Khóa ngoại
    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    CONSTRAINT FK_CTHD_SanPham FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- ================= THÊM DỮ LIỆU BẢNG CHA =================

-- Thêm Thành Phố
INSERT INTO ThanhPho (ThanhPho, TenThanhPho) VALUES
('HN', N'Hà Nội'),
('HCM', N'Hồ Chí Minh'),
('DN', N'Đà Nẵng'),
('CT', N'Cần Thơ');

-- Thêm Sản Phẩm
INSERT INTO SanPham (MaSP, TenSP, DonViTinh, DonGia, Hinh) VALUES
('SP01', N'Bàn phím cơ Logitech', N'Cái', 1200000, NULL),
('SP02', N'Chuột không dây DareU', N'Cái', 350000, NULL),
('SP03', N'Màn hình Dell 24 inch', N'Chiếc', 3200000, NULL),
('SP04', N'Laptop Asus Vivobook', N'Chiếc', 15500000, NULL);

-- Thêm Nhân Viên
INSERT INTO NhanVien (MaNV, Ho, Ten, Nu, NgayNV, DiaChi, DienThoai, Hinh) VALUES
('NV01', N'Nguyễn', N'An', 0, '2022-01-15', N'123 Lê Lợi, HCM', '090111222', NULL),
('NV02', N'Trần Thị', N'Bảo', 1, '2023-05-20', N'45 Cầu Giấy, HN', '091223344', NULL);

-- ================= THÊM DỮ LIỆU BẢNG CON =================

-- Thêm Khách Hàng (Lưu ý cột ThanhPho phải khớp với mã ở trên)
INSERT INTO KhachHang (MaKH, TenCty, DiaChi, ThanhPho, DienThoai) VALUES
('KH01', N'Công ty Cổ phần Alpha', N'Số 1 Nam Kỳ Khởi Nghĩa', 'HCM', '0283123456'),
('KH02', N'Cửa hàng máy tính Beta', N'Số 2 Thái Hà', 'HN', '0243654321'),
('KH03', N'Doanh nghiệp tư nhân Gamma', N'Số 3 Nguyễn Văn Linh', 'DN', '0236987654');

-- Thêm Hóa Đơn
INSERT INTO HoaDon (MaHD, MaKH, MaNV, NgayLapHD, NgayNhanHang) VALUES
('HD001', 'KH01', 'NV01', '2026-03-01', '2026-03-02'),
('HD002', 'KH02', 'NV02', '2026-03-03', '2026-03-05'),
('HD003', 'KH03', 'NV01', '2026-03-04', '2026-03-06');

-- Thêm Chi Tiết Hóa Đơn
INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong) VALUES
('HD001', 'SP01', 5),
('HD001', 'SP02', 5),
('HD002', 'SP03', 2),
('HD003', 'SP04', 1),
('HD003', 'SP01', 3);
GO

CREATE TABLE DangNhap (
    Username NVARCHAR(50),
    Password NVARCHAR(50),
);
GO
INSERT INTO DangNhap (Username, Password) VALUES
('admin','123');
GO