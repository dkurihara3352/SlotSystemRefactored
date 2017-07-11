using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SSMProcess: AbsSSEProcess{
		protected ISlotSystemManager ssm{
			get{return (ISlotSystemManager)sse;}
		}
	}
		public class SSMSelProcess: SSMProcess{}
			public class SSMGreyinProcess: SSMSelProcess{
				public SSMGreyinProcess(ISlotSystemManager ssm, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = ssm;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSMGreyoutProcess: SSMSelProcess{
				public SSMGreyoutProcess(ISlotSystemManager ssm, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = ssm;
					this.coroutineFake = coroutineMock;
				}
			}
		public class SSMActProcess: SSMProcess{}
			public class SSMProbeProcess: SSMActProcess{
				public SSMProbeProcess(ISlotSystemManager ssm, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = ssm;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSMTransactionProcess: SSMActProcess{
				public SSMTransactionProcess(ISlotSystemManager ssm){
					this.sse = ssm;
					this.coroutineFake = ssm.transactionCoroutine;
				}
				public override void Expire(){
					base.Expire();
					ssm.transaction.OnComplete();
				}
			}
}
