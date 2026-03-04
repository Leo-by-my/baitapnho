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
    public partial class Danhmucsanpham : Form
    {
        string connectionString = @"Data Source=LAPTOP-UJU060SC;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public Danhmucsanpham()
        {
            InitializeComponent();
        }

        private void btnTrove_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng Form quay về màn hình trước đó
        }

        private void Danhmucsanpham_Load(object sender, EventArgs e)
        {
            LoadDataSanPham();
        }
        private void LoadDataSanPham()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Lấy toàn bộ dữ liệu từ bảng SanPham như trong SQL
                    string query = "SELECT * FROM SanPham";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Hiển thị lên lưới dữ liệu
                    dgvSanPham.DataSource = dt;

                    // Căn đều các cột cho vừa khung hình
                    dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
