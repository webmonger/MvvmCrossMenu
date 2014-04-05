using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MvvmCrossMenu.Core.ViewModels.Base;
using MvvmCrossMenu.Models;
using MvvmCrossMenu.Services;

namespace MvvmCrossMenu.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
		IMenuService _menuService;

		public MenuViewModel(IMenuService menuService)
		{
			_menuService = menuService;
			MenuItems = _menuService.GetMenuItems ().Select(x=>new MenuItemViewModel(){Title = x.Title,ViewType = x.ViewType}).ToList();
		}

		public void Init(){

		}

		private List<MenuItemViewModel> _menuItems;
		public List<MenuItemViewModel> MenuItems
        {
			get { return _menuItems; }
            set
            {
                this._menuItems = value;
                this.RaisePropertyChanged(() => this.MenuItems);
            }
		}

		private MvxCommand<MenuItemViewModel> _selectMenuItemCommand;
		public ICommand SelectMenuItemCommand
		{
			get {
				_selectMenuItemCommand = _selectMenuItemCommand ?? new MvxCommand<MenuItemViewModel>(DoSelectItem);
				return _selectMenuItemCommand;
			}
		}

		private void DoSelectItem(MenuItemViewModel item)
		{
			Mvx.Trace (item.Title + " " + item.ViewType.ToString());
			ShowView (item.ViewType);
		}

		private void ShowView (MenuType type)
		{
			switch (type) {
			case MenuType.FirstView:
				ShowViewModel<FirstViewModel> ();
				break;
			default:
				Mvx.Trace ("Menu not hooked up");
				break;
			}
		}
    }
}

