using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class GenericInventory: Inventory{

		public override void Add(IInventoryItemInstance item){
			GetItems().Add(item);
		}
		public override void Remove(IInventoryItemInstance item){
			GetItems().Remove(item);
		}
	}
}