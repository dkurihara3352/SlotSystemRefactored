using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class TransactionMNGProcess: SSEProcess, ITAMProcess {
		public ITransactionManager tam{
			get{return (ITransactionManager)sse;}
			set{}
		}
	}
	public interface ITAMProcess: ISSEProcess{
		ITransactionManager tam{get; set;}
	}
	public interface ITAMActProcess: ITAMProcess{}
		public class TAMProbeProcess: TransactionMNGProcess, ITAMActProcess{
			public TAMProbeProcess(ITransactionManager tam, System.Func<IEnumeratorFake> coroutineMock){
				this.sse = tam;
				this.coroutineFake = coroutineMock;
			}
		}
		public class TAMTransactionProcess: TransactionMNGProcess, ITAMActProcess{
			public TAMTransactionProcess(ITransactionManager tam){
				this.sse = tam;
				this.coroutineFake = tam.transactionCoroutine;
			}
			public override void Expire(){
				base.Expire();
				tam.OnCompleteTransaction();
			}
		}
}
