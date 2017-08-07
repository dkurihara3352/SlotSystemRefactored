using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public abstract class TransactionMNGProcess: SSEProcess, ITAMProcess {
		protected ITransactionManager tam{
			get{
				if(_tam != null)
					return _tam;
				else
					throw new InvalidOperationException("tam not set");
			}
		}
			ITransactionManager _tam;
		public TransactionMNGProcess(ITransactionManager tam, Func<IEnumeratorFake> coroutine): base(coroutine){
			_tam = tam;
		}
	}
	public interface ITAMProcess: ISSEProcess{
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
