namespace Phong_Tro_GUI
{
    partial class TrangChuMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupSoDo;
        private System.Windows.Forms.Label lblHanhLang;
        private System.Windows.Forms.Panel panelLegend;
        private System.Windows.Forms.Label lblTrong;
        private System.Windows.Forms.Label lblThue;
        private System.Windows.Forms.Label lblSua;
        private System.Windows.Forms.Panel colorTrong;
        private System.Windows.Forms.Panel colorThue;
        private System.Windows.Forms.Panel colorSua;

        /// <summary>
        /// Giải phóng tài nguyên
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupSoDo = new System.Windows.Forms.GroupBox();
            this.lblHanhLang = new System.Windows.Forms.Label();
            this.panelLegend = new System.Windows.Forms.Panel();
            this.colorTrong = new System.Windows.Forms.Panel();
            this.colorThue = new System.Windows.Forms.Panel();
            this.colorSua = new System.Windows.Forms.Panel();
            this.lblTrong = new System.Windows.Forms.Label();
            this.lblThue = new System.Windows.Forms.Label();
            this.lblSua = new System.Windows.Forms.Label();
            this.groupSoDo.SuspendLayout();
            this.panelLegend.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(198, 233, 227);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(800, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Sơ đồ phòng trọ";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupSoDo
            // 
            this.groupSoDo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSoDo.Controls.Add(this.lblHanhLang);
            this.groupSoDo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupSoDo.Location = new System.Drawing.Point(20, 60);
            this.groupSoDo.Name = "groupSoDo";
            this.groupSoDo.Size = new System.Drawing.Size(760, 360);
            this.groupSoDo.TabIndex = 1;
            this.groupSoDo.TabStop = false;
            this.groupSoDo.Text = "Sơ đồ dãy phòng";
            // 
            // lblHanhLang
            // 
            this.lblHanhLang.BackColor = System.Drawing.Color.LightGray;
            this.lblHanhLang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblHanhLang.ForeColor = System.Drawing.Color.DimGray;
            this.lblHanhLang.Location = new System.Drawing.Point(50, 160);
            this.lblHanhLang.Name = "lblHanhLang";
            this.lblHanhLang.Size = new System.Drawing.Size(660, 40);
            this.lblHanhLang.TabIndex = 0;
            this.lblHanhLang.Text = "Hành lang";
            this.lblHanhLang.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelLegend
            // 
            this.panelLegend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLegend.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelLegend.Controls.Add(this.colorTrong);
            this.panelLegend.Controls.Add(this.lblTrong);
            this.panelLegend.Controls.Add(this.colorThue);
            this.panelLegend.Controls.Add(this.lblThue);
            this.panelLegend.Controls.Add(this.colorSua);
            this.panelLegend.Controls.Add(this.lblSua);
            this.panelLegend.Location = new System.Drawing.Point(20, 430);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(400, 40);
            this.panelLegend.TabIndex = 2;
            // 
            // colorTrong
            // 
            this.colorTrong.BackColor = System.Drawing.Color.LightGreen;
            this.colorTrong.Location = new System.Drawing.Point(10, 10);
            this.colorTrong.Size = new System.Drawing.Size(20, 20);
            // 
            // lblTrong
            // 
            this.lblTrong.Location = new System.Drawing.Point(35, 10);
            this.lblTrong.Size = new System.Drawing.Size(60, 20);
            this.lblTrong.Text = "Trống";
            // 
            // colorThue
            // 
            this.colorThue.BackColor = System.Drawing.Color.LightCoral;
            this.colorThue.Location = new System.Drawing.Point(110, 10);
            this.colorThue.Size = new System.Drawing.Size(20, 20);
            // 
            // lblThue
            // 
            this.lblThue.Location = new System.Drawing.Point(135, 10);
            this.lblThue.Size = new System.Drawing.Size(70, 20);
            this.lblThue.Text = "Đang thuê";
            // 
            // colorSua
            // 
            this.colorSua.BackColor = System.Drawing.Color.Khaki;
            this.colorSua.Location = new System.Drawing.Point(220, 10);
            this.colorSua.Size = new System.Drawing.Size(20, 20);
            // 
            // lblSua
            // 
            this.lblSua.Location = new System.Drawing.Point(245, 10);
            this.lblSua.Size = new System.Drawing.Size(70, 20);
            this.lblSua.Text = "Đang sửa";
            // 
            // TrangChuMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelLegend);
            this.Controls.Add(this.groupSoDo);
            this.Controls.Add(this.lblTitle);
            this.Name = "TrangChuMain";
            this.Size = new System.Drawing.Size(800, 480);
            this.groupSoDo.ResumeLayout(false);
            this.panelLegend.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
