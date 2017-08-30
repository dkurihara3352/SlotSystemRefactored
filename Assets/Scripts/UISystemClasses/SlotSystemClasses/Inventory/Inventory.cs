using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public abstract class Inventory: IInventory{
		public virtual List<Item> Items(){
			if(_items == null)
				_items = new List<Item>();
			return _items;
		}
			List<Item> _items;
		public ISlotGroup GetSG(){
			return _sg;
		}
		public virtual void SetSG(ISlotGroup sg){
			_sg = sg;
		}
			ISlotGroup _sg;
		public virtual void UpdateInventory(){}
	}
	public interface IInventory{
		List<Item> Items();
		ISlotGroup GetSG();
		void SetSG(ISlotGroup sg);
		void UpdateInventory();
	}
}