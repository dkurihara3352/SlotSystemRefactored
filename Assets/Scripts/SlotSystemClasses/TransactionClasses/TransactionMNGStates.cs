using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class TransactionMNGState: SSEState{
		protected ITransactionManager tam;
		public TransactionMNGState(ITransactionManager tam): base(tam){
			this.tam = tam;
		}
	}
	public interface ITransactionMNGState: ISSEState{
	}
	public class TAMActState: TransactionMNGState, ITAMActState{
		public TAMActState(ITransactionManager tam): base(tam){
		}
	}
	public interface ITAMActState: ITransactionMNGState{
	}
	public class TAMStatesFactory: ITAMStatesFactory{
		ITransactionManager tam;
		public TAMStatesFactory(ITransactionManager tam){
			this.tam = tam;
		}
		public ITAMActState MakeWaitForActionState(){
			if(_WaitForActionState == null)
				_WaitForActionState = new TAMWaitForActionState(tam);
			return _WaitForActionState;
		}
			ITAMActState _WaitForActionState;
		public ITAMActState MakeProbingState(){
			if(_ProbingState == null)
				_ProbingState = new TAMProbingState(tam);
			return _ProbingState;
		}
			ITAMActState _ProbingState;
		public ITAMActState MakeTransactionState(){
			if(_TransactionState == null)
				_TransactionState = new TAMTransactionState(tam);
			return _TransactionState;
		}
			ITAMActState _TransactionState;
	}
	public interface ITAMStatesFactory{
		ITAMActState MakeWaitForActionState();
		ITAMActState MakeProbingState();
		ITAMActState MakeTransactionState();
	}
	public class TAMWaitForActionState: TAMActState{
		public TAMWaitForActionState(ITransactionManager tam): base(tam){
		}
		public override void EnterState(){
			tam.SetAndRunActProcess(null);
		}
	}
	public class TAMProbingState: TAMActState{
		public TAMProbingState(ITransactionManager tam): base(tam){
		}
		public override void EnterState(){
			if(tam.wasWaitingForAction)
				tam.SetAndRunActProcess(new TAMProbeProcess(tam, tam.probeCoroutine));
			else
				throw new System.InvalidOperationException("TAMProbingState: Entering from an invalid state");
		}
	}
	public class TAMTransactionState: TAMActState{
		public TAMTransactionState(ITransactionManager tam): base(tam){
		}
		public override void EnterState(){
			tam.SetAndRunActProcess(new TAMTransactionProcess(tam));
		}
	}
}
