using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace XBox
{
    /// <summary>
    /// TextEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TextEditor : UserControl, INotifyPropertyChanged
    {
        public List<string> sTxt_list = new List<string>();

        public List<string> sImage_list = new List<string>();

        public List<string> vs = new List<string>();

        // ① DependencyProperty 정의
        public static readonly DependencyProperty sTB_ContentProperty =
            DependencyProperty.Register(
                "sTB_Content",
                typeof(string),
                typeof(TextEditor),
                new FrameworkPropertyMetadata(
                    "",
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    //,OnSTB_ContentChanged
                    ));

        // ② CLR Wrapper
        public string sTB_Content
        {
            get => (string)GetValue(sTB_ContentProperty);
            set => SetValue(sTB_ContentProperty, value);
        }

        // ③ 값 변경 시 콜백
        //private static void OnSTB_ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var editor = d as TextEditor;
        //    var newValue = e.NewValue as string;


        //    if (Directory.Exists(newValue))
        //    {
        //        var di = new DirectoryInfo(newValue);
        //    }
        //    else
        //    {

        //    }
        //}

        public class DataItem
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        private ObservableCollection<DataItem> _items = new ObservableCollection<DataItem>();
        public ObservableCollection<DataItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items)); // 바뀌었음을 알림
            }
        }

        public TextEditor()
        {
            InitializeComponent();
            Property.ItemsSource = Items;

            DependencyPropertyDescriptor.FromProperty(sTB_ContentProperty, typeof(TextEditor))
                .AddValueChanged(this, (s, e) =>
                {
                    var x = s as TextEditor;

                    SetContent(x.sTB_Content);

                    SetProperty(x.sTB_Content);


                });
        }

        private void SetContent(string sTB_Content)
        {
            Debug.WriteLine($"[SetContent] 경로: {sTB_Content}");

            if (File.Exists(sTB_Content))
            {
                string content = File.ReadAllText(sTB_Content);
                Debug.WriteLine($"[SetContent] 내용 길이: {content.Length}");

                TB_Content.Text = content;

                int lineCount = content.Split('\n').Length;

                var lineNumbers = new StringBuilder();
                for (int i = 1; i <= lineCount; i++)
                {
                    lineNumbers.AppendLine(i.ToString());
                }

                TBL_LineNumber.Text = lineNumbers.ToString();
            }
            else
            {
                Debug.WriteLine("[SetContent] 파일이 존재하지 않음");
                TB_Content.Text = string.Empty;
                TBL_LineNumber.Text = string.Empty;
            }
        }


        private void ContentScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // 수직 스크롤 위치만 맞춰줌
            LineNumberScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
        }



        private void SetProperty(string sTB_Content)
        {

            if (Items.Count > 0)
                Items.Clear();

            if (Directory.Exists(sTB_Content))
            {
                var di = new DirectoryInfo(sTB_Content);

                Items.Add(new DataItem { Key = "Name", Value = di.Name });
                Items.Add(new DataItem { Key = "Create DateTime", Value = di.CreationTime.ToString("yyyy.MM.dd-HH:mm") });
                Items.Add(new DataItem { Key = "Modify DateTime", Value = di.LastWriteTime.ToString("yyyy.MM.dd-HH:mm") });


            }
            else
            {
                var fi = new FileInfo(sTB_Content);

                Items.Add(new DataItem { Key = "Name", Value = fi.Name });
                Items.Add(new DataItem { Key = "Create DateTime", Value = fi.CreationTime.ToString("yyyy.MM.dd-HH:mm") });
                Items.Add(new DataItem { Key = "Modify DateTime", Value = fi.LastWriteTime.ToString("yyyy.MM.dd-HH:mm") });
            }

            Property.ItemsSource = Items;
        }

        #region Opt Windows

        #endregion


        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion

        #region  Show File Opt

        private void ShowTXTPropertise(_TxT_ txt)
        {
            string sFilePath = string.Empty;
            FileInfo fi = null;
            string sFinfo = string.Empty;

            sFilePath = txt.Tag.ToString();
            fi = new FileInfo(sFilePath);


            sFinfo = string.Format("File Name :{0}         \n" +
                                       "Create Date:{1}        \n" +
                                       "Modification Date:{2}  \n",
                                       fi.Name,
                                       fi.CreationTime.ToString("yyyy-MM-HH-mm"),
                                       fi.LastWriteTime);
            //Property_Text = sFinfo;
            //TB_Property.Text = sFinfo;
        }

        private void ShowImgPropertise(_Img_ Img)
        {
            string sFilePath = string.Empty;
            FileInfo fi = null;
            string sFinfo = string.Empty;

            sFilePath = Img.Tag.ToString();
            fi = new FileInfo(sFilePath);


            sFinfo = string.Format("File Name :{0}         \n" +
                                       "Create Date:{1}        \n" +
                                       "Modification Date:{2}  \n",
                                       fi.Name,
                                       fi.CreationTime.ToString("yyyy-MM-HH-mm"),
                                       fi.LastWriteTime);
            //Property_Text = sFinfo;
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
            //Property_Text = sDirInfo;
        }

        #endregion

        #region INIT

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
                sTxt_list.Add(".PRO".ToUpper());

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

        #endregion INIT

    }

}
