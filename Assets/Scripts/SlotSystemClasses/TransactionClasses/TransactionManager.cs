﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionManager : SlotSystemRootElement, ITransactionManager{
		public List<ISlotGroup> focusedSGs{
			get{
				return ssm.focusedSGs;
			}
		}
		public void ExecuteTransaction(){
			Transact();
			transaction.Execute();
		}
		public void OnComplete(){
			transaction.OnComplete();
		}
		public ISlotSystemTransaction transaction{
			get{return m_transaction;}
		}
			ISlotSystemTransaction m_transaction;
			public void SetTransaction(ISlotSystemTransaction transaction){
				if(m_transaction != transaction){
					m_transaction = transaction;
					if(m_transaction != null){
						m_transaction.Indicate();
					}
				}
			}
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
				public void SetTransactionCache(ITransactionCache taCache){
					m_transactionCache = taCache;
				}
			public bool IsTransactionGoingToBeRevert(ISlottable sb){
				return transactionCache.IsTransactionGoingToBeRevert(sb);
			}
			public bool IsTransactionResultRevertFor(ISlotSystemElement sse){
				return transactionCache.IsCachedTAResultRevert(sse);
			}
			public virtual void CreateTransactionResults(){
				transactionCache.CreateTransactionResults();
			}
			public Dictionary<ISlotSystemElement, ISlotSystemTransaction> transactionResults{
				get{return transactionCache.transactionResults;}
			}
			public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered){
				return transactionCache.MakeTransaction(pickedSB, hovered);
			}
			public virtual ISlottable pickedSB{
				get{return transactionCache.pickedSB;}
			}
				public virtual void SetPickedSB(ISlottable sb){
					transactionCache.SetPickedSB(sb);
				}
			public ISlottable targetSB{
				get{return transactionCache.targetSB;}
			}
				public void SetTargetSB(ISlottable sb){
					transactionCache.SetTargetSB(sb);
				}
			public ISlotSystemElement hovered{
				get{return transactionCache.hovered;}
			}
				public virtual void SetHovered(ISlotSystemElement to){
					transactionCache.SetHovered(to);
				}
			public virtual List<InventoryItemInstance> moved{
				get{return transactionCache.moved;}
			}
				public virtual void SetMoved(List<InventoryItemInstance> moved){
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
			public ITransactionSGHandler sgHandler{
				get{
					if(m_sgHandler == null)
						m_sgHandler = new TransactionSGHandler(this);
					return m_sgHandler;
				}
			}
				ITransactionSGHandler m_sgHandler;
				public void SetSGHandler(ITransactionSGHandler sgHandler){
					m_sgHandler = sgHandler;
				}
			//
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
			public ITransactionIconHandler iconHandler{
				get{
					if(m_iconHandler == null)
						m_iconHandler = new TransactionIconHandler(this);
					return m_iconHandler;
				}
			}
				ITransactionIconHandler m_iconHandler;
				public void SetIconHandler(ITransactionIconHandler iconHandler){
					m_iconHandler = iconHandler;
				}
			//
		/* Framework */
			public void SetCurTAM(){
				if(curTAM != null){
					if(curTAM != (ISlotSystemManager)this){
						curTAM.Defocus();
						curTAM = this;
					}else{
						// no change
					}
				}else{
					curTAM = this;
				}
			}
				public static ITransactionManager curTAM;
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
						m_actStateEngine = new SSEStateEngine<ITAMActState>(this);
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
			public virtual bool isActStateNull{
				get{return curActState == null;}
			}
			public virtual bool wasActStateNull{
				get{return prevActState == null;}
			}
			public virtual void ClearCurActState(){
				SetActState(null);
			}
			/* states */
				public virtual void WaitForAction(){
					SetActState(waitForActionState);
				}
				public ITAMActState waitForActionState{
					get{
						if(m_waitForActionState == null)
							m_waitForActionState = new TAMWaitForActionState();
						return m_waitForActionState;
					}
				}
					ITAMActState m_waitForActionState;
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
					get{
						if(m_probingState == null)
							m_probingState = new TAMProbingState();
						return m_probingState;
					}
				}
					ITAMActState m_probingState;
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
					get{
						if(m_transactionState == null)
							m_transactionState = new TAMTransactionState();
						return m_transactionState;
					}
				}
					ITAMActState m_transactionState;
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
		ISlotSystemTransaction transaction{get;}
		void SetTransaction(ISlotSystemTransaction transaction);
		void ExecuteTransaction();
		void OnComplete();
		/* TransactionCache */
			void UpdateFields();
			void InnerUpdateFields(ISlotSystemTransaction ta);
			bool IsTransactionGoingToBeRevert(ISlottable sb);
			void CreateTransactionResults();
			Dictionary<ISlotSystemElement, ISlotSystemTransaction> transactionResults{get;}
			bool IsTransactionResultRevertFor(ISlotSystemElement sse);
			ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered);
			ISlottable pickedSB{get;}
			void SetPickedSB(ISlottable sb);
			ISlottable targetSB{get;}
			void SetTargetSB(ISlottable sb);
			ISlotSystemElement hovered{get;}
			void SetHovered(ISlotSystemElement ele);
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
			ITransactionSGHandler sgHandler{get;}
		/* TAIconHandler */
			void AcceptDITAComp(DraggedIcon di);
			DraggedIcon dIcon1{get;}
			void SetDIcon1(DraggedIcon di);
			DraggedIcon dIcon2{get;}
			void SetDIcon2(DraggedIcon di);
			ITransactionIconHandler iconHandler{get;}
		/* Framework */
			void SetCurTAM();
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
