using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace baikiemtra28_2
{
    public partial class DangNhap : Form
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public DangNhap()
        {
            InitializeComponent();
        }
        public static string UserHienTai = "";

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM DangNhap WHERE Username=@user AND Password=@pass";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                    object result = cmd.ExecuteScalar();
                    int count = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    if (count > 0)
                    {
                        // Lưu tên đăng nhập vào biến static
                        UserHienTai = txtUsername.Text;

                        MessageBox.Show("Đăng nhập thành công!", "Thông báo");

                        // Mở Form chính và đóng Form đăng nhập
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
