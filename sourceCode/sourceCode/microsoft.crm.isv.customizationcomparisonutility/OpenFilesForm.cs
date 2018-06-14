using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Crm.Isv.CustomizationComparisonUtility.Properties;

namespace Microsoft.Crm.Isv.CustomizationComparisonUtility
{
    public partial class OpenFilesForm : Form
    {
        public OpenFilesForm()
        {
            InitializeComponent();
        }

        public string TargetFile { get { return TargetTextBox.Text; } }
        public string SourceFile { get { return SourceTextBox.Text; } }
        public string SourceLabel { get { return SourceLabelTextBox.Text; } }
        public string TargetLabel { get { return TargetLabelTextBox.Text; } }

        private void SourceBrowseButton_Click(object sender, EventArgs e)
        {
            BrowseFileDialog.FileName = SourceTextBox.Text;
            if (BrowseFileDialog.ShowDialog() == DialogResult.OK)
            {
                SourceTextBox.Text = BrowseFileDialog.FileName;
            }
        }

        private void TargetBrowseButton_Click(object sender, EventArgs e)
        {
            BrowseFileDialog.FileName = TargetTextBox.Text;
            if (BrowseFileDialog.ShowDialog() == DialogResult.OK)
            {
                TargetTextBox.Text = BrowseFileDialog.FileName;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
