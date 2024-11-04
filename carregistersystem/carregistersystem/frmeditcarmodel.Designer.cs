namespace carregistersystem
{
    partial class frmEditCarModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditCarModel));
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmbManuf = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbDiesel = new System.Windows.Forms.RadioButton();
            this.rbPetrol = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtVIN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefreshVIN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(79, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(137, 20);
            this.txtName.TabIndex = 1;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // cmbManuf
            // 
            this.cmbManuf.FormattingEnabled = true;
            this.cmbManuf.Location = new System.Drawing.Point(79, 78);
            this.cmbManuf.Name = "cmbManuf";
            this.cmbManuf.Size = new System.Drawing.Size(137, 21);
            this.cmbManuf.TabIndex = 2;
            this.cmbManuf.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbManuf_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Car manufacturer ";
            // 
            // rbDiesel
            // 
            this.rbDiesel.AutoSize = true;
            this.rbDiesel.Checked = true;
            this.rbDiesel.Location = new System.Drawing.Point(81, 195);
            this.rbDiesel.Name = "rbDiesel";
            this.rbDiesel.Size = new System.Drawing.Size(54, 17);
            this.rbDiesel.TabIndex = 6;
            this.rbDiesel.Text = "Diesel";
            this.rbDiesel.UseVisualStyleBackColor = true;
            // 
            // rbPetrol
            // 
            this.rbPetrol.AutoSize = true;
            this.rbPetrol.Location = new System.Drawing.Point(164, 195);
            this.rbPetrol.Name = "rbPetrol";
            this.rbPetrol.Size = new System.Drawing.Size(52, 17);
            this.rbPetrol.TabIndex = 7;
            this.rbPetrol.Text = "Petrol";
            this.rbPetrol.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Fuel type";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(60, 228);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "ACTION";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(164, 228);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtVIN
            // 
            this.txtVIN.Location = new System.Drawing.Point(81, 128);
            this.txtVIN.Name = "txtVIN";
            this.txtVIN.ReadOnly = true;
            this.txtVIN.Size = new System.Drawing.Size(137, 20);
            this.txtVIN.TabIndex = 12;
            this.txtVIN.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "VIN";
            // 
            // btnRefreshVIN
            // 
            this.btnRefreshVIN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshVIN.BackgroundImage")));
            this.btnRefreshVIN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefreshVIN.Location = new System.Drawing.Point(237, 128);
            this.btnRefreshVIN.Name = "btnRefreshVIN";
            this.btnRefreshVIN.Size = new System.Drawing.Size(23, 23);
            this.btnRefreshVIN.TabIndex = 13;
            this.btnRefreshVIN.TabStop = false;
            this.btnRefreshVIN.UseVisualStyleBackColor = true;
            this.btnRefreshVIN.Click += new System.EventHandler(this.btnRefreshVIN_Click);
            // 
            // frmEditCarModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 268);
            this.Controls.Add(this.btnRefreshVIN);
            this.Controls.Add(this.txtVIN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rbPetrol);
            this.Controls.Add(this.rbDiesel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbManuf);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Name = "frmEditCarModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit car models";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmeditcarmodel_FormClosed);
            this.Load += new System.EventHandler(this.frmeditcarmodel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cmbManuf;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbDiesel;
        private System.Windows.Forms.RadioButton rbPetrol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtVIN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRefreshVIN;
    }
}