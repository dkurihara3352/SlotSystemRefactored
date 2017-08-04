using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionManager : SlotSystemElement, ITransactionManager{
		public void ExecuteTransaction(){
			Transact();
			transaction.Execute();
		}
		public void OnCompleteTransaction(){
			transaction.OnCompleteTransaction();
		}
		public void SetTransaction(ISlotSystemTransaction transaction){
			if(m_transaction != transaction){
				m_transaction = transaction;
				if(m_transaction != null){
					m_transaction.Indicate();
				}
			}
		}
			public ISlotSystemTransaction transaction{
				get{return m_transaction;}
			}
			ISlotSystemTransaction m_transaction;
		/* Transaction Cache */
			public void ClearFields(){
				SetSG1(null);
				SetSG2(null);
				SetDIcon1(null);
				SetDIcon2(null);
				SetTransaction(null);
			}
			public void UpdateFields(ISlotSystemTransaction ta){
				SetSG1(ta.sg1);
				SetSG2(ta.sg2);
				SetTransaction(ta);
			}
		/* Sort Engine */
			public void SortSG(ISlotGroup sg, SGSorter sorter){
				sortEngine.SortSG(sg, sorter);
			}
			ISortEngine sortEngine{
				get{
					if(m_sortEngine == null)
						m_sortEngine = new SortEngine(this);
					return m_sortEngine;
				}
			}
				ISortEngine m_sortEngine;
				public void SetSortEngine(ISortEngine sortEngine){
					m_sortEngine = sortEngine;
				}
		/* SGHandling */
			public void AcceptSGTAComp(ISlotGroup sg){
				sgHandler.AcceptSGTAComp(sg);
			}
			public ISlotGroup sg1{
				get{
					return sgHandler.sg1;
				}
			}
				public void SetSG1(ISlotGroup sg){
					sgHandler.SetSG1(sg);
				}
			public ISlotGroup sg2{
				get{
					return sgHandler.sg2;
				}
			}
				public void SetSG2(ISlotGroup sg){
					sgHandler.SetSG2(sg);
				}
			public void SetSGHandler(ITransactionSGHandler sgHandler){
				m_sgHandler = sgHandler;
			}
				ITransactionSGHandler sgHandler{
					get{
						if(m_sgHandler == null)
							m_sgHandler = new TransactionSGHandler(this);
						return m_sgHandler;
					}
				}
				ITransactionSGHandler m_sgHandler;
		/* IconHandling */
			public void AcceptDITAComp(DraggedIcon di){
				iconHandler.AcceptDITAComp(di);
			}
			public virtual DraggedIcon dIcon1{
				get{return iconHandler.dIcon1;}
			}
				public virtual void SetDIcon1(DraggedIcon di){
					iconHandler.SetDIcon1(di);
				}
			public virtual DraggedIcon dIcon2{
				get{return iconHandler.dIcon2;}
			}
				public virtual void SetDIcon2(DraggedIcon di){
					iconHandler.SetDIcon2(di);
				}
			public void SetIconHandler(ITransactionIconHandler iconHandler){
				m_iconHandler = iconHandler;
			}
				ITransactionIconHandler iconHandler{
					get{
						if(m_iconHandler == null)
							m_iconHandler = new TransactionIconHandler(this);
						return m_iconHandler;
					}
				}
				ITransactionIconHandler m_iconHandler;
		/* Other */
			public override void InitializeStates(){
				WaitForAction();
			}
			public void Refresh(){
				WaitForAction();
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
							_statesFactory = new TAMStatesFactory(this);
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
				get{return actProcEngine.process;}
			}
			public IEnumeratorFake probeCoroutine(){
				return null;
			}
			public IEnumeratorFake transactionCoroutine(){
				bool done = true;
				done &= iconHandler.dIcon1Done;
				done &= iconHandler.dIcon2Done;
				done &= sgHandler.sg1Done;
				done &= sgHandler.sg2Done;
				if(done){
					this.actProcess.Expire();
				}
				return null;
			}			
	}
	public interface ITransactionManager: ISlotSystemElement{
		void ExecuteTransaction();
		void OnCompleteTransaction();
		void SetTransaction(ISlotSystemTransaction transaction);
		/* TransactionCache */
			void UpdateFields(ISlotSystemTransaction ta);
		/* SortEngine */
			void SortSG(ISlotGroup sg, SGSorter sorter);
		/* TASGHandler */
			void AcceptSGTAComp(ISlotGroup sg);
			ISlotGroup sg1{get;}
			void SetSG1(ISlotGroup sg);
			ISlotGroup sg2{get;}
			void SetSG2(ISlotGroup sg);
		/* TAIconHandler */
			void AcceptDITAComp(DraggedIcon di);
			DraggedIcon dIcon1{get;}
			void SetDIcon1(DraggedIcon di);
			DraggedIcon dIcon2{get;}
			void SetDIcon2(DraggedIcon di);
		/* Other */
			void Refresh();
		/* ActState */
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
			
		/* Process */
			void SetAndRunActProcess(ITAMActProcess process);
			void ExpireActProcess();
			ISSEProcessEngine<ITAMActProcess> actProcEngine{get;}
			void SetActProcEngine(ISSEProcessEngine<ITAMActProcess> engine);
			ITAMActProcess actProcess{get;}
			IEnumeratorFake transactionCoroutine();
			IEnumeratorFake probeCoroutine();
			
	}
}
