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
    public partial class QuanLyNguoiDung : Form
    {
        string strConn = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        public QuanLyNguoiDung()
        {
            InitializeComponent();
        }

        private void QuanLyNguoiDung_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        void LoadData()
        {
            try
            {
                conn = new SqlConnection(strConn);
                string sql = "SELECT Username, Password FROM DangNhap";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNguoiDung.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                // Kiểm tra xem Username đã tồn tại chưa
                string checkSql = "SELECT COUNT(*) FROM DangNhap WHERE Username = @user";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@user", txtUsername.Text);
                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists > 0)
                {
                    MessageBox.Show("Tên đăng nhập này đã tồn tại!");
                }
                else
                {
                    string sql = "INSERT INTO DangNhap (Username, Password) VALUES (@user, @pass)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công!");
                    LoadData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            finally { conn.Close(); }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string sql = "UPDATE DangNhap SET Password = @pass WHERE Username = @user";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadData();
                }
                else { MessageBox.Show("Không tìm thấy người dùng để sửa!"); }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            finally { conn.Close(); }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    string sql = "DELETE FROM DangNhap WHERE Username = @user";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.ExecuteNonQuery();
                    LoadData();
                    MessageBox.Show("Xóa thành công!");
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
                finally { conn.Close(); }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM DangNhap WHERE Username LIKE @search";
                SqlDataAdapter da = new SqlDataAdapter(sql, strConn);
                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtTimKiem.Text + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNguoiDung.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tìm kiếm: " + ex.Message); }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtTimKiem.Clear();
            LoadData();
        }

        private void dgvNguoiDung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNguoiDung.Rows[e.RowIndex];
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
            }
        }
    }
}