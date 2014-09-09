
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WheresMyStuff.Core;
using WheresMyStuff;

namespace WheresMyStuff
{
	[Activity (Label = "New Item")]			
	public class NewItemActivity : Activity
	{
		Inventory item = new Inventory ();
		//Grab the text
		EditText txtItemName;
		EditText txtNotes;
		Spinner ddlLocations;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.ItemDetailEditable);

			// What happens when one is selected (not needed)
			//ddlLocations.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (ddlLocations_itemSelected);

			// The adapter that grabs the array & populates the dropdown)
			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.arrLocations, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			ddlLocations.Adapter = adapter;


			// Assign the buttons
			Button btnSave = FindViewById<Button> (Resource.Id.btnAddItem);
			Button btnCancel = FindViewById<Button> (Resource.Id.btnCancel);
			btnCancel.Text = (item.ID == 0 ? "Cancel" : "Delete");

			btnCancel.Click += (sender, e) => {CancelDelete();};
			btnSave.Click += ( sender, e) => { Save();};
		}

		private void ddlLocations_itemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			//Spinner ddl = (Spinner)sender;
			// do nothing (for now)
		}

		void Save()
		{
			item.ItemName = txtItemName.Text;
			item.Location = ddlLocations.SelectedItem.ToString ();
			item.Notes = txtNotes.Text;
			InventoryManager.SaveItem (item);
			Finish ();
		}


		private void CancelDelete()
		{
			if (item.ID != 0)
			{
				InventoryManager.DeleteItem (item.ID);
			}
			Finish ();
		}
	}
}

