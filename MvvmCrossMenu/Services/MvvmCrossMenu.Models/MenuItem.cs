namespace MvvmCrossMenu.Models
{
	public class MenuItem
	{        
		public string Title { get; set; }
		public MenuType ViewType { get; set; }
	}

	public enum MenuType{
		FirstView,
		Unknown,
	}
}

