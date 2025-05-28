using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace XBox
{
    //[Export(typeof(IMainViewModel))]
    public sealed class FolderTreeViewModel : ObservableObject, IMainViewModel, INotifyPropertyChanged
    {

        public ICommand ChangeViewCommand { get; }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }


        #region Creator
        [ImportingConstructor]
        public FolderTreeViewModel()
        {
            bSingle = true;

            MakeFolder_Content_Width = 1200;

            ChangeViewCommand = new RelayCommand<string>(ChangeView);

        }

        private void ChangeView(string menu)
        {
            System.Diagnostics.Debug.WriteLine(menu);
            // 각 메뉴에 따라 View(UserControl) 교체
            switch (menu)
            {
                case "FolderTreeView":
                    CurrentView = new FolderTreeView();
                    break;
                case "ColorDetectorView":
                    CurrentView = new ColorDetectorView();
                    break;
            }
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

        public FolderTreeView _MainView_ = null;

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

            _MainView_ = (FolderTreeView)view;
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
            Console.WriteLine("SelectedItemChanged");
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
                    TB_Content_Content += m_x.Tag.ToString();
                    sStatusBarText = m_x.Tag.ToString();
                    bSingle = true;
                }
            }
        }

        
        public void MouseDoubleClick(object item)
        {
            var x = item as FolderTree;

            if (x.SelectedItem is _TxT_)
            {
                try
                {
                    var m_x = x.SelectedItem as _TxT_;

                    var fi = new FileInfo(m_x.Tag.ToString());

                    var TEMP = new LayoutDocument();

                    TEMP.Title = m_x.TB_Header.ToString().Split(':')[1];

                    var TEMP_TextEditor = new TextEditor();

                    TB_RootPath_Text = m_x.Tag.ToString();

                    TEMP_TextEditor.sTB_Content = TB_RootPath_Text;

                    TEMP_TextEditor.TB_Content.Text = File.ReadAllText(m_x.Tag.ToString());
                    TEMP_TextEditor.TB_Content.Tag = m_x.Tag;
                    TEMP_TextEditor.Tag = m_x.Tag.ToString();
                    TEMP_TextEditor.TB_Content.IsReadOnly = false;
                    TEMP.Content = TEMP_TextEditor;

                    bool bCheck = true;
                    int nCnt;
                    for (nCnt = 0; nCnt < _MainView_.TopTap.Children.Count; nCnt++)
                    {
                        bCheck = bCheck & _MainView_.TopTap.Children[nCnt].Title != TEMP.Title;
                        if (bCheck == false)
                            break;
                    }

                    if (bCheck)
                    {
                        _MainView_.TopTap.InsertChildAt(_MainView_.TopTap.Children.Count, TEMP);

                        _MainView_.TopTap.SelectedContentIndex = _MainView_.TopTap.Children.Count;

                        if (_MainView_.TopTap.Children.Count == 0)
                        {
                            _MainView_.TopTap.Children[_MainView_.TopTap.Children.Count].IsActive = true;
                        }
                        else
                        {
                            _MainView_.TopTap.Children[_MainView_.TopTap.Children.Count - 1].IsActive = true;
                        }
                    }
                    else
                    {
                        _MainView_.TopTap.Children[nCnt].IsActive = true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
                }
            }
            else if (x.SelectedItem is _Img_)
            {

            }
        }

        public void StatusBarClick()
        {
            if (string.IsNullOrWhiteSpace(sStatusBarText))
                return;

            var fi = new FileInfo(sStatusBarText);

            var di = new DirectoryInfo(sStatusBarText);

            if (fi.Exists)
            {
                Process.Start("explorer.exe", $"/select,\"{fi.FullName}\"");
            }
            else
            {
                Process.Start("explorer.exe", di.FullName);
            }
        }

        #endregion Event

        #endregion MakeFolderTree



        public ObservableCollection<string> MenuItems { get; } =
         new ObservableCollection<string> { "FolderTreeView", "ColorDetectorView", "About" };

    }

}
