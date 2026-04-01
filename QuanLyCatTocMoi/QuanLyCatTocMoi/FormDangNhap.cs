using QuanLyCatTocMoi;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyCatToc
{
    public partial class FormDangNhap : Form
    {
        string connectionString = @"Data Source=MSI\HAIXEDAP;Initial Catalog=QuanLyCatTocDB_Full;Integrated Security=True";

        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDN AND MatKhau = @Pass";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDN", txtTenDangNhap.Text);
                cmd.Parameters.AddWithValue("@Pass", txtMatKhau.Text);

                conn.Open();
                int result = (int)cmd.ExecuteScalar();

                if (result > 0)
                {
                    Form1 frmMain = new Form1();
                    frmMain.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                }
            }
        }



        private void btnChuyenDangKy_Click(object sender, EventArgs e)
        {
            FormDangKy frmDK = new FormDangKy();
            this.Hide();
            frmDK.ShowDialog();
            this.Show();
        }

        private void btnChuyenDangKy_Click_1(object sender, EventArgs e)
        {

        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}