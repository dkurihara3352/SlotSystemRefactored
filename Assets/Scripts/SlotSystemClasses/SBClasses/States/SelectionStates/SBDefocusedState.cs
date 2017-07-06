using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBDefocusedState: SBSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBSelProcess process = null;
			if(sb.curSelState == Slottable.sbDeactivatedState){
				sb.InstantGreyout();
				process = null;
			}else if(sb.prevSelState == Slottable.sbFocusedState){
				process = new SBGreyoutProcess(sb, sb.greyoutCoroutine);
			}else if(sb.prevSelState == Slottable.sbSelectedState){
				process = new SBGreyoutProcess(sb, sb.greyoutCoroutine);
			}
			sb.SetAndRunSelProcess(process);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
