using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class UnequippedItemsInventory : Inventory, IUnequippedItemsInventory {
		public UnequippedItemsInventory(List<IInventoryItemInstance> items){
			_unequippedItems = items;
		}
		public override List<IInventoryItemInstance> GetItems(){
			Debug.Assert(_unequippedItems != null);
			return _unequippedItems;
		}
			List<IInventoryItemInstance> _unequippedItems;
		public void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears){
			List<IInventoryItemInstance> allEquippedItems = GetAllEquippedItems(equippedBow, equippedWear, equippedCGears);
			foreach(var item in allEquippedItems){
				List<IInventoryItemInstance> items = GetItems();
				if(items.Contains(item))
					items.Remove(item);
			}
		}
		List<IInventoryItemInstance> GetAllEquippedItems(BowInstance bow, WearInstance wear, List<CarriedGearInstance> cGears){
			List<IInventoryItemInstance> result = new List<IInventoryItemInstance>();
			result.Add(bow);
			result.Add(wear);
			foreach(var cGear in cGears)
				result.Add((IInventoryItemInstance)cGear);
			return result;
		}
		public void UpdateItemsEquipState(){
			foreach(var item in GetItems())
				item.SetEquippability(false);
		}
	}
	public interface IUnequippedItemsInventory: IInventory{
		void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears);
		void UpdateItemsEquipState();
	}
}
