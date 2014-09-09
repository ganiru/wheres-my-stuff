using System;
using System.Collections.Generic;

namespace WheresMyStuff.Core
{
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// It contains functions used by the UI
	/// </summary>
	public static class InventoryManager
	{
		static InventoryManager ()
		{
		}

		public static Inventory GetItem(int id)
		{
			return InventoryRepositoryADO.GetItem(id);
		}

		public static IList<Inventory> GetItems ()
		{
			return new List<Inventory>(InventoryRepositoryADO.GetItems());
		}

		public static int SaveItem (Inventory item)
		{
			return InventoryRepositoryADO.SaveItem(item);
		}

		public static int DeleteItem(int id)
		{
			return InventoryRepositoryADO.DeleteItem(id);
		}

	}
}

