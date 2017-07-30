using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionManager : SlotSystemRootElement, ITransactionManager{
		public static ITransactionManager curTAM;
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
		
		public override void InitializeStates(){
			WaitForAction();
		}
		public void Refresh(){
			WaitForAction();
		}
		public void ClearFields(){
			SetPickedSB(null);
			SetTargetSB(null);
			SetSG1(null);
			SetSG2(null);
			SetHovered(null);
			SetDIcon1(null);
			SetDIcon2(null);
			SetTransaction(null);
		}
		/*	States	*/
			/*	Action state	*/
				ISSEStateEngine<ITAMActState> actStateEngine{
					get{
						if(m_actStateEngine == null)
							m_actStateEngine = new SSEStateEngine<ITAMActState>(this);
						return m_actStateEngine;
					}
					}ISSEStateEngine<ITAMActState> m_actStateEngine;
				void SetActStateEngine(ISSEStateEngine<ITAMActState> engine){m_actStateEngine = engine;}
				ITAMActState curActState{
					get{return actStateEngine.curState;}
				}
				ITAMActState prevActState{
					get{return actStateEngine.prevState;}
				}
				void SetActState(ITAMActState state){
					actStateEngine.SetState(state);
					if(state ==null && actProcess != null)
						SetAndRunActProcess(null);
				}
				public virtual bool wasActStateNull{get{return prevActState == null;}}
				public virtual void ClearCurActState(){SetActState(null);}
				/* Static states */
					public ITAMActState waitForActionState{
						get{
							if(m_waitForActionState == null)
								m_waitForActionState = new TAMWaitForActionState();
							return m_waitForActionState;
						}
						}ITAMActState m_waitForActionState;
						public virtual void WaitForAction(){SetActState(waitForActionState);}
						public virtual bool isWaitingForAction{get{return curActState == waitForActionState;}}
						public virtual bool wasWaitingForAction{get{return prevActState == waitForActionState;}}
					public ITAMActState probingState{
						get{
							if(m_probingState == null)
								m_probingState = new TAMProbingState();
							return m_probingState;
						}
						}ITAMActState m_probingState;
						public virtual void Probe(){SetActState(probingState);}
						public virtual bool isProbing{get{return curActState == probingState;}}
						public virtual bool wasProbing{get{return prevActState == probingState;}}
					public ITAMActState transactionState{
						get{
							if(m_transactionState == null)
								m_transactionState = new TAMTransactionState();
							return m_transactionState;
						}
						}ITAMActState m_transactionState;
						public virtual void Transact(){SetActState(transactionState);}
						public virtual bool isTransacting{get{return curActState == transactionState;}}
						public virtual bool wasTransacting{get{return prevActState == transactionState;}}
			/*	process	*/
				/*	Action Process	*/
					public virtual ISSEProcessEngine<ITAMActProcess> actProcEngine{
						get{
							if(m_actProcEngine == null)
								m_actProcEngine = new SSEProcessEngine<ITAMActProcess>();
							return m_actProcEngine;
						}
						}ISSEProcessEngine<ITAMActProcess> m_actProcEngine;
					public virtual void SetActProcEngine(ISSEProcessEngine<ITAMActProcess> engine){m_actProcEngine = engine;}
					public virtual ITAMActProcess actProcess{
						get{return actProcEngine.process;}
					}
					public virtual void SetAndRunActProcess(ITAMActProcess process){
						actProcEngine.SetAndRunProcess(process);
					}
					public void ExpireActProcess(){if(actProcess != null) actProcess.Expire();}
					/* Coroutine */
						public IEnumeratorFake probeCoroutine(){
							return null;
						}
						public IEnumeratorFake transactionCoroutine(){
							bool done = true;
							done &= m_dIcon1Done;
							done &= m_dIcon2Done;
							done &= m_sg1Done;
							done &= m_sg2Done;
							if(done){
								this.actProcess.Expire();
							}
							return null;
						}
		public ITransactionFactory taFactory{
				get{
					if(m_taFactory == null)
						m_taFactory = new TransactionFactory();
					return m_taFactory;
				}
			}ITransactionFactory m_taFactory;
			public void SetTAFactory(ITransactionFactory taFac){m_taFactory = taFac;}
			public ISlotSystemTransaction transaction{get{return m_transaction;}} ISlotSystemTransaction m_transaction;
				public void SetTransaction(ISlotSystemTransaction transaction){
					if(m_transaction != transaction){
						m_transaction = transaction;
						if(m_transaction != null){
							m_transaction.Indicate();
						}
					}
				}
			public void AcceptSGTAComp(ISlotGroup sg){
				if(sg2 != null && sg == sg2) m_sg2Done = true;
				else if(sg1 != null && sg == sg1) m_sg1Done = true;
				if(isTransacting){
					transactionCoroutine();
				}
			}
			public void AcceptDITAComp(DraggedIcon di){
				if(dIcon2 != null && di == dIcon2) m_dIcon2Done = true;
				else if(dIcon1 != null && di == dIcon1) m_dIcon1Done = true;
				if(isTransacting){
					transactionCoroutine();
				}
			}
			public void ExecuteTransaction(){
				Transact();
				transaction.Execute();
			}
			public virtual ISlottable pickedSB{get{return m_pickedSB;}} ISlottable m_pickedSB;
				public virtual void SetPickedSB(ISlottable sb){this.m_pickedSB = sb;}
			public ISlottable targetSB{get{return m_targetSB;}} ISlottable m_targetSB;
				public void SetTargetSB(ISlottable sb){
					if(sb == null || sb != targetSB){
						if(targetSB != null)
							targetSB.Focus();
						this.m_targetSB = sb;
						if(targetSB != null)
							targetSB.Select();
					}
				}
			public ISlotGroup sg1{get{return m_sg1;}} ISlotGroup m_sg1;
				public void SetSG1(ISlotGroup to){
					if(to == null || to != sg1){
						if(sg1 != null)
							sg1.Activate();
						this.m_sg1 = to;
						if(sg1 != null)
							m_sg1Done = false;
						else
							m_sg1Done = true;
					}
				}
				public bool sg1Done{get{return m_sg1Done;}} bool m_sg1Done = true;
			public ISlotGroup sg2{get{return m_sg2;}} ISlotGroup m_sg2;
				public void SetSG2(ISlotGroup sg){
					if(sg == null || sg != sg2){
						if(sg2 != null)
							sg2.Activate();
						this.m_sg2 = sg;
						if(sg2 != null)
							sg.Select();
						if(sg2 != null)
							m_sg2Done = false;
						else
							m_sg2Done = true;
					}
				}
				public bool sg2Done{get{return m_sg2Done;}} bool m_sg2Done = true;
			public virtual DraggedIcon dIcon1{get{return m_dIcon1;}} DraggedIcon m_dIcon1;
				public virtual void SetDIcon1(DraggedIcon di){
					m_dIcon1 = di;
					if(m_dIcon1 == null)
						m_dIcon1Done = true;
					else
						m_dIcon1Done = false;
				}
				public bool dIcon1Done{get{return m_dIcon1Done;}} bool m_dIcon1Done = true;
			public virtual DraggedIcon dIcon2{get{return m_dIcon2;}} DraggedIcon m_dIcon2;
				public virtual void SetDIcon2(DraggedIcon di){
					m_dIcon2 = di;
					if(m_dIcon2 == null)
						m_dIcon2Done = true;
					else
						m_dIcon2Done = false;
				}
				public bool dIcon2Done{get{return m_dIcon2Done;}} bool m_dIcon2Done = true;
			public ISlotSystemElement hovered{get{return m_hovered;}} protected ISlotSystemElement m_hovered;
				public virtual void SetHovered(ISlotSystemElement to){
					if(to == null || to != hovered){
						if(to != null && hovered != null){
							hovered.OnHoverExit();
						}
						m_hovered = to;
						if(hovered != null)
							UpdateTransaction();
					}
				}
			public virtual List<InventoryItemInstance> moved{get{return m_moved;}} List<InventoryItemInstance> m_moved;
			public virtual void SetMoved(List<InventoryItemInstance> moved){this.m_moved = moved;}
			public Dictionary<ISlotSystemElement, ISlotSystemTransaction> transactionResults{
				get{
					if(m_transactionResults == null)
						m_transactionResults = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
					return m_transactionResults;
				}
				}Dictionary<ISlotSystemElement, ISlotSystemTransaction> m_transactionResults;
				public void SetTransactionResults(Dictionary<ISlotSystemElement, ISlotSystemTransaction> results){
					m_transactionResults = results;
				}
			public virtual void CreateTransactionResults(){
				Dictionary<ISlotSystemElement, ISlotSystemTransaction> result = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
				foreach(ISlotGroup sg in ssm.focusedSGs){
					ISlotSystemTransaction ta = MakeTransaction(pickedSB, sg);
					result.Add(sg, ta);
					if(ta is IRevertTransaction)
						sg.DefocusSelf();
					else
						sg.FocusSelf();
					foreach(ISlottable sb in sg){
						if(sb != null){
							ISlotSystemTransaction ta2 = MakeTransaction(pickedSB, sb);
							result.Add(sb, ta2);
							if(ta2 is IRevertTransaction || ta2 is IFillTransaction)
								sb.Defocus();
							else
								sb.Focus();
						}
					}
				}
				SetTransactionResults(result);
			}
			public virtual void UpdateTransaction(){
				if(transactionResults != null){
					ISlotSystemTransaction ta = null;
					if(transactionResults.TryGetValue(hovered, out ta)){
						SetTargetSB(ta.targetSB);
						SetSG1(ta.sg1);
						SetSG2(ta.sg2);
						SetMoved(ta.moved);
						SetTransaction(ta);
					}
				}
			}
			public bool IsTransactionResultRevertFor(ISlotSystemElement sse){
				if(transactionResults != null){
					ISlotSystemTransaction ta = null;
					if(transactionResults.TryGetValue(sse, out ta)){
						if(ta is IRevertTransaction)
							return true;
						else
							return false;
					}
					else
						throw new System.InvalidOperationException("Transaction not found");
				}else
					throw new System.InvalidOperationException("TransactionResults not set");
			}
			public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered){
				return taFactory.MakeTransaction(pickedSB, hovered);
			}
			public void PrePickFilter(ISlottable sb, out bool isFilteredIn){
				bool res = false;
				foreach(ISlotGroup targetSG in ssm.focusedSGs){
					ISlotSystemTransaction ta = MakeTransaction(sb, targetSG);
					if(ta == null)
						throw new System.InvalidOperationException("SlotSystemManager.PrePickFilter: given hoveredSSE does not yield any transaction. something's wrong baby");
					else{
						if(!(ta is IRevertTransaction)){
							res = true; break;
						}
					}
					foreach(ISlottable targetSB in targetSG){
						if(sb != null)
							if(!(MakeTransaction(sb, targetSB) is IRevertTransaction)){
								res = true; break;
							}
					}
				}
				isFilteredIn = res;
			}
			public void SortSG(ISlotGroup sg, SGSorter sorter){
				ISlotSystemTransaction sortTransaction = sortFA.MakeSortTA(sg, sorter);
				SetTargetSB(sortTransaction.targetSB);
				SetSG1(sortTransaction.sg1);
				SetTransaction(sortTransaction);
				transaction.Execute();
			}ISortTransactionFactory sortFA{
				get{
					if(m_sortFA == null)
						m_sortFA = new SortTransactionFactory();
					return m_sortFA;
				}
			}ISortTransactionFactory m_sortFA;
			public void SetSortFA(ISortTransactionFactory fa){m_sortFA = fa;}
	}
	public interface ITransactionManager: ISlotSystemElement{
		void SetCurTAM();
		void Refresh();
		void ClearFields();
		IEnumeratorFake probeCoroutine();
		/* ActState */
				bool wasActStateNull{get;}
				void ClearCurActState();
				ITAMActState waitForActionState{get;}
					void WaitForAction();
					bool isWaitingForAction{get;}
					bool wasWaitingForAction{get;}
				ITAMActState probingState{get;}
					void Probe();
					bool isProbing{get;}
					bool wasProbing{get;}
				ITAMActState transactionState{get;}
					void Transact();
					bool isTransacting{get;}
					bool wasTransacting{get;}
			/* Process */
				ISSEProcessEngine<ITAMActProcess> actProcEngine{get;}
					void SetActProcEngine(ISSEProcessEngine<ITAMActProcess> engine);
					void SetAndRunActProcess(ITAMActProcess process);
					ITAMActProcess actProcess{get;}
						IEnumeratorFake transactionCoroutine();
					void ExpireActProcess();
		ITransactionFactory taFactory{get;}
		ISlotSystemTransaction transaction{get;}
		void AcceptSGTAComp(ISlotGroup sg);
		void AcceptDITAComp(DraggedIcon di);
		ISlottable pickedSB{get;}
		ISlottable targetSB{get;}
		ISlotGroup sg1{get;}
		ISlotGroup sg2{get;}
		DraggedIcon dIcon1{get;}
		DraggedIcon dIcon2{get;}
		ISlotSystemElement hovered{get;}
		List<InventoryItemInstance> moved{get;}
		void UpdateTransaction();
		void CreateTransactionResults();
		bool IsTransactionResultRevertFor(ISlotSystemElement sse);
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered);
		
		void SetTransaction(ISlotSystemTransaction transaction);	
		void SetPickedSB(ISlottable sb);
		void SetTargetSB(ISlottable sb);
		void SetSG1(ISlotGroup sg);
		bool sg1Done{get;}
		void SetSG2(ISlotGroup sg);
		bool sg2Done{get;}
		void SetDIcon1(DraggedIcon di);
		bool dIcon1Done{get;}
		void SetDIcon2(DraggedIcon di);
		bool dIcon2Done{get;}
		void SetHovered(ISlotSystemElement ele);
		void SetMoved(List<InventoryItemInstance> moved);
		void ExecuteTransaction();
		void PrePickFilter(ISlottable sb, out bool isFilteredIn);
		void SortSG(ISlotGroup sg, SGSorter sorter);
	}
	public class SortTransactionFactory: ISortTransactionFactory{
		public ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter){
			return new SortTransaction(sg, sorter);
		}
	}
	public interface ISortTransactionFactory{
		ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter);
	}
}
