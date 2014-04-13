using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using MvvmCrossMenu.Core.ViewModels;

namespace MvvmCrossMenu.Touch.Views.TableViewCells
{
	[Register ("MenuCollectionViewCell")]
	public partial class MenuCollectionViewCell : MvxCollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("MenuCollectionViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("MenuCollectionViewCell");


		public MenuCollectionViewCell (IntPtr handle) : base (string.Empty, handle)
		{
			BackgroundColor = UIColor.Purple;
//			var myFrame = this.Frame;
//			myFrame.Width = 250;
//			myFrame.Height = 50;
//			this.Frame = myFrame;
			this.DelayBind (() => {
				var set = this.CreateBindingSet<MenuCollectionViewCell, MenuItemViewModel> ();
				set.Bind(Title).To(vm => vm.Title);
				set.Apply ();
			});
		}

		public static MenuCollectionViewCell Create ()
		{
			return (MenuCollectionViewCell)Nib.Instantiate (null, null) [0];
		}
	}
}

