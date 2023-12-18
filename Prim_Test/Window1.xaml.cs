using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Prim_Test
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
  
        public Window1()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public string _sTest= null;
        public  string sTest
        { 
            get
            {
                ////return _sTest;
                //return _sTest;
                return (string)GetValue(sTestProperty);
            }
            set
            {
                //_sTest = value;
                //OnPropertyChanged();
                //SetValue(sTestProperty, value);
                SetValue(sTestProperty, value);
            }
        }

        public static readonly DependencyProperty sTestProperty
            =
            DependencyProperty.Register("sTest", typeof(string),
                typeof(Window1),
                new PropertyMetadata("", sTestPropertyCallBack));

        private static void sTestPropertyCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Log((string)d.GetValue(sTestProperty));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sTest = @"C:\Users\kim\Documents\GitHub\XBox";
        }

        public static void Log(string msg, [CallerMemberName] string caller = null)
        {
            System.Diagnostics.Debug.WriteLine("[" + caller + "]" + DateTime.Now.ToString() + "\t" + msg);
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            sTest = @"C:\Users\kim\Documents\GitHub\XBox";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
