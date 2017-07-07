using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGInitItemsCommand: SlotGroupCommand{
		public void Execute(SlotGroup sg){
			List<SlottableItem> items = new List<SlottableItem>(sg.inventory);
			sg.Filter.Filter(ref items);
			/*	Slots	*/
				List<Slot> newSlots = new List<Slot>();
				int slotCountToCreate = sg.initSlotsCount == 0? items.Count: sg.initSlotsCount;
				for(int i = 0; i <slotCountToCreate; i++){
					Slot newSlot = new Slot();
					newSlots.Add(newSlot);
				}
				sg.SetSlots(newSlots);
			/*	SBs	*/
				/*	if the number of filtered items exceeds the slot count, remove unfittable items from the inventory	*/
				while(sg.slots.Count < items.Count){
					items.RemoveAt(sg.slots.Count);
				}
				foreach(SlottableItem item in items){
					GameObject newSBGO = new GameObject("newSBGO");
					Slottable newSB = newSBGO.AddComponent<Slottable>();
					newSB.Initialize((InventoryItemInstance)item);
					newSB.SetSSM(sg.ssm);
					sg.slots[items.IndexOf(item)].sb = newSB;
				}
				sg.SyncSBsToSlots();
			if(sg.isAutoSort)
				sg.InstantSort();
		}
	}
}
