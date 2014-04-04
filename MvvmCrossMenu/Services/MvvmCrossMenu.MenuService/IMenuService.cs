using System.Collections.Generic;
using MvvmCrossMenu.Models;

namespace MvvmCrossMenu.Services
{
	public interface IMenuService
	{
		List<MenuItem> GetMenuItems ();
	}
}

