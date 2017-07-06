using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSMTransactionState: SSMActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			ssm.SetAndRunActProcess(new SSMTransactionProcess(ssm));
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
