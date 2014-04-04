// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using MonoTouch.Foundation;

namespace MvvmCrossMenu.Touch.Views
{
	partial class MenuView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel DisplayText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView MenuTableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DisplayText != null) {
				DisplayText.Dispose ();
				DisplayText = null;
			}

			if (MenuTableView != null) {
				MenuTableView.Dispose ();
				MenuTableView = null;
			}
		}
	}
}
