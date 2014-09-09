using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.IO;
using System.Data;

namespace WheresMyStuff.Core
{	/// <summary>
	/// TaskDatabase uses ADO.NET to create the [Items] table and create,read,update,delete data
	/// </summary>
	public class InventoryDatabase
	{
		// FOr locking the database & tables during a transaction (I think)
		static object locker = new object ();

		// Connection object
		public SqliteConnection connection;

		// The database path
		public string path;

		// Create the database
		public InventoryDatabase (string dbPath)
		{
			var output = "";
			path = dbPath;
			// create the tables
			bool exists = File.Exists (dbPath);

			// If it doesn't exist, create it
			if (!exists)
			{
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, ItemName NTEXT, Location NTEXT, Notes NTEXT);"
				};
				foreach (var command in commands)
				{
					using (var c = connection.CreateCommand ())
					{
						c.CommandText = command;
						var i = c.ExecuteNonQuery ();
					}
				}
			} 

			Console.WriteLine (output);
		}

		/// <summary>Convert from DataReader to Task object</summary>
		Inventory FromReader (SqliteDataReader r)
		{
			var t = new Inventory ();
			t.ID = Convert.ToInt32 (r ["_id"]);
			t.ItemName = r ["ItemName"].ToString ();
			t.Location = r ["Location"].ToString ();
			t.Notes = r ["Notes"].ToString ();
			return t;
		}


		/// <summary>
		/// Get all items
		/// </summary>
		/// <returns>The items as an ienumerable list.</returns>
		public IEnumerable<Inventory> GetItems ()
		{
			var items = new List<Inventory> ();

			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ())
				{
					contents.CommandText = "SELECT [_id], [ItemName], [Location], [Notes] from [Items]";
					var r = contents.ExecuteReader ();
					while (r.Read ())
					{
						items.Add (FromReader(r));
					}
				}
				connection.Close();
			}
			return items;
		}

		/// <summary>
		/// Get a particular item
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="id">Identifier of the item.</param>
		public Inventory GetItem (int id) 
		{
			var item = new Inventory ();
			lock (locker)
			{
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ())
				{
					command.CommandText = "SELECT [_id], [ItemName], [Location], [Notes] from [Items] WHERE [_id] = ?";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id });
					var r = command.ExecuteReader ();
					while (r.Read ()) {
						item = FromReader (r);
						break;
					}
				}
				connection.Close ();
			}
			return item;
		}

		/// <summary>
		/// Saves the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="item">Item.</param>
		public int SaveItem (Inventory item) 
		{
			int r;
			lock (locker)
			{
				if (item.ID != 0)	// Update
				{
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "UPDATE [Items] SET [ItemName] = ?,[Location] = ?, [Notes] = ? WHERE [_id] = ?;";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.ItemName });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Location });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.ID });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				}
				else 	// Create new item
				{
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "INSERT INTO [Items] ([ItemName], [Notes], [Location]) VALUES (? ,?, ?)";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.ItemName });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Location });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				}

			}
		}

		/// <summary>
		/// Deletes the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="id">Identifier.</param>
		public int DeleteItem(int id) 
		{
			lock (locker) {
				int r;
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "DELETE FROM [Items] WHERE [_id] = ?;";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id});
					r = command.ExecuteNonQuery ();
				}
				connection.Close ();
				return r;
			}
		}

	}	// End InventoryDatabase
}	// End namespace