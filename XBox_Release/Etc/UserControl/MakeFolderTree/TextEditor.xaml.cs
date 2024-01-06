using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

namespace XBox
{
    /// <summary>
    /// TextEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public delegate void StatusBarCallBack(string sLog);
    public delegate void SearchCallBack(string sLog);
    public delegate void MoveToLineCallBack(int nIndex);
    public delegate void ReplaceCallBack(string sSearchWord, string sReplaceWord);

    public partial class TextEditor : UserControl, INotifyPropertyChanged
    {
        public List<string> sTxt_list = new List<string>();

        public List<string> sImage_list = new List<string>();

        public List<string> vs = new List<string>();

        public event StatusBarCallBack StatusBarCallBack;
        public event SearchCallBack SearchCallBack;
        public event MoveToLineCallBack MoveToLineCallBack;
        public event ReplaceCallBack ReplaceCallBack;

        public SearchWindow SearchWindow;

        public ReplaceWindow ReplaceWindow;

        public MoveToLineWindow MoveToLineWindow;

        public int nKeyCnt = -1; // 총 있는 값

        public int nfoundnCnt = -1; // 총 있는 값

        public object sTB_Content
        {
            get {
                //this.TB_Content
                return (object)GetValue(sTB_ContentProperty);
            }
            set {
                    SetValue(sTB_ContentProperty, value); 
                }
            
        }

