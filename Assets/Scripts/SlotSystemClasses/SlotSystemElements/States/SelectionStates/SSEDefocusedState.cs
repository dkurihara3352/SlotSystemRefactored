using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSEDefocusedState: SSESelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SSEProcess process = null;
			if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
				process = null;
				sse.InstantGreyout();
			}else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
				process = new SSEGreyoutProcess(sse, sse.greyoutCoroutine);
			else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
				process = new SSEDehighlightProcess(sse, sse.dehighlightCoroutine);
			sse.SetAndRunSelProcess(process);
		}
	}
}