using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSEFocusedState: SSESelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SSEProcess process = null;
			if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
				process = null;
				sse.InstantGreyin();
			}
			else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
				process = new SSEGreyinProcess(sse, sse.greyinCoroutine);
			else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
				process = new SSEDehighlightProcess(sse, sse.dehighlightCoroutine);
			sse.SetAndRunSelProcess(process);
		}
	}
}