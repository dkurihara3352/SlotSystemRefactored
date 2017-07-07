using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGSwapState: SGActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			Slottable added;
				if(sg.ssm.transaction.sg1 == sg)
					added = sg.ssm.transaction.targetSB;
				else
					added = sg.ssm.pickedSB;
			Slottable removed;
				if(sg.ssm.transaction.sg1 == sg)
					removed = sg.ssm.pickedSB;
				else
					removed = sg.ssm.transaction.targetSB;
			List<Slottable> newSBs = new List<Slottable>(sg.toList);
			int origCount = newSBs.Count;
			if(!sg.isPool){
				GameObject newSBGO = new GameObject("newSBGO");
				Slottable newSB = newSBGO.AddComponent<Slottable>();
				newSB.Initialize(added.itemInst);
				newSB.SetSSM(sg.ssm);
				newSB.SetEqpState(Slottable.unequippedState);
				newSB.Defocus();
				newSBs[newSBs.IndexOf(removed)] = newSB;
			}
			if(sg.isAutoSort){
				sg.Sorter.TrimAndOrderSBs(ref newSBs);
				if(!sg.isExpandable){
					while(newSBs.Count <origCount){
						newSBs.Add(null);
					}
				}
			}
			sg.UpdateSBs(newSBs);
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
