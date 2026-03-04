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
    public partial class DMnhomHĐtheoKH : Form
    {
        string connectionString = @"Data Source=DESKTOP-1IR372M\MSSQLSERVER01;Initial Catalog=QuanLyBanHang;Integrated Security=True";

        public DMnhomHĐtheoKH()
        {
            InitializeComponent();
        }

        private void DMnhomHĐtheoKH_Load(object sender, EventArgs e)
        {
            LoadComboBoxMaHoaDon();
            LoadComboBoxKhachHang();
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

        private void LoadComboBoxKhachHang()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaKH, TenCty FROM KhachHang", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow dr = dt.NewRow();
                dr["MaKH"] = "";
                dr["TenCty"] = "--- Chọn Khách Hàng ---";
                dt.Rows.InsertAt(dr, 0);

                cboKhachHang.DataSource = dt;
                cboKhachHang.DisplayMember = "TenCty";
                cboKhachHang.ValueMember = "MaKH";
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
                        KH.TenCty AS [Khách Hàng],
                        HD.MaNV AS [Mã Nhân Viên Lập],
                        HD.NgayLapHD AS [Ngày Lập]
                    FROM HoaDon HD
                    LEFT JOIN KhachHang KH ON HD.MaKH = KH.MaKH
                    WHERE 1=1 ";

                // Lọc theo ComboBox Mã Hóa Đơn
                if (cboMaHoaDon.SelectedValue != null && cboMaHoaDon.SelectedValue.ToString() != "")
                {
                    query += " AND HD.MaHD = @MaHD ";
                }

                // Lọc theo ComboBox Khách Hàng
                if (cboKhachHang.SelectedValue != null && cboKhachHang.SelectedValue.ToString() != "")
                {
                    query += " AND HD.MaKH = @MaKH ";
                }

                // Lọc theo ô Tìm kiếm
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query += " AND (HD.MaHD LIKE @TuKhoa OR KH.TenCty LIKE @TuKhoa OR HD.MaNV LIKE @TuKhoa) ";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (cboMaHoaDon.SelectedValue != null && cboMaHoaDon.SelectedValue.ToString() != "")
                        cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.SelectedValue.ToString());

                    if (cboKhachHang.SelectedValue != null && cboKhachHang.SelectedValue.ToString() != "")
                        cmd.Parameters.AddWithValue("@MaKH", cboKhachHang.SelectedValue.ToString());

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
            if (string.IsNullOrWhiteSpace(cboMaHoaDon.Text) || cboKhachHang.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã Hóa Đơn, chọn Khách Hàng và nhập Mã Nhân Viên (vào ô Mã QL)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        cmd.Parameters.AddWithValue("@MaKH", cboKhachHang.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm hóa đơn thành công!");

                        LoadComboBoxMaHoaDon(); // Cập nhật lại danh sách Mã HD
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: Mã Hóa Đơn đã tồn tại hoặc Mã Nhân Viên không hợp lệ!\nChi tiết: " + ex.Message, "Lỗi SQL");
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách để sửa!", "Thông báo");
                return;
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
                        cmd.Parameters.AddWithValue("@MaKH", cboKhachHang.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật thông tin hóa đơn thành công!");
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Xóa hóa đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                            LoadComboBoxMaHoaDon();
                            btnLamMoi_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: Hóa đơn này đang có sản phẩm bên trong Chi Tiết Hóa Đơn, không thể xóa!", "Lỗi SQL");
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtTimKiem.Clear();

            cboMaHoaDon.Text = "";
            cboMaHoaDon.SelectedIndex = 0;
            cboKhachHang.SelectedIndex = 0;

            // Mở khóa lại
            cboMaHoaDon.Enabled = true;

            HienThiDuLieu();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            HienThiDuLieu(txtTimKiem.Text.Trim());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboMaHoaDon_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void cboKhachHang_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];

                cboMaHoaDon.Text = row.Cells["Mã Hóa Đơn"].Value?.ToString();
                cboKhachHang.Text = row.Cells["Khách Hàng"].Value?.ToString();
                txtMaNV.Text = row.Cells["Mã Nhân Viên Lập"].Value?.ToString();

                // Khóa lại không cho sửa Mã Hóa Đơn (Khóa Chính) khi đang chọn để Sửa/Xóa
                cboMaHoaDon.Enabled = false;
            }
        }
    }
}
