using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGUpdateEquipAtExecutionCommand: SlotGroupCommand{
		public void Execute(SlotGroup sg){
			/*	update inventory
				update item's equip status
				update sb's equip status and state
			*/
			foreach(Slottable sb in sg){
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
}
