namespace openApi_min.Models
{
    internal class DaeguFood
    {
        public int CNT { get; set; }         // 순번
        public string OPENDATA_ID { get; set; }  // 고유 번호
        public string GNG_CS { get; set; }   // 주소
        public string FD_CS { get; set; }    // 음식카테고리
        public string BZ_NM { get; set; }    // 음식점명
        public string TLNO { get; set; }     // 연락처
        public string MBZ_HR { get; set; }   // 영업시간
        public string SEAT_CNT { get; set; }    // 좌석수
        public string PKPL { get; set; }     // 주차장


        public string SBW {  get; set; } // 지하철

        public string BUS {  get; set; } // 버스




        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[daegufood]
                                                       ([OPENDATA_ID]
                                                       ,[GNG_CS]
                                                       ,[FD_CS]
                                                       ,[BZ_NM]
                                                       ,[TLNO]
                                                       ,[MBZ_HR]
                                                       ,[SEAT_CNT]
                                                       ,[PKPL]
                                                       ,[SBW]
                                                       ,[BUS])
                                                 VALUES
                                                       (@OPENDATA_ID 
                                                       ,@GNG_CS
                                                       ,@FD_CS 
                                                       ,@BZ_NM 
                                                       ,@TLNO
                                                       ,@MBZ_HR
                                                       ,@SEAT_CNT
                                                       ,@PKPL
                                                       ,@SBW
                                                       ,@BUS)";
        public static readonly string SELECT_QUERY = @"SELECT 
                                                      ,[OPENDATA_ID]
                                                      ,[GNG_CS]
                                                      ,[FD_CS]
                                                      ,[BZ_NM]
                                                      ,[TLNO]
                                                      ,[MBZ_HR]
                                                      ,[SEAT_CNT]
                                                      ,[PKPL]
                                                      ,[SBW]
                                                      ,[BUS]
                                                  FROM [dbo].[daegufood]";
    }
}
