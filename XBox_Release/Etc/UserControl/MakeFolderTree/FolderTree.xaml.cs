using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace XBox
{
    /// <summary>
    /// TextEditor.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class FolderTree : TreeView , INotifyPropertyChanged
    {

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion PropertyChanged

        public ObservableCollection<_Folder_> _FolderTreeview_items = new ObservableCollection<_Folder_>();
        public ObservableCollection<_Folder_> FolderTreeview_items 
        {
            get
            {
                return _FolderTreeview_items;
            }
            set
            {
                _FolderTreeview_items = value;
                OnPropertyChanged();
            }
        }

        public string sTB_Content
        {
            get
            {
                //this.TB_Content
                return (string)GetValue(sTB_ContentProperty);
            }
            set
            {
                SetValue(sTB_ContentProperty, value);
            }

        }

        public static DependencyProperty sTB_ContentProperty =
            DependencyProperty.Register("sTB_Content", typeof(string), typeof(FolderTree),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public ObservableObject<>

        public FolderTree()
        {
            InitializeComponent();

            SetTextType();

            sTxt_list = sTxt_list.FindAll(s => !string.IsNullOrWhiteSpace(s));

            sImage_list = sImage_list.FindAll(s => !string.IsNullOrWhiteSpace(s));


            Binding binding = new Binding("sTB_Content");
            binding.Source = this;

            // PropertyChanged 이벤트 핸들러 추가
            DependencyPropertyDescriptor.FromProperty(sTB_ContentProperty, typeof(TextEditor))
                .AddValueChanged(this, (sender, args) =>
                {
                    // 여기에 Text 변경 시 실행할 코드를 추가
                    // 예를 들어 YourTextChanged 이벤트를 발생시킬 수 있습니다.

                    var x = sender as FolderTree;

                    if(x!=null)
                    {
                        if(!string.IsNullOrWhiteSpace(x.sTB_Content))
                        {
                            string sData = x.sTB_Content;

                            if(MakeFolderTree(sData)!=null)
                            {
                                FolderTreeview_items.Add(MakeFolderTree(sData));

                                if (null == x.ItemsSource)
                                    x.ItemsSource = FolderTreeview_items;
                            }
                        }
                        else
                        {
                            FolderTreeview_items.Clear();
                        }


                    }
                    FolderTreeview_items[0].IsExpanded = true;
                });
        }

        private _Folder_ MakeFolderTree(string FolderPath)
        {
            var di_folder = new DirectoryInfo(FolderPath);
            var temp_tv_item = new _Folder_();
        
            temp_tv_item.TB_Header.Content = di_folder.Name;
            temp_tv_item.Tag = di_folder.FullName;
            //temp_tv_item.Selected += Folder_Selected;
            temp_tv_item.Expanded += Folder_Expanded;
        
            try
            {
                if (di_folder.GetDirectories().Length > 0)
                {
                    foreach (var item in di_folder.GetDirectories())
                    {
                        if(!string.IsNullOrWhiteSpace(item.FullName))
                        {
                            temp_tv_item.Items.Add(MakeFolderTree(item.FullName));
                            System.Diagnostics.Debug.WriteLine("item.FullName"+ item.FullName);
                        }        
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

        string sBeforeFolder_Path = "";

        private void Folder_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is _Folder_)
            {
                var x = sender as _Folder_;
                if (x.IsSelected && x.IsMouseOver)
                {
                    if (sBeforeFolder_Path != x.Tag.ToString())
                    {
                        sBeforeFolder_Path = x.Tag.ToString();
                        //ShowFolderPropertise(x);
                    }

                }
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var x = sender as _Folder_;

            if (x != null)
            {
                var di_spath = new DirectoryInfo(x.Tag.ToString());
                var fi = di_spath.GetFiles();
                for (int nCnt = 0; nCnt < fi.Count(); nCnt++)
                {
                    if(fi[nCnt].Exists)
                    {
                        if (sTxt_list.IndexOf(fi[nCnt].Extension.ToUpper()) > -1)
                        {
                            _TxT_ Temp = new _TxT_();
                            Temp.Height = 40;
                            Temp.TB_Header.Content = fi[nCnt].Name;
                            Temp.Tag = fi[nCnt].FullName;

                            AddFileList(Temp, x);
                        }

                        else if (sImage_list.IndexOf(fi[nCnt].Extension.ToUpper()) > -1)
                        {
                            _Img_ Temp = new _Img_();
                            Temp.Height = 40;
                            Temp.TB_Header.Content = fi[nCnt].Name;
                            Temp.Tag = fi[nCnt].FullName;
                            Temp.Selected += Folder_Selected;
                            AddFileList(Temp, x);
                        }
                    }
                }
            }
            x.Items.Refresh();
        }


        private void AddFileList(object temp, _Folder_ x)
        {
            if (temp is _TxT_)
            {
                var m_TxT = temp as _TxT_;
                if (vs.IndexOf(m_TxT.Tag.ToString()) == -1)
                {
                    x.Items.Add(temp);
                    vs.Add(m_TxT.Tag.ToString());
                }
            }

            if (temp is _Img_)
            {
                var m_TxT = temp as _Img_;
                if (vs.IndexOf(m_TxT.Tag.ToString()) == -1)
                {
                    x.Items.Add(temp);
                    vs.Add(m_TxT.Tag.ToString());
                }
            }
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

            string sConfigPath = System.IO.Path.Combine(sDirPath, "Config.ini");


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

        public List<string> vs = new List<string>();

        public List<string> sTxt_list = new List<string>();

        public List<string> sImage_list = new List<string>();

    }
}