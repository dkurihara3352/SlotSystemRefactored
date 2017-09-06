using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class UnequippedItemsInventory : Inventory, IUnequippedItemsInventory {
		public UnequippedItemsInventory(List<IInventorySystemItem> items){
			_unequippedItems = items;
		}
		public override List<IInventorySystemItem> Items(){
			Debug.Assert(_unequippedItems != null);
			return _unequippedItems;
		}
			List<IInventorySystemItem> _unequippedItems;
		public void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears){
			List<IInventorySystemItem> allEquippedItems = GetAllEquippedItems(equippedBow, equippedWear, equippedCGears);
			foreach(var item in allEquippedItems){
				List<IInventorySystemItem> items = Items();
				if(items.Contains(item))
					items.Remove(item);
			}
		}
		List<IInventorySystemItem> GetAllEquippedItems(BowInstance bow, WearInstance wear, List<CarriedGearInstance> cGears){
			List<IInventorySystemItem> result = new List<IInventorySystemItem>();
			result.Add(bow);
			result.Add(wear);
			foreach(var cGear in cGears)
				result.Add((IInventorySystemItem)cGear);
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
