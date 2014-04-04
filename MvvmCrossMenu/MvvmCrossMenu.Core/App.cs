using Cirrious.CrossCore.IoC;
using MvvmCrossMenu.Services;

namespace MvvmCrossMenu.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
			CreatableTypes(typeof(IMenuService).Assembly)
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
			RegisterAppStart<ViewModels.RootViewModel>();
        }
    }
}