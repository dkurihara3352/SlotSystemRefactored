using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SSMState: SSEState{
		protected SlotSystemManager ssm{
			get{
				return (SlotSystemManager)base.sse;
			}
		}
	}
		public class SSMSelState: SSMState{}
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
		public class SSMActState: SSMState{}
			public class SSMWaitForActionState: SSMActState{
				public override void EnterState(StateHandler sh){
					base.EnterState(sh);
					ssm.SetAndRunActProcess(null);
				}
				public override void ExitState(StateHandler sh){
					base.ExitState(sh);
				}
			}
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
