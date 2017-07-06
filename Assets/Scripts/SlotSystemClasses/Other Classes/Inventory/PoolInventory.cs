using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class PoolInventory: Inventory{
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
		public SlotGroup sg{get{return m_sg;}}
			SlotGroup m_sg;
			public void SetSG(SlotGroup sg){
				m_sg = sg;
			}
		public void Add(SlottableItem item){
			foreach(SlottableItem it in m_items){
				InventoryItemInstance invInst = (InventoryItemInstance)it;
				InventoryItemInstance addedInst = (InventoryItemInstance)item;
				if(invInst == addedInst && invInst.IsStackable){
					invInst.Quantity += addedInst.Quantity;
					return;
				}
			}
			m_items.Add(item);
			IndexItems();
		}
		public void Remove(SlottableItem item){
			SlottableItem itemToRemove = null;
			foreach(SlottableItem it in m_items){
				InventoryItemInstance checkedInst = (InventoryItemInstance)it;
				InventoryItemInstance removedInst = (InventoryItemInstance)item;
				if(checkedInst == removedInst){
					if(!removedInst.IsStackable)
						itemToRemove = it;
					else{
						checkedInst.Quantity -= removedInst.Quantity;
						if(checkedInst.Quantity <= 0)
							itemToRemove = it;
					}
				}
			}
			if(itemToRemove != null)
				m_items.Remove(itemToRemove);
			IndexItems();
		}
		void IndexItems(){
			for(int i = 0; i < m_items.Count; i ++){
				((InventoryItemInstance)m_items[i]).SetAcquisitionOrder(i);
			}
		}
	}
}