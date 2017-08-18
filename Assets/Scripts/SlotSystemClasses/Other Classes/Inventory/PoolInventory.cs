using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class PoolInventory: Inventory, IPoolInventory{
		public override void Add(IInventoryItemInstance addedItem){
			if(addedItem != null){
				List<IInventoryItemInstance> items = GetItems();
				foreach(IInventoryItemInstance item in items){
					if(object.ReferenceEquals(item, addedItem))
						throw new System.InvalidOperationException("PoolInventory.Add: cannot add multiple same IInventoryItemInstances. Try instantiate another instance with the same InventoryItem instead");
					else
						if(item.Equals(addedItem)){
							int newQuantity = item.GetQuantity() + addedItem.GetQuantity();
							item.SetQuantity(newQuantity);
							return;
						}
				}
				items.Add(addedItem);
				IndexItems();
			}else
				throw new System.ArgumentNullException();
		}
		public override void Remove(IInventoryItemInstance removedItem){
			if(removedItem != null){
				List<IInventoryItemInstance> items = GetItems();
				IInventoryItemInstance itemToRemove = null;
				foreach(IInventoryItemInstance item in items){
					if(item.Equals(removedItem)){
						if(!removedItem.IsStackable())
							itemToRemove = item;
						else{
							if(removedItem.GetQuantity() > item.GetQuantity())
								throw new System.InvalidOperationException("PoolInventory.Remove: cannot remove by greater quantity than there is");
							else{
								int newCheckedInstQuantity = item.GetQuantity() - removedItem.GetQuantity();
								item.SetQuantity(newCheckedInstQuantity);
								if(item.GetQuantity() <= 0)
									itemToRemove = item;
							}
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
		void IndexItems(){
			List<IInventoryItemInstance> items = GetItems();
			for(int i = 0; i < items.Count; i ++){
				((IInventoryItemInstance)items[i]).SetAcquisitionOrder(i);
			}
		}
	}
	public interface IPoolInventory: IInventory{
	}
}