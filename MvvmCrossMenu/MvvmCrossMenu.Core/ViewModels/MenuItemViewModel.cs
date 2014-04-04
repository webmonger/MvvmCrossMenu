using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MvvmCrossMenu.Models;

namespace MvvmCrossMenu.Core.ViewModels
{
	public class MenuItemViewModel : MvxViewModel
    {
        public string Title { get; set; }
		public MenuType ViewType { get; set; }

		public MvxCommand SelectedCommand
		{
			get{
				return new MvxCommand (() => Mvx.Trace (ViewType.GetType ().ToString ()));
			}
		}
    }
}