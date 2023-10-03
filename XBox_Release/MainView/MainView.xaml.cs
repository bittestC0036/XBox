using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;
using Path = System.IO.Path;

namespace XBox
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : UserControl
    {
        public List<string> sTxt_list = new List<string>();

        public List<string> vs = new List<string>();

        public TreeViewItem SelectedTreeviewItem = new TreeViewItem();

        public MainView()
        {
            InitializeComponent();

            SetTextType();
        }

        private void SetTextType()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string sDirPath = System.IO.Path.Combine(fileInfo.DirectoryName, "Config");

            string sConfigPath = System.IO.Path.Combine(sDirPath, "Config.ini");

            if (false == Directory.Exists(sDirPath))
                SetDefault();


            StreamReader sr = new StreamReader(sConfigPath);

            string sLine = string.Empty;

            while ((sLine = sr.ReadLine()) != null)
            {
                if (sLine.IndexOf('=') != -1)
                {
                    if (sLine.IndexOf("TXTLOADTYPE") != -1)
                    {
                        var strTemp = sLine.Split('=');

                        strTemp = strTemp[1].Split(',');

                        for (int nCnt = 0; nCnt < strTemp.Length; nCnt++)
                        {
                            sTxt_list.Add(strTemp[nCnt]);
                        }
                    }
                }
            }
        }

        private void SetDefault()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string sDirPath = System.IO.Path.Combine(fileInfo.DirectoryName, "Config");

            Directory.CreateDirectory(sDirPath);

            string sConfigPath = Path.Combine(sDirPath, "Config.ini");


            StreamWriter sw = new StreamWriter(sConfigPath);

            sw.Write("TXTLOADTYPE=");
            sw.Write(".txt      ,".Replace(" ", ""));
            sw.Write(".c        ,".Replace(" ", ""));
            sw.Write(".cs       ,".Replace(" ", ""));
            sw.Write(".cpp      ,".Replace(" ", ""));
            sw.Write(".log      ,".Replace(" ", ""));
            sw.Write(".h        ,".Replace(" ", ""));
            sw.Write(".csv      ,".Replace(" ", ""));
            sw.Write(".xml      ,".Replace(" ", ""));
            sw.Write(".json     ,".Replace(" ", ""));
            sw.Write(".html     ,".Replace(" ", ""));
            sw.Write(".css      ,".Replace(" ", ""));
            sw.Write(".cfg      ,".Replace(" ", ""));
            sw.Write(".ini      ,".Replace(" ", ""));
            sw.Write(".sql      ,".Replace(" ", ""));
            sw.Write(".config   ,".Replace(" ", ""));
            sw.Write(".pro      ,".Replace(" ", ""));
            sw.Write(".pro.user ,".Replace(" ", ""));
            sw.Write(".csproj   ,".Replace(" ", ""));
            sw.Write(".settings ,".Replace(" ", ""));
            sw.Write(".sln      ,".Replace(" ", ""));
            sw.Write(".ui       ,".Replace(" ", ""));
            sw.Write(".idx      ,".Replace(" ", ""));
            sw.Write(".suo      ,".Replace(" ", ""));
            sw.Write(".o        ,".Replace(" ", ""));
            sw.Write(".obj      ,".Replace(" ", ""));
            sw.Write(".java     ,".Replace(" ", ""));
            sw.Write(".class     ".Replace(" ", ""));
            sw.WriteLine("");
            sw.Close();
        }

        private void INITUI(string rootFolderPath = null)
        {
            if (rootFolderPath == null)
            {
                rootFolderPath = @"D:\Job"; // 설정한 폴더 경로로 변경하세요.
                return;
            }

            folderTreeView.Tag = rootFolderPath;
            folderTreeView.PreviewKeyDown += FolderTreeView_PreviewKeyDown;
            folderTreeView.SelectedItemChanged += Treeview_SelectedChanged;
            folderTreeView.Items.Add(MakeFolderTree(rootFolderPath));
        }

        private void FolderTreeView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var x = sender as TreeView;

            if (x == null)
                return;

            var temp_treeview_item = x.SelectedItem as TreeViewItem;

            if (temp_treeview_item == null)
                return;

            SelectedTreeviewItem = temp_treeview_item;
        }

        //private TreeViewItem MakeFolderTree(string FolderPath)
        private _Folder_ MakeFolderTree(string FolderPath)
        {
            var di_folder = new DirectoryInfo(FolderPath);
            var temp_tv_item = new _Folder_(); //{ Header = di_folder.Name, Tag = di_folder.FullName, Background = Brushes.Gray };

            //int nSize = 100;

            temp_tv_item.TB_Header.Content = di_folder.Name;
            temp_tv_item.Tag = di_folder.FullName;

            try
            {
                temp_tv_item.Expanded += Temp_tv_item_Expanded;
                if (di_folder.GetDirectories().Count() > 0)
                {
                    foreach (var item in di_folder.GetDirectories())
                    {
                        temp_tv_item.Items.Add(MakeFolderTree(item.FullName));
                    }
                }
                return temp_tv_item;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }

        private void Temp_tv_item_Expanded(object sender, RoutedEventArgs e)
        {
            var x = sender as _Folder_;

            if (x != null)
            {
                var di_spath = new DirectoryInfo(x.Tag.ToString());
                var fi = di_spath.GetFiles();
                for (int nCnt = 0; nCnt < fi.Count(); nCnt++)
                {
                    var temp = new _File_();
                    temp.Height = 40;
                    temp.TB_Header.Content = fi[nCnt].Name;
                    temp.Tag = fi[nCnt].FullName;
                    temp.MouseDown += Temp_MouseDown;
                    temp.MouseDoubleClick += Temp_MouseDoubleClick;
                    if (vs.IndexOf(temp.Tag.ToString()) == -1)
                    {
                        x.Items.Add(temp);
                        vs.Add(temp.Tag.ToString());
                    }
                }
            }
        }

        private void Temp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = sender as _File_;
            var fi_item = new FileInfo(x.Tag.ToString());

            if (sTxt_list.IndexOf(fi_item.Extension) > -1)
            {
                this.TB_Content.TB_Content.Text = File.ReadAllText(x.Tag.ToString());
            }
            else
            {

            }
        }

        private void Temp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var x = sender as _File_;

            if (null == x)
                return;

            var TEMP = new LayoutDocument();

            TEMP.Title = x.TB_Header.ToString().Split(':')[1];

            var Temp_TB = new TextEditor();

            Temp_TB.TB_Content.Text = File.ReadAllText(x.Tag.ToString());

            Temp_TB.TB_Content.Tag = x.Tag.ToString();

            Temp_TB.TB_Content.IsReadOnly = false;

            Temp_TB.StatusBarCallBack += SetStatus;

            var fi = new FileInfo(x.Tag.ToString());

            if (sTxt_list.IndexOf(fi.Extension) == -1)
            {
                MessageBox.Show("file type is not Text");
                return;
            }

            TEMP.Content = Temp_TB;
            bool bCheck = true;

            for (int nCnt = 0; nCnt < TopTap.Children.Count; nCnt++)
            {
                bCheck = bCheck & TopTap.Children[nCnt].Title != TEMP.Title;
            }

            if (true == bCheck)
            {
                TopTap.Children.Add(TEMP);
                TopTap.SelectedContentIndex = TopTap.Children.Count - 1;
            }
            else
            {
                for (int nCnt = 0; nCnt < TopTap.Children.Count; nCnt++)
                {
                    if (TopTap.Children[nCnt].Title == TEMP.Title)
                    {
                        TopTap.SelectedContentIndex = nCnt;
                    }
                }
            }
        }

        private void SetStatus(string sLog)
        {
            Tb_StatusBar.Text = sLog;
        }

        private void Log(string sLog)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string sDirPath = Path.Combine(fileInfo.DirectoryName, "Log");

            Directory.CreateDirectory(sDirPath);

            {
                string date = DateTime.Now.ToString("yyyy'-'MM'-'dd");

                date = date.Replace("-", "_");

                string projectName = Assembly.GetExecutingAssembly().GetName().Name + ".log";

                string writefile = Path.Combine(sDirPath, date + projectName);

                string format = "[" + DateTime.Now.ToString("yyyy':'MM':'dd'-'HH':'mm':'ss") + "]";

                sLog = format + sLog;

                StreamWriter sw = new StreamWriter(Path.Combine(sDirPath, writefile), File.Exists(Path.Combine(sDirPath, writefile)));

                sw.Write(sLog);

                sw.Flush();

                sw.Close();
            }
        }

        private void Treeview_SelectedChanged(object sender, RoutedEventArgs e)
        {
            var Y = sender as TreeView;

            if (Y.SelectedItem is _File_)
                ShowFilePropertise(Y.SelectedItem as _File_);

            else if (Y.SelectedItem is _Folder_)
                ShowFolderPropertise(Y.SelectedItem as _Folder_);

        }

        private void ShowFolderPropertise(_Folder_ x)
        {
            string sFolderPath = string.Empty;
            DirectoryInfo di = null;
            string sDirInfo = string.Empty;

            sFolderPath = x.Tag.ToString();
            di = new DirectoryInfo(sFolderPath);
            sDirInfo = string.Format("File Name :{0}         \n" +
                                          "Create Date:{1}        \n" +
                                          "Modification Date:{2}  \n",
                                          di.Name,
                                          di.CreationTime.ToString("yyyy-MM-HH-mm"),
                                          di.LastWriteTime
                                          );
            TB_Properties.Text = sDirInfo;
            TB_Content.TB_Content.Text = string.Empty;
        }

        private void ShowFilePropertise(_File_ x)
        {
            string sFilePath = string.Empty;
            FileInfo fi = null;
            string sFinfo = string.Empty;

            sFilePath = x.Tag.ToString();
            fi = new FileInfo(sFilePath);
            if (sTxt_list.IndexOf(fi.Extension) == -1)
            {
                MessageBox.Show("file type is not Text");
                return;
            }

            TB_Content.TB_Content.Text = File.ReadAllText(sFilePath);
            sFinfo = string.Format("File Name :{0}         \n" +
                                       "Create Date:{1}        \n" +
                                       "Modification Date:{2}  \n",
                                       fi.Name,
                                       fi.CreationTime.ToString("yyyy-MM-HH-mm"),
                                       fi.LastWriteTime);

            TB_Content.StatusBarCallBack += SetStatus;
            TB_Content.ReplaceCallBack += TB_Content_ReplaceCallBack;
            TB_Content.SearchCallBack += TB_Content_SearchCallBack;
            TB_Content.MoveToLineCallBack += TB_Content_MoveToLineCallBack;

            TB_Properties.Text = sFinfo;
        }

        private void TB_Content_StatusBarCallBack(string sLog)
        {
            throw new NotImplementedException();
        }

        private void TB_Content_MoveToLineCallBack(int nIndex)
        {
            int lineIdx = nIndex;

            if (lineIdx >= 0 && lineIdx < TB_Content.TB_Content.LineCount)
            {
                int lineStartPosition = TB_Content.TB_Content.GetCharacterIndexFromLineIndex(lineIdx);
                int lineEndPosition = TB_Content.TB_Content.GetCharacterIndexFromLineIndex(lineIdx + 1);
                TB_Content.TB_Content.Focus();
                TB_Content.TB_Content.Select(lineStartPosition, lineEndPosition - lineStartPosition);
            }
        }

        private void TB_Content_SearchCallBack(string sLog)
        {

        }

        private void TB_Content_ReplaceCallBack(string sSearchWord, string sReplaceWord)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TB_RootPath.Text.ToString()))
            {
                MessageBox.Show("Please Check Path.");
                return;
            }

            var x = sender as TextBox;

            if (x == null)
                return;

            var fi_items = Directory.GetFiles(folderTreeView.Tag.ToString(), x.Text.ToString());


        }

        private void Btn_SetDialog(object sender, RoutedEventArgs e)
        {
            TB_RootPath.Text = "";
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TB_RootPath.Text = cofd.FileName;
            }

            folderTreeView.Items.Clear();
            TB_Content.TB_Content.Text = string.Empty;
            TB_Properties.Text = string.Empty;

            if ("" == TB_RootPath.Text.ToString())
                return;

            INITUI(TB_RootPath.Text.ToString());

            Tap1.Title = TB_RootPath.Text.ToString();

            vs.Clear();
            var temp = folderTreeView.Items[0] as TreeViewItem;
            temp.IsExpanded = true;
        }

        private void Tap1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var x = sender as LayoutDocument;

            if (x.Title == Tap1.Title)
            {
                MessageBox.Show("Not Support Function. Plesae Call Engineer");
                e.Cancel = true;
            }
        }
    }
}
