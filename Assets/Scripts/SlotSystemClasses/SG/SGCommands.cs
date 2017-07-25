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
			foreach(ISlottable sb in sg){
				if(sb != null){
					InventoryItemInstance item = sb.itemInst;
					if(sb.isToBeRemoved){
						sg.SyncEquipped(item, false);
					}else if(sb.isToBeAdded){
						sg.SyncEquipped(item, true);
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
