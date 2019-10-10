namespace Lab_3___3D_Sphere_Volume___Niederreiter_QRNG
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bnDraw = new System.Windows.Forms.Button();
            this.txtError = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.txtActualVol = new System.Windows.Forms.TextBox();
            this.lblActualVol = new System.Windows.Forms.Label();
            this.txtEstVol = new System.Windows.Forms.TextBox();
            this.lblEstVol = new System.Windows.Forms.Label();
            this.txtIterations = new System.Windows.Forms.TextBox();
            this.lblIterations = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(501, 501);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // bnDraw
            // 
            this.bnDraw.Location = new System.Drawing.Point(536, 32);
            this.bnDraw.Name = "bnDraw";
            this.bnDraw.Size = new System.Drawing.Size(74, 35);
            this.bnDraw.TabIndex = 1;
            this.bnDraw.Text = "Estimate";
            this.bnDraw.UseVisualStyleBackColor = true;
            this.bnDraw.Click += new System.EventHandler(this.bnDraw_Click);
            // 
            // txtError
            // 
            this.txtError.Location = new System.Drawing.Point(532, 288);
            this.txtError.Name = "txtError";
            this.txtError.ReadOnly = true;
            this.txtError.Size = new System.Drawing.Size(79, 20);
            this.txtError.TabIndex = 25;
            this.txtError.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblError
            // 
            this.lblError.Location = new System.Drawing.Point(533, 271);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(76, 20);
            this.lblError.TabIndex = 24;
            this.lblError.Text = "Percent Error:";
            // 
            // txtActualVol
            // 
            this.txtActualVol.Location = new System.Drawing.Point(533, 223);
            this.txtActualVol.Name = "txtActualVol";
            this.txtActualVol.ReadOnly = true;
            this.txtActualVol.Size = new System.Drawing.Size(79, 20);
            this.txtActualVol.TabIndex = 23;
            this.txtActualVol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblActualVol
            // 
            this.lblActualVol.Location = new System.Drawing.Point(533, 207);
            this.lblActualVol.Name = "lblActualVol";
            this.lblActualVol.Size = new System.Drawing.Size(78, 13);
            this.lblActualVol.TabIndex = 22;
            this.lblActualVol.Text = "Act Volume:";
            // 
            // txtEstVol
            // 
            this.txtEstVol.Location = new System.Drawing.Point(532, 163);
            this.txtEstVol.Name = "txtEstVol";
            this.txtEstVol.ReadOnly = true;
            this.txtEstVol.Size = new System.Drawing.Size(78, 20);
            this.txtEstVol.TabIndex = 21;
            this.txtEstVol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblEstVol
            // 
            this.lblEstVol.Location = new System.Drawing.Point(533, 145);
            this.lblEstVol.Name = "lblEstVol";
            this.lblEstVol.Size = new System.Drawing.Size(83, 17);
            this.lblEstVol.TabIndex = 20;
            this.lblEstVol.Text = "Est Volume:";
            // 
            // txtIterations
            // 
            this.txtIterations.Location = new System.Drawing.Point(533, 100);
            this.txtIterations.Name = "txtIterations";
            this.txtIterations.Size = new System.Drawing.Size(77, 20);
            this.txtIterations.TabIndex = 19;
            this.txtIterations.Text = "10000";
            this.txtIterations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblIterations
            // 
            this.lblIterations.Location = new System.Drawing.Point(533, 84);
            this.lblIterations.Name = "lblIterations";
            this.lblIterations.Size = new System.Drawing.Size(50, 13);
            this.lblIterations.TabIndex = 18;
            this.lblIterations.Text = "Iterations:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 561);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtActualVol);
            this.Controls.Add(this.lblActualVol);
            this.Controls.Add(this.txtEstVol);
            this.Controls.Add(this.lblEstVol);
            this.Controls.Add(this.txtIterations);
            this.Controls.Add(this.lblIterations);
            this.Controls.Add(this.bnDraw);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "3D Sphere Volume - QRNG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button bnDraw;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.TextBox txtActualVol;
        private System.Windows.Forms.Label lblActualVol;
        private System.Windows.Forms.TextBox txtEstVol;
        private System.Windows.Forms.Label lblEstVol;
        private System.Windows.Forms.TextBox txtIterations;
        private System.Windows.Forms.Label lblIterations;
    }
}

