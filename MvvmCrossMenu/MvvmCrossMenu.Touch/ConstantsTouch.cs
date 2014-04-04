using MonoTouch.UIKit;

namespace MvvmCrossMenu.Touch
{
	public static class ConstantsTouch
	{
		public static bool IsIphone { get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; } }
	}
}

