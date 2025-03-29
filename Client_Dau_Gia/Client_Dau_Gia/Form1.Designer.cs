namespace Client_Dau_Gia
{
    partial class Form1
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtBid = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBid = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.stclient = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvBids = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBids)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(15, 160);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(285, 20);
            this.txtName.TabIndex = 0;
            // 
            // txtBid
            // 
            this.txtBid.Location = new System.Drawing.Point(15, 256);
            this.txtBid.Name = "txtBid";
            this.txtBid.Size = new System.Drawing.Size(282, 20);
            this.txtBid.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(313, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "Connect Server";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnBid
            // 
            this.btnBid.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBid.ForeColor = System.Drawing.Color.Red;
            this.btnBid.Location = new System.Drawing.Point(310, 200);
            this.btnBid.Name = "btnBid";
            this.btnBid.Size = new System.Drawing.Size(88, 55);
            this.btnBid.TabIndex = 3;
            this.btnBid.Text = "Đấu giá";
            this.btnBid.UseVisualStyleBackColor = false;
            this.btnBid.Click += new System.EventHandler(this.btnBid_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Yellow;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button3.Location = new System.Drawing.Point(309, 72);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 41);
            this.button3.TabIndex = 5;
            this.button3.Text = "END CONNECT";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // stclient
            // 
            this.stclient.BackColor = System.Drawing.Color.Aqua;
            this.stclient.Location = new System.Drawing.Point(12, 22);
            this.stclient.Name = "stclient";
            this.stclient.Size = new System.Drawing.Size(285, 73);
            this.stclient.TabIndex = 7;
            this.stclient.Text = "STATUS CLIENT :";
            this.stclient.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(57, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "NHẬP TÊN NGƯỜI ĐẦU GIÁ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(41, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(242, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "NHẬP SỐ TIỀN ĐẤU GIÁ (VND)";
            // 
            // dgvBids
            // 
            this.dgvBids.BackgroundColor = System.Drawing.Color.Bisque;
            this.dgvBids.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBids.Location = new System.Drawing.Point(121, 344);
            this.dgvBids.Name = "dgvBids";
            this.dgvBids.Size = new System.Drawing.Size(440, 197);
            this.dgvBids.TabIndex = 12;
            this.dgvBids.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBids_CellContentClick);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(161, 293);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 45);
            this.button2.TabIndex = 13;
            this.button2.Text = "LỊCH SỬ ĐẤU GIÁ";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnLoadBids_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Location = new System.Drawing.Point(418, 22);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(246, 237);
            this.pictureBox.TabIndex = 14;
            this.pictureBox.TabStop = false;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(572, 265);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(92, 53);
            this.button4.TabIndex = 15;
            this.button4.Text = "SELECT PICTURE";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Magenta;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(344, 293);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(165, 45);
            this.button5.TabIndex = 17;
            this.button5.Text = "XÓA LỊCH SỬ ĐẤU GIÁ";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.btnClearHistory_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(695, 558);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dgvBids);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stclient);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnBid);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtBid);
            this.Controls.Add(this.txtName);
            this.Name = "Form1";
            this.Text = "ĐẤU GIÁ SẢN PHẨM (Client)";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBids)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtBid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnBid;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox stclient;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvBids;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

