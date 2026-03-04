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
    public partial class Danhmucchitiethoadon : Form
    {
        string connectionString = @"Data Source=LAPTOP-UJU060SC;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public Danhmucchitiethoadon()
        {
            InitializeComponent();
        }

        private void Danhmucchitiethoadon_Load(object sender, EventArgs e)
        {
            LoadDataChiTietHoaDon();
        }
        private void LoadDataChiTietHoaDon()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Truy vấn kết hợp bảng ChiTietHoaDon và SanPham để tính Thành Tiền
                    string query = @"
                        SELECT 
                            CT.MaHD AS N'Mã HĐ', 
                            CT.MaSP AS N'Mã SP', 
                            SP.TenSP AS N'Tên Sản Phẩm', 
                            SP.DonViTinh AS N'ĐVT',
                            CT.SoLuong AS N'Số Lượng', 
                            SP.DonGia AS N'Đơn Giá',
                            (CT.SoLuong * SP.DonGia) AS N'Thành Tiền'
                        FROM ChiTietHoaDon CT
                        INNER JOIN SanPham SP ON CT.MaSP = SP.MaSP";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Đổ dữ liệu vào DataGridView
                    dgvChiTietHoaDon.DataSource = dt;

                    // Tùy chỉnh định dạng cho cột Đơn giá và Thành tiền hiển thị số tiền đẹp hơn (VND)
                    if (dgvChiTietHoaDon.Columns["Đơn Giá"] != null)
                        dgvChiTietHoaDon.Columns["Đơn Giá"].DefaultCellStyle.Format = "N0";
                    if (dgvChiTietHoaDon.Columns["Thành Tiền"] != null)
                        dgvChiTietHoaDon.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";

                    // Dàn đều các cột
                    dgvChiTietHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form
        }
    }
}
