using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBUnmarkedState: SBMrkState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			if(sb.prevMrkState == null || sb.prevMrkState == Slottable.unmarkedState){
				/*	when initialized	*/
				return;
			}
			if(sb.sg.isPool){
				if(sb.prevMrkState != null && sb.prevMrkState == Slottable.markedState){
					SBMrkProcess process = new SBUnmarkProcess(sb, sb.unmarkCoroutine);
					sb.SetAndRunMarkProcess(process);
				}
			}
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
