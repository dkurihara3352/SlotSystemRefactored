using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
public class SGSortState: SGActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			List<Slottable> newSBs = new List<Slottable>(sg.toList);
			int origCount = newSBs.Count;
			sg.Sorter.TrimAndOrderSBs(ref newSBs);
			if(!sg.isExpandable){
				while(newSBs.Count <origCount){
					newSBs.Add(null);
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
