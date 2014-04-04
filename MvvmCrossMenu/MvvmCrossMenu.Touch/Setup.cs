using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.UIKit;

namespace MvvmCrossMenu.Touch
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
			: base(applicationDelegate, presenter)
		{
		}

		protected override IMvxApplication CreateApp ()
		{
			return new Core.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override void InitializeFirstChance ()
		{
			//Mvx.RegisterSingleton<IMenuService>(new MenuService());
			base.InitializeFirstChance ();
		}
	}
}