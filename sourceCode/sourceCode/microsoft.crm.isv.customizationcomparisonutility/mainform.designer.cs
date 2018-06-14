namespace Microsoft.Crm.Isv.CustomizationComparisonUtility
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SourceLabel = new System.Windows.Forms.Label();
            this.TargetLabel = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ComparisonListView = new System.Windows.Forms.ListView();
            this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UnchangedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChangedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NotInSourceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NotInTargetColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DifferenceImageList = new System.Windows.Forms.ImageList(this.components);
            this.TreeImageList = new System.Windows.Forms.ImageList(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SourcePathLabel = new System.Windows.Forms.Label();
            this.SourceBusyPicture = new System.Windows.Forms.PictureBox();
            this.SourceWebBrowser = new System.Windows.Forms.WebBrowser();
            this.BrowserContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TargetPathLabel = new System.Windows.Forms.Label();
            this.TargetBusyPicture = new System.Windows.Forms.PictureBox();
            this.TargetWebBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenFilesButton = new System.Windows.Forms.ToolStripButton();
            this.ExportToExcelButton = new System.Windows.Forms.ToolStripButton();
            this.SaveToExcelDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SourceBusyPicture)).BeginInit();
            this.BrowserContextMenuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetBusyPicture)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SourceLabel
            // 
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Location = new System.Drawing.Point(6, 7);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(44, 13);
            this.SourceLabel.TabIndex = 1;
            this.SourceLabel.Text = "Source:";
            this.SourceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // TargetLabel
            // 
            this.TargetLabel.AutoSize = true;
            this.TargetLabel.Location = new System.Drawing.Point(3, 7);
            this.TargetLabel.Name = "TargetLabel";
            this.TargetLabel.Size = new System.Drawing.Size(41, 13);
            this.TargetLabel.TabIndex = 1;
            this.TargetLabel.Text = "Target:";
            this.TargetLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ComparisonListView);
            this.splitContainer2.Panel1.Controls.Add(this.label7);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(849, 512);
            this.splitContainer2.SplitterDistance = 254;
            this.splitContainer2.TabIndex = 7;
            // 
            // ComparisonListView
            // 
            this.ComparisonListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComparisonListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.UnchangedColumnHeader,
            this.ChangedColumnHeader,
            this.NotInSourceColumnHeader,
            this.NotInTargetColumnHeader});
            this.ComparisonListView.FullRowSelect = true;
            this.ComparisonListView.GridLines = true;
            this.ComparisonListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ComparisonListView.HideSelection = false;
            this.ComparisonListView.Location = new System.Drawing.Point(6, 25);
            this.ComparisonListView.Name = "ComparisonListView";
            this.ComparisonListView.Size = new System.Drawing.Size(832, 222);
            this.ComparisonListView.SmallImageList = this.DifferenceImageList;
            this.ComparisonListView.StateImageList = this.TreeImageList;
            this.ComparisonListView.TabIndex = 0;
            this.ComparisonListView.UseCompatibleStateImageBehavior = false;
            this.ComparisonListView.View = System.Windows.Forms.View.Details;
            this.ComparisonListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ComparisonListView_ItemSelectionChanged);
            this.ComparisonListView.SelectedIndexChanged += new System.EventHandler(this.ComparisonListView_SelectedIndexChanged);
            this.ComparisonListView.Click += new System.EventHandler(this.ComparisonListView_Click);
            // 
            // NameColumnHeader
            // 
            this.NameColumnHeader.Text = "Name";
            this.NameColumnHeader.Width = 262;
            // 
            // UnchangedColumnHeader
            // 
            this.UnchangedColumnHeader.Text = "Unchanged";
            this.UnchangedColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.UnchangedColumnHeader.Width = 100;
            // 
            // ChangedColumnHeader
            // 
            this.ChangedColumnHeader.Text = "Changed";
            this.ChangedColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ChangedColumnHeader.Width = 100;
            // 
            // NotInSourceColumnHeader
            // 
            this.NotInSourceColumnHeader.Text = "Not in Source";
            this.NotInSourceColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NotInSourceColumnHeader.Width = 120;
            // 
            // NotInTargetColumnHeader
            // 
            this.NotInTargetColumnHeader.Text = "Not in Target";
            this.NotInTargetColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NotInTargetColumnHeader.Width = 120;
            // 
            // DifferenceImageList
            // 
            this.DifferenceImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DifferenceImageList.ImageStream")));
            this.DifferenceImageList.TransparentColor = System.Drawing.Color.White;
            this.DifferenceImageList.Images.SetKeyName(0, "imnon.png");
            this.DifferenceImageList.Images.SetKeyName(1, "imnaway.png");
            this.DifferenceImageList.Images.SetKeyName(2, "missingsrc.png");
            this.DifferenceImageList.Images.SetKeyName(3, "missingtrg.png");
            // 
            // TreeImageList
            // 
            this.TreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeImageList.ImageStream")));
            this.TreeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeImageList.Images.SetKeyName(0, "dashplus.gif");
            this.TreeImageList.Images.SetKeyName(1, "dashminus.gif");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Analysis Breakdown:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SourcePathLabel);
            this.splitContainer1.Panel1.Controls.Add(this.SourceBusyPicture);
            this.splitContainer1.Panel1.Controls.Add(this.SourceWebBrowser);
            this.splitContainer1.Panel1.Controls.Add(this.SourceLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(849, 254);
            this.splitContainer1.SplitterDistance = 424;
            this.splitContainer1.TabIndex = 2;
            // 
            // SourcePathLabel
            // 
            this.SourcePathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourcePathLabel.AutoEllipsis = true;
            this.SourcePathLabel.Location = new System.Drawing.Point(6, 23);
            this.SourcePathLabel.Name = "SourcePathLabel";
            this.SourcePathLabel.Size = new System.Drawing.Size(410, 13);
            this.SourcePathLabel.TabIndex = 3;
            this.SourcePathLabel.Text = "C:\\...";
            this.SourcePathLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // SourceBusyPicture
            // 
            this.SourceBusyPicture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SourceBusyPicture.BackColor = System.Drawing.Color.White;
            this.SourceBusyPicture.Image = ((System.Drawing.Image)(resources.GetObject("SourceBusyPicture.Image")));
            this.SourceBusyPicture.Location = new System.Drawing.Point(194, 123);
            this.SourceBusyPicture.Name = "SourceBusyPicture";
            this.SourceBusyPicture.Size = new System.Drawing.Size(32, 32);
            this.SourceBusyPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.SourceBusyPicture.TabIndex = 2;
            this.SourceBusyPicture.TabStop = false;
            this.SourceBusyPicture.Visible = false;
            // 
            // SourceWebBrowser
            // 
            this.SourceWebBrowser.AllowNavigation = false;
            this.SourceWebBrowser.AllowWebBrowserDrop = false;
            this.SourceWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceWebBrowser.ContextMenuStrip = this.BrowserContextMenuStrip;
            this.SourceWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.SourceWebBrowser.Location = new System.Drawing.Point(6, 39);
            this.SourceWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.SourceWebBrowser.Name = "SourceWebBrowser";
            this.SourceWebBrowser.Size = new System.Drawing.Size(410, 202);
            this.SourceWebBrowser.TabIndex = 1;
            // 
            // BrowserContextMenuStrip
            // 
            this.BrowserContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyMenuItem,
            this.SelectAllMenuItem});
            this.BrowserContextMenuStrip.Name = "BrowserContextMenuStrip";
            this.BrowserContextMenuStrip.Size = new System.Drawing.Size(123, 48);
            // 
            // CopyMenuItem
            // 
            this.CopyMenuItem.Name = "CopyMenuItem";
            this.CopyMenuItem.Size = new System.Drawing.Size(122, 22);
            this.CopyMenuItem.Text = "Copy";
            this.CopyMenuItem.Click += new System.EventHandler(this.CopyMenuItem_Click);
            // 
            // SelectAllMenuItem
            // 
            this.SelectAllMenuItem.Name = "SelectAllMenuItem";
            this.SelectAllMenuItem.Size = new System.Drawing.Size(122, 22);
            this.SelectAllMenuItem.Text = "Select All";
            this.SelectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TargetPathLabel);
            this.panel2.Controls.Add(this.TargetBusyPicture);
            this.panel2.Controls.Add(this.TargetWebBrowser);
            this.panel2.Controls.Add(this.TargetLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(417, 250);
            this.panel2.TabIndex = 5;
            // 
            // TargetPathLabel
            // 
            this.TargetPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetPathLabel.AutoEllipsis = true;
            this.TargetPathLabel.Location = new System.Drawing.Point(6, 23);
            this.TargetPathLabel.Name = "TargetPathLabel";
            this.TargetPathLabel.Size = new System.Drawing.Size(402, 13);
            this.TargetPathLabel.TabIndex = 4;
            this.TargetPathLabel.Text = "C:\\...";
            this.TargetPathLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // TargetBusyPicture
            // 
            this.TargetBusyPicture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TargetBusyPicture.BackColor = System.Drawing.Color.White;
            this.TargetBusyPicture.Image = ((System.Drawing.Image)(resources.GetObject("TargetBusyPicture.Image")));
            this.TargetBusyPicture.Location = new System.Drawing.Point(192, 123);
            this.TargetBusyPicture.Name = "TargetBusyPicture";
            this.TargetBusyPicture.Size = new System.Drawing.Size(32, 32);
            this.TargetBusyPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.TargetBusyPicture.TabIndex = 2;
            this.TargetBusyPicture.TabStop = false;
            this.TargetBusyPicture.Visible = false;
            // 
            // TargetWebBrowser
            // 
            this.TargetWebBrowser.AllowNavigation = false;
            this.TargetWebBrowser.AllowWebBrowserDrop = false;
            this.TargetWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetWebBrowser.ContextMenuStrip = this.BrowserContextMenuStrip;
            this.TargetWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.TargetWebBrowser.Location = new System.Drawing.Point(6, 39);
            this.TargetWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.TargetWebBrowser.Name = "TargetWebBrowser";
            this.TargetWebBrowser.ScriptErrorsSuppressed = true;
            this.TargetWebBrowser.Size = new System.Drawing.Size(404, 202);
            this.TargetWebBrowser.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFilesButton,
            this.ExportToExcelButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(849, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // OpenFilesButton
            // 
            this.OpenFilesButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenFilesButton.Image")));
            this.OpenFilesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenFilesButton.Name = "OpenFilesButton";
            this.OpenFilesButton.Size = new System.Drawing.Size(91, 22);
            this.OpenFilesButton.Text = "Open Files...";
            this.OpenFilesButton.Click += new System.EventHandler(this.OpenFilesButton_Click);
            // 
            // ExportToExcelButton
            // 
            this.ExportToExcelButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportToExcelButton.Image")));
            this.ExportToExcelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportToExcelButton.Name = "ExportToExcelButton";
            this.ExportToExcelButton.Size = new System.Drawing.Size(115, 22);
            this.ExportToExcelButton.Text = "Export To Excel...";
            this.ExportToExcelButton.Click += new System.EventHandler(this.ExportToExcelButton_Click);
            // 
            // SaveToExcelDialog
            // 
            this.SaveToExcelDialog.Filter = "Excel Files|*.xls";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 537);
            this.ContextMenuStrip = this.BrowserContextMenuStrip;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "CRM Customization Comparison Utility";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SourceBusyPicture)).EndInit();
            this.BrowserContextMenuStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetBusyPicture)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser SourceWebBrowser;
        private System.Windows.Forms.WebBrowser TargetWebBrowser;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.Label TargetLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListView ComparisonListView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ColumnHeader NameColumnHeader;
        private System.Windows.Forms.ImageList TreeImageList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton OpenFilesButton;
        private System.Windows.Forms.ToolStripButton ExportToExcelButton;
        private System.Windows.Forms.SaveFileDialog SaveToExcelDialog;
        private System.Windows.Forms.ColumnHeader UnchangedColumnHeader;
        private System.Windows.Forms.ColumnHeader ChangedColumnHeader;
        private System.Windows.Forms.ColumnHeader NotInSourceColumnHeader;
        private System.Windows.Forms.ColumnHeader NotInTargetColumnHeader;
        private System.Windows.Forms.ImageList DifferenceImageList;
        private System.Windows.Forms.PictureBox TargetBusyPicture;
        private System.Windows.Forms.ContextMenuStrip BrowserContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem CopyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SelectAllMenuItem;
        private System.Windows.Forms.Label SourcePathLabel;
        private System.Windows.Forms.Label TargetPathLabel;
        private System.Windows.Forms.PictureBox SourceBusyPicture;
    }
}

