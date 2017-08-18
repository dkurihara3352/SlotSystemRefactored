using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TAMActStateHandler : ITAMActStateHandler {
		ITransactionManager GetTAM(){
			return _tam;
		}
			ITransactionManager _tam;
		public TAMActStateHandler(ITransactionManager tam){
			_tam = tam;
		}
		/*	state */
			ISSEStateEngine<ITAMActState> actStateEngine{
				get{
					if(_actStateEngine == null)
						_actStateEngine = new SSEStateEngine<ITAMActState>();
					return _actStateEngine;
				}
			}
				ISSEStateEngine<ITAMActState> _actStateEngine;
				void SetActStateEngine(ISSEStateEngine<ITAMActState> engine){
					_actStateEngine = engine;
				}
			void SetActState(ITAMActState state){
				actStateEngine.SetState(state);
				if(state ==null && GetActProcess() != null)
					SetAndRunActProcess(null);
			}
				ITAMActState curActState{
					get{return actStateEngine.GetCurState();}
				}
				ITAMActState prevActState{
					get{return actStateEngine.GetPrevState();}
				}
			/* states */
				ITAMStatesFactory statesFactory{
					get{
						if(_statesFactory == null)
							_statesFactory = new TAMStatesFactory(GetTAM(), this);
						return _statesFactory;
					}
				}
					ITAMStatesFactory _statesFactory;
				public virtual void ClearCurActState(){
					SetActState(null);
				}
					public virtual bool IsActStateNull(){
						return curActState == null;
					}
					public virtual bool WasActStateNull(){
						return prevActState == null;
					}
				public virtual void WaitForAction(){
					SetActState(waitForActionState);
				}
					ITAMActState waitForActionState{
						get{return statesFactory.MakeWaitForActionState();}
					}
					public virtual bool IsWaitingForAction(){
						return curActState == waitForActionState;
					}
					public virtual bool WasWaitingForAction(){
						return prevActState == waitForActionState;
					}
				public virtual void Probe(){
					SetActState(probingState);
				}
					ITAMActState probingState{
						get{return statesFactory.MakeProbingState();}
					}
					public virtual bool IsProbing(){
						return curActState == probingState;
					}
					public virtual bool WasProbing(){
						return prevActState == probingState;
					}
				public virtual void Transact(){
					SetActState(transactionState);
				}
					ITAMActState transactionState{
						get{return statesFactory.MakeTransactionState();}
					}
					public virtual bool IsTransacting(){
						return curActState == transactionState;
					}
					public virtual bool WasTransacting(){
						return prevActState == transactionState;
					}
		/*	process	*/
			public virtual void SetAndRunActProcess(ITAMActProcess process){
				actProcEngine.SetAndRunProcess(process);
			}
			public void ExpireActProcess(){
				ITAMActProcess actProcess = GetActProcess();
				if(actProcess != null)
					actProcess.Expire();
			}
			public virtual ISSEProcessEngine<ITAMActProcess> actProcEngine{
				get{
					if(_actProcEngine == null)
						_actProcEngine = new SSEProcessEngine<ITAMActProcess>();
					return _actProcEngine;
				}
			}
				ISSEProcessEngine<ITAMActProcess> _actProcEngine;
			public virtual void SetActProcEngine(ISSEProcessEngine<ITAMActProcess> engine){
				_actProcEngine = engine;
			}
			public virtual ITAMActProcess GetActProcess(){
				return actProcEngine.GetProcess();
			}
			public IEnumeratorFake probeCoroutine(){
				return null;
			}
			public IEnumeratorFake transactionCoroutine(){
				return GetTAM().TransactionCoroutine();
			}
	}
	public interface ITAMActStateHandler{
			void ClearCurActState();
				bool IsActStateNull();
				bool WasActStateNull();

			void WaitForAction();
				bool IsWaitingForAction();
				bool WasWaitingForAction();

			void Probe();
				bool IsProbing();
				bool WasProbing();

			void Transact();
				bool IsTransacting();
				bool WasTransacting();
			
			void SetAndRunActProcess(ITAMActProcess process);
			void ExpireActProcess();
			ISSEProcessEngine<ITAMActProcess> actProcEngine{get;}
			void SetActProcEngine(ISSEProcessEngine<ITAMActProcess> engine);
			ITAMActProcess GetActProcess();
			IEnumeratorFake transactionCoroutine();
			IEnumeratorFake probeCoroutine();
	}
}
