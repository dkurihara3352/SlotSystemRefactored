using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class PoolInventory: Inventory, IPoolInventory{
		public void Add(Item addedItem){
			if(addedItem != null){
				List<Item> items = Items();
				foreach(Item item in items){
					CheckForNonDuplicate(item, addedItem);
					if(item.Equals(addedItem)){
						IncreaseAndSetQuanitity(item, addedItem);
						return;
					}
				}
				items.Add(addedItem);
				IndexItems();
			}else
				throw new System.ArgumentNullException();
		}
		void CheckForNonDuplicate(Item item, Item other){
			if(object.ReferenceEquals(item, other))
				throw new System.InvalidOperationException("PoolInventory.Add: cannot add multiple same IInventoryItemInstances. Try instantiate another instance with the same InventoryItem instead");
		}
		void IncreaseAndSetQuanitity(Item item, Item addedItem){
			int newQuantity = item.GetQuantity() + addedItem.GetQuantity();
			item.SetQuantity(newQuantity);
		}
		public void Remove(Item removedItem){
			if(removedItem != null){
				List<Item> items = Items();
				Item itemToRemove = null;
				foreach(Item item in items){
					if(item.Equals(removedItem)){
						if(!removedItem.IsStackable())
							itemToRemove = item;
						else{
							CheckForRemovedQuantityNonExceedance(item, removedItem);
							DecreaseAndSetQuantity(item, removedItem);
							if(item.GetQuantity() <= 0)
								itemToRemove = item;
						}
					}
				}
				if(itemToRemove != null){
					items.Remove(itemToRemove);
					IndexItems();
				}
			}else
				throw new System.ArgumentNullException();
		}
		void CheckForRemovedQuantityNonExceedance(Item target, Item removedItem){
			if(removedItem.GetQuantity() > target.GetQuantity())
				throw new System.InvalidOperationException("PoolInventory.Remove: cannot remove by greater quantity than there is");	
		}
		void DecreaseAndSetQuantity(Item target, Item removedItem){
			int newCheckedInstQuantity = target.GetQuantity() - removedItem.GetQuantity();
			target.SetQuantity(newCheckedInstQuantity);
		}
		void IndexItems(){
			List<Item> items = Items();
			for(int i = 0; i < items.Count; i ++){
				((Item)items[i]).SetAcquisitionOrder(i);
			}
		}
	}
	public interface IPoolInventory: IInventory{
		void Add(Item addedItem);
		void Remove(Item removedItem);
	}
}