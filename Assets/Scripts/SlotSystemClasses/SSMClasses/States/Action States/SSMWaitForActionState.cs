using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSMWaitForActionState: SSMActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			ssm.SetAndRunActProcess(null);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
