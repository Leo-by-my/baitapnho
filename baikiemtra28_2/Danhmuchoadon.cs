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
    public partial class Danhmuchoadon : Form
    {
        string connectionString = @"Data Source=LAPTOP-UJU060SC;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public Danhmuchoadon()
        {
            InitializeComponent();
        }

        private void Danhmuchoadon_Load(object sender, EventArgs e)
        {
            LoadDataHoaDon();
        }
       

        // Hàm tải dữ liệu
        private void LoadDataHoaDon()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Lấy dữ liệu và đổi tên cột hiển thị
                    string query = @"SELECT 
                                        MaHD AS N'Mã Hóa Đơn', 
                                        MaKH AS N'Mã Khách Hàng', 
                                        MaNV AS N'Mã Nhân Viên', 
                                        NgayLapHD AS N'Ngày Lập HĐ', 
                                        NgayNhanHang AS N'Ngày Nhận Hàng' 
                                     FROM HoaDon";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Hiển thị lên DataGridView
                    dgvHoaDon.DataSource = dt;

                    // Định dạng hiển thị ngày tháng
                    if (dgvHoaDon.Columns["Ngày Lập HĐ"] != null)
                        dgvHoaDon.Columns["Ngày Lập HĐ"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    if (dgvHoaDon.Columns["Ngày Nhận Hàng"] != null)
                        dgvHoaDon.Columns["Ngày Nhận Hàng"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    // Dàn đều các cột
                    dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
