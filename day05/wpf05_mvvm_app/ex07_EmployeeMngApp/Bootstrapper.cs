using Caliburn.Micro;
using ex07_EmployeeMngApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ex07_EmployeeMngApp
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();   //Caliburn.Micro MVVM 시작하도록 초기화.
        }
        // MVVM 애플리케이션이 처음 시작될때 이벤트핸들러.
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(sender, e);
            DisplayRootViewForAsync<MainViewModel>();   // MainViewModel 과 뷰화면을 합쳐서 표시
        }
    }
}
