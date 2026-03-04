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
    public partial class Danhmucthanhpho : Form
    {
        string connectionString = @"Data Source=LAPTOP-UJU060SC;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public Danhmucthanhpho()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Danhmucthanhpho_Load(object sender, EventArgs e)
        {
            LoadDataThanhPho();
        }
        private void LoadDataThanhPho()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Lấy dữ liệu và đổi tên cột cho đẹp khi hiển thị
                    string query = "SELECT ThanhPho AS N'Mã Thành Phố', TenThanhPho AS N'Tên Thành Phố' FROM ThanhPho";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Đổ dữ liệu vào lưới
                    dgvThanhPho.DataSource = dt;

                    // Tự động căn chỉnh chiều rộng cột cho vừa với Form
                    dgvThanhPho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
