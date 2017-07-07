using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSESelectedState : SSESelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SSEProcess process = null;
			if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
				process = null;
				sse.InstantHighlight();
			}
			else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
				process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
			else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
				process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
			sse.SetAndRunSelProcess(process);
		}
	}
}
