namespace Menekulj
{
    partial class MenekuljWindow
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
            NewGameBtn = new Button();
            SuspendLayout();
            // 
            // NewGameBtn
            // 
            NewGameBtn.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            NewGameBtn.Location = new Point(271, 200);
            NewGameBtn.Margin = new Padding(3, 2, 3, 2);
            NewGameBtn.Name = "NewGameBtn";
            NewGameBtn.Size = new Size(178, 44);
            NewGameBtn.TabIndex = 0;
            NewGameBtn.Text = "New Game";
            NewGameBtn.UseVisualStyleBackColor = true;
            NewGameBtn.Click += NewGameBtn_Click;
            // 
            // MenekuljWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(800, 800);
            Controls.Add(NewGameBtn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            Name = "MenekuljWindow";
            Padding = new Padding(20);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menekulj";
            ResumeLayout(false);
        }

        #endregion

        private Button NewGameBtn;
    }
}