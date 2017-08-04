using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public abstract class TransactionMNGProcess: SSEProcess, ITAMProcess {
		public TransactionMNGProcess(ISSEStateHandler handler, Func<IEnumeratorFake> coroutine): base(handler, coroutine){
		}
		public ITransactionManager tam{
			get{return (ITransactionManager)handler;}
		}
	}
	public interface ITAMProcess: ISSEProcess{
		ITransactionManager tam{get;}
	}
	public interface ITAMActProcess: ITAMProcess{}
		public class TAMProbeProcess: TransactionMNGProcess, ITAMActProcess{
			public TAMProbeProcess(ITransactionManager tam, Func<IEnumeratorFake> coroutine): base(tam, coroutine){
			}
		}
		public class TAMTransactionProcess: TransactionMNGProcess, ITAMActProcess{
			public TAMTransactionProcess(ITransactionManager tam, Func<IEnumeratorFake> coroutine):base(tam, coroutine){
			}
			public override void Expire(){
				base.Expire();
				tam.OnCompleteTransaction();
			}
		}
}
