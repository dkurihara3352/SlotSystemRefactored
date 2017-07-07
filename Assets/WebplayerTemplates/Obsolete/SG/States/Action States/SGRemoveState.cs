using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGRemoveState: SGActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			List<InventoryItemInstance> cache = sg.ssm.transaction.moved;
			List<Slottable> newSBs = sg.toList;
			int origCount = newSBs.Count;
			List<Slottable> removedList = new List<Slottable>();
			List<Slottable> nonremoved = new List<Slottable>();
			foreach(InventoryItemInstance itemInst in cache){
				foreach(Slottable sb in newSBs){
					if(sb!= null){
						if(sb.itemInst == itemInst){
							if(itemInst.Item.IsStackable){
								sb.itemInst.Quantity -= itemInst.Quantity;
								if(sb.itemInst.Quantity <= 0)
									removedList.Add(sb);
							}else{
								removedList.Add(sb);
							}
						}
					}
				}
			}
			foreach(Slottable sb in removedList){
				newSBs[newSBs.IndexOf(sb)] = null;
			}
			if(sg.isAutoSort){
				sg.Sorter.TrimAndOrderSBs(ref newSBs);
				if(!sg.isExpandable){
					while(newSBs.Count <origCount){
						newSBs.Add(null);
					}
				}
			}else{
				if(sg.isExpandable)
					SlotSystemUtil.Trim(ref newSBs);
			}
			sg.SetNewSBs(nonremoved);
			sg.CreateNewSlots();
			sg.SetSBsActStates();
			if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
				SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
				sg.SetAndRunActProcess(process);
			}
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
