using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MvvmCrossMenu.Core.ViewModels;
using MvvmCrossMenu.Touch.Helpers;
using MvvmCrossMenu.Touch.Views.TableViewCells;
using MonoTouch.UIKit;

namespace MvvmCrossMenu.Touch.Views
{
	[Register("MenuView")]
	public partial class MenuView : MvxViewController
    {
		public MenuView ()
			: base (ConstantsTouch.IsIphone ? "MenuView_iPhone" : "MenuView_iPad", null)
		{
		}

		public new MenuViewModel ViewModel{
			get{ return ViewModel as MenuViewModel; }
			set{ ViewModel = value; }
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

			MenuCollectionView.RegisterNibForCell(MenuCollectionViewCell.Nib, MenuCollectionViewCell.Key);
			var source = new MvxCollectionViewSource (MenuCollectionView, MenuCollectionViewCell.Key);
			MenuCollectionView.Source = source;
		    MenuCollectionView.SetCollectionViewLayout(new SpringFlowLayout(), true);
			var set = this.CreateBindingSet<MenuView, MenuViewModel> ();
			set.Bind (source).To (vm => vm.MenuItems);
			set.Bind (source).For (e => e.SelectionChangedCommand).To (vm =>vm.SelectMenuItemCommand);
			set.Apply();

			MenuCollectionView.ReloadData ();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

			ReleaseDesignerOutlets ();
		}
    }
}