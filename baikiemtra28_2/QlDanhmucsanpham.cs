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
    public partial class QlDanhmucsanpham : Form
    {
        String ketnoi = "Data Source=TRUNG\\SQLEXPRESS;Database=quanlybanhang1: User Id=sa;Password=123456";
        // Đối tượng kết nối
        SqlConnection conn = null;
        // Đối tượng đưa dữ liệu vào DataTable dtTable
        SqlDataAdapter daSanPham = null;
        // Đối tượng hiển thị dữ liệu trên Form
        DataTable dtSanPham = null;
        bool Them;
        public QlDanhmucsanpham()
        {
            InitializeComponent();
        }

        void LoadData()
        {
            try
            {
                // Khởi tạo connection
                conn = new SqlConnection(ketnoi);
                // Vận chuyển dữ liệu lên DataTabke dtSanPham
                dtSanPham = new SqlDataAdapter("SELECT * SANPHAM", conn);
                dtSanPham = new DataTable();
                dtSanPham.Clear();
                daSanPham.Fill(dtSanPham);
                // Đưa dữ liệu lên DataGridView
                dgvSanpham.DataSource = dtSanPham;
                // Xóa trống các đổi tượng trong Panel
                this.txtSanpham.ResetText();
                this.txtTensanpham.ResetText();
                // Không cho thao tác trên các nút Lưu / Hủy
                this.btnLuu.Enabled = false;
                this.btnHuybo.Enabled = false;
                this.panel1.Enabled = false;
                // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
                this.btnThem.Enabled = true;
                this.btnSua.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnThoat.Enabled = true;
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung của table SANPHAM. Lỗi rồi!!!");
            }
        }

        private void QlDanhmucsanpham_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Danhmucsanpham_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtSanPham.Dispose();
            dtSanPham = null;
            // Hủy kết nối
            conn = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến Thêm
            Them = true;
            // Xóa trống các đối tượng trong Pane1
            this.txtSanpham.ResetText();
            this.txtTensanpham.ResetText();
            // Cho thao tác trên các nút Lưu / Hủy / Pane1
            this.btnLuu.Enabled = true;
            this.btnHuybo.Enabled = true;
            this.panel1.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnThem.Enabled = false;
            // Đưa con trỏ đến TextField txtSanPham
            this.txtSanpham.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến Sửa
            Them = false;
            // Cho phép thao tác trên Panel
            this.panel1.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvSanpham.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel
            this.txtSanpham.Text = dgvSanpham.Rows[r].Cells[0].Value.ToString();
            this.txtTensanpham.Text = dgvSanpham.Rows[r].Cells[1].Value.ToString();
            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuybo.Enabled = true;
            this.panel1.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnThoat.Enabled = false;
            // Đưa con trỏ đến TextField txtMaKH
            this.txtSanpham.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            try
            {
                // Thực hiện lệnh
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                // Lấy thứ tự recard hiện hành
                int r = dgvSanpham.CurrentCell.RowIndex;
                // Lấy MaKH của record hiện hành
                string strSANPHAM = dgvSanpham.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From SanPham Where SanPham = '" + strSANPHAM + "'");
                cmd.CommandType = CommandType.Text;
                // Thực hiện câu lệnh SQL
                cmd.ExecuteNonQuery();
                // Cập nhật lại DataGridView
                LoadData();
                // Thông  báo
                MessageBox.Show("Đã xóa xong!");
            }
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!");
            }
        }

        private void btnHuybo_Click(object sender, EventArgs e)
        {
            // Xóa trống các đối tượng trong Panel
            this.txtSanpham.ResetText();
            this.txtTensanpham.ResetText();
            // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnThem.Enabled = true;
            this.btnSua.Enabled = true;
            this.btnXoa.Enabled = true;
            this.btnThoat.Enabled = true;
            // Không cho thao tác trên các nút Lưu / Hủy / panel
            this.btnLuu.Enabled = false;
            this.btnHuybo.Enabled = false;
            this.panel1.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            // Thêm dữ liệu
            if (Them)
            {
                try
                {
                    // Thực hiện lệnh
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    // Lệnh Insert Into
                    cmd.CommandText = System.String.Concat("Insert Into SanPham Values(" + "'" +
                        this.txtSanpham.Text.ToString() + "',N" +
                        this.txtTensanpham.Text.ToString() + "')");
                    cmd.ExecuteNonQuery();
                    // Load lại dữ liệu trên DataGridView
                    LoadData();
                    // Thông báo
                    MessageBox.Show("Đã thêm xong!");
                }
                catch (SqlException)
                {
                    MessageBox.Show("Không thêm được. Lỗi rồi!");
                }
            }
            if (!Them)
            {
                // Thực hiện lệnh
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                // Thứ tự dòng hiện hành
                int r = dgvSanpham.CurrentCell.RowIndex;
                // MaKH hiện hành
                string strSANPHAM = dgvSanpham.Rows[r].Cells[0].ToString();
                // Câu lệnh SQL
                cmd.CommandText = System.String.Concat("Update SanPham Set TenSanPham = '" +
                    this.txtTensanpham.Text.ToString() + "' Where SanPham = '" + strSANPHAM + "'");
                // Cập nhật
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                // Load lại dữ liệu trên DataGridView
                LoadData();
                // Thông báo
                MessageBox.Show("Đã sửa xong!");
            }
            // Đóng kết nối
            conn.Close();
        }
    }
}

