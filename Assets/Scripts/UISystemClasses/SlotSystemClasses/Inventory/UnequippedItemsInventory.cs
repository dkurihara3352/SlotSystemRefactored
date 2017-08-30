using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class UnequippedItemsInventory : Inventory, IUnequippedItemsInventory {
		public UnequippedItemsInventory(List<Item> items){
			_unequippedItems = items;
		}
		public override List<Item> Items(){
			Debug.Assert(_unequippedItems != null);
			return _unequippedItems;
		}
			List<Item> _unequippedItems;
		public void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears){
			List<Item> allEquippedItems = GetAllEquippedItems(equippedBow, equippedWear, equippedCGears);
			foreach(var item in allEquippedItems){
				List<Item> items = Items();
				if(items.Contains(item))
					items.Remove(item);
			}
		}
		List<Item> GetAllEquippedItems(BowInstance bow, WearInstance wear, List<CarriedGearInstance> cGears){
			List<Item> result = new List<Item>();
			result.Add(bow);
			result.Add(wear);
			foreach(var cGear in cGears)
				result.Add((Item)cGear);
			return result;
		}
		public void UpdateItemsEquipState(){
			foreach(var item in Items())
				item.SetEquippability(false);
		}
	}
	public interface IUnequippedItemsInventory: IInventory{
		void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears);
		void UpdateItemsEquipState();
	}
}
