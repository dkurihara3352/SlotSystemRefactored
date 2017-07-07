using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSMDeactivatedState: SSMSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			ssm.SetAndRunSelProcess(null);
			ssm.SetActState(SlotSystemManager.ssmWaitForActionState);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
