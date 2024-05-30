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
        bool isArea = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e) { }

        private async void TxtAreaName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            isArea = true;
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

            if (isArea)
            {
                ShowAreaRestaurantInfo();
            }
            else
            {
                // 해당 식당 카테고리에 해당하는 식당 정보만 보여주기
                ShowRestaurantInfo();

            }
        }

        private async void ShowAreaRestaurantInfo()
        {
            string openApiUri = $"https://www.daegufood.go.kr/kor/api/tasty.html?mode=json&addr={TxtAreaName.Text}";
            string result = string.Empty;

            // WebRequest, WebResponse 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;
            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
                return;
            }

            var jsonResult = JObject.Parse(result);
            var status = Convert.ToString(jsonResult["status"]);

            if (status == "DONE")
            {
                var data = jsonResult["data"];
                var jsonArray = data as JArray;

                var daegufood = new List<DaeguFood>();
                foreach (var item in jsonArray)
                {
                    // ComboBox 선택값과 FD_CS 값이 동일한 데이터들만 추가
                    if (Convert.ToString(item["FD_CS"]) == Helpers.Common.Index)
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
                this.DataContext = daegufood;
            }
        }

        private async void ShowRestaurantInfo()
        {
            this.DataContext = null;

            try
            {
                var CategoryList = new List<DaeguFood>();

                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(Models.DaeguFood.SELECT_CATE_QUERY, conn);
                    cmd.Parameters.AddWithValue("@FD_CS", Helpers.Common.Index);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        CategoryList.Add(new DaeguFood
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
                GrdResult.ItemsSource = CategoryList;

            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"데이터 목록 불러오기 오류: {ex.Message}");
            }
        }


        private async Task<List<DaeguFood>> GetDaeguFoodData(string area)
        {
            string openApiUri = $"https://www.daegufood.go.kr/kor/api/tasty.html?mode=json&addr={area}";
            string result = string.Empty;

            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;
            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();
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
                return daegufood;
            }
            return null;
        }

        // 실시간 조회 및 저장
        private async void BtnReqRealtime_Click(object sender, RoutedEventArgs e)
        {
            isArea = false;
            TxtAreaName.Text = string.Empty;
            ComboFoodType.Text = string.Empty;

            string[] areas = { "중구", "남구", "북구" };
            List<DaeguFood> allDaeguFood = new List<DaeguFood>();

            foreach (var area in areas)
            {
                var data = await GetDaeguFoodData(area);
                if (data != null)
                {
                    allDaeguFood.AddRange(data);
                }
            }

            this.DataContext = allDaeguFood;

            if (GrdResult.Items.Count == 0)
            {
                await this.ShowMessageAsync("저장오류", "실시간 조회 후 저장하십시오.");
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

                        // 저장되기 전에 이미 저장된 데이터인지 확인 후 
                        SqlCommand chkCmd = new SqlCommand(Models.DaeguFood.CHECK_QUERY, conn);
                        chkCmd.Parameters.AddWithValue("@BZ_NM", item.BZ_NM);
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

                        insRes += cmd.ExecuteNonQuery();
                    }

                    if (insRes > 0)
                    {
                        await this.ShowMessageAsync("저장", "DB저장성공");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("저장오류", $"저장오류{ex.Message}");
            }
        }

        private async void GrdResult_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GrdResult.SelectedItem != null)
            {
                var selectedItem = GrdResult.SelectedItem as DaeguFood;
                if (selectedItem != null)
                {
                    //string message = $"지하철 이용시: {selectedItem.SBW}\n 버스 이용시: {selectedItem.BUS}";
                    //MessageBox.Show(message, "대중교통", MessageBoxButton.OK, MessageBoxImage.Information);
                    string message = $"지하철 이용시: {selectedItem.SBW}\n버스 이용시: {selectedItem.BUS}";
                    await this.ShowMessageAsync("대중교통", message);
                }
            }
        }
    }
}