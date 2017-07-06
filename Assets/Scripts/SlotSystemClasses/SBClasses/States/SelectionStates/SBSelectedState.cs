using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBSelectedState: SBSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBSelProcess process = null;
			if(sb.prevSelState == Slottable.sbDeactivatedState){
				sb.InstantHighlight();
			}else if(sb.prevSelState == Slottable.sbDefocusedState){
				process = new SBHighlightProcess(sb, sb.highlightCoroutine);
			}else if(sb.prevSelState == Slottable.sbFocusedState){
				process = new SBHighlightProcess(sb, sb.highlightCoroutine);
			}
			sb.SetAndRunSelProcess(process);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
