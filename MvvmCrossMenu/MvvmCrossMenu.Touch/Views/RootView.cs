using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MvvmCrossMenu.Core.ViewModels;

namespace MvvmCrossMenu.Touch.Views
{
	[Register("RootView")]
	public partial class RootView : MvxViewController
    {
		public new RootViewModel ViewModel
		{
			get
			{
				return base.ViewModel as RootViewModel;
			}
		}

		public RootView () : base (ConstantsTouch.IsIphone ? "RootView_iPhone" : "RootView_iPad", null)
		{
			//NavigationItem.LeftBarButtonItem = CreateSliderButton ("Images/NavBar/BurgerMenu.png", PanelType.LeftPanel);


			//NavigationItem.RightBarButtonItem = CreateSliderButton("Images/SlideLeft40.png", PanelType.RightPanel);
		}

        public override void ViewDidLoad()
        {
			//View = new UIView(){ BackgroundColor = Theme.Background};
            base.ViewDidLoad();

			// ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
               EdgesForExtendedLayout = UIRectEdge.None;
			   
			// Perform any additional setup after loading the view, typically from a nib.
			LeftArrowImage.Image = UIImage.FromBundle("Images/LeftArrow.png");
			UpArrowImage.Image = UIImage.FromBundle("Images/UpArrow.png");


			var set = this.CreateBindingSet<RootView, Core.ViewModels.RootViewModel>();

            set.Apply();
        }

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear(animated);
			//Theme.SetNavBarStyle (NavigationController);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}
    }
}