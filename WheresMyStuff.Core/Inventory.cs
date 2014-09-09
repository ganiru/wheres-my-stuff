using System;

namespace WheresMyStuff.Core
{
	public class Inventory
	{
		public Inventory()
		{
		}

		public int ID { get; set; }
		public string ItemName {get; set;}
		public string Location {get; set;}
		public string Notes {get; set;}
	}
}

