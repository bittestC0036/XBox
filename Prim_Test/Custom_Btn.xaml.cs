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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prim_Test
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Custom_Btn : Button
    {
        public Custom_Btn()
        {
            InitializeComponent();
            DataContext = this;
            //sContent = "";

            DependencyPropertyDescriptor.FromProperty(sContentProperty, typeof(Custom_Btn))
            .AddValueChanged(this, (sender, args) =>
            {
                var x = sender as Custom_Btn;


            
            });
            sContent = "";
        }

        public string sContent
        {
            get
            {
                return (string)GetValue(sContentProperty);
            }
            set
            {
                SetValue(sContentProperty, value);
            }
        }

        public static readonly DependencyProperty sContentProperty =
            DependencyProperty.Register("sContent", typeof(string), typeof(Custom_Btn),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        //    new PropertyMetadata("", Frame));

        private static void sContentPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Log((string)d.GetValue(sContentProperty));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public static void Log(string msg, [CallerMemberName]string caller = null)
        {
            System.Diagnostics.Debug.WriteLine("["+caller+"]"+DateTime.Now.ToString()+"\t"+msg);
        }
    }
}
