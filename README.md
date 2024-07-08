# WPF 윈폼 개발학습
IoT 개발자 WPF 학습리포지토리 

## 1일차
- WPF(Window Presentation Foundation) 기본학습
    - Winforms 확장한 WPF 
        - 이전의 Winforms는 이미지 비트맴방식(2D)
        - WPF 이미지 벡터방식
        - XAML 화면 디자인 - 안드로이드 개발시 Java XML로 화면디자인과 PyQt로 디자인과 동일

    - XAML(엑스에이엠엘 , 재믈)
        - 여는 태그 <Window>, 닫는 태그 </Window>
        - 합치면 <Window /> - Winodw 태그 안에 다른객체가 없단 뜻
        - 여는 태그와 닫는 태그사이에 다른 태그(객체)를 넣어서 디자인
        

    - WPF 기본 사용법
        - Winforms와는 다르게 코딩으로 디자인을 함 


    - 레이아웃 
        1. Grid - WPF에서 가장 많이 쓰는 대표적인 레이아웃(중요!)
        2. StackPanel - 스택으로 컨트롤을 쌓는 레이아웃
        3. Canvas - 미술에서 캔버스와 유사
        4. DockPanel - 컨트롤을 방향에 따라서 도킹시키는 레이아웃
        5. Margin - 여백기능 , 앵커링 같이함(중요!)


