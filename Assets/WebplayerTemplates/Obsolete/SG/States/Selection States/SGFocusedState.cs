using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGFocusedState: SGSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SGSelProcess process = null;
			if(sg.prevSelState == SlotGroup.sgDeactivatedState){
				process = null;
				sg.InstantGreyin();
			}
			else if(sg.prevSelState == SlotGroup.sgDefocusedState)
				process = new SGGreyinProcess(sg, sg.greyinCoroutine);
			else if(sg.prevSelState == SlotGroup.sgSelectedState)
				process = new SGDehighlightProcess(sg, sg.dehighlightCoroutine);
			sg.SetAndRunSelProcess(process);
		}	
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
