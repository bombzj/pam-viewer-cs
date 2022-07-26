
namespace PamViewer
{
    partial class PamForm
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
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.comboBoxGroup = new System.Windows.Forms.ComboBox();
            this.comboBoxPam = new System.Windows.Forms.ComboBox();
            this.comboBoxSprite = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(221, 40);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(73, 29);
            this.buttonSearch.TabIndex = 0;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(12, 40);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(203, 27);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSearch_KeyPress);
            // 
            // comboBoxGroup
            // 
            this.comboBoxGroup.FormattingEnabled = true;
            this.comboBoxGroup.Location = new System.Drawing.Point(13, 85);
            this.comboBoxGroup.Name = "comboBoxGroup";
            this.comboBoxGroup.Size = new System.Drawing.Size(281, 28);
            this.comboBoxGroup.TabIndex = 2;
            this.comboBoxGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxGroup_SelectedIndexChanged);
            // 
            // comboBoxPam
            // 
            this.comboBoxPam.FormattingEnabled = true;
            this.comboBoxPam.Location = new System.Drawing.Point(12, 131);
            this.comboBoxPam.Name = "comboBoxPam";
            this.comboBoxPam.Size = new System.Drawing.Size(282, 28);
            this.comboBoxPam.TabIndex = 3;
            this.comboBoxPam.SelectedIndexChanged += new System.EventHandler(this.comboBoxPam_SelectedIndexChanged);
            // 
            // comboBoxSprite
            // 
            this.comboBoxSprite.FormattingEnabled = true;
            this.comboBoxSprite.Location = new System.Drawing.Point(12, 175);
            this.comboBoxSprite.Name = "comboBoxSprite";
            this.comboBoxSprite.Size = new System.Drawing.Size(282, 28);
            this.comboBoxSprite.TabIndex = 4;
            this.comboBoxSprite.SelectedIndexChanged += new System.EventHandler(this.comboBoxSprite_SelectedIndexChanged);
            // 
            // PamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(381, 303);
            this.ControlBox = false;
            this.Controls.Add(this.comboBoxSprite);
            this.Controls.Add(this.comboBoxPam);
            this.Controls.Add(this.comboBoxGroup);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.buttonSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PamForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PamForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.ComboBox comboBoxGroup;
        private System.Windows.Forms.ComboBox comboBoxPam;
        private System.Windows.Forms.ComboBox comboBoxSprite;
    }
}

