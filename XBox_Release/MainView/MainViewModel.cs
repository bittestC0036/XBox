using Caliburn.Micro;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.AvalonDock.Layout;

namespace XBox
{
    [Export(typeof(IMainViewModel))]
    public sealed class MainViewModel : ObservableObject, IMainViewModel, INotifyPropertyChanged
    {
        public List<string> sTxt_list = new List<string>();

        public List<string> sImage_list = new List<string>();

        public List<string> vs = new List<string>();

        public MainView _MainView_ = null;

        #region Binding Property

        private ObservableCollection<TreeViewItem> _FolderTreeview_items = new ObservableCollection<TreeViewItem>();

        public ObservableCollection<TreeViewItem> FolderTreeview_items
        {
            get
            {
                return _FolderTreeview_items;
            }
            set
            {
                SetProperty(ref _FolderTreeview_items, value);
            }
        }

        private ObservableCollection<LayoutContent> _TopTap_items = new ObservableCollection<LayoutContent>();

        public ObservableCollection<LayoutContent> TopTap_items
        {
            get
            {
                return _TopTap_items;
            }
            set
            {
                SetProperty(ref _TopTap_items, value);
            }
        }

        private string _TB_RootPath_Text = "";
        public string TB_RootPath_Text
        {
            get 
            { 
                return _TB_RootPath_Text; 
            }
            set
            {
                SetProperty(ref _TB_RootPath_Text, value);
            }
        }

        private string _sStatusBarText = "";
        public string sStatusBarText
        {
            get
            {
                return _sStatusBarText;
            }
            set
            {
                SetProperty(ref _sStatusBarText, value);
            }
        }

        private string _Tap1_Title = "";
        public string Tap1_Title
        {
            get { return _Tap1_Title; }
            set
            {
                SetProperty(ref _Tap1_Title, value);
            }
        }

        private string _folderTreeview_Tag = "";
        public string folderTreeview_Tag
        {
            get { return _folderTreeview_Tag; }
            set
            {
                SetProperty(ref _folderTreeview_Tag, value);
            }
        }

        private string _TB_Content_Text = "";
        public string TB_Content_Text
        {
            get { return _TB_Content_Text; }
            set
            {
                SetProperty(ref _TB_Content_Text, value);
            }
        }

        private string _TB_Content_Content = "";
        public string TB_Content_Content
        {
            get { return _TB_Content_Content; }
            set
            {
                SetProperty(ref _TB_Content_Content, value);
            }
        }

        private string _TB_Properties_Text = "";
        public string TB_Properties_Text
        {
            get
            {
                return _TB_Properties_Text;
            }
            set
            {
                SetProperty(ref _TB_Properties_Text, value);
            }
        }

        private ImageSource _Image_Content_Source = null;

        public ImageSource Image_Content_Source
        {
            get
            {
                return _Image_Content_Source;
            }
            set
            {
                SetProperty(ref _Image_Content_Source, value);
            }
        }

        private Visibility _TB_Content_Visability = Visibility.Visible;

        public Visibility TB_Content_Visability
        {
            get { return _TB_Content_Visability; }
            set
            {
                SetProperty(ref _TB_Content_Visability, value);
            }
        }

        private Visibility _Img_Content_Visability = Visibility.Collapsed;

        public Visibility Img_Content_Visability
        {
            get { return _Img_Content_Visability; }
            set
            {
                SetProperty(ref _Img_Content_Visability, value);
            }
        }




        #endregion Binding Property

        #region Creator
        [ImportingConstructor]
        public MainViewModel()
        {
            SetTextType();
        }

