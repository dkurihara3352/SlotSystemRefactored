using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGDefocusedState: SGSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SGSelProcess process = null;
			if(sg.prevSelState == SlotGroup.sgDeactivatedState){
				process = null;
				sg.InstantGreyout();
			}else if(sg.prevSelState == SlotGroup.sgFocusedState)
				process = new SGGreyoutProcess(sg, sg.greyoutCoroutine);
			else if(sg.prevSelState == SlotGroup.sgSelectedState)
				process = new SGDehighlightProcess(sg, sg.greyoutCoroutine);
			sg.SetAndRunSelProcess(process);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}