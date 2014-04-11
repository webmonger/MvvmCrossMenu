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
			//this.BackgroundColor = UIColor.Clear;
			//			InvokeOnMainThread(()=>{
			//			Title.TextColor = UIColor.White;
			//			});
			var frame = ContentView.Frame;
			frame.Width = 250;
			ContentView.Frame = frame;
			ContentView.Layer.BorderWidth = 2.0f; 

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

