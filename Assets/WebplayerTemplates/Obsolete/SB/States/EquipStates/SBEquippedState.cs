using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBEquippedState: SBEqpState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			if(sb.sg.isPool){
				if(sb.prevEqpState != null && sb.prevEqpState == Slottable.unequippedState){
					SBEqpProcess process = new SBEquipProcess(sb, sb.EquipCoroutine);
					sb.SetAndRunEquipProcess(process);
				}
			}
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}