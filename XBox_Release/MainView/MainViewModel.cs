using Caliburn.Micro;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xceed.Wpf.AvalonDock.Layout;

namespace XBox
{
    [Export(typeof(IMainViewModel))]
    public sealed partial class MainViewModel : Screen,IMainViewModel
    {
        public List<string> sTxt_list = new List<string>();

        public List<string> sImage_list = new List<string>();

        public List<string> vs = new List<string>();

        public TreeViewItem SelectedTreeviewItem = new TreeViewItem();

        public MainView _MainView_ = null;

        [ImportingConstructor]
        public MainViewModel()
        {
            SetTextType();
        }

        public void Window_Loaded(object dataContext)
        {
            var x = dataContext as MainViewModel;

            _MainView_ = x.GetView() as MainView;
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
                sTxt_list.Add("");

                if (sLine.IndexOf("IMAGETYPE") > -1)
                {
                    var strTemp = sLine.Split('=');

                    strTemp = strTemp[1].Split(',');

                    for (int nCnt = 0; nCnt < strTemp.Length; nCnt++)
                    {
                        if (false == string.IsNullOrWhiteSpace(strTemp[nCnt].ToString()))
                        {
                            sImage_list.Add(strTemp[nCnt].ToString());
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


            sw.Write("IMAGETYPE=");
            sw.Write(".BMP      ,".Replace(" ", ""));
            sw.Write(".RLE      ,".Replace(" ", ""));
            sw.Write(".JPEG      ,".Replace(" ", ""));
            sw.Write(".JPG      ,".Replace(" ", ""));
            sw.Write(".GIF      ,".Replace(" ", ""));
            sw.Write(".PNG      ,".Replace(" ", ""));
            sw.Write(".PSD      ,".Replace(" ", ""));
            sw.Write(".PDD      ,".Replace(" ", ""));
            sw.Write(".TIFF      ,".Replace(" ", ""));
            sw.Write(".TIF      ,".Replace(" ", ""));
            sw.Write(".Exif      ,".Replace(" ", ""));
            sw.Write(".PDF      ,".Replace(" ", ""));
            sw.Write(".RAW      ,".Replace(" ", ""));
            sw.Write(".AI      ,".Replace(" ", ""));
            sw.Write(".PCX      ,".Replace(" ", ""));
            sw.Write(".EPS      ,".Replace(" ", ""));
            sw.Write(".SVG      ,".Replace(" ", ""));
            sw.Write(".SVGZ      ,".Replace(" ", ""));
            sw.Write(".IFF      ,".Replace(" ", ""));
            sw.Write(".FPX      ,".Replace(" ", ""));
            sw.Write(".FRM      ,".Replace(" ", ""));
            sw.Write(".PCT      ,".Replace(" ", ""));
            sw.Write(".PIC      ,".Replace(" ", ""));
            sw.Write(".PXR      ,".Replace(" ", ""));
            sw.Write(".SCT      ,".Replace(" ", ""));
            sw.Write(".TGA      ,".Replace(" ", ""));
            sw.Write(".VDA      ,".Replace(" ", ""));
            sw.Write(".ICB      ,".Replace(" ", ""));
            sw.Write(".VST      ,".Replace(" ", ""));
            sw.Write(".PPM      ,".Replace(" ", ""));
            sw.Write(".PGM      ,".Replace(" ", ""));
            sw.Write(".PBM      ,".Replace(" ", ""));
            sw.Write(".PNM      ,".Replace(" ", ""));
            sw.Write(".WEBP      ,".Replace(" ", ""));
            sw.Write(".BPG      ,".Replace(" ", ""));
            sw.Write(".CGM      ,".Replace(" ", ""));
            sw.Write(".SVG      ,".Replace(" ", ""));
            sw.Write(".HEIC      ,".Replace(" ", ""));
            sw.Close();
        }

        public void Btn_SetPath()
        {
            Reset();
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _MainView_.TB_RootPath.Text = cofd.FileName;

                INITUI(_MainView_.TB_RootPath.Text.ToString());

                _MainView_.Tap1.Title = _MainView_.TB_RootPath.Text.ToString();

                vs.Clear();
                var temp = _MainView_.folderTreeView.Items[0] as TreeViewItem;
                temp.IsExpanded = true;
            }
        }

        public void FindFileName()
        {

        }

        private void Reset()
        {
            _MainView_.TB_RootPath.Text = "";
            _MainView_.folderTreeView.Items.Clear();
            _MainView_.TB_Content.TB_Content.Text = string.Empty;
            _MainView_.TB_Properties.Text = string.Empty;
            _MainView_.Img_Content.Source = null;
        }

        private void INITUI(string rootFolderPath = null)
        {
            if (rootFolderPath == null)
            {
                rootFolderPath = @"D:\Job"; // 설정한 폴더 경로로 변경하세요.
                return;
            }

            _MainView_.folderTreeView.Tag = rootFolderPath;
            _MainView_.folderTreeView.PreviewKeyDown += FolderTreeView_PreviewKeyDown;
            _MainView_.folderTreeView.SelectedItemChanged += Treeview_SelectedChanged;
            _MainView_.folderTreeView.Items.Add(MakeFolderTree(rootFolderPath));
        }

        private void Treeview_SelectedChanged(object sender, RoutedEventArgs e)
        {
            var Y = sender as TreeView;

            if (Y.SelectedItem is _TxT_)
                ShowFilePropertise(Y.SelectedItem as _TxT_);

            else if (Y.SelectedItem is _Img_)
                ShowImgPropertise(Y.SelectedItem as _Img_);

            else if (Y.SelectedItem is _Folder_)
                ShowFolderPropertise(Y.SelectedItem as _Folder_);

        }
        
        private void ShowImgPropertise(_Img_ img_)
        {
            string sFilePath = string.Empty;
            FileInfo fi = null;
            string sFinfo = string.Empty;

            sFilePath = img_.Tag.ToString();
            fi = new FileInfo(sFilePath);

            _MainView_.TB_Content.Visibility = Visibility.Collapsed;
            _MainView_.Img_Content.Visibility = Visibility.Visible;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(sFilePath, UriKind.Absolute);
            bitmapImage.EndInit();
            _MainView_.Img_Content.Source = bitmapImage;


            sFinfo = string.Format("File Name :{0}         \n" +
                                       "Create Date:{1}        \n" +
                                       "Modification Date:{2}  \n",
                                       fi.Name,
                                       fi.CreationTime.ToString("yyyy-MM-HH-mm"),
                                       fi.LastWriteTime);

            _MainView_. TB_Content.StatusBarCallBack += SetStatus;
            _MainView_. TB_Content.ReplaceCallBack += TB_Content_ReplaceCallBack;
            _MainView_. TB_Content.SearchCallBack += TB_Content_SearchCallBack;
            _MainView_. TB_Content.MoveToLineCallBack += TB_Content_MoveToLineCallBack;

            _MainView_.TB_Properties.Text = sFinfo;
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
            _MainView_.TB_Properties.Text = sDirInfo;
            _MainView_. TB_Content.TB_Content.Text = string.Empty;
        }

        private void ShowFilePropertise(_TxT_ x)
        {
            string sFilePath = string.Empty;
            FileInfo fi = null;
            string sFinfo = string.Empty;

            sFilePath = x.Tag.ToString();
            fi = new FileInfo(sFilePath);

            _MainView_.TB_Content.Visibility = Visibility.Visible;
            _MainView_.Img_Content.Visibility = Visibility.Collapsed;


            _MainView_.TB_Content.TB_Content.Text = File.ReadAllText(sFilePath);
            sFinfo = string.Format("File Name :{0}         \n" +
                                       "Create Date:{1}        \n" +
                                       "Modification Date:{2}  \n",
                                       fi.Name,
                                       fi.CreationTime.ToString("yyyy-MM-HH-mm"),
                                       fi.LastWriteTime);

            _MainView_.TB_Content.StatusBarCallBack += SetStatus;
            _MainView_.TB_Content.ReplaceCallBack += TB_Content_ReplaceCallBack;
            _MainView_.TB_Content.SearchCallBack += TB_Content_SearchCallBack;
            _MainView_.TB_Content.MoveToLineCallBack += TB_Content_MoveToLineCallBack;

            _MainView_.TB_Properties.Text = sFinfo;
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

        private _Folder_ MakeFolderTree(string FolderPath)
        {
            var di_folder = new DirectoryInfo(FolderPath);
            var temp_tv_item = new _Folder_();

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
                    var temp = new _TxT_();
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

        public void ShowMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
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

        private void SetStatus(string sLog)
        {
            _MainView_.Tb_StatusBar.Text = sLog;
        }

        private void TB_Content_MoveToLineCallBack(int nIndex)
        {
            int lineIdx = nIndex;

            if (lineIdx >= 0 && lineIdx < _MainView_.TB_Content.TB_Content.LineCount)
            {
                int lineStartPosition = _MainView_.TB_Content.TB_Content.GetCharacterIndexFromLineIndex(lineIdx);
                int lineEndPosition = _MainView_.TB_Content.TB_Content.GetCharacterIndexFromLineIndex(lineIdx + 1);
                _MainView_.TB_Content.TB_Content.Focus();
                _MainView_.TB_Content.TB_Content.Select(lineStartPosition, lineEndPosition - lineStartPosition);
            }
        }

        private void TB_Content_SearchCallBack(string sLog)
        {

        }

        private void TB_Content_ReplaceCallBack(string sSearchWord, string sReplaceWord)
        {

        }


        private void Temp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is _TxT_)
            {
                var x = sender as _TxT_;
                var fi_item = new FileInfo(x.Tag.ToString());
                _MainView_.TB_Content.TB_Content.Text = File.ReadAllText(x.Tag.ToString());
            }
            else if (sender is _Img_)
            {
                _MainView_.TB_Content.Visibility = Visibility.Collapsed;
                _MainView_.Img_Content.Visibility = Visibility.Visible;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri((sender as _Img_).Tag.ToString(), UriKind.Absolute);
                bitmapImage.EndInit();
                _MainView_.Img_Content.Source = bitmapImage;
            }
            else
            {

            }
        }

        private void Temp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var x = sender as _TxT_;

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

            for (int nCnt = 0; nCnt < _MainView_.TopTap.Children.Count; nCnt++)
            {
                bCheck = bCheck & _MainView_.TopTap.Children[nCnt].Title != TEMP.Title;
            }
            
            if (true == bCheck)
            {
                _MainView_.TopTap.Children.Add(TEMP);
                _MainView_.TopTap.SelectedContentIndex = _MainView_.TopTap.Children.Count - 1;
            }
            else
            {
                for (int nCnt = 0; nCnt < _MainView_.TopTap.Children.Count; nCnt++)
                {
                    if (_MainView_.TopTap.Children[nCnt].Title == TEMP.Title)
                    {
                        _MainView_.TopTap.SelectedContentIndex = nCnt;
                    }
                }
            }
        }

    }
}
