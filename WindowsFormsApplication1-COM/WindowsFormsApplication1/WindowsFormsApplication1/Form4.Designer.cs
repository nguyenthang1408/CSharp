namespace WindowsFormsApplication1
{
    partial class Form4
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
            this.txt_empCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_confirm = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_empCode
            // 
            this.txt_empCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_empCode.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_empCode.Location = new System.Drawing.Point(322, 190);
            this.txt_empCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txt_empCode.Multiline = true;
            this.txt_empCode.Name = "txt_empCode";
            this.txt_empCode.ReadOnly = true;
            this.txt_empCode.Size = new System.Drawing.Size(324, 59);
            this.txt_empCode.TabIndex = 0;
            this.txt_empCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Cornsilk;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 25.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(139, 95);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(591, 41);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vui lòng quét mã nhân viên để bắt đầu !";
            // 
            // lb_confirm
            // 
            this.lb_confirm.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lb_confirm.AutoSize = true;
            this.lb_confirm.BackColor = System.Drawing.Color.Cornsilk;
            this.lb_confirm.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_confirm.Location = new System.Drawing.Point(192, 297);
            this.lb_confirm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_confirm.Name = "lb_confirm";
            this.lb_confirm.Size = new System.Drawing.Size(0, 43);
            this.lb_confirm.TabIndex = 2;
            this.lb_confirm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(147, 204);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 43);
            this.label2.TabIndex = 0;
            this.label2.Text = "MÃ NV:";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.ClientSize = new System.Drawing.Size(819, 518);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_confirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_empCode);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form4";
            this.Text = "Nhân viên quét mã";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form4_FormClosed);
            this.Load += new System.EventHandler(this.Form4_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txt_empCode;
        private System.Windows.Forms.Label lb_confirm;
        private System.Windows.Forms.Label label2;
    }
}