using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGReorderState: SGActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			Slottable sb1 = sg.ssm.pickedSB;
			Slottable sb2 = sg.ssm.targetSB;
			List<Slottable> newSBs = new List<Slottable>(sg.toList);
			newSBs.Reorder(sb1, sb2);
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
