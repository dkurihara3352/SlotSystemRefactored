using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSMProbingState: SSMActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			if(ssm.prevActState == SlotSystemManager.ssmWaitForActionState)
				ssm.SetAndRunActProcess(new SSMProbeProcess(ssm, ssm.probeCoroutine));
			else
				throw new System.InvalidOperationException("SGMProbingState: Entering from an invalid state");
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
