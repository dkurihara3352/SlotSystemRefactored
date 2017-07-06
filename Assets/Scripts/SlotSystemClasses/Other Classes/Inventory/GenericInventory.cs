using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class GenericInventory: Inventory{
		public IEnumerator<SlottableItem> GetEnumerator(){
			foreach(SlottableItem item in items){
				yield return item;
			}
			}IEnumerator IEnumerable.GetEnumerator(){
				return GetEnumerator();
			}

		public int count{
			get{return items.Count;}
		}
		public bool Contains(SlottableItem item){
			return items.Contains(item);
		}
		public SlottableItem this[int i]{
			get{return items[i];}
		}
		List<SlottableItem> items = new List<SlottableItem>();
		public void Add(SlottableItem item){
			items.Add(item);
		}
		public void Remove(SlottableItem item){
			items.Remove(item);
		}
		public SlotGroup sg{
			get{return m_sg;}
			}SlotGroup m_sg;
			public void SetSG(SlotGroup sg){
				m_sg = sg;
			}
	}
}