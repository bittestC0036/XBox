using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Report
{
    public delegate bool SQLSELECTCALLBACK();
    public delegate void LogCALLBACK(string msg);
    public class MainWindowModel
    {
        public SQLSELECTCALLBACK sQLSELECTCALLBACK;
        public LogCALLBACK logCALLBACK;

        private const string sTableCreate_Query = "CREATE TABLE IF NOT EXISTS User (ID int AUTO_INCREMENT ,Name varchar(10), Age varchar(20),  Phone varchar(100))";

        private const string sDataSelected_Query = "Select u.Name, u.age, u.phone FROM User u";

        private string ConnectionString = "Data Source=mydatabase.db;Version=3;";

        private string sDBFileName = "mydatabase.db";

        private List<string> ColumnsName = new List<string>();


        public  MainWindowModel()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string sDirPath = Path.Combine(fileInfo.DirectoryName, "");

            string sConfigPath = Path.Combine(sDirPath, sDBFileName);

            if (false == File.Exists(sConfigPath))
            {
                SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
                sqliteConn.Open();

                string strsql = sTableCreate_Query;
                SQLiteCommand cmd = new SQLiteCommand(strsql, sqliteConn);
                cmd.ExecuteNonQuery();

                for (int nCnt = 0; nCnt < 10; nCnt++)
                {
                    strsql = "Insert into User" +
                             "(Name, age, phone) " +
                             "values " +
                             "(" +
                             "'" + nCnt + "Name'," +
                             "'" + nCnt + "age'," +
                             "'" + nCnt + "phone'" +
                             ")";
                    cmd = new SQLiteCommand(strsql, sqliteConn);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public List<Person> GetSelectData()
        {
            try
            {
                SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
                sqliteConn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sDataSelected_Query, sqliteConn);
                var person_items = new List<Person>();
                var person_item = new Person();
        
                SQLiteDataReader rdr = cmd.ExecuteReader();
                person_items.Clear();

                while (rdr.Read())
                {
                    person_item = new Person();
                    person_item.sName       = (string)rdr["Name"];
                    person_item.sAge        = (string)rdr["age"];
                    person_item.sPhone      = (string)rdr["phone"];
                    person_item.bSelected   = false;

                    logCALLBACK("person_item.sName      "+person_item.sName     );
                    logCALLBACK("person_item.sAge       "+person_item.sAge      );
                    logCALLBACK("person_item.sPhone     "+person_item.sPhone    );
                    logCALLBACK("person_item.bSelected  " + person_item.bSelected);

                    person_items.Add(person_item);
                }
                sqliteConn.Close();
                return person_items;

            }
            catch (Exception ex)
            {
                logCALLBACK(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }

        //public async Task<List<Person>> GetSelectData()
        //{
        //    try
        //    {
        //
        //        SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
        //        sqliteConn.Open();
        //
        //        SQLiteCommand cmd = new SQLiteCommand(sDataSelected_Query, sqliteConn);
        //        var person_items = new List<Person>();
        //
        //        var rdr = cmd.ExecuteReaderAsync().Result;
        //
        //        while (rdr.Read())
        //        {
        //            var person_item = new Person();
        //            person_item.sName = (string)rdr["Name"];
        //            person_item.sAge = (string)rdr["age"];
        //            person_item.sPhone = (string)rdr["phone"];
        //            person_item.bSelected = false;
        //            person_items.Add(person_item);
        //        }
        //
        //        // 비동기적으로 1초 기다림
        //        //await Task.Delay(1000);
        //
        //        sqliteConn.Close();
        //
        //        return person_items;
        //    }
        //    catch (Exception ex)
        //    {
        //        logCALLBACK(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
        //        return null;
        //    }
        //}

        /*
        public async Task<List<Person>> GetSelectData()
        {
            try
            {
                SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
                sqliteConn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sDataSelected_Query, sqliteConn);
                var person_items = new List<Person>();
                var person_item = new Person();

                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    person_item.sName = (string)rdr["Name"];
                    person_item.sAge = (string)rdr["age"];
                    person_item.sPhone = (string)rdr["phone"];
                    person_item.bSelected = false;
                    person_items.Add(person_item);
                }

                sqliteConn.Close();
                Task.Delay(1000);
                //await Task.Delay(1);
                return person_items;
            }
            catch (Exception ex)
            {
                logCALLBACK(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }
        */

        public List<Person> GetSelectDatas(string ColumnName, string ColumValue)
        {
            try
            {
                string sQuery = "";
                if (ColumnName == "Name")
                {
                    sQuery = "where u.Name='" + ColumValue + "'";
                }
                else if (ColumnName == "Age")
                {
                    sQuery = "where u.Age='" + ColumValue + "'";
                }
                else if (ColumnName == "phone")
                {
                    sQuery = "where u.phone='" + ColumValue + "'";
                }

                SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
                sqliteConn.Open();

                SQLiteCommand cmd = new SQLiteCommand(sDataSelected_Query+"\n"+sQuery, sqliteConn);
                var person_items = new List<Person>();
                var person_item = new Person();

                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    person_item.sName = (string)rdr["Name"];
                    person_item.sAge = (string)rdr["age"];
                    person_item.sPhone = (string)rdr["phone"];
                    person_item.bSelected = false;
                    person_items.Add(person_item);
                }

                sqliteConn.Close();
                return person_items;
            }
            catch (Exception ex)
            {
                logCALLBACK(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }

        public bool InsertData(string sName, string sAge, string sPhoneNumber)
        {
            try
            {
                SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
                sqliteConn.Open();
                string strsql = null;

                strsql = "Insert into User" +
                          "(Name, age, phone) " +
                          "values " +
                          "(" +
                          "'" + sName + " '," +
                          "'" + sAge + " '," +
                          "'" + sPhoneNumber + "'" +
                          ")";

                SQLiteCommand cmd = new SQLiteCommand(strsql, sqliteConn);
                cmd.ExecuteNonQuery();
                sqliteConn.Close();
                logCALLBACK(strsql);
                return true;
            }
            catch (Exception ex)
            {
                logCALLBACK(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        public bool RemoveData(string sName, string sAge, string sPhoneNumber)
        {
            try
            {
                SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
                sqliteConn.Open();
                string strsql = null;

                strsql =
                    "DELETE " +
                    "From User " + 
                    "WHERE Name ='" + sName + "' And" +
                         " Age= '" + sAge + "' And" +
                         " Phone= '" + sPhoneNumber + "'";
                

                SQLiteCommand cmd = new SQLiteCommand(strsql, sqliteConn);
                cmd.ExecuteNonQuery();
                sqliteConn.Close();
                logCALLBACK(strsql);
                return true;
            }
            catch (Exception ex)
            {
                logCALLBACK(string.Format("{0}\n\r{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        public List<string> GetColumnNames()
        {
            SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString);
            sqliteConn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sDataSelected_Query, sqliteConn);

            string strsql = "PRAGMA table_info(User);";
            cmd = new SQLiteCommand(strsql, sqliteConn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            
            ColumnsName.Add("");

            while (rdr.Read())
            {
                ColumnsName.Add(rdr["name"].ToString());
            }

            return ColumnsName;
        }
    }
}
