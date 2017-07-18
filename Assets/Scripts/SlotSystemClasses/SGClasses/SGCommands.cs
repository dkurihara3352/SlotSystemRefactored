using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SlotGroupCommand{
		void Execute(ISlotGroup Sg);
	}
	public class SGEmptyCommand: ISGEmptyCommand{
		public void Execute(ISlotGroup sg){
		}
	}
	public interface ISGEmptyCommand: SlotGroupCommand{}
	public class SGInitItemsCommand: ISGInitItemsCommand{
		public void Execute(ISlotGroup sg){
			List<SlottableItem> items = new List<SlottableItem>(sg.inventory);
			// sg.RunFilter(ref items);
			items = sg.FilterItem(items);
			sg.InitSlots(items);
			sg.InitSBs(items);
			sg.SyncSBsToSlots();
			if(sg.isAutoSort)
				sg.InstantSort();
		}
	}
		public interface ISGInitItemsCommand: SlotGroupCommand{}
	public class SGUpdateEquipAtExecutionCommand: ISGUpdateEquipAtExeecutionCommand{
		public void Execute(ISlotGroup sg){
			/*	update inventory
				update item's equip status
				update sb's equip status and state
			*/
			foreach(ISlottable sb in sg){
				if(sb != null){
					InventoryItemInstance item = sb.itemInst;
					if(sb.newSlotID == -1){/* removed	*/
						sg.SyncEquipped(item, false);
						/*	Set unequipped with transition
								all sbp in FocusedSGP
							Set unequipped without transition
								sll sbp in Defocused SGPs
						*/
					}else if(sb.slotID == -1){/*	added	*/
						sg.SyncEquipped(item, true);
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
		public interface ISGUpdateEquipAtExeecutionCommand: SlotGroupCommand{}
	public class SGUpdateEquipStatusCommand: ISGUpdateEquipStatusCommand{
		public void Execute(ISlotGroup sg){
			sg.UpdateEquipStatesOnAll();
		}
	}
		public interface ISGUpdateEquipStatusCommand: SlotGroupCommand{}
}
