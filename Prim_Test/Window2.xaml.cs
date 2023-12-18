using OpenCvSharp;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Prim_Test
{
    /// <summary>
    /// Window2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window2 : System.Windows.Window
    {
        //public 
        private Mat image;

        public string sFilePath = null;
        public Window2()
        {
            InitializeComponent();

            sFilePath = @"C:\Users\kim\Documents\GitHub\XBox\ac.png";
            image = Cv2.ImRead(sFilePath);

            if(image.Empty()==false)
            {
                int m_Height = (int)displayedImage.ActualHeight;

                int m_Width = (int)displayedImage.ActualWidth;

                if((0<m_Height )&& (0<m_Width))
                {
                    Cv2.Resize(image, image, new Size() { Height = m_Height, Width = m_Width });
                }


                //image = temp;

                displayedImage.Source = ConvertMatToImageSource(image);

                displayedImage2.Source = ConvertMatToImageSource(image);
            }
        }

        public ImageSource ConvertMatToImageSource(Mat image)
        {
            System.Drawing.Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);

            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                // Bitmap을 MemoryStream에 복사
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                // BitmapImage에 MemoryStream를 이용하여 이미지 설정
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
            }

            return (ImageSource)bitmapImage;
        }

        private void OnImageMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point clickPoint = e.GetPosition(displayedImage);

            int x = (int)clickPoint.X;
            int y = (int)clickPoint.Y;

            if (x >= 0 && x < image.Width && y >= 0 && y < image.Height)
            {
                Vec3b pixel = image.At<Vec3b>(y, x);
                Scalar m_color = new Scalar(pixel.Item0, pixel.Item1, pixel.Item2);

                Console.WriteLine($"Clicked at: ({x}, {y})");
                Console.WriteLine($"RGB Value: ({m_color.Val0}, {m_color.Val1}, {m_color.Val2})");

                Lb_Data.Content = $"Clicked at: ({x}, {y}) \n" + $"RGB Value: ({m_color.Val0}, {m_color.Val1}, {m_color.Val2})";

                Show_slider_R.Value = m_color.Val0;
                Show_slider_G.Value = m_color.Val1;
                Show_slider_B.Value = m_color.Val2;

            }

            System.Windows.Media.Color color
                = System.Windows.Media.Color.FromArgb(255, (byte)Show_slider_R.Value, (byte)Show_slider_G.Value, (byte)Show_slider_B.Value);
            SolidColorBrush solidColorBrush = new SolidColorBrush(color);
            Show_Color_.Background = solidColorBrush;

            image = Cv2.ImRead(sFilePath);

            Mat temp = new Mat();

            Cv2.Resize(image, temp, new Size() { Height = (int)displayedImage.ActualHeight, 
                Width = (int)displayedImage.ActualWidth });

            image = temp;


            Mat hsvImage = image;
            //Cv2.CvtColor(image, hsvImage, ColorConversionCodes.BGR2HSV);

            //hsvColor
            //Vec3b rgbColor = new Vec3b((byte)slider_R.Value, (byte)slider_G.Value, (byte)slider_B.Value);
            //Vec3b hsvColor = ConvertRgbToHsv(rgbColor);
            //byte byte_r = hsvColor.Item0;
            //byte byte_g = hsvColor.Item1;
            //byte byte_b = hsvColor.Item2;

            byte byte_r = (byte)slider_R.Value;
            byte byte_g = (byte)slider_G.Value;
            byte byte_b = (byte)slider_B.Value;

            if (byte_r < 0)
                byte_r =  0;

            if (byte_g < 0)
                byte_g = 0;

            if (byte_b < 0)
                byte_b = 0;


            // 빨간색 범위 지정 (HSV 값)
            //Scalar lowerRed = new Scalar(byte_r, byte_g, byte_b); // 빨간색의 하한값
            Scalar lowerRed = new Scalar(10, 0, 0); // 빨간색의 하한값
            Scalar upperRed = new Scalar(byte_r, byte_g, byte_b); // 빨간색의 상한값
            System.Diagnostics.Debug.WriteLine(byte_r.ToString(), byte_g.ToString(), byte_b.ToString());

            // 지정한 범위 내의 픽셀을 검출
            Mat redMask = new Mat();
            Cv2.InRange(hsvImage, lowerRed, upperRed, redMask);

            // 검출된 픽셀 좌표에 원 그리기
            //Mat contours = new Mat();
            //Cv2.FindNonZero(redMask, contours);            
            // 찾은 좌표를 처리합니다.
            //for (int i = 0; i < contours.Rows; i++)
            //{
            //    Point point = new Point((int)contours.At<Vec2i>(i).Item0, (int)contours.At<Vec2i>(i).Item1);
            //    Cv2.Circle(image, point, 5, new Scalar(0, 255, 255), 2); // 검출된 픽셀 주위에 원 그리기
            //}

            Cv2.FindContours(redMask, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            foreach(var item in contours)
            {
                double area = Cv2.ContourArea(item);

                if(area>=Convert.ToInt32(Lb_Size.Content))
                {
                    Rect boundingRect = Cv2.BoundingRect(item);
                    Cv2.Rectangle(image, boundingRect, Scalar.Red, 2);
                }
            }

            displayedImage2.Source = ConvertMatToImageSource(image);
        }

        public Vec3b ConvertRgbToHsv(Vec3b rgb)
        {
            double s_r = slider_R.Value;
            double s_g = slider_G.Value;
            double s_b = slider_B.Value;

            double r = s_r;
            double g = s_g;
            double b = s_b;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            double h = 0;
            if (delta == 0)
                h = 0;
            else if (max == r)
                h = 60 * (((g - b) / delta) % 6);
            else if (max == g)
                h = 60 * (((b - r) / delta) + 2);
            else if (max == b)
                h = 60 * (((r - g) / delta) + 4);

            double s = (max == 0) ? 0 : delta / max;
            double v = max;

            return new Vec3b((byte)(h / 2), (byte)(s * 255), (byte)(v * 255));
        }

        private void slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            double R_value = slider_R.Value;
            Lb_R.Content = Math.Round(R_value,2);
            double G_value = slider_G.Value;
            Lb_G.Content = Math.Round(G_value);
            double B_value = slider_B .Value;
            Lb_B.Content = Math.Round(B_value);

            Vec3b vec3B = new Vec3b((byte)slider_R.Value, (byte)slider_G.Value, (byte)slider_B.Value);

            System.Diagnostics.Debug.WriteLine(
                "slider_R.Value" +  slider_R.Value+ "\n" +
                "slider_G.Value:" + slider_G.Value+"\n"+
                "slider_B.Value:" + slider_B.Value);

            Lb_Size.Content=slider_Size.Value;



            System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(255,(byte)slider_R.Value   , (byte)slider_G.Value, (byte)slider_B.Value);
            SolidColorBrush solidColorBrush = new SolidColorBrush(color);
            _Color_.Background = solidColorBrush;
            //_Color_.Background


            //Color customColor = Color.FromArgb(255, redValue, greenValue, blueValue); // RGB 값을 사용하여 Color 객체 생성
            //SolidColorBrush solidColorBrush = new SolidColorBrush(customColor); // SolidBrush에 Color 적용
            //myGrid.Background = solidColorBrush;
        }

        private void Btn_SetImagePath_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            if(dia.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                sFilePath = dia.FileName;
                image = Cv2.ImRead(sFilePath);

                Mat temp = new Mat();

                if((displayedImage.ActualHeight!=0)&& displayedImage.ActualWidth!=0)
                {
                    Cv2.Resize(image, temp, new Size() { Height = (int)displayedImage.ActualHeight, Width = (int)displayedImage.ActualWidth });
                    image = temp;
                }

                displayedImage.Source = ConvertMatToImageSource(image);

                displayedImage2.Source = ConvertMatToImageSource(image);
            }
        }



        /*
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
        */

    }    
}
