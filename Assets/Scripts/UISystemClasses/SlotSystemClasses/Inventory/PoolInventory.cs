using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class PoolInventory: Inventory, IPoolInventory{
		public void Add(IInventorySystemItem addedItem){
			if(addedItem != null){
				List<IInventorySystemItem> items = Items();
				foreach(IInventorySystemItem item in items){
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
		void CheckForNonDuplicate(IInventorySystemItem item, IInventorySystemItem other){
			if(object.ReferenceEquals(item, other))
				throw new System.InvalidOperationException("PoolInventory.Add: cannot add multiple same IInventoryItemInstances. Try instantiate another instance with the same InventoryItem instead");
		}
		void IncreaseAndSetQuanitity(IInventorySystemItem item, IInventorySystemItem addedItem){
			int newQuantity = item.GetQuantity() + addedItem.GetQuantity();
			item.SetQuantity(newQuantity);
		}
		public void Remove(IInventorySystemItem removedItem){
			if(removedItem != null){
				List<IInventorySystemItem> items = Items();
				IInventorySystemItem itemToRemove = null;
				foreach(IInventorySystemItem item in items){
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
		void CheckForRemovedQuantityNonExceedance(IInventorySystemItem target, IInventorySystemItem removedItem){
			if(removedItem.GetQuantity() > target.GetQuantity())
				throw new System.InvalidOperationException("PoolInventory.Remove: cannot remove by greater quantity than there is");	
		}
		void DecreaseAndSetQuantity(IInventorySystemItem target, IInventorySystemItem removedItem){
			int newCheckedInstQuantity = target.GetQuantity() - removedItem.GetQuantity();
			target.SetQuantity(newCheckedInstQuantity);
		}
		void IndexItems(){
			List<IInventorySystemItem> items = Items();
			for(int i = 0; i < items.Count; i ++){
				((IInventorySystemItem)items[i]).SetAcquisitionOrder(i);
			}
		}
	}
	public interface IPoolInventory: IInventory{
		void Add(IInventorySystemItem addedItem);
		void Remove(IInventorySystemItem removedItem);
	}
}