using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using WheresMyStuff.Core;
using WheresMyStuffAndroid;


namespace WheresMyStuff
{
	[Activity (Label = "Where's My Stuff?", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		WheresMyStuffAndroid.Adapters.ItemlistAdapter itemlist;
		IList<Inventory> items;
		ListView itemListView;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button btnAdd = FindViewById<Button> (Resource.Id.btnAdd); 
			itemListView = FindViewById<ListView> (Resource.Id.lvItemList);
			TextView lblcount = FindViewById<TextView> (Resource.Id.lblCount);

			if (itemListView.Count > 0)
				lblcount.Text = itemListView.Count + " items.\nPress any of them to edit or delete";

			if (btnAdd != null)
			{
				btnAdd.Click += (sender, e) =>
				{
					StartActivity(typeof(ItemDetailEditable));
				};
			}


			// When you tap any item, the detail should show
			if (itemListView != null)
			{
				itemListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
				{
					var itemDetails = new Intent (this, typeof(ItemDetailEditable));
					itemDetails.PutExtra("itemID", items[e.Position].ID);
					StartActivity(itemDetails);
				};
			}

		}


		protected override void OnResume()
		{
			base.OnResume();

			items = InventoryManager.GetItems ();

			// Create the adapter
			itemlist = new WheresMyStuffAndroid.Adapters.ItemlistAdapter(this, items);

			// Hook it up to the list view
			itemListView.Adapter = itemlist;

			// Update the quantity
			if (itemListView.Count > 0)
				FindViewById<TextView> (Resource.Id.lblCount).Text = itemlist.Count + " items. Press any one to edit or delete";
			else
				FindViewById<TextView> (Resource.Id.lblCount).Text = "No items. Press 'Add' to add to your inventory";

		}
	}
}