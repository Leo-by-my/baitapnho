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

namespace baikiemtra28_2
{
    public partial class FormDoiMatKhau : Form
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public FormDoiMatKhau()
        {
            InitializeComponent();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string mkCu = txtMatKhauCu.Text;
            string mkMoi = txtMatKhauMoi.Text;
            string xacNhan = txtXacNhanMK.Text;

            // 1. Kiểm tra đầu vào
            if (string.IsNullOrEmpty(mkCu) || string.IsNullOrEmpty(mkMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (mkMoi != xacNhan)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận không khớp!");
                return;
            }

            // 2. Thực hiện cập nhật vào Database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Câu lệnh SQL: Chỉ Update nếu Username và Mật khẩu cũ đều khớp
                    string sql = "UPDATE DangNhap SET Password=@newPass WHERE Username=@user AND Password=@oldPass";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@newPass", mkMoi);
                    cmd.Parameters.AddWithValue("@user", DangNhap.UserHienTai); // Gọi biến từ Form Đăng Nhập
                    cmd.Parameters.AddWithValue("@oldPass", mkCu);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu cũ không chính xác!", "Lỗi");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
