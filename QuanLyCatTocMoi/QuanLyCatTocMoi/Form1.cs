using QuanLyCatToc; // Giữ lại theo code của bạn
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyCatTocMoi
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=MSI\HAIXEDAP;Initial Catalog=QuanLyCatTocDB_Full;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDichVu();
            LoadLichHen(""); // Truyền chuỗi rỗng để load tất cả danh sách ban đầu
        }

        private void LoadDichVu()
        {
            string query = "SELECT * FROM DichVu";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbDichVu.DataSource = dt;
                cbDichVu.DisplayMember = "TenDichVu";
                cbDichVu.ValueMember = "MaDichVu";
            }
        }

        // Đã sửa lại hoàn toàn cho ListView
        private void LoadLichHen(string tuKhoa = "")
        {
            listView1.Items.Clear(); // Xóa dữ liệu cũ trên bảng

            string query = "SELECT lh.MaLich, lh.HoTen, lh.SoDienThoai, lh.Email, lh.NgayGioHen, dv.TenDichVu " +
                           "FROM LichHen lh JOIN DichVu dv ON lh.MaDichVu = dv.MaDichVu ";

            // Thêm điều kiện tìm kiếm nếu có từ khóa
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query += " WHERE lh.HoTen LIKE @TuKhoa OR lh.SoDienThoai LIKE @TuKhoa";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["MaLich"].ToString());
                    item.SubItems.Add(reader["HoTen"].ToString());
                    item.SubItems.Add(reader["SoDienThoai"].ToString());
                    item.SubItems.Add(reader["Email"].ToString());

                    DateTime ngayHen = Convert.ToDateTime(reader["NgayGioHen"]);
                    item.SubItems.Add(ngayHen.ToString("dd/MM/yyyy HH:mm"));
                    item.SubItems.Add(reader["TenDichVu"].ToString());

                    listView1.Items.Add(item);
                }
            }
        }

        private void btnDatLich_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text;
            string sdt = txtSoDienThoai.Text;
            string email = txtEmail.Text;
            DateTime ngayHen = dtpNgayHen.Value;
            int maDV = Convert.ToInt32(cbDichVu.SelectedValue);

            if (hoTen == "" || sdt == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và Số điện thoại!");
                return;
            }

            string query = "INSERT INTO LichHen (HoTen, SoDienThoai, Email, NgayGioHen, MaDichVu) VALUES (@Ten, @SDT, @Email, @Ngay, @MaDV)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", hoTen);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Ngay", ngayHen);
                cmd.Parameters.AddWithValue("@MaDV", maDV);

                conn.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Đặt lịch thành công!");

                txtHoTen.Clear();
                txtSoDienThoai.Clear();
                txtEmail.Clear();
                LoadLichHen("");
            }
        }

        // Hàm Sửa nâng cấp cho ListView
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // Lấy mã lịch từ cột đầu tiên của ListView
                string maLich = listView1.SelectedItems[0].SubItems[0].Text;
                string query = "UPDATE LichHen SET HoTen = @Ten, SoDienThoai = @SDT, Email = @Email, NgayGioHen = @Ngay, MaDichVu = @MaDV WHERE MaLich = @MaLich";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Ten", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@SDT", txtSoDienThoai.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Ngay", dtpNgayHen.Value);
                    cmd.Parameters.AddWithValue("@MaDV", cbDichVu.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaLich", maLich);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật lịch hẹn thành công!");
                    LoadLichHen("");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!");
            }
        }

        // Hàm Xóa nâng cấp cho ListView
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string maLich = listView1.SelectedItems[0].SubItems[0].Text;
                DialogResult xacNhan = MessageBox.Show("Bạn có chắc chắn muốn xóa lịch hẹn này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (xacNhan == DialogResult.Yes)
                {
                    string query = "DELETE FROM LichHen WHERE MaLich = @MaLich";
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaLich", maLich);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Đã xóa lịch hẹn thành công!");
                        LoadLichHen("");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng click chọn một dòng trong bảng bên phải để xóa!");
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide(); // Ẩn form chính
                FormDangNhap frmLogin = new FormDangNhap();
                frmLogin.ShowDialog(); // Mở lại form đăng nhập
                this.Close(); // Đóng hẳn form chính
            }
        }

        // Hàm đẩy dữ liệu ngược lên TextBox khi click vào 1 dòng trên ListView
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];

                txtHoTen.Text = item.SubItems[1].Text;
                txtSoDienThoai.Text = item.SubItems[2].Text;
                txtEmail.Text = item.SubItems[3].Text;

                // Xử lý cẩn thận đoạn ngày tháng để tránh lỗi Convert
                DateTime ngayHen;
                if (DateTime.TryParseExact(item.SubItems[4].Text, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out ngayHen))
                {
                    dtpNgayHen.Value = ngayHen;
                }

                cbDichVu.Text = item.SubItems[5].Text;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            LoadLichHen(tuKhoa);
        }
    }
}