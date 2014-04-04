using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;

namespace MvvmCrossMenu.Touch.Views
{
    [Register("FirstView")]
	public partial class FirstView : MvxViewController
    {
        public FirstView()
            : base(ConstantsTouch.IsIphone ? "FirstView_iPhone" : "FirstView_iPad", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();


		}
    }
}