        private void INITUI(string rootFolderPath = null)
        {
            if (rootFolderPath == null)
            {
                rootFolderPath = @"D:\Job"; // 설정한 폴더 경로로 변경하세요.
                return;
            }

            TB_RootPath_Text = rootFolderPath;            
            FolderTreeview_items.Add(MakeFolderTree(TB_RootPath_Text));
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
                            sTxt_list.Add(strTemp[nCnt].ToUpper());
                        }
                    }
                }
                sTxt_list.Add(".taos_lin".ToUpper());
                sTxt_list.Add(".bat".ToUpper());
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
            sw.Write(".txt      ,".Replace(" ", "").ToUpper());
            sw.Write(".c        ,".Replace(" ", "").ToUpper());
            sw.Write(".cs       ,".Replace(" ", "").ToUpper());
            sw.Write(".cpp      ,".Replace(" ", "").ToUpper());
            sw.Write(".log      ,".Replace(" ", "").ToUpper());
            sw.Write(".h        ,".Replace(" ", "").ToUpper());
            sw.Write(".csv      ,".Replace(" ", "").ToUpper());
            sw.Write(".xml      ,".Replace(" ", "").ToUpper());
            sw.Write(".json     ,".Replace(" ", "").ToUpper());
            sw.Write(".html     ,".Replace(" ", "").ToUpper());
            sw.Write(".css      ,".Replace(" ", "").ToUpper());
            sw.Write(".cfg      ,".Replace(" ", "").ToUpper());
            sw.Write(".ini      ,".Replace(" ", "").ToUpper());
            sw.Write(".sql      ,".Replace(" ", "").ToUpper());
            sw.Write(".config   ,".Replace(" ", "").ToUpper());
            sw.Write(".pro      ,".Replace(" ", "").ToUpper());
            sw.Write(".pro.user ,".Replace(" ", "").ToUpper());
            sw.Write(".csproj   ,".Replace(" ", "").ToUpper());
            sw.Write(".settings ,".Replace(" ", "").ToUpper());
            sw.Write(".sln      ,".Replace(" ", "").ToUpper());
            sw.Write(".ui       ,".Replace(" ", "").ToUpper());
            sw.Write(".idx      ,".Replace(" ", "").ToUpper());
            sw.Write(".suo      ,".Replace(" ", "").ToUpper());
            sw.Write(".o        ,".Replace(" ", "").ToUpper());
            sw.Write(".obj      ,".Replace(" ", "").ToUpper());
            sw.Write(".java     ,".Replace(" ", "").ToUpper());
            sw.Write(".class     ".Replace(" ", "").ToUpper());
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
        #endregion Creator

        #region Event Command
        public void Window_Loaded(object dataContext, object view)
        {
            var x = dataContext as MainViewModel;

            var y = view as MainView;

            _MainView_ = y;
        }

        public void Btn_SetPath()
        {
            Reset();
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TB_RootPath_Text = cofd.FileName;
                
                INITUI(TB_RootPath_Text);

                Tap1_Title = TB_RootPath_Text;

                vs.Clear();

                var temp = FolderTreeview_items[0];
                temp.IsExpanded = true;
            }
        }

        public void FindFileName()
        {

        }
        
        #endregion Event Command

        #region Instance Method

        private void Reset()
        {
            _TB_RootPath_Text = "";
            FolderTreeview_items.Clear();
            _TB_Content_Content = string.Empty;
            _TB_Properties_Text = string.Empty;
            _Image_Content_Source = null;

        }

        private _Folder_ MakeFolderTree(string FolderPath)
        {
            var di_folder = new DirectoryInfo(FolderPath);
            var temp_tv_item = new _Folder_();

            temp_tv_item.TB_Header.Content = di_folder.Name;
            temp_tv_item.Tag = di_folder.FullName;

            try
            {
                if (di_folder.GetDirectories().Count() > 0)
                {
                    foreach (var item in di_folder.GetDirectories())
                    {
                        temp_tv_item.Items.Add(MakeFolderTree(item.FullName));
                        temp_tv_item.Selected += Temp_tv_item_Selected;
                        temp_tv_item.Expanded += Temp_tv_item_Expanded;
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

        private void Temp_tv_item_Selected(object sender, RoutedEventArgs e)
        {
            if ( sender is _Folder_) 
            {
                var x = sender as _Folder_;
                if(x.IsSelected)
                {
                    ShowFolderPropertise(x);
                }
            }
        }

        private void ShowTXTPropertise(_TxT_ txt)
        {
            string sFilePath = string.Empty;
            FileInfo fi = null;
            string sFinfo = string.Empty;

            sFilePath = txt.Tag.ToString();
            fi = new FileInfo(sFilePath);

            TB_Content_Visability = Visibility.Visible;
            Img_Content_Visability = Visibility.Collapsed;

            sFinfo = string.Format("File Name :{0}         \n" +
                                       "Create Date:{1}        \n" +
                                       "Modification Date:{2}  \n",
                                       fi.Name,
                                       fi.CreationTime.ToString("yyyy-MM-HH-mm"),
                                       fi.LastWriteTime);
            TB_Properties_Text = sFinfo;
            SetStatus(txt.Tag.ToString() + " Open");
        }

        private void ShowImgPropertise(_Img_ Img)
        {
            string sFilePath = string.Empty;
            FileInfo fi = null;
            string sFinfo = string.Empty;

            sFilePath = Img.Tag.ToString();
            fi = new FileInfo(sFilePath);

            Img_Content_Visability = Visibility.Collapsed; //TB_Content_Visability = Visibility.Visible;
            Img_Content_Visability = Visibility.Visible ;  //Img_Content_Visability = Visibility.Collapsed;

            sFinfo = string.Format("File Name :{0}         \n" +
                                       "Create Date:{1}        \n" +
                                       "Modification Date:{2}  \n",
                                       fi.Name,
                                       fi.CreationTime.ToString("yyyy-MM-HH-mm"),
                                       fi.LastWriteTime);
            TB_Properties_Text = sFinfo;
            SetStatus(Img.Tag.ToString() + " Open");
        }

        private void ShowFolderPropertise(_Folder_ x)
        {
            string sFolderPath = string.Empty;
            DirectoryInfo di = null;
            string sDirInfo = string.Empty;

            sFolderPath = x.Tag.ToString();
            di = new DirectoryInfo(sFolderPath);
            sDirInfo = string.Format("Folder Name :{0}         \n" +
                                          "Create Date:{1}        \n" +
                                          "Modification Date:{2}  \n",
                                          di.Name,
                                          di.CreationTime.ToString("yyyy-MM-HH-mm"),
                                          di.LastWriteTime
                                          );
            TB_Properties_Text = sDirInfo;
            TB_Content_Content = string.Empty;
        }


        #endregion Instance Method

        #region Event 


        public void FolderTreeView_PreviewKeyDown()
        {
            System.Diagnostics.Debug.WriteLine("Hello");
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
                    if (sTxt_list.IndexOf(fi[nCnt].Extension.ToUpper()) >-1)
                    {
                        _TxT_ Temp = new _TxT_();
                        Temp.Height = 40;
                        Temp.TB_Header.Content = fi[nCnt].Name;
                        Temp.Tag = fi[nCnt].FullName;

                        Temp.MouseDoubleClick += Temp_MouseDoubleClick;
                        Temp.Selected += Temp_Selected;
                        //Temp.MouseUp += Temp_MouseUp;
                        
                        AddFileList(Temp, x);
                    }

                    else if (sImage_list.IndexOf(fi[nCnt].Extension.ToUpper()) > -1)
                    {
                        _Img_ Temp = new _Img_();
                        Temp.Height = 40;
                        Temp.TB_Header.Content = fi[nCnt].Name;
                        Temp.Tag = fi[nCnt].FullName;
                        Temp.Selected += Temp_Selected;
                        //Temp.MouseUp += Temp_MouseUp;
                        AddFileList(Temp, x);
                    }
                }
            }
        }

        private void Temp_Selected(object sender, RoutedEventArgs e)
        {
            if( sender is _TxT_)
            {
                var x = sender as _TxT_;
                var fi_item = new FileInfo(x.Tag.ToString());
                TB_Content_Content = File.ReadAllText(x.Tag.ToString());

                ShowTXTPropertise(x);
            }
            else if (sender is _Img_)
            {
                TB_Content_Visability = Visibility.Collapsed;
                Img_Content_Visability = Visibility.Visible;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri((sender as _Img_).Tag.ToString(), UriKind.Absolute);
                bitmapImage.EndInit();
                Image_Content_Source = bitmapImage;

                ShowImgPropertise(sender as _Img_);
            }
            //else if (sender is _Video_)
            //{
            //
            //}
        }

        private void AddFileList(object temp, _Folder_ x)
        {
            if(temp is _TxT_)
            {
                var m_TxT = temp as _TxT_;
                if (vs.IndexOf(m_TxT.Tag.ToString()) == -1)
                {
                    x.Items.Add(temp);
                    vs.Add(m_TxT.Tag.ToString());
                }
            }

            if(temp is _Img_)
            {
                var m_TxT = temp as _Img_;
                if (vs.IndexOf(m_TxT.Tag.ToString()) == -1)
                {
                    x.Items.Add(temp);
                    vs.Add(m_TxT.Tag.ToString());
                }
            }

        }

        private void Temp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is _TxT_)
            {
                var x = sender as _TxT_;
                var fi_item = new FileInfo(x.Tag.ToString());
                TB_Content_Content = File.ReadAllText(x.Tag.ToString());

                ShowTXTPropertise(x);
            }
            else if (sender is _Img_)
            {
                TB_Content_Visability = Visibility.Collapsed;
                Img_Content_Visability = Visibility.Visible;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri((sender as _Img_).Tag.ToString(), UriKind.Absolute);
                bitmapImage.EndInit();
                Image_Content_Source = bitmapImage;

                ShowImgPropertise(sender as _Img_);
            }
        }

        private void Temp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var x = sender as _TxT_;
        
            if (null == x)
                return;
        
            var TEMP = new LayoutDocument();

            TEMP.IsEnabled = true;
            
            TEMP.IsSelected = true;


            TEMP.Title = x.TB_Header.ToString().Split(':')[1];
        
            var TEMP_TextEditor = new TextEditor();

            //Temp_TB.sTB_Content= File.ReadAllText(x.Tag.ToString());    //Temp_TB.TB_Content.Text = File.ReadAllText(x.Tag.ToString());

            TEMP_TextEditor.sTB_Content = File.ReadAllText(x.Tag.ToString());

            TEMP_TextEditor.Tag = x.Tag.ToString();

            TEMP_TextEditor.TB_Content.IsReadOnly = false;

            //_MainView_.TopTap.ChildrenTreeChanged += TopTap_ChildrenTreeChanged;
            
           
            var fi = new FileInfo(x.Tag.ToString());
        
            if (sTxt_list.IndexOf(fi.Extension.ToUpper()) == -1)
            {
                MessageBox.Show("file type is not Text");
                return;
            }

            //TEMP.Content = TEMP_TextEditor;
            TEMP_TextEditor.ForceCursor = true;
            LayoutDocument TEMP_Content = new LayoutDocument();
            TEMP_Content.Content = TEMP_TextEditor;
            TEMP.Content = TEMP_Content;

            TEMP_Content.IsSelected = true;
            //TEMP_Content.IsVisible = true;
            TEMP_Content.CanFloat = true;
            TEMP_Content.IsEnabled = true;

            //TEMP
            //TEMP_Content.Content

            //TEMP.Content=

            bool bCheck = true;
        
            for (int nCnt = 0; nCnt < TopTap_items.Count; nCnt++)
          {
              bCheck = bCheck & TopTap_items[nCnt].Title != TEMP.Title;
          }

            if (true == bCheck)
            {
                //TopTap_items.Add(TEMP);

                //_MainView_.TopTap.Children.Add(TEMP);

                _MainView_.TopTap.InsertChildAt(_MainView_.TopTap.Children.Count, TEMP);

                _MainView_.TopTap.SelectedContentIndex = _MainView_.TopTap.Children.Count;

                _MainView_.TopTap.Children[_MainView_.TopTap.Children.Count-1].IsActive = true;

                //_MainView_.TopTap.Children[_MainView_.TopTap.Children.Count]

                //_MainView_.TopTap.ChildrenSorted;

            }
            else
            {
            
            }
                //for (int nCnt = 0; nCnt < _MainView_.TopTap.Children.Count; nCnt++)
                //{
                //    if (_MainView_.TopTap.Children[nCnt].Title == TEMP.Title)
                //    {
                //        _MainView_.TopTap.SelectedContentIndex = nCnt;
                //    }
                //}
        }

        private void TopTap_ChildrenTreeChanged(object sender, ChildrenTreeChangedEventArgs e)
        {
            var x = sender as LayoutDocumentPane;

            //e.Change=ChildrenTreeChange

            //x.IsVisible = true;

            //x.SelectedContentIndex = x.ChildrenCount;
        }

#endregion Event

#region CallBack Method

        private void Log(string sLog)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string sDirPath = Path.Combine(fileInfo.DirectoryName, "Log");

            Directory.CreateDirectory(sDirPath);

            {
                //string date = DateTime.Now.ToString("yyyy'-'MM'-'dd");

                string date = DateTime.Now.ToString("yyyy'_'MM'_'dd");

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
            //_MainView_.Tb_StatusBar.Text = sLog;
            sStatusBarText = sLog;
        }

#endregion CallBack Method

    }
}
