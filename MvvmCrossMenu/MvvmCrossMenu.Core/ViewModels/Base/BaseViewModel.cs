using Cirrious.MvvmCross.ViewModels;

namespace MvvmCrossMenu.Core.ViewModels.Base
{
	public class BaseViewModel : MvxViewModel
	{
		private long _id;
		/// <summary>
		/// Gets or sets the unique ID for the menu
		/// </summary>
		public long Id
		{
			get { return this._id; }
			set { this._id = value; this.RaisePropertyChanged(() => this.Id); }
		}

		private string _title = string.Empty;
		/// <summary>
		/// Gets or sets the name of the menu
		/// </summary>
		public string Title
		{
			get { return this._title; }
			set { this._title = value; this.RaisePropertyChanged(() => this.Title); }
		}
	}
}

