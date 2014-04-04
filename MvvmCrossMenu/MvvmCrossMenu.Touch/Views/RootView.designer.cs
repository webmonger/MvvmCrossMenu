using MonoTouch.Foundation;

namespace MvvmCrossMenu.Touch.Views
{
	partial class RootView
	{
		[Outlet]
		MonoTouch.UIKit.UITextView CenterText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView RightArrowImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView LeftArrowImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView UpArrowImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView NavigateText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton NavigateButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CenterText != null) {
				CenterText.Dispose ();
				CenterText = null;
			}

			if (RightArrowImage != null) {
				RightArrowImage.Dispose ();
				RightArrowImage = null;
			}

			if (LeftArrowImage != null) {
				LeftArrowImage.Dispose ();
				LeftArrowImage = null;
			}

			if (UpArrowImage != null) {
				UpArrowImage.Dispose ();
				UpArrowImage = null;
			}

			if (NavigateText != null) {
				NavigateText.Dispose ();
				NavigateText = null;
			}

			if (NavigateButton != null) {
				NavigateButton.Dispose ();
				NavigateButton = null;
			}
		}
	}
}

