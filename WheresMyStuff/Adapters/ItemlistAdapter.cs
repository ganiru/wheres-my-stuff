using System.Collections.Generic;
using Android.App;
using Android.Widget;
using WheresMyStuff.Core;
using WheresMyStuff;

namespace WheresMyStuffAndroid.Adapters
{
	/// <summary>
	/// Adapter that presents the items in a row-view
	/// </summary>
	public class ItemlistAdapter : BaseAdapter<Inventory>
	{	Activity context = null;
		IList<Inventory> items = new List<Inventory>();


		public ItemlistAdapter (Activity context, IList<Inventory> items) : base()
		{
			this.context = context;
			this.items = items;
		}

		public override Inventory this[int position]
		{
			get { return items [position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get {return items.Count;}
		}

		// For populating each list item
		public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = items [position];

			// Reuse the convertView if it's not null, for some performance boost
			var view = (convertView ??
			           context.LayoutInflater.Inflate (
						Resource.Layout.EachListItem, parent, false)) as LinearLayout;

			// Stuff missing here...
			var lblItemName = view.FindViewById<TextView> (Resource.Id.lblListItemName);
			var lblItemLocation = view.FindViewById<TextView> (Resource.Id.lblListItemLocation);

			// Assign their values to the various subviews
			lblItemName.SetText (item.ItemName, TextView.BufferType.Normal);
			lblItemLocation.SetText (item.Location, TextView.BufferType.Normal);

			// Return the view
			return view;
		}
	}
}

