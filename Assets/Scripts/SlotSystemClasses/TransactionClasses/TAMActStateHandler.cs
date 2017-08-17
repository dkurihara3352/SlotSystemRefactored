using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TAMActStateHandler : ITAMActStateHandler {
		ITransactionManager tam;
		public TAMActStateHandler(ITransactionManager tam){
			this.tam = tam;
		}
		/*	state */
			ISSEStateEngine<ITAMActState> actStateEngine{
				get{
					if(m_actStateEngine == null)
						m_actStateEngine = new SSEStateEngine<ITAMActState>();
					return m_actStateEngine;
				}
			}
				ISSEStateEngine<ITAMActState> m_actStateEngine;
				void SetActStateEngine(ISSEStateEngine<ITAMActState> engine){
					m_actStateEngine = engine;
				}
			void SetActState(ITAMActState state){
				actStateEngine.SetState(state);
				if(state ==null && actProcess != null)
					SetAndRunActProcess(null);
			}
				ITAMActState curActState{
					get{return actStateEngine.curState;}
				}
				ITAMActState prevActState{
					get{return actStateEngine.prevState;}
				}
			/* states */
				ITAMStatesFactory statesFactory{
					get{
						if(_statesFactory == null)
							_statesFactory = new TAMStatesFactory(tam, this);
						return _statesFactory;
					}
				}
					ITAMStatesFactory _statesFactory;
				public virtual void ClearCurActState(){
					SetActState(null);
				}
					public virtual bool isActStateNull{
						get{return curActState == null;}
					}
					public virtual bool wasActStateNull{
						get{return prevActState == null;}
					}
				public virtual void WaitForAction(){
					SetActState(waitForActionState);
				}
					public ITAMActState waitForActionState{
						get{return statesFactory.MakeWaitForActionState();}
					}
					public virtual bool isWaitingForAction{
						get{return curActState == waitForActionState;}
					}
					public virtual bool wasWaitingForAction{
						get{return prevActState == waitForActionState;}
					}
				public virtual void Probe(){
					SetActState(probingState);
				}
					public ITAMActState probingState{
						get{return statesFactory.MakeProbingState();}
					}
					public virtual bool isProbing{
						get{return curActState == probingState;}
					}
					public virtual bool wasProbing{
						get{return prevActState == probingState;}
					}
				public virtual void Transact(){
					SetActState(transactionState);
				}
					public ITAMActState transactionState{
						get{return statesFactory.MakeTransactionState();}
					}
					public virtual bool isTransacting{
						get{return curActState == transactionState;}
					}
					public virtual bool wasTransacting{
						get{return prevActState == transactionState;}
					}
		/*	process	*/
			public virtual void SetAndRunActProcess(ITAMActProcess process){
				actProcEngine.SetAndRunProcess(process);
			}
			public void ExpireActProcess(){
				if(actProcess != null)
					actProcess.Expire();
			}
			public virtual ISSEProcessEngine<ITAMActProcess> actProcEngine{
				get{
					if(m_actProcEngine == null)
						m_actProcEngine = new SSEProcessEngine<ITAMActProcess>();
					return m_actProcEngine;
				}
			}
				ISSEProcessEngine<ITAMActProcess> m_actProcEngine;
			public virtual void SetActProcEngine(ISSEProcessEngine<ITAMActProcess> engine){
				m_actProcEngine = engine;
			}
			public virtual ITAMActProcess actProcess{
				get{return actProcEngine.GetProcess();}
			}
			public IEnumeratorFake probeCoroutine(){
				return null;
			}
			public IEnumeratorFake transactionCoroutine(){
				return tam.transactionCoroutine();
			}
	}
	public interface ITAMActStateHandler{
			bool wasActStateNull{get;}
			void ClearCurActState();

			void WaitForAction();
			ITAMActState waitForActionState{get;}
			bool isWaitingForAction{get;}
			bool wasWaitingForAction{get;}

			void Probe();
			ITAMActState probingState{get;}
			bool isProbing{get;}
			bool wasProbing{get;}

			void Transact();
			ITAMActState transactionState{get;}
			bool isTransacting{get;}
			bool wasTransacting{get;}
			
			void SetAndRunActProcess(ITAMActProcess process);
			void ExpireActProcess();
			ISSEProcessEngine<ITAMActProcess> actProcEngine{get;}
			void SetActProcEngine(ISSEProcessEngine<ITAMActProcess> engine);
			ITAMActProcess actProcess{get;}
			IEnumeratorFake transactionCoroutine();
			IEnumeratorFake probeCoroutine();
	}
}
