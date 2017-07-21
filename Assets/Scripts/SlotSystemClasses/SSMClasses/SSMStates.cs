using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SSMState: SSEState{
		protected ISlotSystemManager ssm{
			get{
				return (ISlotSystemManager)base.sse;
			}
		}
	}
		public class SSMActState: SSMState{}
			public class SSMWaitForActionState: SSMActState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					ssm.SetAndRunActProcess(null);
				}
				public override void ExitState(IStateHandler sh){
					base.ExitState(sh);
				}
			}
			public class SSMProbingState: SSMActState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(ssm.prevActState == SlotSystemManager.ssmWaitForActionState)
						ssm.SetAndRunActProcess(new SSMProbeProcess(ssm, ssm.probeCoroutine));
					else
						throw new System.InvalidOperationException("SGMProbingState: Entering from an invalid state");
				}
				public override void ExitState(IStateHandler sh){
					base.ExitState(sh);
				}
			}
			public class SSMTransactionState: SSMActState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					ssm.SetAndRunActProcess(new SSMTransactionProcess(ssm));
				}
				public override void ExitState(IStateHandler sh){
					base.ExitState(sh);
				}
			}
}
