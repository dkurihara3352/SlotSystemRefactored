using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class PoolInventory: Inventory, IPoolInventory{
		public void Add(IInventoryItemInstance addedItem){
			if(addedItem != null){
				List<IInventoryItemInstance> items = GetItems();
				foreach(IInventoryItemInstance item in items){
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
		void CheckForNonDuplicate(IInventoryItemInstance item, IInventoryItemInstance other){
			if(object.ReferenceEquals(item, other))
				throw new System.InvalidOperationException("PoolInventory.Add: cannot add multiple same IInventoryItemInstances. Try instantiate another instance with the same InventoryItem instead");
		}
		void IncreaseAndSetQuanitity(IInventoryItemInstance item, IInventoryItemInstance addedItem){
			int newQuantity = item.GetQuantity() + addedItem.GetQuantity();
			item.SetQuantity(newQuantity);
		}
		public void Remove(IInventoryItemInstance removedItem){
			if(removedItem != null){
				List<IInventoryItemInstance> items = GetItems();
				IInventoryItemInstance itemToRemove = null;
				foreach(IInventoryItemInstance item in items){
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
		void CheckForRemovedQuantityNonExceedance(IInventoryItemInstance target, IInventoryItemInstance removedItem){
			if(removedItem.GetQuantity() > target.GetQuantity())
				throw new System.InvalidOperationException("PoolInventory.Remove: cannot remove by greater quantity than there is");	
		}
		void DecreaseAndSetQuantity(IInventoryItemInstance target, IInventoryItemInstance removedItem){
			int newCheckedInstQuantity = target.GetQuantity() - removedItem.GetQuantity();
			target.SetQuantity(newCheckedInstQuantity);
		}
		void IndexItems(){
			List<IInventoryItemInstance> items = GetItems();
			for(int i = 0; i < items.Count; i ++){
				((IInventoryItemInstance)items[i]).SetAcquisitionOrder(i);
			}
		}
	}
	public interface IPoolInventory: IInventory{
		void Add(IInventoryItemInstance addedItem);
		void Remove(IInventoryItemInstance removedItem);
	}
}