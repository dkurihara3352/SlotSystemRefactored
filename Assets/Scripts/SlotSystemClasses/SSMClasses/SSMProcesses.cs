using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SSMProcess: AbsSSEProcess, ISSMProcess{
		public ISlotSystemManager ssm{
			get{return (ISlotSystemManager)sse;}
			set{}
		}
	}
	public interface ISSMProcess: ISSEProcess{
		ISlotSystemManager ssm{get; set;}
	}
		public interface ISSMSelProcess: ISSMProcess{}
			public class SSMGreyinProcess: SSMProcess, ISSMSelProcess{
				public SSMGreyinProcess(ISlotSystemManager ssm, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = ssm;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSMGreyoutProcess: SSMProcess, ISSMSelProcess{
				public SSMGreyoutProcess(ISlotSystemManager ssm, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = ssm;
					this.coroutineFake = coroutineMock;
				}
			}
		public interface ISSMActProcess: ISSMProcess{}
			public class SSMProbeProcess: SSMProcess, ISSMActProcess{
				public SSMProbeProcess(ISlotSystemManager ssm, System.Func<IEnumeratorFake> coroutineMock){
					this.sse = ssm;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SSMTransactionProcess: SSMProcess, ISSMActProcess{
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
