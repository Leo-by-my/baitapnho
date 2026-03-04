using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baikiemtra28_2
{
    public partial class QlDanhmucthanhpho : Form
    {
        String ketnoi = "Data Source=TRUNG\\SQLEXPRESS;Database=quanlybanhang1: User Id=sa;Password=123456";
        // Đối tượng kết nối
        SqlConnection conn = null;
        // Đối tượng đưa dữ liệu vào DataTable dtTable
        SqlDataAdapter daThanhPho = null;
        // Đối tượng hiển thị dữ liệu trên Form
        DataTable dtThanhPho = null;
        bool Them;
        public QlDanhmucthanhpho()
        {
            InitializeComponent();
        }

        void LoadData()
        {
            try
            {
                // Khởi tạo connection
                conn = new SqlConnection(ketnoi);
                // Vận chuyển dữ liệu lên DataTabke dtThanhPho
                daThanhPho = new SqlDataAdapter("SELECT * THANHPHO", conn);
                dtThanhPho = new DataTable();
                dtThanhPho.Clear();
                daThanhPho.Fill(dtThanhPho);
                // Đưa dữ liệu lên DataGridView
                dgvThanhpho.DataSource = dtThanhPho;
                // Xóa trống các đổi tượng trong Panel
                this.txtThanhpho.ResetText();
                this.txtTenthanhpho.ResetText();
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
                MessageBox.Show("Không lấy được nội dung của table THANHPHO. Lỗi rồi!!!");
            }
        }

        private void QlDanhmucthanhpho_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Danhmucthanhpho_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtThanhPho.Dispose();
            dtThanhPho = null;
            // Hủy kết nối
            conn = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến Thêm
            Them = true;
            // Xóa trống các đối tượng trong Pane1
            this.txtThanhpho.ResetText();
            this.txtTenthanhpho.ResetText();
            // Cho thao tác trên các nút Lưu / Hủy / Pane1
            this.btnLuu.Enabled = true;
            this.btnHuybo.Enabled = true;
            this.panel1.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnThem.Enabled = false;
            // Đưa con trỏ đến TextField txtThanhPho
            this.txtThanhpho.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến Sửa
            Them = false;
            // Cho phép thao tác trên Panel
            this.panel1.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvThanhpho.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel
            this.txtThanhpho.Text = dgvThanhpho.Rows[r].Cells[0].Value.ToString();
            this.txtTenthanhpho.Text = dgvThanhpho.Rows[r].Cells[1].Value.ToString();
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
            this.txtThanhpho.Focus();
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
                int r = dgvThanhpho.CurrentCell.RowIndex;
                // Lấy MaKH của record hiện hành
                string strTHANHPHO = dgvThanhpho.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From ThanhPho Where ThanhPho = '" + strTHANHPHO + "'");
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
            this.txtThanhpho.ResetText();
            this.txtTenthanhpho.ResetText();
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
                    cmd.CommandText = System.String.Concat("Insert Into ThanhPho Values(" + "'" +
                        this.txtThanhpho.Text.ToString() + "',N" +
                        this.txtTenthanhpho.Text.ToString() + "')");
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
                int r = dgvThanhpho.CurrentCell.RowIndex;
                // MaKH hiện hành
                string strTHANHPHO = dgvThanhpho.Rows[r].Cells[0].ToString();
                // Câu lệnh SQL
                cmd.CommandText = System.String.Concat("Update ThanhPho Set TenThanhPho = '" +
                    this.txtTenthanhpho.Text.ToString() + "' Where ThanhPho = '" + strTHANHPHO + "'");
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


