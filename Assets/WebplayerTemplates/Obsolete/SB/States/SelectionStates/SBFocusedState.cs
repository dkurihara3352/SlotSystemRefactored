using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBFocusedState: SBSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBSelProcess process = null;
			if(sb.prevSelState == Slottable.sbDeactivatedState){
				sb.InstantGreyin();
			}else if(sb.prevSelState == Slottable.sbDefocusedState){
				process = new SBGreyinProcess(sb, sb.greyinCoroutine);
			}else if(sb.prevSelState == Slottable.sbSelectedState){
				process = new SBDehighlightProcess(sb, sb.dehighlightCoroutine);
			}
			sb.SetAndRunSelProcess(process);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
