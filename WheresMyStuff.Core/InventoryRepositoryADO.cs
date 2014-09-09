using System;
using System.Collections.Generic;
using System.IO;

namespace WheresMyStuff.Core
{
	public class InventoryRepositoryADO
	{
		InventoryDatabase db = null;
		protected static string dbLocation;		
		protected static InventoryRepositoryADO me;		

		// Constructor
		static InventoryRepositoryADO()
		{
			me = new InventoryRepositoryADO ();
		}

		protected InventoryRepositoryADO()
		{	// Set the location of the database (from the function below)
			dbLocation = DatabaseFilePath;

			// Instantiate it
			db = new InventoryDatabase (dbLocation);
		}

		// Set the file path
		public static string DatabaseFilePath
		{
			get
			{ 
				var sqliteFilename = "InventoryDatabase.db3";
				#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
				#else

				#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
				#else

				#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
				string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
				#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				#endif
				var path = Path.Combine (libraryPath, sqliteFilename);
				#endif

				#endif
				return path;	
			}
		}// End of file path setting


		public static Inventory GetItem(int id)
		{
			return me.db.GetItem(id);
		}

		public static IEnumerable<Inventory> GetItems ()
		{
			return me.db.GetItems();
		}

		public static int SaveItem (Inventory item)
		{
			return me.db.SaveItem(item);
		}

		public static int DeleteItem(int id)
		{
			return me.db.DeleteItem(id);
		}
	}// End of class
}// End of namespace