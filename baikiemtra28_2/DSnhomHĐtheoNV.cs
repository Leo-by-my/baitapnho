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
    public partial class DSnhomHĐtheoNV : Form
    {
        string connectionString = @"Data Source=DESKTOP-1IR372M\MSSQLSERVER01;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public DSnhomHĐtheoNV()
        {
            InitializeComponent();
        }

        private void DSnhomHĐtheoNV_Load(object sender, EventArgs e)
        {
            LoadComboBoxMaHoaDon();
            LoadComboBoxNhanVien();
            HienThiDuLieu();
        }
        private void LoadComboBoxMaHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaHD FROM HoaDon", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow dr = dt.NewRow();
                dr["MaHD"] = ""; // Dòng trống để bỏ lọc
                dt.Rows.InsertAt(dr, 0);

                cboMaHoaDon.DataSource = dt;
                cboMaHoaDon.DisplayMember = "MaHD";
                cboMaHoaDon.ValueMember = "MaHD";
            }
        }

        private void LoadComboBoxNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Nối cột Họ và Tên lại với nhau để hiển thị
                string query = "SELECT MaNV, (Ho + ' ' + Ten) AS HoTen FROM NhanVien";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow dr = dt.NewRow();
                dr["MaNV"] = "";
                dr["HoTen"] = "--- Chọn Nhân Viên ---";
                dt.Rows.InsertAt(dr, 0);

                cboNhanVien.DataSource = dt;
                cboNhanVien.DisplayMember = "HoTen";
                cboNhanVien.ValueMember = "MaNV";
            }
        }

        // ================= HIỂN THỊ VÀ LỌC DỮ LIỆU =================

        private void HienThiDuLieu(string tuKhoa = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        HD.MaHD AS [Mã Hóa Đơn], 
                        HD.MaKH AS [Mã KH],
                        (NV.Ho + ' ' + NV.Ten) AS [Nhân Viên Phụ Trách],
                        HD.NgayLapHD AS [Ngày Lập Hóa Đơn]
                    FROM HoaDon HD
                    LEFT JOIN NhanVien NV ON HD.MaNV = NV.MaNV
                    WHERE 1=1 ";

                // Lọc theo ComboBox Mã Hóa Đơn
                if (cboMaHoaDon.SelectedValue != null && cboMaHoaDon.SelectedValue.ToString() != "")
                {
                    query += " AND HD.MaHD = @MaHD ";
                }

                // Lọc theo ComboBox Nhân Viên
                if (cboNhanVien.SelectedValue != null && cboNhanVien.SelectedValue.ToString() != "")
                {
                    query += " AND HD.MaNV = @MaNV ";
                }

                // Lọc theo ô Tìm kiếm tự do
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query += " AND (HD.MaHD LIKE @TuKhoa OR NV.Ten LIKE @TuKhoa OR HD.MaKH LIKE @TuKhoa) ";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (cboMaHoaDon.SelectedValue != null && cboMaHoaDon.SelectedValue.ToString() != "")
                        cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.SelectedValue.ToString());

                    if (cboNhanVien.SelectedValue != null && cboNhanVien.SelectedValue.ToString() != "")
                        cmd.Parameters.AddWithValue("@MaNV", cboNhanVien.SelectedValue.ToString());

                    if (!string.IsNullOrEmpty(tuKhoa))
                        cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDanhSach.DataSource = dt;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboMaHoaDon.Text) || cboNhanVien.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng nhập Mã Hóa Đơn và chọn Nhân Viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO HoaDon (MaHD, MaKH, MaNV, NgayLapHD) VALUES (@MaHD, @MaKH, @MaNV, GETDATE())";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaKH", txtMaQL.Text.Trim()); // Vẫn dùng txtMaQL làm Mã KH
                        cmd.Parameters.AddWithValue("@MaNV", cboNhanVien.SelectedValue.ToString());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm hóa đơn thành công!");

                        LoadComboBoxMaHoaDon(); // Tải lại danh sách mã hóa đơn
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thêm dữ liệu (Mã Hóa Đơn có thể đã tồn tại): " + ex.Message);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo"); return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE HoaDon SET MaKH = @MaKH, MaNV = @MaNV WHERE MaHD = @MaHD";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaKH", txtMaQL.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaNV", cboNhanVien.SelectedValue.ToString());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sửa thông tin hóa đơn thành công!");
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật: " + ex.Message);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xóa!", "Thông báo"); return;
            }

            if (MessageBox.Show("Xóa hóa đơn này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM HoaDon WHERE MaHD = @MaHD";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.Text.Trim());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Xóa thành công!");

                            LoadComboBoxMaHoaDon(); // Tải lại danh sách mã HD
                            btnLamMoi_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa. Hóa đơn này đang có sản phẩm bên trong Chi Tiết Hóa Đơn!", "Lỗi");
                    }
                }
            }
        }

        

        private void btnTim_Click(object sender, EventArgs e)
        {
            HienThiDuLieu(txtTimKiem.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboMaHoaDon_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void cboNhanVien_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];

                cboMaHoaDon.Text = row.Cells["Mã Hóa Đơn"].Value?.ToString();
                txtMaQL.Text = row.Cells["Mã KH"].Value?.ToString();
                cboNhanVien.Text = row.Cells["Nhân Viên Phụ Trách"].Value?.ToString();

                cboMaHoaDon.Enabled = false; // Khóa Mã Hóa Đơn lại, không cho sửa khi đang thao tác cập nhật
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaQL.Clear();
            cboMaHoaDon.SelectedIndex = 0; // Đưa về dòng trống
            cboMaHoaDon.Text = "";
            cboMaHoaDon.Enabled = true;    // Mở khóa lại cho phép nhập mã mới
            txtTimKiem.Clear();
            cboNhanVien.SelectedIndex = 0;
            HienThiDuLieu();
        }

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
