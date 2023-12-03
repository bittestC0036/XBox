using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// DBController.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DB_Title : UserControl, INotifyPropertyChanged
    {
        #region Creator
        
        public DB_Title()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #endregion Creator

        #region Binding Propery

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

        #endregion


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion 

    }

    //event DBConnectionException(string msg);
    delegate void DBConnectionException(string msg);
    public class DBController
    {
        public event DBConnectionException (string) DBConnectionException;
        string connectionString = null;
        string sIP              = null;
        string sPort            = null;
        string sDBname          = null;
        string sID              = null;
        string sPW              = null;

        MySqlConnection connection = null;

        Thread thread_db_checker = null;

        
        #region Creator & Singleton
        public DBController()
        {
                         sIP = "127.0.0.1";
                         sPort = "3306";
                         sDBname = "database_name";
                         sID = "root";
                         sPW = "qwer1234";
            connectionString = $"Server={sIP};Port={sPort};Database={sDBname};Uid={sID};Pwd={sPW};";

            //thread_db_checker = new Thread(new ThreadStart(MIMI));


            if(thread_db_checker==null)
            {
                thread_db_checker = new Thread(new ThreadStart(ConnectionCheckerer));
            }

            //Thread thread = new Thread(() => Console.WriteLine("이것은 람다식을 사용한 스레드입니다."));
            //thread.Start();
        }

        public bool GetConnectioStatus ()
        {
            try
            {
                this.connection.Open();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private void ConnectionCheckerer()
        {
            while(true)
            {
                connection = new MySqlConnection(connectionString);

                if (GetConnectioStatus())
                    continue;
                for (int nCnt = 0; nCnt < 10; nCnt++)
                {
                    Thread.Sleep(1000);
                    System.Diagnostics.Debug.WriteLine(connectionString);

                    if (GetConnectioStatus())
                    {
                        nCnt = nCnt + 10;
                    }
                }
            }
        }

        private static DBController instance;
        public static DBController GetInstance()
        {
            if (instance == null)
            {
                instance = new DBController();
            }
            return instance;
        }

        #endregion Creator & Singleton

        #region Parameter

        #endregion Parameter



    }
}
