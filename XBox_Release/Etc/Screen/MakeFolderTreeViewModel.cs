using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XBox
{
    public class MakeFolderTreeViewModel : ObservableObject, INotifyPropertyChanged
    {
        public MakeFolderTreeViewModel()
        {
            INITUI();
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

        private double _dWidth = 0;
        public double dWidth
        {
            get
            {
                return _dWidth;
            }
            set
            {
                _dWidth = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _TB_Content_Visability = Visibility.Visible;

        public Visibility TB_Content_Visability
        {
            get { return _TB_Content_Visability; }
            set
            {
                SetProperty(ref _TB_Content_Visability, value);
            }
        }

        #endregion Binding Property

        #region Event Command
        public void Window_Loaded(object dataContext, object view)
        {
            dWidth = 1200;
        }

        public void Btn_SetPath()
        {
            Reset();
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TB_RootPath_Text = cofd.FileName;

                INITUI(TB_RootPath_Text);
            }
        }

        public void FindFileName()
        {

        }

        public void StatusBarClick(object sender)
        {
            //string sData = sStatusBarText;
            //sData = sData.Replace("Open","");
            //if (string.IsNullOrWhiteSpace(sData))
            //    return;
            //
            //if(File.Exists(sData))
            //{
            //    var fi_item = new FileInfo(sData);
            //    if(sTxt_list.IndexOf(fi_item.Extension.ToUpper())!=-1)
            //    {
            //        System.Diagnostics.Process.Start("notepad", fi_item.FullName);
            //    }
            //    else if(sImage_list.IndexOf(fi_item.Extension.ToUpper()) != -1)
            //    {
            //        System.Diagnostics.Process.Start("mspaint", "\"" + fi_item.FullName + "\"");
            //    }
            //}
            //else if(Directory.Exists(sData))
            //{
            //    System.Diagnostics.Process.Start("explorer.exe", sData);
            //}
            //else
            //{
            //
            //}            
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

        #endregion Event Command

        #region Instance Method

        private void Reset()
        {
            TB_RootPath_Text = "";
        }

        #endregion Instance Method

        #region Event 




        #endregion Event

        #region CallBack Method

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

        private void SetStatus(string sLog)
        {
            sStatusBarText = sLog;
        }

        #endregion CallBack Method


    }
}
