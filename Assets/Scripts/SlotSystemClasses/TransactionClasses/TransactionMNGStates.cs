using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class TransactionMNGState: SSEState{
		protected ITransactionManager tam{
			get{
				return (ITransactionManager)base.sse;
			}
		}
	}
	public interface ITransactionMNGState: ISSEState{}
		public class TAMActState: TransactionMNGState, ITAMActState{}
		public interface ITAMActState: ITransactionMNGState{}
			public class TAMWaitForActionState: TAMActState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					tam.SetAndRunActProcess(null);
				}
				public override void ExitState(IStateHandler sh){
					base.ExitState(sh);
				}
			}
			public class TAMProbingState: TAMActState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(tam.wasWaitingForAction)
						tam.SetAndRunActProcess(new TAMProbeProcess(tam, tam.probeCoroutine));
					else
						throw new System.InvalidOperationException("TAMProbingState: Entering from an invalid state");
				}
				public override void ExitState(IStateHandler sh){
					base.ExitState(sh);
				}
			}
			public class TAMTransactionState: TAMActState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					tam.SetAndRunActProcess(new TAMTransactionProcess(tam));
				}
				public override void ExitState(IStateHandler sh){
					base.ExitState(sh);
				}
			}
}