## 2일차 
- WPF 기본학습
    - 데이터바인딩 - 데이터소스(DB, 엑셀, txt, 클라우드에 보관된 데이터원본)에 데이터를 쉽게 가져다쓰기 위해 데이터 핸들링방법
        - xaml : {Binding Path=속성, ElementName=객체, Mode=(oneWay , TwoWay), StringFormat={}{0:#,#}}
        - dataContext : 데이터를 담아서 전달하는 이름 
        - 전통적인 윈폼에서 코드비하인드에서 데이터를 처리하는 것을 지양 - 디자인, 개발 부분 분리

## 3일차
- WPF에서 중요한 개념(윈폼과 차이점)
    1. 데이터바인딩 - 바인딩 키워드로 코드와 분리
    2. 옵저버패턴 - 값이 변경된 사실을 사용자에게 공지 OnPropertyChanged 이벤트
    3. 디자인리소스 - 각 컨트롤마다 디자인(x) , 리소스로 디자인 공유
        - 각 화면당 Resource - 자기 화면에만 적용되는 디자인
        - App.xaml Resources - 애플리케이션 전체에 적용된 디자인
        - 리소스사전 - 공유할 디자인 내용이 많을때 파일로 따로 지정

- WPF MVVM
    - MVC(Model View Controller패턴)
        - 웹개발(Spring , ASP.NET , dJango, etc....) 현재도 사용되고 있음
        - Model : Data 입출력 처리를 담당
        - View : 디스플레이 화면 담당
        - Controller : View를 제어 , Model 처리 중앙에 관장

        - MVVM(Model View ViewModel)
            - Model : Data 입출력(DB side)
            - View : 화면 , 순수 xaml로만 구성
            - ViewModel : 뷰에 대한 메서드 , 액션, INotifyPropertyChanged를 구현 

            ![MVVM패턴](https://raw.githubusercontent.com/leekminxx/basic-wpf-2024/main/imges/wpf001.png)
        
        - 권장 구현방법
            - ViewModel 생성 , 알림 속성 구현,
            - View에 ViewModel을 데이터바인딩
            - Model DB작업 독립적으로 구현 

        - MVVM 구현 도와주는 프레임워크 
            0. Mvvmlight.ToolKit - 3rd Party 개발. 2009년부터 시작 2014년도 부터 더이상 개발이나 지원이 없음.
            1. **Caliburn.Micro** - 3rd Party 개발. MVVM이 아주 간단. 강력. 디버깅이 조금 어려움
            2. AvaloniaUI - 3rd Party 개발 . 크로스플랫폼 . 디자인이 좋음
            3. Prism - Microsoft 개발. 난이도가 굉장히 높다. 대규모 프로젝트 활용

- Caliburn.Micro
    1. 프로젝트 생성 후  MainWindow.xaml 삭제
    2. Models, Views, ViewModels 폴더(네임스페이스) 생성
    3. 종속성 Nuget패키지 Caliburn.Micro 설치
    4. 루트 폴더에 Bootstrapper.cs 클래스 생성 , 작성
    5. App.xaml 에서 StartupUri 삭제
    6. App.xaml에 Bootstrapper 클래스를 리소스사전에 등록
    7. App.xaml.cs에 App() 생성자 추가
    8. ViewModels 폴더에 MainCiewModel.cs 클래스 생성 
    9. Bootstrapper.cs에 OneStartup() 에 내용을 변경
    10. Views 폴더에 MainView.xaml 생성

    - 작업(3명) 분리
        - DB 개발자 - DBMS 테이블 생성 , Models에 클래스 작업
        - Xaml디자이너 - Views 폴더에 있는 xaml 파일을 디자인작업 

## 4일차
- Caliburn.Micro
    - 작업 분리
        - Xaml 디자이너 - xaml 파일만 디자인
        - ViewModel 개발자 - Model에 있는 DB관련정보와 View와 연계 전체구현 작업

    - Calibrun.Micro 특징
        - Xaml 디자인시{Binding....} 잘 사용하지 않음
        - 대신 x:Name을 사용 

    - MVVM특징
        - 예외발생 시 예외메시지 표시없이 프로그램 종료
        - ViewModel에서 디버깅 시작
        - View.xaml 바인딩, 버튼클릭 이름(viewModel 속성, 메서드) 지정 주의 
        - Model은 DB 테이블 컬럼 이르 일치 , CRUD 쿼리문 오타 주의
        - ViewModel 부분
            - 변수 , 속성으로 분리
            - 속성이 Model내의 속성과 이름이 일치 
            - List 사용불가 -> BindableCollection으로 변경
            - 메서드와 이름이 동일한 Can... 프로퍼티 지정 , 버튼 활성 / 비활성화
            - 모든 속성에 NotifyPropertyChange() 메서드 존재!!(값 변경 알림) 
    ![실행화면](https://raw.githubusercontent.com/leekminxx/basic-wpf-2024/main/imges/wpf002.png)

## 5일차
-  MahApps.Metro (https://mahapps.com/)
    - Metro(Modern UI) 디자인 접목

     ![실행화면](https://raw.githubusercontent.com/leekminxx/basic-wpf-2024/main/imges/wpf003.png)

     ![실행화면](https://raw.githubusercontent.com/leekminxx/basic-wpf-2024/main/imges/wpf004.png)

- Movie API 연동 앱
    - 좋아하는 영화 즐겨찾기 앱
    - SQLServer 데이터베이스 연동
    - MahApps.Metro UI & IconPacks
    - CefSharp WebBrowser 패키지
    - Gogle.Apis 패키지 
    - Newtonsoft.Json 패키지
    - MVVM은 시간부족으로 사용안함 
    - 좋아하는 영화 즐겨찾기 앱
    - [TMDB](https://www.themoviedb.org/) OpenAPI 활용
        - 회원가입 후 API 신청
    - [Youtube API](https://console.cloud.google.com/) 활용
        - 새 프로젝트 생성
        - API 및 서비스 , 라이브러리 선택 
        - YouTube Data API v3 선택 
        - 사용자 인증정보 만들기 클릭
            1. 사용자 데이터 라디오버튼 클릭 , 다음
            2. OAutho 동의화면 , 기본내용 입력 후 다음 
            3. 범위는 저장 후 계속      
            4. OAutho Client ID, 앱유형을 데스크톱앱 , 이름 입력 후 만들기 클릭

## 6일차
- MovieFinder 2024 남은것
    - 즐겨찾기 후 다시 선택 즐겨찾기 막아야함
    - 즐겨찾기 삭제 구현
    - 즐겨찾기 일부만 저장기능 추가
    - 그리드뷰 영화를 더블클릭하면 영화소개 팝업

## 7일차
- MovieFinder 2024 완료

   https://github.com/leekminxx/basic-wpf-2024/assets/158007500/d7adf20f-f8ab-4c43-b566-f352879c64fb


- 데이터포털 API 연동앱 예제
    - CefSharp 사용시 플랫폼 대상 AnyCPU에서 x64로 변경필수!
    - MahApps.Metro UI, IconPacks
    - Newtonsoft.Json
    - 5월 13일 개인프로젝트 참조소스

## 8일차 
- WPF 개인프로젝트 포트폴리오 작업
  - 대구 맛집 검색 WPF 애플리케이션
    - 사용자가 대구 지역의 맛집을 음식 종류와 위치에 따라 검색할 수 있도록 함 대구 음식 오픈 API 에서 데티어를 가져와 사용자에게 보여줌으로써 사용자는 맛집 데이터를 로컬 데이터베이스에 저장할 수 있고 해당 맛집으로 가는 대중교통 정보를 확인 할 수 있음 

  - 시스템 아키텍처
    - 프론트엔드 : WPF (Windows Presentation Foundation) - MahApps.Metro를 사용하여 UI 스타일링.
    - 백엔드 : C# - 로직 및 데이터 처리 담당.
    - 데이터베이스: Microsoft SQL Server - 맛집 데이터 저장.
    - API: 대구 음식 오픈 API - 맛집 정보 제공.

  - Open source
    - MahApps.Metro
    - Newtonsoft.Json
    - Microsoft.Data.SqlClient

  - 주요 기능들
    - 지역별 검색: 사용자가 지역 이름을 입력하여 해당 지역의 맛집을 검색할 수 있음.
    - 음식 종류 필터링: 사용자가 드롭다운 메뉴에서 음식 종류를 선택하여 맛집을 필터링할 수 있음.
    - 맛집 정보 보기: 맛집의 세부 정보를 표시.
    - 데이터베이스 저장: 사용자가 가져온 맛집 정보를 로컬 데이터베이스에 저장할 수 있음.
    - 대중교통 정보 보기: 선택한 맛집으로 가는 대중교통 옵션을 볼 수 있음.
    - 실시간 데이터 가져오기: API에서 최신 맛집 정보를 가져와서 표시하고 데이터베이스에 저장할 수 있음.


https://github.com/leekminxx/basic-wpf-2024/assets/158007500/96f71226-dd29-4ee1-b0fa-2605576f51a8


