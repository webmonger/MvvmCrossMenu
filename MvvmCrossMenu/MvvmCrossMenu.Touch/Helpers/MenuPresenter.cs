using System.Drawing;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using MvvmCrossMenu.Touch.Views;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;

namespace MvvmCrossMenu.Touch.Helpers
{
	public class MenuPresenter : MvxSlidingPanelsTouchViewPresenter//, IModalDisplay
	{
		private UIViewController _currentModal;

		public MenuPresenter (UIApplicationDelegate applicationDelegate, UIWindow window) :base(applicationDelegate, window)
		{
			
		}

		public override void Show (IMvxTouchView view)
		{
			var viewController = view as UIViewController;
			if (viewController == null)
				throw new MvxException("Passed in IMvx TouchView is not a UIViewController");

			bool isRootView = view is RootView || view is FirstView;

			if (this.MasterNavigationController == null) {
				ShowFirstView (viewController);
			} else {
				if (viewController is IMvxModalTouchView) {
					bool animate = true;

					_currentModal = viewController;

					MasterNavigationController.PresentViewController (viewController, animate, null);
				} else {
					if (isRootView)
						this.MasterNavigationController.ViewControllers = new UIViewController[0];
					this.MasterNavigationController.PushViewController (viewController, true /*animated*/);
					var slidingPanelNavController = this.MasterNavigationController as SlidingPanelsNavigationViewController;

					var container = slidingPanelNavController.ExistingContainerForType (PanelType.LeftPanel);

					if (container.IsVisible) {
						slidingPanelNavController.HidePanel (container);
					}
				}
			}
			if (isRootView)
				viewController.NavigationItem.LeftBarButtonItem = CreateSliderButton ("Images/NavBar/BurgerMenu.png", PanelType.LeftPanel);
		}

		private UIBarButtonItem CreateSliderButton(string imageName, PanelType panelType)
		{
			UIButton button = new UIButton(new RectangleF(0, 0, 20f, 20f));
//			var hamburgerMenu = new HamburgerMenu (new RectangleF(0, 0, 20f, 20f));
//			button.Add (hamburgerMenu);
			button.SetBackgroundImage(UIImage.FromBundle(imageName), UIControlState.Normal);
			button.TouchUpInside += delegate
			{
				SlidingPanelsNavigationViewController navController = this.MasterNavigationController as SlidingPanelsNavigationViewController;
				navController.TogglePanel(panelType);
			};

			return new UIBarButtonItem(button);
		}
	}
}

