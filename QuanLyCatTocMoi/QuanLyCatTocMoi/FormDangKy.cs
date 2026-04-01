using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCatTocMoi
{
    public partial class FormDangKy : Form
    {
        string connectionString = @"Data Source=MSI\HAIXEDAP;Initial Catalog=QuanLyCatTocDB_Full;Integrated Security=True";
        public FormDangKy()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormDangKy_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen) VALUES (@TenDN, @Pass, @HoTen)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDN", txtTenDangNhap.Text);
                cmd.Parameters.AddWithValue("@Pass", txtMatKhau.Text);
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Đăng ký thành công!");
            this.Close();
        }
    }
}

