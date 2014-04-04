using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using MvvmCrossMenu.Droid.Helpers;

namespace MvvmCrossMenu.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override IMvxAndroidViewPresenter CreateViewPresenter ()
		{
			CustomPresenter customPresenter = new CustomPresenter ();
			Mvx.RegisterSingleton<ICustomPresenter>(customPresenter);
			return customPresenter;
		}
    }
}