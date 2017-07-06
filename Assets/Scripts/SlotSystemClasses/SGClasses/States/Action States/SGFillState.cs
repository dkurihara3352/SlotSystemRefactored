using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGFillState: SGActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			Slottable added;
				if(sg.ssm.transaction.sg1 == sg)
					added = null;
				else
					added = sg.ssm.pickedSB;
			Slottable removed;
				if(sg.ssm.transaction.sg1 == sg)
					removed = sg.ssm.pickedSB;
				else
					removed = null;

			List<Slottable> newSBs = new List<Slottable>(sg.toList);
			int origCount = newSBs.Count;
			if(!sg.isPool){
				if(added != null){
					GameObject newSBGO = new GameObject("newSBGO");
					Slottable newSB = newSBGO.AddComponent<Slottable>();
					newSB.Initialize(added.itemInst);
					newSB.SetSSM(sg.ssm);
					newSB.Defocus();
					newSB.SetEqpState(Slottable.unequippedState);
					Util.AddInEmptyOrConcat(ref newSBs, newSB);
				}
				if(removed != null){
					Slottable rem = null;
					foreach(Slottable sb in newSBs){
						if(sb != null){
							if(sb.itemInst == removed.itemInst)
								rem = sb;
						}
					}
					newSBs[newSBs.IndexOf(rem)] = null;
				}
			}
			if(sg.isAutoSort){
				sg.Sorter.TrimAndOrderSBs(ref newSBs);
			}
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
