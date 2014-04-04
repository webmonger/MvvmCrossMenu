using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MvvmCrossMenu.Core.ViewModels.Base;
using MvvmCrossMenu.Models;
using MvvmCrossMenu.Services;

namespace MvvmCrossMenu.Core.ViewModels
{
	public class RootViewModel 
		: BaseViewModel
    {
		readonly IMenuService _menuService;
		byte[] _returnedPicture;

		private List<MenuItem> _menuItems;
		public List<MenuItem> MenuItems
		{
			get { return this._menuItems; }
			set { this._menuItems = value; this.RaisePropertyChanged(() => this.MenuItems); }
		}

		private MvxCommand<MenuItem> _selectMenuItemCommand;
		public ICommand SelectMenuItemCommand
		{
			get {
				_selectMenuItemCommand = _selectMenuItemCommand ?? new MvxCommand<MenuItem>(DoSelectItem);
				return _selectMenuItemCommand;
			}
		}

		private byte[] _bytes;
		public byte[] Bytes
		{
			get { return _bytes; }
			set { _bytes = value; RaisePropertyChanged(() => Bytes); }
		}

		public RootViewModel (IMenuService menuService)
		{
			_menuService = menuService;
			MenuItems = _menuService.GetMenuItems ();
		}

		public Type GetSectionForViewModelType(MenuType type)
		{

			if (type == MenuType.FirstView)
				return typeof(FirstViewModel);

			return null;
		}

		public MenuType GetSectionForViewModelType(Type type)
		{

			if (type == typeof(FirstViewModel))
				return MenuType.FirstView;

			return MenuType.Unknown;
		}

		private void DoSelectItem(MenuItem item)
		{
			//ShowViewModel<DetailViewModel>(item);
			Mvx.Trace (item.Title + " " + item.ViewType.ToString());
		}

    }
}
