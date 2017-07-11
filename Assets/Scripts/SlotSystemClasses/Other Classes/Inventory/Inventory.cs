using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface Inventory: IEnumerable<SlottableItem>{
		void Add(SlottableItem item);
		void Remove(SlottableItem item);
		ISlotGroup sg{get;}
		void SetSG(ISlotGroup sg);
		SlottableItem this[int i]{get;}
		int count{get;}
		bool Contains(SlottableItem item);
	}
}