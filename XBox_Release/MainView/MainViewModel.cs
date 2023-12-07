
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;

using System.Reflection;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.AvalonDock.Layout;

namespace XBox
{
    [Export(typeof(IMainViewModel))]
    public sealed class MainViewModel : ObservableObject, IMainViewModel, INotifyPropertyChanged
    {
        #region Creator
        [ImportingConstructor]
        public MainViewModel()
        {
            bSingle = true;

            MakeFolder_Content_Width = 1200;

            DB_title_height = 300;
        }

        private void INITUI(string rootFolderPath = null)
        {
            if (rootFolderPath == null)
            {
                rootFolderPath = @"D:\Job"; // 설정한 폴더 경로로 변경하세요.
                return;
            }
            TB_RootPath_Text = rootFolderPath; 
        }

        #endregion Creator

        #region MakeFolderTree

        #region Binding Property

        private string _TB_RootPath_Text = "";
        public string TB_RootPath_Text
        {
            get
            {
                return _TB_RootPath_Text;
            }
            set
            {
                SetProperty(ref _TB_RootPath_Text, value);
            }
        }

        private string _TB_Content_Content = "";
        public string TB_Content_Content
        {
            get
            {
                return _TB_Content_Content;
            }
            set
            {
                SetProperty(ref _TB_Content_Content, value);
            }
        }

        public bool _bSingle = true;
        public bool bSingle
        {
            get
            {
                return _bSingle;
            }
            set
            {
                SetProperty(ref _bSingle, value);
            }
        }

        private string _sStatusBarText = "";
        public string sStatusBarText
        {
            get
            {
                return _sStatusBarText;
            }
            set
            {
                SetProperty(ref _sStatusBarText, value);
            }
        }

        private double _MakeFolder_Content_Width = 0;
        public double MakeFolder_Content_Width
        {
            get
            {
                return _MakeFolder_Content_Width;
            }
            set
            {
                _MakeFolder_Content_Width = value;
                RaisePropertyChanged();
            }
        }


        #endregion Binding Property

        #region Event

        public void Window_Loaded(object dataContext, object view)
        {
            var x = dataContext as MainViewModel;
        }

        public void Btn_SetPath()
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TB_RootPath_Text = cofd.FileName;

                INITUI(TB_RootPath_Text);
            }
        }

        public void SelectedItemChanged(object item)
        {
            var x = item as FolderTree;

            if (x.SelectedItem is _Folder_)
            {
                var m_x = x.SelectedItem as _Folder_;
                TB_Content_Content = m_x.Tag.ToString();
                sStatusBarText = m_x.Tag.ToString();
            }
            else
            {
                if (x.SelectedItem is _TxT_)
                {
                    var m_x = x.SelectedItem as _TxT_;
                    TB_Content_Content = m_x.Tag.ToString();
                    sStatusBarText = m_x.Tag.ToString();
                    bSingle = true;
                }
                else if (x.SelectedItem is _Img_)
                {
                    var m_x = x.SelectedItem as _Img_;

                    sStatusBarText = m_x.Tag.ToString();
                }
            }
            //sStatusBarText = (x.SelectedItem as UserControl).Tag.ToString();
        }

        public void MouseDoubleClick(object item)
        {
            //LayoutRoot temp = new LayoutRoot();
            //temp.ActiveContent
            //temp.ActiveContent
        }

        #endregion Event

        #endregion MakeFolderTree

        #region DB 

        #region Binding Property

        public double _DB_title_height = 0;

        public double DB_title_height
        {
            get
            {
                return _DB_title_height;
            }
            set
            {
                _DB_title_height = value;
                RaisePropertyChanged();
            }
         }

        #endregion Binding Property

        #endregion DB

        #region Log

        private void Log(string sLog)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string sDirPath = Path.Combine(fileInfo.DirectoryName, "Log");

            Directory.CreateDirectory(sDirPath);

            {
                //string date = DateTime.Now.ToString("yyyy'-'MM'-'dd");

                string date = DateTime.Now.ToString("yyyy'_'MM'_'dd");

                string projectName = Assembly.GetExecutingAssembly().GetName().Name + ".log";

                string writefile = Path.Combine(sDirPath, date + projectName);

                string format = "[" + DateTime.Now.ToString("yyyy':'MM':'dd'-'HH':'mm':'ss") + "]";

                sLog = format + sLog;

                StreamWriter sw = new StreamWriter(Path.Combine(sDirPath, writefile), File.Exists(Path.Combine(sDirPath, writefile)));

                sw.Write(sLog);

                sw.Flush();

                sw.Close();
            }
        }
        #endregion Log
    }
}
