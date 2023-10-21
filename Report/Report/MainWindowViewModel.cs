using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Report
{
    public delegate bool InsertDataCallBack(string sName, string sAge, string sPhoneNumber);
    public delegate bool RemoveDataCallBack(string sName, string sAge, string sPhoneNumber);
    public delegate List<Person> SelectDataCallBack ();
    public delegate List<Person> SelectDataConditionCallBack(string ColumnName, string ColumValue);
    public class MainWindowViewModel : ViewModelBase
    {
        public InsertDataCallBack insertDataCallBack;
        public RemoveDataCallBack removeDataCallBack;

        public SelectDataCallBack selectDataCallBack;
        public SelectDataConditionCallBack selectDataConditionCallBack;
        MainWindowModel mvmodel = null;

        #region Creator
        public MainWindowViewModel()
        {
            SearckBtnClickCommand = new RelayCommand(SearckBtnClickCommandAction);
            AddBtnClickCommand = new RelayCommand(AddBtnClickCommandAction);
            RmBtnClickCommand = new RelayCommand(RmBtnClickCommandAction);
            CheckBtnClickCommand = new RelayCommand(CheckBtnClickCommandAction);

            mvmodel = new MainWindowModel();
            mvmodel.logCALLBACK += Log ;
            this.insertDataCallBack += mvmodel.InsertData;
            this.removeDataCallBack += mvmodel.RemoveData;
            this.selectDataCallBack += mvmodel.GetSelectData ;
            this.selectDataConditionCallBack += mvmodel.GetSelectDatas;

            var Coumns= mvmodel.GetColumnNames();
            for (int nCnt=0;nCnt< Coumns.Count();nCnt++)
            {
                if(Coumns[nCnt]!="ID")
                {
                    _sColumnNames.Add(Coumns[nCnt]);
                }

            }

            var initdata = mvmodel.GetSelectData();
            Task.Delay(2);
            for (int nCnt=0;nCnt<initdata.Count;nCnt++)
            {
                people.Add(
                new Person()
                {
                    sAge        = initdata[nCnt].sAge,
                    sName       = initdata[nCnt].sName,
                    sPhone      = initdata[nCnt].sPhone,
                    bSelected = initdata[nCnt].bSelected
                }); ;
            }

            Application.Current.Dispatcher.ShutdownStarted += (s, e) =>
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;

                FileInfo fileInfo = new FileInfo(assemblyLocation);

                string sDirPath = System.IO.Path.Combine(fileInfo.DirectoryName, "Config");

                Directory.CreateDirectory(sDirPath);

                var sw = new StreamWriter(Path.Combine(sDirPath, "RecordingData.ini"));

                foreach(var item in Person_items)
                {
                    sw.WriteLine(item.sName+","
                                +item.sAge + ","
                                +item.sPhone + ","
                                +item.bSelected.ToString() +"");
                }
                sw.Close();
            };

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string sDirPath = System.IO.Path.Combine(fileInfo.DirectoryName, "Config");

            Directory.CreateDirectory(sDirPath);

            var sRecordingDataPath= Path.Combine(sDirPath, "RecordingData.ini");

            if (File.Exists(sRecordingDataPath))
            {
                var sr = new StreamReader(sRecordingDataPath);

                string sLine = string.Empty;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    while ((sLine = sr.ReadLine()) != null)
                    {
                        if (sLine.IndexOf(",") != -1)
                        {
                            var sPersonDatas = sLine.Split(',');
                            _Person_items.Add(new Person()
                            {
                                sName = sPersonDatas[0],
                                sAge = sPersonDatas[1],
                                sPhone = sPersonDatas[2],
                                bSelected = sPersonDatas[3].ToString() == "True" ? true : false
                            }); ;
                        }
                    }

                    sr.Close();
                });
            }
            
            _AddBtnVisability = Visibility.Visible;
            _CheckBtnVisability = Visibility.Collapsed;
        }


        #endregion Creator

        #region Binding 

        #region Binding Property

        private string _sKeyword { get; set; }
        public string sKeyword
        {
            get
            {
                return _sKeyword;
            }
            set
            {
                _sKeyword = value;
                OnPropertyChanged();
            }
        }


        private string _sInsertName { get; set; }
        public string sInsertName
        {
            get
            {
                return _sInsertName;
            }
            set
            {
                _sInsertName = value;
                OnPropertyChanged();
            }
        }

        private string _sInsertAge { get; set; }
        public string sInsertAge
        {
            get
            {
                return _sInsertAge;
            }
            set
            {
                _sInsertAge = value;
                OnPropertyChanged();
            }
        }

        private string _sInsertphone { get; set; }
        public string sInsertphone
        {
            get
            {
                return _sInsertphone;
            }
            set
            {
                _sInsertphone = value;
                OnPropertyChanged();
            }
        }


        #region Buttn Vis
        private Visibility _AddBtnVisability { get; set; }

        public Visibility AddBtnVisability
        {
            get
            {
                return _AddBtnVisability;
            }
            set
            {
                _AddBtnVisability = value;
                if(_AddBtnVisability==Visibility.Collapsed)
                {
                    _CheckBtnVisability = Visibility.Visible;
                }
                OnPropertyChanged();
            }
        }

        private Visibility _CheckBtnVisability { get; set; }

        public Visibility CheckBtnVisability
        {
            get
            {
                return _CheckBtnVisability;
            }
            set
            {
                _CheckBtnVisability = value;
                OnPropertyChanged();
            }
        }
        
        #endregion Btn Vis
        
        /// <summary>
        /// Can't Remove Data
        /// </summary>
        public List<Person> people = new List<Person>();

        private Person _SelectedPerson { get; set; }        

        public Person SelectedPerson
        {
            get
            {
                return _SelectedPerson;
            }
            set
            {
                _SelectedPerson = value;                
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> _Person_items = new ObservableCollection<Person>();
        public ObservableCollection<Person> Person_items
        {
            get
            {
                return _Person_items;
            }
            set
            {
                _Person_items = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<string> _sColumnNames = new ObservableCollection<string>();
        public ObservableCollection<string> sColumnNames
        {
            get
            {
                return _sColumnNames;
            }
            set
            {
                _sColumnNames = value;
                OnPropertyChanged();
            }
        }

        private string _SelectedLB_item { get; set; }

        public string SelectedLB_item
        {
            get
            {
                return _SelectedLB_item;
            }
            set
            {
                _SelectedLB_item = value;
                OnPropertyChanged();
            }
        }

        #endregion Binding Property

        #region Binding Method

        public RelayCommand AddBtnClickCommand { get; }

        private void AddBtnClickCommandAction(object obj)
        {
            Printf("Start - AddBtnClickCommandAction");
            AddBtnVisability = Visibility.Collapsed;
            CheckBtnVisability = Visibility.Visible;

            _SelectedPerson = new Person();
            _SelectedPerson.sName = "";
            _SelectedPerson.sAge = "";
            _SelectedPerson.sPhone = "";
            _SelectedPerson.bSelected = false;

            Printf("End - AddBtnClickCommandAction");
        }
        public RelayCommand CheckBtnClickCommand { get; }

        private void CheckBtnClickCommandAction(object obj)
        {
            Printf("Start - CheckBtnClickCommandAction");
            AddBtnVisability = Visibility.Visible;
            try
            {
                Task.Run(() =>
                {
                    insertDataCallBack(
                    _sInsertName,
                    _sInsertAge,
                    _sInsertphone);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Task.Delay(1000);
                        _Person_items.Add(new Person()
                        {
                            sName = _sInsertName,
                            sAge = _sInsertAge,
                            sPhone = sInsertphone,
                            bSelected = false
                        });
                    });

                });

            }
            catch(Exception ex)
            {
                Log(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
            }
            CheckBtnVisability = Visibility.Collapsed;
            Printf("End - CheckBtnClickCommandAction");
        }
        public RelayCommand RmBtnClickCommand { get; }
        private void RmBtnClickCommandAction(object obj)
        {
            if(bDataCehck(SelectedPerson))
            {
                if(SelectedPerson.bSelected==true)
                {
                    Task.Run(() =>
                    {
                        for (int nCnt = 0; nCnt < Person_items.Count; nCnt++)
                        {
                            if (Person_items[nCnt].bSelected == true)
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    Task.Delay(1000);
                                    removeDataCallBack(Person_items[nCnt].sName,
                                    Person_items[nCnt].sAge,
                                    Person_items[nCnt].sPhone);
                                    _Person_items.Remove(Person_items[nCnt]); //Person_items[nCnt]
                                });
                            }
                        }
                    });



                }
            }else
            {
                Printf(string.Format("Can't do it."));
            }
        }

        public RelayCommand SearckBtnClickCommand { get; }
        private void SearckBtnClickCommandAction(object obj)
        {
            Log("[Search] Start : "+_sKeyword+" Condition:"+ _SelectedLB_item);

            if(string.IsNullOrWhiteSpace(_sKeyword)&&
                string.IsNullOrWhiteSpace(_SelectedLB_item))    // ALL
            {
                //var temp =mvmodel.GetSelectData();    //selectDataCallBack
                Task.Run(() =>
                {
                    var temp = selectDataCallBack();    //selectDataCallBack

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Task.Delay(1000);
                        _Person_items.Clear();
                        for (int nCnt = 0; nCnt < temp.Count; nCnt++)
                        {
                            _Person_items.Add(temp[nCnt]);
                        }
                    });

                });

            }else
            {
                Task.Run(()
                 =>
                {
                    var mimi = selectDataConditionCallBack(_SelectedLB_item, _sKeyword);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Task.Delay(1000);
                        Person_items.Clear();
                        for (int nCnt = 0; nCnt < mimi.Count; nCnt++)
                        {
                            Person_items.Add(mimi[nCnt]);
                        }
                    });
                });
            }

            Log("[Search] Fin : " + _sKeyword);
        }

        #endregion Binding Method

        #endregion Binding 

        #region Instance Method

        public void Log(string msg)
        {
            try
            {
                string basePath = @"C:\Temp";
                string date = DateTime.Now.ToString("yyyy'-'MM'-'dd");
                date = date.Replace("-", "\\");
                string folderpath = basePath + "\\" + date;
                Directory.CreateDirectory(folderpath);
                string LogFile = "Report.txt";
                string writefile = folderpath + "\\" + LogFile;
                string format = "[" + DateTime.Now.ToString("yyyy':'MM':'dd'-'HH':'mm':'ss") + "] ";
                string sConent = format + msg;
                StreamWriter sw = new StreamWriter(writefile, File.Exists(writefile));

                sw.WriteLine(sConent);
                sw.Close();
                Printf(sConent);
            }
            catch(Exception ex)
            {
                Log(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
            }
        }

        public void Printf(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public bool bDataCehck (Person person)
        {
            try
            {
                //if(people.IndexOf(person)==-1)
                //{ 
                //    return true;
                //} 

                for(int nCnt=0;nCnt<people.Count;nCnt++)
                {
                    if(
                        (people[nCnt].sName  ==   person .sName )&&
                        (people[nCnt].sPhone ==   person .sPhone) &&
                        (people[nCnt].sAge   ==    person.sAge)
                        )
                    {
                        return false;
                    }
                }

                return true;
                
            }catch(Exception ex)
            {
                Printf(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        #endregion Instance Method


    }
}
