using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MvvmCrossMenu.Core.ViewModels;
using MvvmCrossMenu.Touch.Views.TableViewCells;

namespace MvvmCrossMenu.Touch.Views
{
	[Register("MenuView")]
	public partial class MenuView : MvxViewController
    {
		public MenuView ()
			: base (ConstantsTouch.IsIphone ? "MenuView_iPhone" : "MenuView_iPad", null)
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
			//this.View.BackgroundColor = Theme.Background;

			var source = new MvxSimpleTableViewSource(MenuTableView, MenuTableViewCell.Key, MenuTableViewCell.Key);
			MenuTableView.Source = source;
			//MenuTableView.BackgroundColor = Theme.Background;
			//MenuTableView.SeparatorColor = Theme.HighLight;

			var set = this.CreateBindingSet<MenuView, MenuViewModel>();
			set.Bind (source).To (vm=> vm.MenuItems);
			set.Bind (source).For (e => e.SelectionChangedCommand).To (vm =>vm.SelectMenuItemCommand);
			set.Apply();

			MenuTableView.ReloadData();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

			ReleaseDesignerOutlets ();
		}
    }
}