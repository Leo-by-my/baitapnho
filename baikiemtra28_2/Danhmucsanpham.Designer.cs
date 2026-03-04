namespace baikiemtra28_2
{
    partial class Danhmucsanpham
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTrove = new System.Windows.Forms.Button();
            this.dgvSanPham = new System.Windows.Forms.DataGridView();
            this.lblDanhmuc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTrove
            // 
            this.btnTrove.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrove.Location = new System.Drawing.Point(341, 300);
            this.btnTrove.Name = "btnTrove";
            this.btnTrove.Size = new System.Drawing.Size(126, 44);
            this.btnTrove.TabIndex = 5;
            this.btnTrove.Text = "Trở về";
            this.btnTrove.UseVisualStyleBackColor = true;
            this.btnTrove.Click += new System.EventHandler(this.btnTrove_Click);
            // 
            // dgvSanPham
            // 
            this.dgvSanPham.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPham.Location = new System.Drawing.Point(65, 95);
            this.dgvSanPham.Name = "dgvSanPham";
            this.dgvSanPham.RowHeadersWidth = 51;
            this.dgvSanPham.RowTemplate.Height = 24;
            this.dgvSanPham.Size = new System.Drawing.Size(402, 178);
            this.dgvSanPham.TabIndex = 4;
            // 
            // lblDanhmuc
            // 
            this.lblDanhmuc.AutoSize = true;
            this.lblDanhmuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDanhmuc.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblDanhmuc.Location = new System.Drawing.Point(113, 31);
            this.lblDanhmuc.Name = "lblDanhmuc";
            this.lblDanhmuc.Size = new System.Drawing.Size(306, 32);
            this.lblDanhmuc.TabIndex = 3;
            this.lblDanhmuc.Text = "Danh Mục Sản  Phẩm\r\n";
            // 
            // Danhmucsanpham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 412);
            this.Controls.Add(this.btnTrove);
            this.Controls.Add(this.dgvSanPham);
            this.Controls.Add(this.lblDanhmuc);
            this.Name = "Danhmucsanpham";
            this.Text = "Danhmucsanpham";
            this.Load += new System.EventHandler(this.Danhmucsanpham_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrove;
        private System.Windows.Forms.DataGridView dgvSanPham;
        private System.Windows.Forms.Label lblDanhmuc;
    }
}