using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public abstract class Inventory: IInventory{
		public virtual List<IInventorySystemItem> Items(){
			if(_items == null)
				_items = new List<IInventorySystemItem>();
			return _items;
		}
			List<IInventorySystemItem> _items;
		public ISlotGroup SlotGroup(){
			return _sg;
		}
		public virtual void SetSG(ISlotGroup sg){
			_sg = sg;
		}
			ISlotGroup _sg;
		public virtual void UpdateInventory(){}
	}
	public interface IInventory{
		List<IInventorySystemItem> Items();
		ISlotGroup SlotGroup();
		void SetSG(ISlotGroup sg);
		void UpdateInventory();
	}
}