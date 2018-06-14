using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.Crm.Isv.CustomizationComparisonUtility
{
    public partial class MainForm : Form
    {
        const int StateImageIndexDashPlus = 0;
        const int StateImageIndexDashMinus = 1;

        const int DifferenceImageIndexUnchanged = 0;
        const int DifferenceImageIndexChanged = 1;
        const int DifferenceImageIndexNotInSource = 2;
        const int DifferenceImageIndexNotInTarget = 3;

        const int LineCountLimit = 1000;

        private delegate void UpdateBrowsersInvoker(CustomizationComparison comparison, DiffResult<String>[] results);
        UpdateBrowsersInvoker _updateBrowsersCaller;

        public MainForm()
        {
            InitializeComponent();
            _updateBrowsersCaller = new UpdateBrowsersInvoker(UpdateBrowsers);
        }

        private bool CompareFiles(string sourceFileName, string targetFileName)
        {
            CustomizationComparer comparer = new CustomizationComparer();
            CustomizationComparison comparison = null;
            
           // try
           // {
                comparison = comparer.Compare(sourceFileName, targetFileName);
           // }
           // catch (Exception e)
           // {
           //     MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
           //     return false;
           // }

            BindList(comparison);
            return true;
        }

        private void BindList(CustomizationComparison comparison)
        {
            ComparisonListView.Items.Clear();

            AddItem(comparison, null);
        }

        private int GetImageIndex(CustomizationComparison comparison)
        {
            if (comparison.IsDifferent)
            {
                if (comparison.SourceValue == null)
                {
                    return DifferenceImageIndexNotInSource;
                }
                else if (comparison.TargetValue == null)
                {
                    return DifferenceImageIndexNotInTarget;
                }
                else
                {
                    return DifferenceImageIndexChanged;
                }
            }
            else
            {
                return DifferenceImageIndexUnchanged;
            }
        }

        private void AddItem(CustomizationComparison comparison, ListViewItem parentItem)
        {
            ListViewItem item = new ListViewItem()
            {
                Text = comparison.Name,
                Checked = false,
                StateImageIndex = comparison.Children.Count > 0 ? StateImageIndexDashPlus : -1,
                Tag = comparison,
                ImageIndex = GetImageIndex(comparison),
                IndentCount = parentItem == null ? 0 : parentItem.IndentCount + 1,
            };

            object obj = comparison.SourceValue ?? comparison.TargetValue;
            if (obj != null && comparison.Children.Count > 0)
            {
                item.SubItems.Add(comparison.GetUnchangedCount().ToString());
                item.SubItems.Add(comparison.GetChangedCount().ToString());
                item.SubItems.Add(comparison.GetMissingInSourceCount().ToString());
                item.SubItems.Add(comparison.GetMissingInTargetCount().ToString());
            }

            ComparisonListView.Items.Insert(parentItem == null ? 0 : parentItem.Index + 1, item);
        }

        private void UpdateBrowsers(CustomizationComparison comparison, DiffResult<String>[] results)
        {
            if (!ComparisonListView.SelectedItems.Cast<ListViewItem>().Any(i => i.Tag == comparison)) return;

            SourceBusyPicture.Visible = false;
            TargetBusyPicture.Visible = false;

            StringBuilder sourceBuffer = new StringBuilder();
            StringBuilder targetBuffer = new StringBuilder();

            HtmlTextWriter sourceWriter = new HtmlTextWriter(new StringWriter(sourceBuffer));
            HtmlTextWriter targetWriter = new HtmlTextWriter(new StringWriter(targetBuffer));

            int sourceLineCount = 0;
            int targetLineCount = 0;

            DiffOwner previousOwner = DiffOwner.Both;

            for (int i = 0; i < results.Length; i++)
            {
                DiffResult<String> result = results[i];

                DiffOwner nextOwner = i + 1 < results.Length ? results[i + 1].Owner : DiffOwner.Both;
                bool isolatedChange = previousOwner == DiffOwner.Both && nextOwner == DiffOwner.Both;

                switch (result.Owner)
                {
                    case DiffOwner.Target:
                        AppendLines(targetWriter, result.Value, isolatedChange ? "missing" : "changed");
                        targetLineCount += result.Value.Count;
                        break;

                    case DiffOwner.Source:
                        AppendLines(sourceWriter, result.Value, isolatedChange ? "missing" : "changed");
                        sourceLineCount += result.Value.Count;
                        break;

                    case DiffOwner.Both:
                        while (sourceLineCount < targetLineCount)
                        {
                            AppendLine(sourceWriter, " ", "blank");
                            sourceLineCount++;
                        }

                        while (targetLineCount < sourceLineCount)
                        {
                            AppendLine(targetWriter, " ", "blank");
                            targetLineCount++;
                        }

                        AppendLines(sourceWriter, result.Value);
                        AppendLines(targetWriter, result.Value);

                        sourceLineCount += result.Value.Count;
                        targetLineCount += result.Value.Count;
                        break;
                }

                previousOwner = result.Owner;
            }

            sourceWriter.Flush();
            targetWriter.Flush();

            SourceWebBrowser.Document.Body.InnerHtml = sourceBuffer.ToString();
            TargetWebBrowser.Document.Body.InnerHtml = targetBuffer.ToString();

            SourceWebBrowser.Document.Body.ScrollTop = 0;
            TargetWebBrowser.Document.Body.ScrollTop = 0;
        }

        private void AppendLines(HtmlTextWriter writer, IEnumerable<string> lines)
        {
            AppendLines(writer, lines, null);
        }

        private void AppendLines(HtmlTextWriter writer, IEnumerable<string> lines, string cssClassName)
        {
            foreach (string line in lines)
            {
                AppendLine(writer, line, cssClassName);
            }
        }

        private void AppendLine(HtmlTextWriter writer, string line, string cssClassName)
        {
            if (!String.IsNullOrEmpty(cssClassName))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClassName);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            line = HttpUtility.HtmlEncode(line);
            writer.Write(line.Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitBrowser(SourceWebBrowser);
            InitBrowser(TargetWebBrowser);

            if (!CompareFiles())
            {
                Close();
            }
        }

        private void InitBrowser(WebBrowser webBrowser)
        {
            webBrowser.DocumentText = "";
            HtmlDocument doc = webBrowser.Document;
            doc.OpenNew(true);
            doc.Write("<html>");
            doc.Write("<head>");
            doc.Write("<style type='text/css'>");
            doc.Write("* {padding: 0px; margin: 0px;}");
            doc.Write("body {overflow: auto; font: 10pt Courier New; border: 1px solid ActiveBorder;}");
            doc.Write("p {white-space: nowrap;}");
            doc.Write(".missing, .changed, .blank {background-color: #DEDFFF;}");
            doc.Write(".missing span {background-color: #CE6563;}");
            doc.Write(".changed span {background-color: yellow;}");
            doc.Write("</style>");
            doc.Write("</head>");
            doc.Write("<body>");
            doc.Write("</body>");
            doc.Write("</html>");
            doc.Window.Scroll += new HtmlElementEventHandler(Window_Scroll);
        }

        void Window_Scroll(object sender, HtmlElementEventArgs e)
        {
            HtmlWindow window = (HtmlWindow)sender;

            if (window.DomWindow == SourceWebBrowser.Document.Window.DomWindow)
            {
                TargetWebBrowser.Document.Body.ScrollTop = SourceWebBrowser.Document.Body.ScrollTop;
                TargetWebBrowser.Document.Body.ScrollLeft = SourceWebBrowser.Document.Body.ScrollLeft;
            }
            else
            {
                SourceWebBrowser.Document.Body.ScrollTop = TargetWebBrowser.Document.Body.ScrollTop;
                SourceWebBrowser.Document.Body.ScrollLeft = TargetWebBrowser.Document.Body.ScrollLeft;
            }
        }

        private void OpenFilesButton_Click(object sender, EventArgs e)
        {
            CompareFiles();
        }

        private bool CompareFiles()
        {
            OpenFilesForm dlg = new OpenFilesForm();

            while (dlg.ShowDialog(this) == DialogResult.OK)
            {
                SourceLabel.Text = String.Concat(dlg.SourceLabel, ":");
                TargetLabel.Text = String.Concat(dlg.TargetLabel, ":");
                SourcePathLabel.Text = dlg.SourceFile;
                TargetPathLabel.Text = dlg.TargetFile;
                NotInSourceColumnHeader.Text = String.Format("Not in {0}", dlg.SourceLabel);
                NotInTargetColumnHeader.Text = String.Format("Not in {0}", dlg.TargetLabel);
                if (CompareFiles(dlg.SourceFile, dlg.TargetFile))
                {
                    return true;
                }
            }

            return false;
        }

        private void ComparisonListView_Click(object sender, EventArgs e)
        {

            ListViewHitTestInfo hitTest = ComparisonListView.HitTest(ComparisonListView.PointToClient(System.Windows.Forms.Control.MousePosition));
            if (hitTest.Location == ListViewHitTestLocations.StateImage)
            {
                ToggleItem(hitTest.Item);
            }
        }

        private void ToggleItem(ListViewItem item)
        {
            if (item.Checked) // expanded
            {
                using (new LockRedraw(ComparisonListView.Handle))
                {
                    while (ComparisonListView.Items.Count > item.Index + 1 &&
                        ComparisonListView.Items[item.Index + 1].IndentCount > item.IndentCount)
                    {
                        ComparisonListView.Items.RemoveAt(item.Index + 1);
                    }

                    item.Checked = false;
                    item.StateImageIndex = StateImageIndexDashPlus;
                }
            }
            else
            {
                CustomizationComparison comparison = (CustomizationComparison)item.Tag;
                if (comparison.Children.Count > 0)
                {
                    using (new LockRedraw(ComparisonListView.Handle))
                    {
                        foreach (CustomizationComparison childComparison in comparison.Children.AsEnumerable().Reverse())
                        {
                            AddItem(childComparison, item);
                        }
                        item.StateImageIndex = StateImageIndexDashMinus;
                        item.Checked = true;
                    }
                }
            }
        }

        private void ComparisonListView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void ComparisonListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                CustomizationComparison comparison = (CustomizationComparison)e.Item.Tag;

                SourceBusyPicture.Visible = true;
                TargetBusyPicture.Visible = true;

                SourceWebBrowser.Document.Body.InnerHtml = "";
                TargetWebBrowser.Document.Body.InnerHtml = "";

                ThreadPool.QueueUserWorkItem(delegate(object state)
                {
                    if (comparison.IsDifferent)
                    {
                        CustomizationSerializer serializer = new CustomizationSerializer();
                        IEnumerable<String> sourceLines = serializer.SerializeObjectToLines(comparison, comparison.SourceValue);
                        IEnumerable<String> targetLines = serializer.SerializeObjectToLines(comparison, comparison.TargetValue);

                        Diff diff = new Diff();
                        DiffResult<String>[] results = diff.Compare(sourceLines.Take(LineCountLimit).ToArray(), targetLines.Take(LineCountLimit).ToArray()).ToArray();

                        this.Invoke(_updateBrowsersCaller, comparison, results);
                    }
                    else
                    {
                        CustomizationSerializer serializer = new CustomizationSerializer();
                        IEnumerable<String> lines = serializer.SerializeObjectToLines(comparison, comparison.TargetValue);

                        DiffResult<String>[] results = new DiffResult<string>[]
                        {
                            new DiffResult<String> 
                            {
                                Owner = DiffOwner.Both,
                            }
                        };

                        results[0].AddValues(lines.ToList());

                        this.Invoke(_updateBrowsersCaller, comparison, results);
                    }
                });
            }
        }

        private void ExportToExcelButton_Click(object sender, EventArgs e)
        {
            CustomizationComparison comparison = ComparisonListView.Items[0].Tag as CustomizationComparison;
            if (comparison != null)
            {                
                if (SaveToExcelDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileWithPath = SaveToExcelDialog.FileName;
                    ExportToExcel exportToExcel = new ExportToExcel(fileWithPath, comparison);
                    exportToExcel.Export();
                }
            }
        }

        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            WebBrowser browser = (WebBrowser)BrowserContextMenuStrip.SourceControl;
            browser.Document.ExecCommand("Copy", false, null);
        }

        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            WebBrowser browser = (WebBrowser)BrowserContextMenuStrip.SourceControl;
            browser.Document.ExecCommand("SelectAll", false, null);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (SourceWebBrowser.Focused)
            {

            }
        }
        
    }

}
