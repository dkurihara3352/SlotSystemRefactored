using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface Inventory: IEnumerable<InventoryItemInstance>{
		void Add(InventoryItemInstance item);
		void Remove(InventoryItemInstance item);
		ISlotGroup sg{get;}
		void SetSG(ISlotGroup sg);
		InventoryItemInstance this[int i]{get;}
		int count{get;}
		bool Contains(InventoryItemInstance item);
	}
}