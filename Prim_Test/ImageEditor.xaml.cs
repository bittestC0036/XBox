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

namespace Prim_Test
{
    /// <summary>
    /// ImageEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageEditor : UserControl
    {
        public ImageEditor()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var x = sender as MenuItem;

            if(x!=null)
            {
                System.Diagnostics.Debug.WriteLine(x.Header);

                string sData = x.Header.ToString();

                if(sData.IndexOf("1")!=-1)
                {
                    SliderValue.Visibility = Visibility.Visible;
                }
                else if(sData.IndexOf("2")!=-1)
                {
                    SliderValue.Visibility = Visibility.Visible;
                }
                else if(sData.IndexOf("3")!=-1)
                {
                    SliderValue.Visibility = Visibility.Visible;
                }
                else if(sData.IndexOf("4")!=-1)
                {
                    SliderValue.Visibility = Visibility.Visible;
                }
                else if (sData.IndexOf("5") != -1)
                {
                    SliderValue.Visibility = Visibility.Visible;
                }
                else if (sData.IndexOf("6") != -1)
                {
                    SliderValue.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SliderValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
