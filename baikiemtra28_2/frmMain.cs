using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baikiemtra28_2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void kháchHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DSnhomKHtheoTP dSnhomKHtheoTP = new DSnhomKHtheoTP();
            dSnhomKHtheoTP.ShowDialog();
        }

        private void hóaĐơnTheoKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DMnhomHĐtheoKH dMnhomHĐtheoKH = new DMnhomHĐtheoKH();
            dMnhomHĐtheoKH.ShowDialog();
        }

        private void hóaĐơnTheoSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DSnhomHĐtheoSP dSnhomHĐtheoSP = new DSnhomHĐtheoSP();
            dSnhomHĐtheoSP.ShowDialog();
        }

        private void hóaĐơnTheoNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DSnhomHĐtheoNV dSnhomHĐtheoNV = new DSnhomHĐtheoNV();
            dSnhomHĐtheoNV.ShowDialog();
        }

        private void chiTiếtHóaĐơnTheoNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DSnhomCTHĐtheoNV dSnhomCTHĐtheoNV = new DSnhomCTHĐtheoNV();
            dSnhomCTHĐtheoNV .ShowDialog();

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void đăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DangNhap dangNhap = new DangNhap();
            dangNhap.ShowDialog();
        }

        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDoiMatKhau formDoiMatKhau = new FormDoiMatKhau();
            formDoiMatKhau.ShowDialog();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void quảnLýNgườiDùngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyNguoiDung quanLyNguoiDung = new QuanLyNguoiDung();
            quanLyNguoiDung.ShowDialog();
        }

        private void giúpĐỡToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GiupDo giupDo = new GiupDo();
            giupDo.ShowDialog();
        }
    }
}
