using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBMarkedState: SBMrkState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			if(sb.sg.isPool){
				if(sb.prevMrkState != null && sb.prevMrkState == Slottable.unmarkedState){
					SBMrkProcess process = new SBMarkProcess(sb, sb.markCoroutine);
					sb.SetAndRunMarkProcess(process);
				}
			}
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}