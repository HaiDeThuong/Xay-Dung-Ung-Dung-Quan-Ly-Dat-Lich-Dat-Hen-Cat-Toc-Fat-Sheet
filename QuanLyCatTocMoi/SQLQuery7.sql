USE QuanLyCatTocDB_Full;
GO

-- Xóa trắng dữ liệu dịch vụ cũ bị lộn xộn
DELETE FROM DichVu;
-- Reset lại cột ID tự tăng (nếu có) để bắt đầu lại từ 1 cho đẹp
DBCC CHECKIDENT ('DichVu', RESEED, 0);
GO

-- Thêm bộ dữ liệu mới đã được căn chỉnh gọn gàng
INSERT INTO DichVu (TenDichVu)
VALUES 
-- Nhóm Cắt Tóc
(N'[CẮT] Combo 1: Cắt Sấy Tạo Kiểu - 50.000đ'),
(N'[CẮT] Combo 2: Cắt Tóc, Cạo Mặt - 60.000đ'),
(N'[CẮT] Combo 3: Cắt Tóc, Cạo Mặt, Ráy Tai - 70.000đ'),
(N'[CẮT] Combo 4: Cắt Tóc, Cạo Mặt, Ráy Tai, Xả, Lột Mụn - 85.000đ'),

-- Nhóm Uốn Tóc (Đã bổ sung giá minh họa cho đồng bộ)
(N'[UỐN] Combo 1: Cắt Tóc, Uốn cơ bản - 150.000đ'),
(N'[UỐN] Combo 2: Cắt Tóc, Uốn Ruffled - 250.000đ'),
(N'[UỐN] Combo 3: Cắt Tóc, Uốn con sâu - 300.000đ');
GO