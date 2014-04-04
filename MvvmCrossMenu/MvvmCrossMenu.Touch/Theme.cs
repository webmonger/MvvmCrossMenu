using MonoTouch.UIKit;

namespace MvvmCrossMenu.Touch
{
	public static class Theme
	{

		// Colours
		public static UIColor TextColour { get { return UIColor.White; } }
		public static UIColor Background { get { return UIColor.Black; } }
		public static UIColor HighLight { get { return UIColor.Purple; } }



		// Objects
		public static UIColor BackgroundColour { get { return Background; } }

		public static void SetNavBarStyle(UINavigationController navController){
			var navigationBar = navController.NavigationBar;
			navigationBar.BarTintColor = Background;
			navigationBar.TintColor = HighLight;
			navigationBar.SetTitleTextAttributes(new UITextAttributes() { TextColor = HighLight });
			navigationBar.Opaque = true;
			navigationBar.SetBackgroundImage (UIImage.FromFile ("Images/NavBar/background.png"), UIBarMetrics.Default);
			navigationBar.Translucent = false;
		}
	}
}

