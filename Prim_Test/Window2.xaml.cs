using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace Prim_Test
{
    /// <summary>
    /// Window2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();


            mimi();
        }

        private void mimi()
        {
            string connectionString = "Server=your_server_address;Port=your_port_number;Database=your_database_name;Uid=your_username;Pwd=your_password;";

            string sIP = "192.168.254.127";

            sIP = "127.0.0.1";

            string sPort = "6448";

            sPort = "3306";

            string sDBname = "database_name";

            string sID = "root";

            string sPW = "qwer1234";

            connectionString = $"Server={sIP};Port={sPort};Database={sDBname};Uid={sID};Pwd={sPW};";

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                // MySQL 서버에 연결
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("MySQL 서버에 성공적으로 연결되었습니다.");

                    // SQL 쿼리 작성
                    string query = "SELECT * FROM your_table_name";

                    query = "select * from users";

                    query = "DESCRIBE users;";

                    // 쿼리 실행을 위한 MySqlCommand 객체 생성
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    // 데이터를 가져오기 위한 데이터 리더 생성
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    // 결과 반복해서 처리
                    while (dataReader.Read())
                    {
                        // 여기에서 필요한 작업을 수행
                        // 각 행에서 데이터를 가져오는 방법: dataReader.GetString(컬럼_인덱스_또는_이름)
                        // 예: string value = dataReader.GetString("column_name");

                        System.Diagnostics.Debug.WriteLine(dataReader.GetString(0));
                    }

                    dataReader.Close(); // 데이터 리더 닫기
                }
                else
                {
                    Console.WriteLine("MySQL 서버 연결에 실패했습니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("오류 발생: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close(); // 연결 종료
                }
            }

            Console.ReadLine();
        }
    }    
}