        public static DependencyProperty sTB_ContentProperty =
            DependencyProperty.Register("sTB_Content", typeof(object),typeof(TextEditor),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private bool _bisReadOnly = true;
        public bool bisReadOnly
        {
            get
            {
                return _bisReadOnly;
                //return (bool)GetValue(bisReadOnly_Property);
            }
            set
            {
                bisReadOnly = value;
                OnPropertyChanged();
                //SetValue(bisReadOnly_Property, value);
            }
        }

        public static DependencyProperty bisReadOnly_Property =
        DependencyProperty.Register("bisReadOnly", typeof(bool), typeof(TextEditor),
        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        private string _Property_Text = "";
        public string Property_Text
        {
            get
            {
                return _Property_Text;
            }
            set
            {
                _Property_Text = value;
                OnPropertyChanged();
            }
        }

        private Visibility _PropertyVisability = Visibility.Visible;
        public Visibility PropertyVisability
        {
            get
            {
                return _PropertyVisability;
            }
            set
            {
                if (value == Visibility.Visible)
                {
                    PropertyVisability = Visibility.Visible;
                }
                else
                {
                    PropertyVisability = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        public TextEditor()
        {
            InitializeComponent();

            //this.DataContext = this;

            Binding binding = new Binding("Property_Text");
            binding.Source = this.DataContext;
            this.TB_Property.SetBinding(TextBox.TextProperty, binding);

            Binding binding2 = new Binding("bisReadOnly");
            binding2.Source = this.DataContext;
            this.TB_Content.SetBinding(TextBox.IsReadOnlyProperty, binding2);

            Binding binding3 = new Binding("sTB_Content");
            binding3.Source = this;
            this.SetBinding(TreeView.TagProperty, binding3);


            SetTextType();

            // PropertyChanged 이벤트 핸들러 추가
            DependencyPropertyDescriptor.FromProperty(sTB_ContentProperty, typeof(TextEditor))
                .AddValueChanged(this, (sender, args) =>
                {
                     var x = sender as TextEditor;


                    if(File.Exists(x.sTB_Content.ToString()))
                    {
                        if (x.TB_Content.Tag != x.sTB_Content)
                        {
                            x.TB_Content.Text = File.ReadAllText(x.sTB_Content.ToString());
                            x.TB_Content.Tag = sTB_Content;
                            Content_TextChanged(x.TB_Content, null);
                        }
                    }else
                    {
                        x.TB_Content.Text = string.Empty;
                    }


                    var fi = new FileInfo(sTB_Content.ToString());

                    if(sTxt_list.IndexOf(fi.Extension.ToUpper())!=-1)
                    {
                        _TxT_ temp = new _TxT_();
                        temp.Tag = sTB_Content;
                        ShowTXTPropertise(temp);
                    }
                    else if(sImage_list.IndexOf(fi.Extension)!=-1)
                    {
                        _Img_ temp = new _Img_();
                        temp.Tag = sTB_Content;
                        ShowImgPropertise(temp);
                    }
                    else if(Directory.Exists(sTB_Content.ToString()))
                    {
                        _Folder_ temp = new _Folder_();
                        temp.Tag = sTB_Content;
                        ShowFolderPropertise(temp);
                    }
                });

                DependencyPropertyDescriptor.FromProperty(bisReadOnly_Property, typeof(TextEditor))
                .AddValueChanged(this, (sender, args) =>
                {

                 // 여기에 Text 변경 시 실행할 코드를 추가
                 // 예를 들어 YourTextChanged 이벤트를 발생시킬 수 있습니다.
                 
                 var x = sender as TextEditor;
                    x.bisReadOnly = true;
                });

            SearchWindow = new SearchWindow();
            SearchWindow.BtnSearch.Click += BtnSearch_Click;

            ReplaceWindow = new ReplaceWindow();
            ReplaceWindow.Btn_Replace.Click += Btn_Replace_Click;

            MoveToLineWindow = new MoveToLineWindow();

            SearchWindow.Visibility = Visibility.Collapsed;
            ReplaceWindow.Visibility = Visibility.Collapsed;
            MoveToLineWindow.Visibility = Visibility.Collapsed;

        }

        #region Opt Windows

        public string sBeforeSearchKeyword = string.Empty;
        public string sBeforeReplace_SearchKeyword = string.Empty;
        public string sBeforeReplace_ReplaceKeyword = string.Empty;

        int index_Search = -1;
        int index_Replace = -1;

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string sSearchKeyword = SearchWindow.TB_Keyword.Text.ToString();

            if (sBeforeSearchKeyword != sSearchKeyword)
            {
                index_Search = -1;
            }

            if (string.IsNullOrWhiteSpace(sSearchKeyword))
            {
                MessageBox.Show(string.Format("Please Check Keywrod again"));
                return;
            }


            index_Search = TB_Content.Text.ToString().IndexOf(sSearchKeyword, index_Search + 1);

            if (index_Search >= 0)
            {
                TB_Content.Select(index_Search, sSearchKeyword.Length); // 해당 위치로 선택
                TB_Content.Focus(); // TextBox에 포커스 설정
            }

            if (TB_Content.Text.ToString().IndexOf(sSearchKeyword, index_Search + 1) >= 0)
            {
                SearchWindow.BtnSearch.Content = "다음 찾기";
            }
            else
            {
                SearchWindow.BtnSearch.Content = "찾기";
            }

            if (index_Search == -1)
            {
                MessageBox.Show($"'{sSearchKeyword}'를 찾을 수 없습니다.");
            }
        }

        private void Btn_Replace_Click(object sender, RoutedEventArgs e)
        {
            string sSearchKeyword = ReplaceWindow.TB_Search.Text.ToString();

            string sReplaceKeyword = ReplaceWindow.TB_Replace.Text.ToString();

            if (sBeforeSearchKeyword != sSearchKeyword)
            {
                index_Replace = -1;
            }

            if (string.IsNullOrWhiteSpace(sSearchKeyword))
            {
                MessageBox.Show(string.Format("Please Check Keywrod again"));
                return;
            }


            index_Replace = TB_Content.Text.ToString().IndexOf(sSearchKeyword, index_Replace + 1);

            if (index_Replace >= 0)
            {
                TB_Content.Select(index_Replace, sSearchKeyword.Length); // 해당 위치로 선택
                TB_Content.Focus(); // TextBox에 포커스 설정
            }

            if (TB_Content.Text.ToString().IndexOf(sSearchKeyword, index_Replace + 1) >= 0)
            {
                SearchWindow.BtnSearch.Content = "다음 바꾸기";
            }
            else
            {
                SearchWindow.BtnSearch.Content = "바꾸기";
            }

            if (index_Replace == -1)
            {
                MessageBox.Show($"'{sSearchKeyword}'를 찾을 수 없습니다.");
            }
        }

        private void Content_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = sender as TextBox;

            TBL_LineNumber.Text = "";

            for (int nCnt = 1; nCnt <= x.Text.Split('\n').Count(); nCnt++)
            {
                TBL_LineNumber.Text += (nCnt).ToString() + Environment.NewLine;
            }


            int caretIndex = x.CaretIndex;
            int lineIndex = x.GetLineIndexFromCharacterIndex(caretIndex);
            int lineLength = x.GetLineLength(lineIndex);

            if (lineLength < 0)
                return;
        }

        private void Content_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var x = sender as TextBox;

            if (x == null)
                return;
        }

        private void TBScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            LineNumberScrollViewer.ScrollToHorizontalOffset(TBScrollViewer.HorizontalOffset); //System.Diagnostics.Debug.WriteLine("TBScrollViewer.HorizontalOffset is" + TBScrollViewer.HorizontalOffset);
            LineNumberScrollViewer.ScrollToVerticalOffset(TBScrollViewer.VerticalOffset);     //System.Diagnostics.Debug.WriteLine("TBScrollViewer.VerticalOffset" + TBScrollViewer.VerticalOffset);
        }

        private void TB_Content_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var x = sender as TextBox;

            if (x == null)
                return;

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                // Ctrl + S가 눌렸을 때 원하는 동작 수행 // 예를 들어, 파일 저장 기능을 호출하거나 다른 작업을 수행할 수 있습니다.
                if (x.Text != File.ReadAllText(x.Tag.ToString()))
                {
                    var sw = new StreamWriter(x.Tag.ToString());
                    sw.Write(x.Text.ToString());
                    sw.Flush();
                    sw.Close();
                }
                e.Handled = true; // 이벤트 처리 완료
                StatusBarCallBack("File is saved Succesfully"); //StatusBarCallBack(string.Format("{0} : {1}",x.Tag.ToString()," is Save Sucessfully"));
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.G) // 줄 이동
            {
                if (MoveToLineWindow.Visibility != Visibility.Visible)
                {
                    MoveToLineWindow.Show();
                }
                StatusBarCallBack("줄 이동");
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F) //찾기
            {
                if (SearchWindow.Visibility != Visibility.Visible)
                {
                    SearchWindow.Show();
                }
                StatusBarCallBack("찾기");
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.H) //바꾸기
            {
                if (ReplaceWindow.Visibility != Visibility.Visible)
                {
                    ReplaceWindow.Show();
                }
                StatusBarCallBack("바꾸기");
            }
        }

        private void SearchWindow_SearchCallBack(string sLog)
        {
            string sTextEditor_Content = this.TB_Content.Text.ToString();
            int nCnt = -1;

            if (sTextEditor_Content.IndexOf(sLog) > -1)
            {
                MatchCollection matches = Regex.Matches(sTextEditor_Content, sLog);
            }
            else
            {
                MessageBox.Show(string.Format("{0} is nothing .\n" +
                    "Please Check it again", sLog));
            }
        }

        private void TBL_LineNumber_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

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
            Property_Text = sFinfo;
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
            Property_Text = sFinfo;
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
            Property_Text = sDirInfo;


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
