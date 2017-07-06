using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBUnequippedState: SBEqpState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			if(sb.prevEqpState == null || sb.prevEqpState == Slottable.unequippedState){
				/*	when initialized	*/
				return;
			}
			if(sb.sg.isPool){
				if(sb.prevEqpState != null && sb.prevEqpState == Slottable.equippedState){
					SBEqpProcess process = new SBUnequipProcess(sb, sb.UnequipCoroutine);
					sb.SetAndRunEquipProcess(process);
				}
			}
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}