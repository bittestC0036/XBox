using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace XBox
{
    /// <summary>
    /// TextEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public delegate void StatusBarCallBack(string sLog);
    public delegate void SearchCallBack(string sLog);
    public delegate void MoveToLineCallBack(int nIndex);
    public delegate void ReplaceCallBack(string sSearchWord, string sReplaceWord);

    public partial class TextEditor : UserControl
    {
        public event StatusBarCallBack StatusBarCallBack;
        public event SearchCallBack SearchCallBack;
        public event MoveToLineCallBack MoveToLineCallBack;
        public event ReplaceCallBack ReplaceCallBack;

        public SearchWindow SearchWindow;

        public ReplaceWindow ReplaceWindow;

        public MoveToLineWindow MoveToLineWindow;

        public TextEditor()
        {
            InitializeComponent();

            SearchWindow = new SearchWindow();

            ReplaceWindow = new ReplaceWindow();

            MoveToLineWindow = new MoveToLineWindow();

            SearchWindow.Visibility = Visibility.Collapsed;
            ReplaceWindow.Visibility = Visibility.Collapsed;
            MoveToLineWindow.Visibility = Visibility.Collapsed;

        }

        private void Content_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = sender as TextBox;

            if (x == null)
                return;

            if (x.Text == string.Empty)
            {
                TBL_LineNumber.Text = string.Empty;
                return;
            }


            var sContent = x.Text.ToString().Split('\n');

            TBL_LineNumber.Text = "";
            for (int nCnt=1;nCnt<sContent.Count()+10;nCnt++)
            {
                TBL_LineNumber.Text += nCnt + "\n";
            }

            int caretIndex = x.CaretIndex;
            int lineIndex = x.GetLineIndexFromCharacterIndex(caretIndex);
            int lineLength = x.GetLineLength(lineIndex);

            if (lineLength<0)
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
            LineNumberScrollViewer.ScrollToHorizontalOffset(TBScrollViewer.HorizontalOffset); System.Diagnostics.Debug.WriteLine("TBScrollViewer.HorizontalOffset is"+ TBScrollViewer.HorizontalOffset);
            LineNumberScrollViewer.ScrollToVerticalOffset(TBScrollViewer.VerticalOffset);     System.Diagnostics.Debug.WriteLine("TBScrollViewer.VerticalOffset"+ TBScrollViewer.VerticalOffset);
        }

        private void TB_Content_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var x = sender as TextBox;

            if (x == null)
                return;

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                // Ctrl + S가 눌렸을 때 원하는 동작 수행 // 예를 들어, 파일 저장 기능을 호출하거나 다른 작업을 수행할 수 있습니다.
                if(x.Text!=File.ReadAllText(x.Tag.ToString()))
                {
                    var sw = new StreamWriter(x.Tag.ToString());
                    sw.Write(x.Text.ToString());
                    sw.Flush();
                    sw.Close();
                }
                e.Handled = true; // 이벤트 처리 완료
                StatusBarCallBack("File is saved Succesfully"); //StatusBarCallBack(string.Format("{0} : {1}",x.Tag.ToString()," is Save Sucessfully"));
            }
            else if(Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.G) // 줄 이동
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

        private void TBL_LineNumber_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
