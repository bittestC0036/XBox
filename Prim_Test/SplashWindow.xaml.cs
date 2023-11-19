using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Prim_Test
{
    /// <summary>
    /// SplashWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            //InitUI();
            //DoubleAnimation_Start();
        }

        Canvas MainCanvas = new Canvas();
        Canvas MyEllipseCanvas = new Canvas();
        Ellipse MyEllipse = new Ellipse();

        public void InitUI()
        {
            // 윈도우 크기
            this.Width = 600;
            this.Height = 100;

            // 원 크기 및 색상
            MyEllipse.Width = 50;
            MyEllipse.Height = 50;
            MyEllipse.Fill = new SolidColorBrush(Colors.Blue);   // 색상

            //원을 품고 있는 캔버스를 다시 자식으로.
            MainCanvas.Children.Add(MyEllipse);
            Content = MainCanvas;
        }

        public void DoubleAnimation_Start()
        {
            //더블 애니메이션 하나 설정 했다.
            DoubleAnimation MyDoubleAnimation = new DoubleAnimation();
            MyDoubleAnimation.From = 0.0;
            MyDoubleAnimation.To = this.Width - MyEllipse.Width;

            //가속도값 설정하기
            MyDoubleAnimation.AccelerationRatio = 1;
            MyDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            //애니메이션 효과를 적용한 후에는 속성 값을 변경하기
            MyDoubleAnimation.FillBehavior = FillBehavior.HoldEnd;   // Stop 는 원위치에서 종료, HoldEnd는 현위치에서 종료
            MyDoubleAnimation.AutoReverse = true;   // 자동복원 설정
            MyDoubleAnimation.Completed += new EventHandler(MyDoubleAnimation_Completed);   // 종료 이벤트 설정
            MyEllipse.BeginAnimation(Canvas.LeftProperty, MyDoubleAnimation);
        }

        // 종료 이벤트 설정
        void MyDoubleAnimation_Completed(object sender, EventArgs e)
        {
            DoubleAnimation_Start();   // 다시 시작 설정
        }

    }
}
