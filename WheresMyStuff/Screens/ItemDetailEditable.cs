
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

namespace WheresMyStuff
{
	[Activity (Label = "Item Details")]			
	public class ItemDetailEditable : Activity
	{
		EditText txtItemName;
		EditText txtNotes;
		Spinner ddlLocations;
		Inventory item = new Inventory ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			int itemID = Intent.GetIntExtra ("itemID", 0);
			if (itemID > 0)
			{
				item = InventoryManager.GetItem (itemID);
			}

			// Create your application here
			SetContentView (Resource.Layout.ItemDetailEditable);

			txtItemName = FindViewById<EditText> (Resource.Id.txtName);
			txtNotes = FindViewById<EditText> (Resource.Id.txtNotes);
			ddlLocations = FindViewById<Spinner> (Resource.Id.ddlLocation);

			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.arrLocations, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			ddlLocations.Adapter = adapter;

			// Populate the textboxes
			if (item.ID > 0) 
			{
				txtItemName.Text = item.ItemName;
				txtNotes.Text = item.Notes;
				ArrayAdapter adap = (ArrayAdapter)ddlLocations.Adapter;
				int pos = adap.GetPosition (item.Location);
				ddlLocations.SetSelection (pos);
			}

			// Assign the buttons
			Button btnSave = FindViewById<Button> (Resource.Id.btnAddItem);
			Button btnCancel = FindViewById<Button> (Resource.Id.btnCancel);
			btnCancel.Text = (item.ID == 0 ? "Cancel" : "Delete");
			btnSave.Text = (item.ID == 0 ? "Add" : "Save Changes");

			btnCancel.Click += (sender, e) => {CancelDelete();};
			btnSave.Click += ( sender, e) => { Save();};
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

