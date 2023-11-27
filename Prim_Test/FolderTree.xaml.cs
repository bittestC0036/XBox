using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// FolderTree.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FolderTree : TreeView
    {
        public FolderTree()
        {
            InitializeComponent();
            this.DataContext = this;
            sRootDirPath = "";
        }

        public string sRootDirPath
        {
            get
            {
                return (string)GetValue(sRootDirPath_Property);

            }
            set
            {
              
                SetValue(sRootDirPath_Property, value);
                if (!string.IsNullOrWhiteSpace(sRootDirPath))
                {
                    var sTemp = MakeFolderTree(sRootDirPath);
                    TreeviewItems.Add(sTemp);
                }
                   

            }
        }

        public static readonly DependencyProperty sRootDirPath_Property =
            DependencyProperty.Register("sRootDirPath", typeof(string),
                typeof(FolderTree), 
                new FrameworkPropertyMetadata("",sRootDirPath_PropertyCallBack));

        private static void sRootDirPath_PropertyCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Log((string)d.GetValue(sRootDirPath_Property));   
        }

        private ObservableCollection<TreeViewItem> TreeviewItems
        {
            get
            {
                return (ObservableCollection<TreeViewItem>)GetValue(TreeviewItems_Property);
            }
            set
            {
                SetValue(TreeviewItems_Property, value);
            }
        }

        private static readonly DependencyProperty TreeviewItems_Property =
            DependencyProperty.Register("TreeviewItems", typeof(ObservableCollection<TreeViewItem>),
                typeof(FolderTree), new FrameworkPropertyMetadata( TreeviewItems_PropertyCallBack));

        private static void TreeviewItems_PropertyCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Log((string)d.GetValue(TreeviewItems_Property));
        }

        public static void Log(string msg, [CallerMemberName] string caller = null)
        {
            System.Diagnostics.Debug.WriteLine("[" + caller + "]" + DateTime.Now.ToString() + "\t" + msg);
        }

        private static TreeViewItem MakeFolderTree(string FolderPath)
        { 
            var di_folder = new DirectoryInfo(FolderPath);
            var temp_tv_item = new TreeViewItem();

            temp_tv_item.Header = di_folder.Name;
            temp_tv_item.Tag = di_folder.FullName;
            //temp_tv_item.Selected += Folder_Selected;
            //temp_tv_item.Expanded += Folder_Expanded;

            try
            {
                if (di_folder.GetDirectories().Count() > 0)
                {
                    foreach (var item in di_folder.GetDirectories())
                    {
                        temp_tv_item.Items.Add(MakeFolderTree(item.FullName));

                    }
                }
                return temp_tv_item;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }
    }
}
