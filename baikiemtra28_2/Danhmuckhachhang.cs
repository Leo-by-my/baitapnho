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
    public partial class Danhmuckhachhang : Form
    {
        string connectionString = @"Data Source=LAPTOP-UJU060SC;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public Danhmuckhachhang()
        {
            InitializeComponent();
        }

        private void btnTrove_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
        }

        private void Danhmuckhachhang_Load(object sender, EventArgs e)
        {
            LoadDataKhachHang();
        }
        private void LoadDataKhachHang()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Câu truy vấn kết hợp bảng KhachHang và ThanhPho để lấy Tên thành phố
                    string query = @"SELECT 
                                        KH.MaKH AS N'Mã KH', 
                                        KH.TenCty AS N'Tên Công Ty', 
                                        KH.DiaChi AS N'Địa Chỉ', 
                                        TP.TenThanhPho AS N'Thành Phố', 
                                        KH.DienThoai AS N'Điện Thoại'
                                     FROM KhachHang KH
                                     LEFT JOIN ThanhPho TP ON KH.ThanhPho = TP.ThanhPho";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Đổ dữ liệu vào DataGridView
                    dgvKhachHang.DataSource = dt;

                    // Tùy chỉnh độ rộng các cột cho đẹp mắt (Tùy chọn)
                    dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
