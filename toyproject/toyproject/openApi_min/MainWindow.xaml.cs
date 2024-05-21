using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using openApi_min.Models;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;

namespace openApi_min
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        
        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            InitComboDateFromDB();
            BtnReqRealtime_Click(sender, e);
        }


        private void InitComboDateFromDB()
        {
            // DB
        }

        private async void DataGrid()
        {

            

        }

        private async void BtnReqRealtime_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            string openApiUri = $"https://www.daegufood.go.kr/kor/api/tasty.html?mode=json&addr=%EC%A4%91%EA%B5%AC";
            string result = string.Empty;

            //WebRequest, WebResponse 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;
            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();

                //await this.ShowMessageAsync("결과", result);
                //Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
            }

            var jsonResult = JObject.Parse(result);
            var status = Convert.ToString(jsonResult["status"]);

            if (status == "DONE")
            {
                var data = jsonResult["data"];
                var jsonArray = data as JArray;

                var daegufood = new List<DaeguFood>();
                if (string.IsNullOrEmpty(Helpers.Common.Index))
                {
                    foreach (var item in jsonArray)
                    {
                        daegufood.Add(new DaeguFood()
                        {
                            OPENDATA_ID = Convert.ToString(item["OPENDATA_ID"]),
                            GNG_CS = Convert.ToString(item["GNG_CS"]),
                            FD_CS = Convert.ToString(item["FD_CS"]),
                            BZ_NM = Convert.ToString(item["BZ_NM"]),
                            TLNO = Convert.ToString(item["TLNO"]),
                            MBZ_HR = Convert.ToString(item["MBZ_HR"]),
                            SEAT_CNT = Convert.ToString(item["SEAT_CNT"]),
                            PKPL = Convert.ToString(item["PKPL"]),
                            SBW = Convert.ToString(item["SBW"]),
                            BUS = Convert.ToString(item["BUS"]),
                        });

                    }
                }

                else
                {
                    foreach (var item in jsonArray)
                    {
                        if (Convert.ToString(item["FD_CS"]) == Helpers.Common.Index)
                            daegufood.Add(new DaeguFood()
                            {
                                OPENDATA_ID = Convert.ToString(item["OPENDATA_ID"]),
                                GNG_CS = Convert.ToString(item["GNG_CS"]),
                                FD_CS = Convert.ToString(item["FD_CS"]),
                                BZ_NM = Convert.ToString(item["BZ_NM"]),
                                TLNO = Convert.ToString(item["TLNO"]),
                                MBZ_HR = Convert.ToString(item["MBZ_HR"]),
                                SEAT_CNT = Convert.ToString(item["SEAT_CNT"]),
                                PKPL = Convert.ToString(item["PKPL"]),
                                SBW = Convert.ToString(item["SBW"]),
                                BUS = Convert.ToString(item["BUS"]),
                            });

                    }
                }


                this.DataContext = daegufood;
                StsResult.Content = $"OpenAPI {daegufood.Count} 건 조회완료!";
            }
            // ComboBox에서 선택된 지역 가져오기


            // 지역별 API 요청 URL 생성



        }

        private async void BtnSaveData_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(GrdResult.Items.Count ==0)
            {
                await this.ShowMessageAsync("저장오류", "실시간 조회후 저장하십시오.");
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var insRes = 0;
                    foreach (DaeguFood item in GrdResult.Items)
                    {
                        SqlCommand cmd = new SqlCommand(Models.DaeguFood.INSERT_QUERY, conn);
                        cmd.Parameters.AddWithValue("@OPENDATA_ID", item.OPENDATA_ID);
                        cmd.Parameters.AddWithValue("@GNG_CS", item.GNG_CS);
                        cmd.Parameters.AddWithValue("@FD_CS", item.FD_CS);
                        cmd.Parameters.AddWithValue("@BZ_NM", item.BZ_NM);
                        cmd.Parameters.AddWithValue("@TLNO", item.TLNO);
                        cmd.Parameters.AddWithValue("@MBZ_HR", item.MBZ_HR);
                        cmd.Parameters.AddWithValue("@SEAT_CNT", item.SEAT_CNT);
                        cmd.Parameters.AddWithValue("@PKPL", item.PKPL);
                        cmd.Parameters.AddWithValue("@SBW", item.SBW);
                        cmd.Parameters.AddWithValue("@BUS", item.BUS);

                        insRes += cmd.ExecuteNonQuery();
                    }

                    if (insRes > 0)
                    {
                        await this.ShowMessageAsync("저장", "DB저장성공");
                        StsResult.Content = $"DB 저장 {insRes}건 성공!";
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("저장오류", $"저장오류{ex.Message}");
            }
            
            InitComboDateFromDB();
        }

        private void ComboBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           
        }

        private void ComboBoxItem_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            // 선택된 지역에 따라 필요한 작업 수행
            
        }

        private void ComboFoodType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int num = ComboFoodType.SelectedIndex;
            string index = "";

            switch (num)
            {
                case 0:
                    index = "한식";
                    break;
                case 1:
                    index = "양식";
                    break;
                case 2:
                    index = "일식";
                    break;
                case 3:
                    index = "전통차/커피전문점";
                    break;
                case 4:
                    index = "디저트/베이커리";
                    break;
                case 5:
                    index = "세계요리";
                    break;
                case 6:
                    index = "특별한 술집";
                    break;
            }
            Helpers.Common.Index = index;
            BtnReqRealtime_Click(sender, e);



        }

        //private void GrdResult_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    var curItem = GrdResult.SelectedItem as DaeguFood;

        //    if (curItem != null)
        //    {
        //        var detailsWindow = new DetailsWindow(curItem.SBW, curItem.BUS);
        //        detailsWindow.ShowDialog();
        //    }

        //}
    }
}