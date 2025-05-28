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
using Xceed.Wpf.AvalonDock.Layout;

namespace XBox
{
    /// <summary>
    /// FolderTreeView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FolderTreeView : UserControl
    {
        public FolderTreeView()
        {
            InitializeComponent();
        }

        private void Tap1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var x = sender as LayoutDocument;

            if (x.Title == Tap1.Title)
            {
                MessageBox.Show("Not Support Function. Plesae Call Engineer");
                e.Cancel = true;
            }
        }
    }
}
