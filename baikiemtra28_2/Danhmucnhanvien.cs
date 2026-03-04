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
    public partial class Danhmucnhanvien : Form
    {
        string connectionString = @"Data Source=LAPTOP-UJU060SC;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public Danhmucnhanvien()
        {
            InitializeComponent();
        }

        private void Danhmucnhanvien_Load(object sender, EventArgs e)
        {
            LoadDataNhanVien();
        }
        private void LoadDataNhanVien()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Đã sửa lại thành cột Gioitinh cho đúng với database của bạn
                    string query = @"SELECT 
                                Manv AS N'Mã NV', 
                                Ho AS N'Họ', 
                                Ten AS N'Tên', 
                                Gioitinh AS N'Giới Tính', 
                                Ngaynv AS N'Ngày Vào Làm', 
                                Diachi AS N'Địa Chỉ', 
                                Dienthoai AS N'Điện Thoại' 
                             FROM nhanvien";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Đổ dữ liệu vào lưới
                    dgvNhanVien.DataSource = dt;

                    // Định dạng ngày tháng (nếu cần)
                    if (dgvNhanVien.Columns["Ngày Vào Làm"] != null)
                    {
                        dgvNhanVien.Columns["Ngày Vào Làm"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }

                    // Tự động dàn đều cột
                    dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form
        }
    }
}
