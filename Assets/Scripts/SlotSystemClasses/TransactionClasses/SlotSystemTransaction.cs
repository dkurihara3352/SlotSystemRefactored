using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SlotSystemTransaction{
		Slottable targetSB{get;}
		SlotGroup sg1{get;}
		SlotGroup sg2{get;}
		List<InventoryItemInstance> moved{get;}
		void Indicate();
		void Execute();
		void OnComplete();
	}
}
