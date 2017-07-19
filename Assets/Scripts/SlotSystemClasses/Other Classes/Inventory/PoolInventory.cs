using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class PoolInventory: IPoolInventory{
		public IEnumerator<SlottableItem> GetEnumerator(){
			foreach(SlottableItem item in m_items){
				yield return item;
			}
			}IEnumerator IEnumerable.GetEnumerator(){
				return GetEnumerator();
			}
		public bool Contains(SlottableItem item){
			return m_items.Contains(item);
		}
		public int count{
			get{return m_items.Count;}
		}
		public SlottableItem this[int i]{
			get{return m_items[i];}
		}
		List<SlottableItem> m_items = new List<SlottableItem>();
		public ISlotGroup sg{get{return m_sg;}}
			ISlotGroup m_sg;
			public void SetSG(ISlotGroup sg){
				m_sg = sg;
			}
		public void Add(SlottableItem item){
			if(item != null){
				foreach(SlottableItem it in m_items){
					InventoryItemInstance invInst = (InventoryItemInstance)it;
					InventoryItemInstance addedInst = (InventoryItemInstance)item;
					if(object.ReferenceEquals(invInst, addedInst))
						throw new System.InvalidOperationException("PoolInventory.Add: cannot add multiple same InventoryItemInstances. Try instantiate another instance with the same InventoryItem instead");
					if(invInst == addedInst){
						invInst.quantity += addedInst.quantity;
						return;
					}
				}
				m_items.Add(item);
				IndexItems();
			}else
			throw new System.ArgumentNullException();
		}
		public void Remove(SlottableItem item){
			if(item != null){
				SlottableItem itemToRemove = null;
				foreach(SlottableItem it in m_items){
					InventoryItemInstance checkedInst = (InventoryItemInstance)it;
					InventoryItemInstance removedInst = (InventoryItemInstance)item;
					if(checkedInst == removedInst){
						if(!removedInst.IsStackable)
							itemToRemove = it;
						else{
							if(removedInst.quantity > checkedInst.quantity)
								throw new System.InvalidOperationException("PoolInventory.Remove: cannot remove by greater quantity than there is");
							else{
								checkedInst.quantity -= removedInst.quantity;
								if(checkedInst.quantity <= 0)
									itemToRemove = it;
							}
						}
					}
				}
				if(itemToRemove != null){
					m_items.Remove(itemToRemove);
					IndexItems();
				}
			}else
				throw new System.ArgumentNullException();
		}
		void IndexItems(){
			for(int i = 0; i < m_items.Count; i ++){
				((InventoryItemInstance)m_items[i]).SetAcquisitionOrder(i);
			}
		}
	}
	public interface IPoolInventory: Inventory{
	}
}