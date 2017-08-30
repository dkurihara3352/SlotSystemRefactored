using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class InventoryItemSB : Slottable, IInventoryItemSB {
		public InventoryItemSB(ISlot slot, InventoryItemInstance item, IInventoryItemHandler itemHandler): base(slot, item, itemHandler){
		}
		public int AcquisitionOrder(){
			IItemHandler itemHandler = ItemHandler();
			Debug.Assert(itemHandler != null && (itemHandler is IInventoryItemHandler));
			IInventoryItemHandler invItemHandler = (IInventoryItemHandler)itemHandler;
			
			return invItemHandler.AcquisitionOrder();
		}
		public void SetAcquisitionOrder(int order){
			IItemHandler itemHandler = ItemHandler();
			Debug.Assert(itemHandler != null && (itemHandler is IInventoryItemHandler));
			IInventoryItemHandler inventoryItemHandler = (IInventoryItemHandler)itemHandler;
			
			inventoryItemHandler.SetAcquitionOrder(order);
		}
	}
	public interface IInventoryItemSB: ISlottable{
		int AcquisitionOrder();
		void SetAcquisitionOrder(int order);
	}
}
