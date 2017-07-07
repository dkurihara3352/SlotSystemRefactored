using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGSelectedState: SGSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SGSelProcess process = null;
			if(sg.prevSelState == SlotGroup.sgDeactivatedState){
				sg.InstantHighlight();
			}else if(sg.prevSelState == SlotGroup.sgDefocusedState)
				process = new SGHighlightProcess(sg, sg.highlightCoroutine);
			else if(sg.prevSelState == SlotGroup.sgFocusedState)
				process = new SGHighlightProcess(sg, sg.highlightCoroutine);
			sg.SetAndRunSelProcess(process);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
