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
    public partial class DSnhomHĐtheoSP : Form
    {
        string connectionString = @"Data Source=DESKTOP-1IR372M\MSSQLSERVER01;Initial Catalog=QuanLyBanHang;Integrated Security=True";

        public DSnhomHĐtheoSP()
        {
            InitializeComponent();
        }

        private void DSnhomHĐtheoSP_Load(object sender, EventArgs e)
        {
            LoadComboBoxHoaDon();
            LoadComboBoxSanPham();
            HienThiDuLieu();
        }
        private void LoadComboBoxHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaHD FROM HoaDon", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow dr = dt.NewRow();
                dr["MaHD"] = "--- Chọn Hóa Đơn ---";
                dt.Rows.InsertAt(dr, 0);

                cboMaHoaDon.DataSource = dt;
                cboMaHoaDon.DisplayMember = "MaHD";
                cboMaHoaDon.ValueMember = "MaHD";
            }
        }

        private void LoadComboBoxSanPham()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaSP, TenSP FROM SanPham", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow dr = dt.NewRow();
                dr["MaSP"] = "";
                dr["TenSP"] = "--- Chọn Sản Phẩm ---";
                dt.Rows.InsertAt(dr, 0);

                cboSanPham.DataSource = dt;
                cboSanPham.DisplayMember = "TenSP";
                cboSanPham.ValueMember = "MaSP";
            }
        }

        private void HienThiDuLieu(string tuKhoaTimKiem = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        CT.MaHD AS [Mã Hóa Đơn], 
                        SP.TenSP AS [Tên Sản Phẩm], 
                        CT.SoLuong AS [Số Lượng],
                        SP.DonGia AS [Đơn Giá],
                        (CT.SoLuong * SP.DonGia) AS [Thành Tiền]
                    FROM ChiTietHoaDon CT
                    JOIN SanPham SP ON CT.MaSP = SP.MaSP
                    WHERE 1=1 ";

                // Lọc theo ComboBox Hóa Đơn
                if (cboMaHoaDon.SelectedValue != null && cboMaHoaDon.SelectedValue.ToString() != "--- Chọn Hóa Đơn ---")
                {
                    query += " AND CT.MaHD = @MaHD ";
                }

                // Lọc theo ComboBox Sản Phẩm
                if (cboSanPham.SelectedValue != null && cboSanPham.SelectedValue.ToString() != "")
                {
                    query += " AND CT.MaSP = @MaSP ";
                }

                // Lọc theo TextBox Tìm Kiếm tự do
                if (!string.IsNullOrEmpty(tuKhoaTimKiem))
                {
                    query += " AND (CT.MaHD LIKE @TuKhoa OR SP.TenSP LIKE @TuKhoa) ";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (cboMaHoaDon.SelectedValue != null && cboMaHoaDon.SelectedValue.ToString() != "--- Chọn Hóa Đơn ---")
                        cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.SelectedValue.ToString());

                    if (cboSanPham.SelectedValue != null && cboSanPham.SelectedValue.ToString() != "")
                        cmd.Parameters.AddWithValue("@MaSP", cboSanPham.SelectedValue.ToString());

                    if (!string.IsNullOrEmpty(tuKhoaTimKiem))
                        cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoaTimKiem + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDanhSach.DataSource = dt;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboMaHoaDon.SelectedIndex <= 0 || cboSanPham.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng chọn Hóa Đơn, Sản Phẩm và nhập Số Lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên dương!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong) VALUES (@MaHD, @MaSP, @SoLuong)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@MaSP", cboSanPham.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm chi tiết hóa đơn thành công!");
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thêm dữ liệu (Có thể sản phẩm này đã tồn tại trong hóa đơn): " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (cboMaHoaDon.SelectedIndex <= 0 || cboSanPham.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn mục cần sửa từ danh sách!", "Thông báo");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Khóa chính là (MaHD, MaSP) nên ta chỉ có thể cập nhật Số lượng
                    string query = "UPDATE ChiTietHoaDon SET SoLuong = @SoLuong WHERE MaHD = @MaHD AND MaSP = @MaSP";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@MaSP", cboSanPham.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SoLuong", int.Parse(txtSoLuong.Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật số lượng thành công!");
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
            if (cboMaHoaDon.SelectedIndex <= 0 || cboSanPham.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này khỏi hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM ChiTietHoaDon WHERE MaHD = @MaHD AND MaSP = @MaSP";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaHD", cboMaHoaDon.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@MaSP", cboSanPham.SelectedValue.ToString());

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Xóa thành công!");
                            btnLamMoi_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message);
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtSoLuong.Clear();
            txtTimKiem.Clear();
            cboMaHoaDon.SelectedIndex = 0;
            cboSanPham.SelectedIndex = 0;

            // Mở khóa lại ComboBox để có thể thêm mới
            cboMaHoaDon.Enabled = true;
            cboSanPham.Enabled = true;

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

        private void cboSanPham_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];

                cboMaHoaDon.Text = row.Cells["Mã Hóa Đơn"].Value.ToString();
                cboSanPham.Text = row.Cells["Tên Sản Phẩm"].Value.ToString();
                txtSoLuong.Text = row.Cells["Số Lượng"].Value.ToString();

                // Khóa 2 ComboBox lại vì Khóa chính (MaHD, MaSP) không được phép sửa đổi
                // Nếu muốn đổi sản phẩm khác, người dùng phải Xóa dòng này đi và Thêm dòng mới
                cboMaHoaDon.Enabled = false;
                cboSanPham.Enabled = false;
            }
        }

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
