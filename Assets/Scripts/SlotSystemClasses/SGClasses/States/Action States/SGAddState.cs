using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGAddState: SGActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			List<InventoryItemInstance> cache = sg.ssm.transaction.moved;
			List<Slottable> newSBs = sg.toList;
			int origCount = newSBs.Count;
			foreach(InventoryItemInstance itemInst in cache){
				bool found = false;
				foreach(Slottable sb in newSBs){
					if(sb!= null){
						if(sb.itemInst == itemInst){
							if(itemInst.Item.IsStackable){
								sb.itemInst.Quantity += itemInst.Quantity;
								found = true;
							}
						}
					}
				}
				if(!found){
					GameObject newSBSG = new GameObject("newSBSG");
					Slottable newSB = newSBSG.AddComponent<Slottable>();
					newSB.Initialize(itemInst);
					newSB.SetSSM(sg.ssm);
					newSB.Defocus();
					SlotSystemUtil.AddInEmptyOrConcat(ref newSBs, newSB);
				}
			}
			if(sg.isAutoSort)
				sg.Sorter.TrimAndOrderSBs(ref newSBs);
			if(!sg.isExpandable){
				while(newSBs.Count <origCount){
					newSBs.Add(null);
				}
			}
			sg.SetNewSBs(newSBs);
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
