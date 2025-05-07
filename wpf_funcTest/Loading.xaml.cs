using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace wpf_funcTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Loading : Window
    {
        private DispatcherTimer _timer;
        private List<Rectangle> _bars;
        private int _currentIndex = 0;
        private bool _isResetting = false;

        public Loading()
        {
            Console.WriteLine("Loading");
            InitializeComponent();
            InitBars();
            StartAnimating();

            // 윈도우를 화면 중앙에 배치하는 코드
            //var window = new Loading();
            //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //window.Show();
            //Console.WriteLine("Loading");

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void InitBars()
        {
            _bars = new List<Rectangle>
        {
            TopLeftBar,
            TopBar,
            TopRightBar,
            RightBar,
            BottomRightBar,
            BottomBar,
            BottomLeftBar,
            LeftBar
        };
        }

        private void StartAnimating()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(40); // 0.2초 간격으로 하나씩
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isResetting)
            {
                if (_currentIndex < _bars.Count)
                {
                    _bars[_currentIndex].Fill = Brushes.White; // 순서대로 흰색으로 복구
                    _currentIndex++;
                }
                else
                {
                    _isResetting = false;
                    _currentIndex = 0;
                }
            }
            else
            {
                foreach (var bar in _bars)
                {
                    bar.Fill = Brushes.Black; // 전체를 검은색으로 덮기
                }
                _isResetting = true; // 이제 복구 시작
            }
        }
    }

}