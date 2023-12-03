using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace XBox
{
    /// <summary>
    /// DBController.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DB_Title : UserControl, INotifyPropertyChanged
    {
        public DB_Title()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public ObservableCollection<TabItem> _DB_List = new ObservableCollection<TabItem>();
        public ObservableCollection <TabItem> DB_List
        {
            get
            {
                return _DB_List;
            }
            set
            {
                _DB_List = value;
                OnPropertyChanged();
            }
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
