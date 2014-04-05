using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Android.Views;

namespace MvvmCrossMenu.Droid.Views
{
    [Activity(Label = "View for FirstView")]
	public class FirstView : MvxFragment
    {
		public FirstView()
        {
            this.RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			this.HasOptionsMenu = true;
			var ignored = base.OnCreateView (inflater, container, savedInstanceState);
			return this.BindingInflate (Resource.Layout.FirstView, null);
		}

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
			//inflater.Inflate(Resource.Menu.refresh, menu);
        }
    }
}
