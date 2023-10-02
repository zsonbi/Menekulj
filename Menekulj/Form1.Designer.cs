namespace Menekulj
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
            PausePanel = new Panel();
            PauseSaveGameBtn = new Button();
            PauseLoadGameBtn = new Button();
            PauseLabel = new Label();
            ResumeBtn = new Button();
            LoadNewGameBtn = new Button();
            PauseBtn = new Button();
            PausePanel.SuspendLayout();
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
            LoadGameBtn.Click += LoadGameBtn_Click;
            // 
            // PausePanel
            // 
            PausePanel.AllowDrop = true;
            PausePanel.Controls.Add(PauseSaveGameBtn);
            PausePanel.Controls.Add(PauseLoadGameBtn);
            PausePanel.Controls.Add(PauseLabel);
            PausePanel.Controls.Add(ResumeBtn);
            PausePanel.Location = new Point(199, 158);
            PausePanel.Name = "PausePanel";
            PausePanel.Size = new Size(408, 360);
            PausePanel.TabIndex = 5;
            PausePanel.Visible = false;
            // 
            // PauseSaveGameBtn
            // 
            PauseSaveGameBtn.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            PauseSaveGameBtn.Location = new Point(120, 223);
            PauseSaveGameBtn.Name = "PauseSaveGameBtn";
            PauseSaveGameBtn.Size = new Size(150, 43);
            PauseSaveGameBtn.TabIndex = 3;
            PauseSaveGameBtn.Text = "Save Game";
            PauseSaveGameBtn.UseVisualStyleBackColor = true;
            PauseSaveGameBtn.Click += PauseSaveGameBtn_Click;
            // 
            // PauseLoadGameBtn
            // 
            PauseLoadGameBtn.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            PauseLoadGameBtn.Location = new Point(120, 159);
            PauseLoadGameBtn.Name = "PauseLoadGameBtn";
            PauseLoadGameBtn.Size = new Size(150, 43);
            PauseLoadGameBtn.TabIndex = 2;
            PauseLoadGameBtn.Text = "Load Game";
            PauseLoadGameBtn.UseVisualStyleBackColor = true;
            PauseLoadGameBtn.Click += PauseLoadGameBtn_Click;
            // 
            // PauseLabel
            // 
            PauseLabel.AutoSize = true;
            PauseLabel.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            PauseLabel.Location = new Point(49, 25);
            PauseLabel.Name = "PauseLabel";
            PauseLabel.Size = new Size(315, 45);
            PauseLabel.TabIndex = 1;
            PauseLabel.Text = "The game is paused";
            // 
            // ResumeBtn
            // 
            ResumeBtn.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            ResumeBtn.Location = new Point(120, 94);
            ResumeBtn.Name = "ResumeBtn";
            ResumeBtn.Size = new Size(150, 43);
            ResumeBtn.TabIndex = 0;
            ResumeBtn.Text = "Resume";
            ResumeBtn.UseVisualStyleBackColor = true;
            ResumeBtn.Click += ResumeBtn_Click;
            // 
            // LoadNewGameBtn
            // 
            LoadNewGameBtn.Location = new Point(0, 0);
            LoadNewGameBtn.Name = "LoadNewGameBtn";
            LoadNewGameBtn.Size = new Size(75, 23);
            LoadNewGameBtn.TabIndex = 0;
            // 
            // PauseBtn
            // 
            PauseBtn.BackgroundImage = (Image)resources.GetObject("PauseBtn.BackgroundImage");
            PauseBtn.BackgroundImageLayout = ImageLayout.Zoom;
            PauseBtn.Location = new Point(750, 0);
            PauseBtn.Name = "PauseBtn";
            PauseBtn.Size = new Size(50, 50);
            PauseBtn.TabIndex = 6;
            PauseBtn.UseVisualStyleBackColor = true;
            PauseBtn.Visible = false;
            PauseBtn.Click += pauseBtn_Click;
            // 
            // View
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(800, 800);
            Controls.Add(PauseBtn);
            Controls.Add(PausePanel);
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
            PausePanel.ResumeLayout(false);
            PausePanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button NewGameBtn;
        private RadioButton SmallRadio;
        private RadioButton MediumRadio;
        private RadioButton BigRadio;
        private Button LoadGameBtn;
        private Panel PausePanel;
        private Button LoadNewGameBtn;
        private Button ResumeBtn;
        private Label PauseLabel;
        private Button PauseLoadGameBtn;
        private Button PauseSaveGameBtn;
        private Button PauseBtn;
    }
}