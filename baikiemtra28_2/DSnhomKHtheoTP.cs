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
    public partial class DSnhomKHtheoTP : Form
    {
        string connectionString = @"Data Source=DESKTOP-1IR372M\MSSQLSERVER01;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        public DSnhomKHtheoTP()
        {
            InitializeComponent();
        }

        private void DSnhomKHtheoTP_Load(object sender, EventArgs e)
        {
            HienThiDuLieu();
            LoadComboBoxThanhPho();
            LoadComboBoxKhachHang();
        }
        private void LoadComboBoxThanhPho()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT ThanhPho, TenThanhPho FROM ThanhPho", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Thêm dòng "Tất cả" để bỏ lọc
                DataRow dr = dt.NewRow();
                dr["ThanhPho"] = "";
                dr["TenThanhPho"] = "--- Chọn Thành Phố ---";
                dt.Rows.InsertAt(dr, 0);

                cboThanhPho.DataSource = dt;
                cboThanhPho.DisplayMember = "TenThanhPho";
                cboThanhPho.ValueMember = "ThanhPho"; // Lưu mã thành phố
            }
        }

        // 2. ĐỔ DỮ LIỆU TỪ SQL VÀO COMBOBOX KHÁCH HÀNG
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
        private void HienThiDuLieu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT KH.MaKH, KH.TenCty, KH.DiaChi, TP.TenThanhPho, KH.DienThoai
                FROM KhachHang KH
                INNER JOIN ThanhPho TP ON KH.ThanhPho = TP.ThanhPho
                WHERE 1=1 ";

                    // Lấy giá trị đang chọn từ ComboBox
                    string maTP = cboThanhPho.SelectedValue?.ToString();
                    string maKH = cboKhachHang.SelectedValue?.ToString();

                    // Nếu người dùng chọn một mục cụ thể (khác rỗng/khác "Tất cả")
                    if (!string.IsNullOrEmpty(maTP))
                    {
                        query += " AND KH.ThanhPho = @MaTP ";
                    }
                    if (!string.IsNullOrEmpty(maKH))
                    {
                        query += " AND KH.MaKH = @MaKH ";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(maTP)) cmd.Parameters.AddWithValue("@MaTP", maTP);
                        if (!string.IsNullOrEmpty(maKH)) cmd.Parameters.AddWithValue("@MaKH", maKH);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvDanhSach.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message);
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaQL.Clear();
            txtMaQL.Enabled = true; // Mở khóa lại ô Mã QL
            txtTimKiem.Clear();
            cboThanhPho.SelectedIndex = 0;
            cboKhachHang.SelectedIndex = 0;
            HienThiDuLieu();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];

                txtMaQL.Text = row.Cells["Mã QL"].Value.ToString();
                cboKhachHang.Text = row.Cells["Tên Khách Hàng"].Value.ToString();

                // Tìm vị trí tương ứng trong cboThanhPho để gán
                cboThanhPho.SelectedIndex = cboThanhPho.FindStringExact(row.Cells["Thành Phố"].Value.ToString());

                txtMaQL.Enabled = false; // Khóa Mã QL không cho sửa khi click vào danh sách
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaQL.Text) || cboThanhPho.SelectedValue == null || cboThanhPho.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Vui lòng nhập Mã QL và chọn Thành Phố!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Lưu ý: Giao diện không có Địa Chỉ và Điện Thoại nên ta chỉ Insert 3 trường có trên form
                    string query = "INSERT INTO KhachHang (MaKH, TenCty, ThanhPho) VALUES (@MaKH, @TenCty, @ThanhPho)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", txtMaQL.Text.Trim());
                        // Dùng .Text để lấy tên khách hàng (cho phép gõ tên mới vào ComboBox)
                        cmd.Parameters.AddWithValue("@TenCty", cboKhachHang.Text.Trim());
                        cmd.Parameters.AddWithValue("@ThanhPho", cboThanhPho.SelectedValue.ToString());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm thành công!");

                        LoadComboBoxKhachHang(); // Cập nhật lại danh sách KH
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaQL.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa!", "Thông báo"); return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE KhachHang SET TenCty = @TenCty, ThanhPho = @ThanhPho WHERE MaKH = @MaKH";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", txtMaQL.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenCty", cboKhachHang.Text.Trim());
                        cmd.Parameters.AddWithValue("@ThanhPho", cboThanhPho.SelectedValue.ToString());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sửa thành công!");

                        LoadComboBoxKhachHang();
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaQL.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!", "Thông báo"); return;
            }

            if (MessageBox.Show("Xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM KhachHang WHERE MaKH = @MaKH";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaKH", txtMaQL.Text.Trim());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Xóa thành công!");

                            LoadComboBoxKhachHang();
                            btnLamMoi_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa. Khách hàng này có thể đã có hóa đơn!", "Lỗi");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn thoát chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close(); 
            }
        }

        private void cboKhachHang_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void cboThanhPho_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }
    }
}
