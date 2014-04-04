using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;

namespace MvvmCrossMenu.Droid
{
    [Activity(
        Label = "MvvmCrossMenu.Droid"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
	}
}