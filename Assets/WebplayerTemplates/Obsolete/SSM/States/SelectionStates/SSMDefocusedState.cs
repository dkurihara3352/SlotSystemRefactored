using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSMDefocusedState: SSMSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			if(ssm.prevSelState == SlotSystemManager.ssmFocusedState)
				ssm.SetAndRunSelProcess(new SSMGreyoutProcess(ssm, ssm.greyoutCoroutine));
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
