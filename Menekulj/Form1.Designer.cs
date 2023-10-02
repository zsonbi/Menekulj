﻿namespace Menekulj
{
    partial class View
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
            NewGameBtn = new Button();
            SmallRadio = new RadioButton();
            MediumRadio = new RadioButton();
            BigRadio = new RadioButton();
            LoadGameBtn = new Button();
            SuspendLayout();
            // 
            // NewGameBtn
            // 
            NewGameBtn.BackColor = SystemColors.ControlDarkDark;
            NewGameBtn.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            NewGameBtn.Location = new Point(291, 265);
            NewGameBtn.Margin = new Padding(3, 2, 3, 2);
            NewGameBtn.Name = "NewGameBtn";
            NewGameBtn.Size = new Size(178, 45);
            NewGameBtn.TabIndex = 0;
            NewGameBtn.Text = "New Game";
            NewGameBtn.UseVisualStyleBackColor = false;
            NewGameBtn.Click += NewGameBtn_Click;
            // 
            // SmallRadio
            // 
            SmallRadio.AutoSize = true;
            SmallRadio.Checked = true;
            SmallRadio.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            SmallRadio.Location = new Point(291, 325);
            SmallRadio.Name = "SmallRadio";
            SmallRadio.Size = new Size(118, 29);
            SmallRadio.TabIndex = 1;
            SmallRadio.TabStop = true;
            SmallRadio.Text = "Small map";
            SmallRadio.UseVisualStyleBackColor = true;
            // 
            // MediumRadio
            // 
            MediumRadio.AutoSize = true;
            MediumRadio.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            MediumRadio.Location = new Point(291, 360);
            MediumRadio.Name = "MediumRadio";
            MediumRadio.Size = new Size(142, 29);
            MediumRadio.TabIndex = 2;
            MediumRadio.Text = "Medium map";
            MediumRadio.UseVisualStyleBackColor = true;
            // 
            // BigRadio
            // 
            BigRadio.AutoSize = true;
            BigRadio.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            BigRadio.Location = new Point(291, 395);
            BigRadio.Name = "BigRadio";
            BigRadio.Size = new Size(99, 29);
            BigRadio.TabIndex = 3;
            BigRadio.Text = "Big map";
            BigRadio.UseVisualStyleBackColor = true;
            // 
            // LoadGameBtn
            // 
            LoadGameBtn.BackColor = SystemColors.ControlDarkDark;
            LoadGameBtn.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            LoadGameBtn.Location = new Point(291, 201);
            LoadGameBtn.Name = "LoadGameBtn";
            LoadGameBtn.Size = new Size(178, 45);
            LoadGameBtn.TabIndex = 4;
            LoadGameBtn.Text = "Load Game";
            LoadGameBtn.UseVisualStyleBackColor = false;
            // 
            // View
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(800, 800);
            Controls.Add(LoadGameBtn);
            Controls.Add(BigRadio);
            Controls.Add(MediumRadio);
            Controls.Add(SmallRadio);
            Controls.Add(NewGameBtn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            Name = "View";
            Padding = new Padding(20);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menekulj";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button NewGameBtn;
        private RadioButton SmallRadio;
        private RadioButton MediumRadio;
        private RadioButton BigRadio;
        private Button LoadGameBtn;
    }
}