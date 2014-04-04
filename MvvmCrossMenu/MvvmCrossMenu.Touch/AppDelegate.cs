using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MvvmCrossMenu.Touch.Helpers;

namespace MvvmCrossMenu.Touch
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : MvxApplicationDelegate
	{
		UIWindow _window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_window = new UIWindow (UIScreen.MainScreen.Bounds);
			_window.BackgroundColor = Theme.BackgroundColour;

			var presenter = new MenuPresenter(this, _window);
			var setup = new Setup(this, presenter);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			_window.MakeKeyAndVisible ();

			return true;
		}
	}
}