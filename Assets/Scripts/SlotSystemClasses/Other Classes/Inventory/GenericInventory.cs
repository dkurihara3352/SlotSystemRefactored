using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class GenericInventory: Inventory{
		public IEnumerator<InventoryItemInstance> GetEnumerator(){
			foreach(InventoryItemInstance item in items){
				yield return item;
			}
			}IEnumerator IEnumerable.GetEnumerator(){
				return GetEnumerator();
			}

		public int count{
			get{return items.Count;}
		}
		public bool Contains(InventoryItemInstance item){
			return items.Contains(item);
		}
		public InventoryItemInstance this[int i]{
			get{return items[i];}
		}
		List<InventoryItemInstance> items = new List<InventoryItemInstance>();
		public void Add(InventoryItemInstance item){
			items.Add(item);
		}
		public void Remove(InventoryItemInstance item){
			items.Remove(item);
		}
		public ISlotGroup sg{
			get{return m_sg;}
			}ISlotGroup m_sg;
			public void SetSG(ISlotGroup sg){
				m_sg = sg;
			}
	}
}