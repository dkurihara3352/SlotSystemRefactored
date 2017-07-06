using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSMFocusedState: SSMSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			if(ssm.prevSelState == SlotSystemManager.ssmDefocusedState)
				ssm.SetAndRunSelProcess(new SSMGreyinProcess(ssm, ssm.greyinCoroutine));
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}	
}
