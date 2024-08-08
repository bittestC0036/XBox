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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SplashWindow
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer t = null;
        int m_nCnt = 1;

        public MainWindow()
        {
            InitializeComponent();

            if (t == null)
            {
                t = new DispatcherTimer();
                t.Interval = TimeSpan.FromMilliseconds(40);
                t.Tick += T_Tick;
                t.Start();
            }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            m_nCnt = m_nCnt % 9;
            UpdateLights(m_nCnt);
            m_nCnt++;
        }

        private void UpdateLights(int tick)
        {
            if (this.Visibility != Visibility.Visible) return;

            clock1.Fill = tick == 1 ? Brushes.WhiteSmoke : Brushes.Transparent;
            clock3.Fill = tick == 2 ? Brushes.WhiteSmoke : Brushes.Transparent;
            clock5.Fill = tick == 3 ? Brushes.WhiteSmoke : Brushes.Transparent;
            clock6.Fill = tick == 4 ? Brushes.WhiteSmoke : Brushes.Transparent;
            clock7.Fill = tick == 5 ? Brushes.WhiteSmoke : Brushes.Transparent;
            clock9.Fill = tick == 6 ? Brushes.WhiteSmoke : Brushes.Transparent;
            clock11.Fill = tick == 7 ? Brushes.WhiteSmoke : Brushes.Transparent;
            clock12.Fill = tick == 8 ? Brushes.WhiteSmoke : Brushes.Transparent;
        }

        ~MainWindow()
        {
            if (t != null)
            {
                t.Stop();
            }

        }
    }
}
