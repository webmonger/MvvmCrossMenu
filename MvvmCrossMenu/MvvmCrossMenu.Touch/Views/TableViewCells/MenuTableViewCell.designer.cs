// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using MonoTouch.Foundation;

namespace MvvmCrossMenu.Touch.Views.TableViewCells
{
	[Register ("MenuTableViewCell")]
	partial class MenuTableViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel Title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Title != null) {
				Title.Dispose ();
				Title = null;
			}
		}
	}
}
