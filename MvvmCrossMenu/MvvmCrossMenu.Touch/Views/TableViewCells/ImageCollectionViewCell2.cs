using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.BindingContext;
using MyLegoCreations.Core.ViewModels;

namespace MyLegoCreations.Touch
{
	public partial class ImageCollectionViewCell : MvxCollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("ImageCollectionViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ImageCollectionViewCell");

		public ImageCollectionViewCell (IntPtr handle)
			: base(handle)
		{
			this.DelayBind(() => {
				var set = this.CreateBindingSet<ImageCollectionViewCell, ImageCollectionViewModel>();
				//set.Bind(Title).To(vm => vm.Title);
				set.Apply ();
			});
		}

		public static ImageCollectionViewCell Create ()
		{
			return (ImageCollectionViewCell)Nib.Instantiate (null, null) [0];
		}
	}
}

