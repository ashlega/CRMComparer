namespace Microsoft.Crm.Isv.CustomizationComparisonUtility
{
    partial class OpenFilesForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.SourceBrowseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this._CancelButton = new System.Windows.Forms.Button();
            this.TargetBrowseButton = new System.Windows.Forms.Button();
            this.BrowseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.TargetLabelTextBox = new System.Windows.Forms.TextBox();
            this.SourceLabelTextBox = new System.Windows.Forms.TextBox();
            this.TargetTextBox = new System.Windows.Forms.TextBox();
            this.SourceTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customization Files:";
            // 
            // SourceBrowseButton
            // 
            this.SourceBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceBrowseButton.Location = new System.Drawing.Point(361, 28);
            this.SourceBrowseButton.Name = "SourceBrowseButton";
            this.SourceBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.SourceBrowseButton.TabIndex = 1;
            this.SourceBrowseButton.Text = "Browse...";
            this.SourceBrowseButton.UseVisualStyleBackColor = true;
            this.SourceBrowseButton.Click += new System.EventHandler(this.SourceBrowseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(398, 117);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 7;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // _CancelButton
            // 
            this._CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._CancelButton.Location = new System.Drawing.Point(479, 117);
            this._CancelButton.Name = "_CancelButton";
            this._CancelButton.Size = new System.Drawing.Size(75, 23);
            this._CancelButton.TabIndex = 6;
            this._CancelButton.Text = "Cancel";
            this._CancelButton.UseVisualStyleBackColor = true;
            // 
            // TargetBrowseButton
            // 
            this.TargetBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetBrowseButton.Location = new System.Drawing.Point(361, 57);
            this.TargetBrowseButton.Name = "TargetBrowseButton";
            this.TargetBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.TargetBrowseButton.TabIndex = 4;
            this.TargetBrowseButton.Text = "Browse...";
            this.TargetBrowseButton.UseVisualStyleBackColor = true;
            this.TargetBrowseButton.Click += new System.EventHandler(this.TargetBrowseButton_Click);
            // 
            // BrowseFileDialog
            // 
            this.BrowseFileDialog.Filter = "Customization Files|*.xml|All files|*.*";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(443, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Labels:";
            // 
            // TargetLabelTextBox
            // 
            this.TargetLabelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetLabelTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default, "TargetLabel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TargetLabelTextBox.Location = new System.Drawing.Point(444, 59);
            this.TargetLabelTextBox.Name = "TargetLabelTextBox";
            this.TargetLabelTextBox.Size = new System.Drawing.Size(110, 20);
            this.TargetLabelTextBox.TabIndex = 5;
            this.TargetLabelTextBox.Text = global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default.TargetLabel;
            // 
            // SourceLabelTextBox
            // 
            this.SourceLabelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceLabelTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default, "SourceLabel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SourceLabelTextBox.Location = new System.Drawing.Point(443, 29);
            this.SourceLabelTextBox.Name = "SourceLabelTextBox";
            this.SourceLabelTextBox.Size = new System.Drawing.Size(111, 20);
            this.SourceLabelTextBox.TabIndex = 2;
            this.SourceLabelTextBox.Text = global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default.SourceLabel;
            // 
            // TargetTextBox
            // 
            this.TargetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default, "TargetFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TargetTextBox.Location = new System.Drawing.Point(13, 59);
            this.TargetTextBox.Name = "TargetTextBox";
            this.TargetTextBox.Size = new System.Drawing.Size(342, 20);
            this.TargetTextBox.TabIndex = 3;
            this.TargetTextBox.Text = global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default.TargetFile;
            // 
            // SourceTextBox
            // 
            this.SourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default, "SourceFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SourceTextBox.Location = new System.Drawing.Point(13, 30);
            this.SourceTextBox.Name = "SourceTextBox";
            this.SourceTextBox.Size = new System.Drawing.Size(341, 20);
            this.SourceTextBox.TabIndex = 0;
            this.SourceTextBox.Text = global::Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties.Settings.Default.SourceFile;
            // 
            // OpenFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 152);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TargetLabelTextBox);
            this.Controls.Add(this.SourceLabelTextBox);
            this.Controls.Add(this.TargetBrowseButton);
            this.Controls.Add(this.TargetTextBox);
            this.Controls.Add(this._CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.SourceBrowseButton);
            this.Controls.Add(this.SourceTextBox);
            this.Controls.Add(this.label1);
            this.Name = "OpenFilesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CRM Customization Comparison Utility - Open Files";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SourceTextBox;
        private System.Windows.Forms.Button SourceBrowseButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button _CancelButton;
        private System.Windows.Forms.Button TargetBrowseButton;
        private System.Windows.Forms.TextBox TargetTextBox;
        private System.Windows.Forms.OpenFileDialog BrowseFileDialog;
        private System.Windows.Forms.TextBox SourceLabelTextBox;
        private System.Windows.Forms.TextBox TargetLabelTextBox;
        private System.Windows.Forms.Label label3;
    }
}