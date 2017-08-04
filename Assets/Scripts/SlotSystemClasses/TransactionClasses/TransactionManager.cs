using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionManager : SlotSystemElement, ITransactionManager{
		public List<ISlotGroup> focusedSGs{
			get{
				return ssm.focusedSGs;
			}
		}
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
			public void UpdateFields(){
				transactionCache.UpdateFields();
			}
			public void InnerUpdateFields(ISlotSystemTransaction ta){
				SetSG1(ta.sg1);
				SetSG2(ta.sg2);
				SetTransaction(ta);
			}
			ITransactionCache transactionCache{
				get{
					if(m_transactionCache == null)
						m_transactionCache = new TransactionCache(this);
					return m_transactionCache;
				}
			}
				ITransactionCache m_transactionCache;
			public bool IsTransactionGoingToBeRevert(ISlottable sb){
				return transactionCache.IsTransactionGoingToBeRevert(sb);
			}
			public bool IsTransactionResultRevertFor(IHoverable hoverable){
				return transactionCache.IsCachedTAResultRevert(hoverable);
			}
			public void CreateTransactionResults(){
				transactionCache.CreateTransactionResults();
			}
			public Dictionary<IHoverable, ISlotSystemTransaction> transactionResults{
				get{return transactionCache.transactionResults;}
			}
			public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered){
				return transactionCache.MakeTransaction(pickedSB, hovered);
			}
			public ISlottable pickedSB{
				get{return transactionCache.pickedSB;}
			}
				public void SetPickedSB(ISlottable sb){
					transactionCache.SetPickedSB(sb);
				}
			public ISlottable targetSB{
				get{return transactionCache.targetSB;}
			}
				public void SetTargetSB(ISlottable sb){
					transactionCache.SetTargetSB(sb);
				}
			public IHoverable hovered{
				get{return transactionCache.hovered;}
			}
				public void SetHovered(IHoverable to){
					transactionCache.SetHovered(to);
				}
			public List<InventoryItemInstance> moved{
				get{return transactionCache.moved;}
			}
				public void SetMoved(List<InventoryItemInstance> moved){
					transactionCache.SetMoved(moved);
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
			public void SetTAMRecursively(){
				ssm.PerformInHierarchy(SetTAMInHi);
			}
				void SetTAMInHi(ISlotSystemElement ele){
					if(ele is IHoverable)
						((IHoverable)ele).SetTAM(this);
				}
			public override void InitializeStates(){
				WaitForAction();
			}
			public void Refresh(){
				WaitForAction();
			}
			public void ClearFields(){
				transactionCache.ClearFields();
				SetSG1(null);
				SetSG2(null);
				SetDIcon1(null);
				SetDIcon2(null);
				SetTransaction(null);
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
		List<ISlotGroup> focusedSGs{get;}
		void ExecuteTransaction();
		void OnCompleteTransaction();
		void SetTransaction(ISlotSystemTransaction transaction);
		/* TransactionCache */
			void UpdateFields();
			void InnerUpdateFields(ISlotSystemTransaction ta);
			bool IsTransactionGoingToBeRevert(ISlottable sb);
			void CreateTransactionResults();
			Dictionary<IHoverable, ISlotSystemTransaction> transactionResults{get;}
			bool IsTransactionResultRevertFor(IHoverable sse);
			ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered);
			ISlottable pickedSB{get;}
			void SetPickedSB(ISlottable sb);
			ISlottable targetSB{get;}
			void SetTargetSB(ISlottable sb);
			IHoverable hovered{get;}
			void SetHovered(IHoverable ele);
			List<InventoryItemInstance> moved{get;}
			void SetMoved(List<InventoryItemInstance> moved);
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
			void SetTAMRecursively();
			void Refresh();
			void ClearFields();
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
