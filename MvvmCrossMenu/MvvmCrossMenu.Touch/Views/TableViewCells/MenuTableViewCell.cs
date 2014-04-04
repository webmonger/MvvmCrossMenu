using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MvvmCrossMenu.Core.ViewModels;

namespace MvvmCrossMenu.Touch.Views.TableViewCells
{
	public partial class MenuTableViewCell : MvxTableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("MenuTableViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("MenuTableViewCell");

		public MenuTableViewCell (IntPtr handle)
			: base(handle)
		{
			this.BackgroundColor = UIColor.Clear;
//			InvokeOnMainThread(()=>{
//			Title.TextColor = UIColor.White;
//			});

			this.DelayBind(() => {
				var set = this.CreateBindingSet<MenuTableViewCell, MenuItemViewModel>();
				set.Bind(Title).To(vm => vm.Title);
				set.Apply ();
			});
		}

		public static MenuTableViewCell Create ()
		{
			return (MenuTableViewCell)Nib.Instantiate (null, null) [0];
		}
	}
}

