using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prim_Test
{
    /// <summary>
    /// Custom_Label.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Custom_Label : Label
    {
        public Custom_Label()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string sTest
        {
            get
            {
                return (string)GetValue(sTestProperty);
            }
            set
            {
                SetValue(sTestProperty, value);
            }
        }

        public static readonly DependencyProperty sTestProperty
            =
            DependencyProperty.Register("sTest", typeof(string),
                typeof(Custom_Label),
                new PropertyMetadata("", sTestPropertyCallBack));

        private static void sTestPropertyCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Log((string)d.GetValue(sTestProperty));
            //var x = d.
        }

        public static void Log(string msg, [CallerMemberName] string caller = null)
        {
            System.Diagnostics.Debug.WriteLine("[" + caller + "]" + DateTime.Now.ToString() + "\t" + msg);
        }
    }
}
