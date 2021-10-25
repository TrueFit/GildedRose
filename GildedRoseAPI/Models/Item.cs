using System;
using System.ComponentModel.DataAnnotations;

namespace GildedRoseAPI.Models
{
	public class Item
	{
		public int ItemID { get; set; }
		[Required] public string Name { get; set; }
		[Required] public string Category { get; set; }
		[Required] public int SellIn { get; set; }
		[Required] public int Quality { get; set; }
	}
}