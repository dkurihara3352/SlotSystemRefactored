using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class Inventory: IInventory{
		public abstract void Add(IInventoryItemInstance item);
		public abstract void Remove(IInventoryItemInstance item);
		public virtual List<IInventoryItemInstance> GetItems(){
			if(_items == null)
				_items = new List<IInventoryItemInstance>();
			return _items;
		}
			List<IInventoryItemInstance> _items;
		public ISlotGroup GetSG(){
			return _sg;
		}
		public virtual void SetSG(ISlotGroup sg){
			_sg = sg;
		}
			ISlotGroup _sg;
	}
	public interface IInventory{
		void Add(IInventoryItemInstance item);
		void Remove(IInventoryItemInstance item);
		List<IInventoryItemInstance> GetItems();
		ISlotGroup GetSG();
		void SetSG(ISlotGroup sg);
	}
}