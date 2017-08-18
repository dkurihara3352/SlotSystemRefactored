using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class TransactionMNGState: SSEState{
		protected ITransactionManager tam;
		public TransactionMNGState(ITransactionManager tam){
			this.tam = tam;
		}
	}
	public interface ITransactionMNGState: ISSEState{
	}
	public class TAMActState: TransactionMNGState, ITAMActState{
		protected ITAMActStateHandler handler;
		public TAMActState(ITransactionManager tam, ITAMActStateHandler handler): base(tam){
			this.handler = handler;
		}
	}
	public interface ITAMActState: ITransactionMNGState{
	}
	public class TAMStatesFactory: ITAMStatesFactory{
		ITransactionManager tam;
		ITAMActStateHandler handler;
		public TAMStatesFactory(ITransactionManager tam, ITAMActStateHandler handler){
			this.tam = tam;
			this.handler = handler;
		}
		public ITAMActState MakeWaitForActionState(){
			if(_WaitForActionState == null)
				_WaitForActionState = new TAMWaitForActionState(tam, handler);
			return _WaitForActionState;
		}
			ITAMActState _WaitForActionState;
		public ITAMActState MakeProbingState(){
			if(_ProbingState == null)
				_ProbingState = new TAMProbingState(tam, handler);
			return _ProbingState;
		}
			ITAMActState _ProbingState;
		public ITAMActState MakeTransactionState(){
			if(_TransactionState == null)
				_TransactionState = new TAMTransactionState(tam, handler);
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
		public TAMWaitForActionState(ITransactionManager tam, ITAMActStateHandler handler): base(tam, handler){
		}
		public override void EnterState(){
			handler.SetAndRunActProcess(null);
		}
	}
	public class TAMProbingState: TAMActState{
		public TAMProbingState(ITransactionManager tam, ITAMActStateHandler handler): base(tam, handler){
		}
		public override void EnterState(){
			if(handler.WasWaitingForAction())
				handler.SetAndRunActProcess(new TAMProbeProcess(tam, handler.probeCoroutine));
			else
				throw new System.InvalidOperationException("TAMProbingState: Entering from an invalid state");
		}
	}
	public class TAMTransactionState: TAMActState{
		public TAMTransactionState(ITransactionManager tam, ITAMActStateHandler handler): base(tam, handler){
		}
		public override void EnterState(){
			handler.SetAndRunActProcess(new TAMTransactionProcess(tam, handler.transactionCoroutine));
		}
	}
}
