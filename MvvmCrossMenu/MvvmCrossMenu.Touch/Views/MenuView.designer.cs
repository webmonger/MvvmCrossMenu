// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace MvvmCrossMenu.Touch.Views
{
	partial class MenuView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel DisplayText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UICollectionView MenuCollectionView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MenuCollectionView != null) {
				MenuCollectionView.Dispose ();
				MenuCollectionView = null;
			}

			if (DisplayText != null) {
				DisplayText.Dispose ();
				DisplayText = null;
			}
		}
	}
}
