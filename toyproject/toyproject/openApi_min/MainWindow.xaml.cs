using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using openApi_min.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace openApi_min
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        bool isFavorite = false;
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

        private async void BtnFavoite_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("즐겨찾기", "추가할 맛집을 선택하세요(복수선택가능)!!");
                return;
            }
            if (isFavorite == true)  // 즐겨찾기 보기한 뒤 영화를 다시 즐겨찾기하려고 할때 막음.
            {
                await this.ShowMessageAsync("즐겨찾기", "이미 즐겨찾기한 맛집입니다");
                return;
            }

            var DaeguFood = new List<DaeguFood>();
            foreach (DaeguFood item in GrdResult.SelectedItems)
            {
                DaeguFood.Add(item);
            }
            Debug.WriteLine(DaeguFood.Count);
            try
            {
                var insRes = 0;

                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    foreach (DaeguFood item in DaeguFood)
                    {
                        // 저장되기 전에 이미 저장된 데이터인지 확인 후 
                        SqlCommand chkCmd = new SqlCommand(Models.DaeguFood.CHECK_QUERY, conn);
                        chkCmd.Parameters.AddWithValue("@Id", item.OPENDATA_ID);
                        var cnt = Convert.ToInt32(chkCmd.ExecuteScalar()); // COUNT

                        if (cnt == 1) continue; // 이미 데이터가 있으면 패스

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

                        insRes += cmd.ExecuteNonQuery(); //데이터 하나마다 INSERT 쿼리 실행 
                    }
                }

                if (insRes == DaeguFood.Count)
                {
                    await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 저장성공!");
                }
                else
                {
                    await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기{DaeguFood.Count} 건중{insRes}건 저장성공!");
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기 오류{ex.Message}");
            }
        }

        private async void BtnViewFavorite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var favoriteList = new List<DaeguFood>();

                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM FavoriteDaeguFood", conn);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        favoriteList.Add(new DaeguFood
                        {
                            OPENDATA_ID = reader["OPENDATA_ID"].ToString(),
                            GNG_CS = reader["GNG_CS"].ToString(),
                            FD_CS = reader["FD_CS"].ToString(),
                            BZ_NM = reader["BZ_NM"].ToString(),
                            TLNO = reader["TLNO"].ToString(),
                            MBZ_HR = reader["MBZ_HR"].ToString(),
                            SEAT_CNT = reader["SEAT_CNT"].ToString(),
                            PKPL = reader["PKPL"].ToString(),
                            SBW = reader["SBW"].ToString(),
                            BUS = reader["BUS"].ToString(),
                        });
                    }
                }

                if (favoriteList.Count > 0)
                {
                    GrdResult.ItemsSource = favoriteList;
                    await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 목록을 불러왔습니다.");
                }
                else
                {
                    await this.ShowMessageAsync("즐겨찾기", "저장된 즐겨찾기가 없습니다.");
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기 목록 불러오기 오류: {ex.Message}");
            }
        }



        private void GrdResult_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GrdResult.SelectedItem != null)
            {
                var selectedItem = GrdResult.SelectedItem as DaeguFood;
                if (selectedItem != null)
                {
                    string message = $"지하철 이용시: {selectedItem.SBW}\n 버스 이용시: {selectedItem.BUS}";
                    MessageBox.Show(message, "Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void TxtMovieName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSearch_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}