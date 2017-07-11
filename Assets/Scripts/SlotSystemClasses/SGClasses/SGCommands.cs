using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SlotGroupCommand{
		void Execute(ISlotGroup Sg);
	}
	public class SGEmptyCommand: SlotGroupCommand{
		public void Execute(ISlotGroup sg){
		}
	}
	public class SGInitItemsCommand: SlotGroupCommand{
		public void Execute(ISlotGroup sg){
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
					ISlottable newSB = newSBGO.AddComponent<Slottable>();
					newSB.Initialize((InventoryItemInstance)item);
					newSB.SetSSM(sg.ssm);
					sg.slots[items.IndexOf(item)].sb = newSB;
				}
				sg.SyncSBsToSlots();
			if(sg.isAutoSort)
				sg.InstantSort();
		}
	}
	public class SGUpdateEquipAtExecutionCommand: SlotGroupCommand{
		public void Execute(ISlotGroup sg){
			/*	update inventory
				update item's equip status
				update sb's equip status and state
			*/
			foreach(ISlottable sb in sg){
				if(sb != null){
					InventoryItemInstance item = sb.itemInst;
					if(sb.newSlotID == -1){/* removed	*/
						sg.inventory.Remove(item);
						sg.ssm.MarkEquippedInPool(item, false);
						sg.ssm.SetEquippedOnAllSBs(item, false);
						/*	Set unequipped with transition
								all sbp in FocusedSGP
							Set unequipped without transition
								sll sbp in Defocused SGPs
						*/
					}else if(sb.slotID == -1){/*	added	*/
						sg.inventory.Add(item);
						sg.ssm.MarkEquippedInPool(item, true);
						sg.ssm.SetEquippedOnAllSBs(item, true);
						/*	Set equipped with transition
								all the sbp in Focused SGP
								all sbe in FocusedSGEs (NOT those defocused)
							Set equipped without transition
								all sbp in defocused SGPs
						*/
					}else{/*	merely moved	*/
						/*	do nothing	*/
					}
				}
			}
		}
	}
	public class SGUpdateEquipStatusCommand: SlotGroupCommand{
		public void Execute(ISlotGroup sg){
			sg.ssm.UpdateEquipStatesOnAll();
		}
	}
}
