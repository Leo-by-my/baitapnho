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
    public partial class DSnhomCTHĐtheoNV : Form
    {
        string connectionString = @"Data Source=DESKTOP-1IR372M\MSSQLSERVER01;Initial Catalog=QuanLyBanHang;Integrated Security=True";

        public DSnhomCTHĐtheoNV()
        {
            InitializeComponent();
        }

        private void DSnhomCTHĐtheoNV_Load(object sender, EventArgs e)
        {
            LoadComboBoxSanPham();
            LoadComboBoxNhanVien();
            HienThiDuLieu();
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

                // Gán vào ComboBox "Chi Tiết Hóa Đơn" (Đóng vai trò chọn Sản Phẩm)
                cboChiTietHoaDon.DataSource = dt;
                cboChiTietHoaDon.DisplayMember = "TenSP";
                cboChiTietHoaDon.ValueMember = "MaSP";
            }
        }

        private void LoadComboBoxNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
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
                // Truy vấn kết hợp 4 bảng: ChiTietHoaDon, HoaDon, SanPham, NhanVien
                string query = @"
                    SELECT 
                        CT.MaHD AS [Mã Hóa Đơn], 
                        SP.TenSP AS [Sản Phẩm], 
                        CT.SoLuong AS [Số Lượng],
                        (NV.Ho + ' ' + NV.Ten) AS [Nhân Viên Lập],
                        HD.NgayLapHD AS [Ngày Lập]
                    FROM ChiTietHoaDon CT
                    JOIN HoaDon HD ON CT.MaHD = HD.MaHD
                    JOIN SanPham SP ON CT.MaSP = SP.MaSP
                    JOIN NhanVien NV ON HD.MaNV = NV.MaNV
                    WHERE 1=1 ";

                // Lọc theo Sản Phẩm
                if (cboChiTietHoaDon.SelectedValue != null && cboChiTietHoaDon.SelectedValue.ToString() != "")
                {
                    query += " AND CT.MaSP = @MaSP ";
                }

                // Lọc theo Nhân Viên
                if (cboNhanVien.SelectedValue != null && cboNhanVien.SelectedValue.ToString() != "")
                {
                    query += " AND HD.MaNV = @MaNV ";
                }

                // Tìm kiếm tự do
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query += " AND (CT.MaHD LIKE @TuKhoa OR SP.TenSP LIKE @TuKhoa) ";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (cboChiTietHoaDon.SelectedValue != null && cboChiTietHoaDon.SelectedValue.ToString() != "")
                        cmd.Parameters.AddWithValue("@MaSP", cboChiTietHoaDon.SelectedValue.ToString());

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
            if (string.IsNullOrWhiteSpace(txtMaQL.Text) || cboChiTietHoaDon.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã Hóa Đơn, chọn Sản Phẩm và nhập Số Lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem số lượng nhập vào có phải là số nguyên dương không
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Lưu ý: Mã Hóa Đơn (MaHD) phải tồn tại trong bảng HoaDon trước thì mới thêm Chi Tiết Hóa Đơn được
                    string query = "INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong) VALUES (@MaHD, @MaSP, @SoLuong)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", txtMaQL.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaSP", cboChiTietHoaDon.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong); // Dùng số lượng từ textbox

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm chi tiết hóa đơn thành công!");
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: Mã hóa đơn chưa tồn tại hoặc sản phẩm này đã có trong hóa đơn!\nChi tiết: " + ex.Message, "Lỗi SQL");
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaQL.Text) || cboChiTietHoaDon.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng dữ liệu từ danh sách bên dưới để sửa!", "Thông báo");
                return;
            }

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Trong bảng ChiTietHoaDon, Khóa chính là (MaHD, MaSP) nên ta chỉ cập nhật cột SoLuong
                    string query = "UPDATE ChiTietHoaDon SET SoLuong = @SoLuong WHERE MaHD = @MaHD AND MaSP = @MaSP";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", txtMaQL.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaSP", cboChiTietHoaDon.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật số lượng thành công!");
                            btnLamMoi_Click(sender, e);
                        }
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
            if (string.IsNullOrWhiteSpace(txtMaQL.Text) || cboChiTietHoaDon.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng dữ liệu bên dưới để xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này khỏi hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM ChiTietHoaDon WHERE MaHD = @MaHD AND MaSP = @MaSP";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaHD", txtMaQL.Text.Trim());
                            cmd.Parameters.AddWithValue("@MaSP", cboChiTietHoaDon.SelectedValue.ToString());

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Xóa thành công!");
                            btnLamMoi_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi");
                    }
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            HienThiDuLieu(txtTimKiem.Text.Trim());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboChiTietHoaDon_SelectionChangeCommitted(object sender, EventArgs e)
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

                txtMaQL.Text = row.Cells["Mã Hóa Đơn"].Value?.ToString();
                cboChiTietHoaDon.Text = row.Cells["Sản Phẩm"].Value?.ToString();
                txtSoLuong.Text = row.Cells["Số Lượng"].Value?.ToString(); // Đổ dữ liệu lên TextBox Số Lượng
                cboNhanVien.Text = row.Cells["Nhân Viên Lập"].Value?.ToString();

                // Khóa lại không cho sửa Khóa Chính (Mã HD & Mã SP) khi đang chọn dữ liệu để Sửa hoặc Xóa
                txtMaQL.Enabled = false;
                cboChiTietHoaDon.Enabled = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaQL.Clear();
            txtSoLuong.Clear(); // Xóa rỗng ô Số Lượng
            txtTimKiem.Clear();

            cboChiTietHoaDon.SelectedIndex = 0;
            cboNhanVien.SelectedIndex = 0;

            // Mở khóa lại các ô để cho phép thêm mới
            txtMaQL.Enabled = true;
            cboChiTietHoaDon.Enabled = true;

            HienThiDuLieu();
        }
    }
}
