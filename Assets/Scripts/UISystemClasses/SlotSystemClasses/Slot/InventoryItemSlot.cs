using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface IInventoryItemSlot: ISlot{
		int AcquisitionOrder();
		void SetAcquisitionOrder(int order);
	}
	public class InventoryItemSlot : Slot, IInventoryItemSlot {
		public InventoryItemSlot(RectTransformFake rectTrans, ISBSelStateRepo selStateRepo, ITapCommand tapCommand, ISSEEventCommandsRepo eventCommandsRepo, InventoryItemInstance item, bool leavesGhost): base(rectTrans, selStateRepo, tapCommand, eventCommandsRepo, item, leavesGhost){
		}
		public int AcquisitionOrder(){
			return -1;
		}
		public void SetAcquisitionOrder(int order){
		}
	}
}
