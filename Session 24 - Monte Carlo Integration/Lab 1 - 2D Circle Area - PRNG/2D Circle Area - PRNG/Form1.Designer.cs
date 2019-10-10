﻿namespace Lab_1___2D_Circle_Area___Monte_Carlo
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
            this.lblIterations = new System.Windows.Forms.Label();
            this.txtIterations = new System.Windows.Forms.TextBox();
            this.lblEstimatedArea = new System.Windows.Forms.Label();
            this.txtEstimatedArea = new System.Windows.Forms.TextBox();
            this.lblActualArea = new System.Windows.Forms.Label();
            this.txtActualArea = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.txtError = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
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
            this.bnDraw.Location = new System.Drawing.Point(538, 30);
            this.bnDraw.Name = "bnDraw";
            this.bnDraw.Size = new System.Drawing.Size(79, 35);
            this.bnDraw.TabIndex = 1;
            this.bnDraw.Text = "Estimate";
            this.bnDraw.UseVisualStyleBackColor = true;
            this.bnDraw.Click += new System.EventHandler(this.bnDraw_Click);
            // 
            // lblIterations
            // 
            this.lblIterations.Location = new System.Drawing.Point(538, 89);
            this.lblIterations.Name = "lblIterations";
            this.lblIterations.Size = new System.Drawing.Size(50, 13);
            this.lblIterations.TabIndex = 2;
            this.lblIterations.Text = "Iterations:";
            // 
            // txtIterations
            // 
            this.txtIterations.Location = new System.Drawing.Point(538, 105);
            this.txtIterations.Name = "txtIterations";
            this.txtIterations.Size = new System.Drawing.Size(77, 20);
            this.txtIterations.TabIndex = 3;
            this.txtIterations.Text = "100000";
            this.txtIterations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblEstimatedArea
            // 
            this.lblEstimatedArea.Location = new System.Drawing.Point(538, 150);
            this.lblEstimatedArea.Name = "lblEstimatedArea";
            this.lblEstimatedArea.Size = new System.Drawing.Size(83, 17);
            this.lblEstimatedArea.TabIndex = 4;
            this.lblEstimatedArea.Text = "Estimated Area:";
            // 
            // txtEstimatedArea
            // 
            this.txtEstimatedArea.Location = new System.Drawing.Point(537, 168);
            this.txtEstimatedArea.Name = "txtEstimatedArea";
            this.txtEstimatedArea.ReadOnly = true;
            this.txtEstimatedArea.Size = new System.Drawing.Size(78, 20);
            this.txtEstimatedArea.TabIndex = 5;
            this.txtEstimatedArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblActualArea
            // 
            this.lblActualArea.Location = new System.Drawing.Point(538, 212);
            this.lblActualArea.Name = "lblActualArea";
            this.lblActualArea.Size = new System.Drawing.Size(67, 13);
            this.lblActualArea.TabIndex = 6;
            this.lblActualArea.Text = "Actual Area:";
            // 
            // txtActualArea
            // 
            this.txtActualArea.Location = new System.Drawing.Point(538, 228);
            this.txtActualArea.Name = "txtActualArea";
            this.txtActualArea.ReadOnly = true;
            this.txtActualArea.Size = new System.Drawing.Size(79, 20);
            this.txtActualArea.TabIndex = 7;
            this.txtActualArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblError
            // 
            this.lblError.Location = new System.Drawing.Point(538, 276);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(76, 20);
            this.lblError.TabIndex = 8;
            this.lblError.Text = "Percent Error:";
            // 
            // txtError
            // 
            this.txtError.Location = new System.Drawing.Point(537, 294);
            this.txtError.Name = "txtError";
            this.txtError.ReadOnly = true;
            this.txtError.Size = new System.Drawing.Size(79, 20);
            this.txtError.TabIndex = 9;
            this.txtError.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 561);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtActualArea);
            this.Controls.Add(this.lblActualArea);
            this.Controls.Add(this.txtEstimatedArea);
            this.Controls.Add(this.lblEstimatedArea);
            this.Controls.Add(this.txtIterations);
            this.Controls.Add(this.lblIterations);
            this.Controls.Add(this.bnDraw);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "2D Circle Area - PRNG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button bnDraw;
        private System.Windows.Forms.Label lblIterations;
        private System.Windows.Forms.TextBox txtIterations;
        private System.Windows.Forms.Label lblEstimatedArea;
        private System.Windows.Forms.TextBox txtEstimatedArea;
        private System.Windows.Forms.Label lblActualArea;
        private System.Windows.Forms.TextBox txtActualArea;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.TextBox txtError;
    }
}

