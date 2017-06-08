using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SlotGroupManager : MonoBehaviour, StateHandler{
			public static SlotGroupManager CurSGM;
			void SetCurSGM(){
				if(CurSGM != null){
					if(CurSGM != this){
						CurSGM.Defocus();
						CurSGM = this;
					}else{
						// no change
					}
				}else{
					CurSGM = this;
				}
			}
		/*	transaction	*/
			SlotSystemTransaction m_transaction;
				public SlotSystemTransaction Transaction{
					get{return m_transaction;}
				}
				public void SetTransaction(SlotSystemTransaction transaction){
					if(m_transaction != transaction){
						m_transaction = transaction;
						if(m_transaction != null){
							m_transaction.Indicate();
						}
					}
				}
			public void CompleteTransactionOnSB(Slottable sb){
				if(m_pickedSBForTS != null && sb == m_pickedSBForTS) m_pickedSBDoneTransaction = true;
				else if(m_selectedSBForTS != null && sb == m_selectedSBForTS)m_selectedSBDoneTransaction = true;
				IEnumeratorMock tryInvoke = ((AbsSGMProcess)StateProcess).CoroutineMock();
			}
			public void CompleteTransactionOnSG(SlotGroup sg){
				if(m_selectedSGForTS != null && sg == m_selectedSGForTS)
					m_selectedSGDoneTransaction = true;
				else if(m_origSGForTS != null && sg == m_origSGForTS)
					m_origSGDoneTransaction = true;
				else if(sg.CurActState == SlotGroup.PerformingTransactionState)
					m_selectedSGDoneTransaction = true;
				IEnumeratorMock tryInvoke = ((AbsSGMProcess)StateProcess).CoroutineMock();
			}
			public void CompleteAllTransaction(){
				Transaction.OnComplete();
			}
			public void SetTransactionFields(Slottable pickedSB, Slottable selectedSB, SlotGroup origSG, SlotGroup selectedSG){
				SetPickedSBDoneTransaction(pickedSB);
				SetSelectedSBDoneTransaction(selectedSB);
				SetOrigSGDoneTransaction(origSG);
				SetSelectedSGDoneTransaction(selectedSG);
			}
			bool m_pickedSBDoneTransaction = true;
				Slottable m_pickedSBForTS;
				public bool PickedSBDoneTransaction{
					get{return m_pickedSBDoneTransaction;}
				}
				public void SetPickedSBDoneTransaction(Slottable sb){
					m_pickedSBDoneTransaction = sb == null;
					m_pickedSBForTS = sb;
				}
			bool m_selectedSBDoneTransaction = true;
				Slottable m_selectedSBForTS;
				public bool SelectedSBDoneTransaction{
					get{return m_selectedSBDoneTransaction;}
				}
				public void SetSelectedSBDoneTransaction(Slottable sb){
					m_selectedSBDoneTransaction = sb == null;
					m_selectedSBForTS = sb;
				}
			bool m_origSGDoneTransaction = true;
				SlotGroup m_origSGForTS;
				public bool OrigSGDoneTransaction{
					get{return m_origSGDoneTransaction;}
				}
				public void SetOrigSGDoneTransaction(SlotGroup sg){
					m_origSGDoneTransaction = sg == null;
					m_origSGForTS = sg;
				}
			bool m_selectedSGDoneTransaction = true;
				SlotGroup m_selectedSGForTS;
				public bool SelectedSGDoneTransaction{
					get{return m_selectedSGDoneTransaction;}
				}
				public void SetSelectedSGDoneTransaction(SlotGroup sg){
					m_selectedSGDoneTransaction = sg ==null;
					m_selectedSGForTS = sg;
				}

		/*	command	methods */
			// public void SetupCommands(){
			// 	// m_updateTransactionCommand = new UpdateTransactionCommand();
			// 	m_postPickFilterCommand = new PostPickFilterCommand();
			// }
			// public void UpdateTransaction(){
				// 	m_updateTransactionCommand.Execute(this);
				// 	}SGMCommand m_updateTransactionCommand;
				// 	public SGMCommand UpdateTransactionCommand{
				// 		get{return m_updateTransactionCommand;}
				// 	}
			// SGMCommand m_postPickFilterCommand;
			// 	public SGMCommand PostPickFilterCommand{
			// 		get{return m_postPickFilterCommand;}
			// 	}
			// 	public void PostPickFilter(){
			// 		if(m_postPickFilterCommand != null)
			// 		m_postPickFilterCommand.Execute(this);
			// 	}
		/*	process	*/
			List<SGMProcess> m_runningProcess = new List<SGMProcess>();
				public List<SGMProcess> RunningProcess{
					get{
						return m_runningProcess;
					}
				}
				public void StopAllProcess(){
					SetAndRunStateProcess(null);
				}
			SGMProcess m_stateProcess;
				public SGMProcess StateProcess{
					get{return m_stateProcess;}
				}
			public void SetAndRunStateProcess(SGMProcess process){	
				if(StateProcess != null)
					StateProcess.Stop();
				m_stateProcess = process;
				if(StateProcess != null)
					StateProcess.Start();
			}
			SGMProcess m_cachedProcess;
			public SGMProcess CachedProcess{
				get{return m_cachedProcess;}
				set{m_cachedProcess = value;}
			}
			/*	coroutines
			*/
				public IEnumeratorMock ProbeCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForTransactionDone(){
					bool done = true;
					done &= m_pickedSBDoneTransaction;
					done &= m_selectedSBDoneTransaction;
					done &= m_origSGDoneTransaction;
					done &= m_selectedSGDoneTransaction;
					if(done){
						this.StateProcess.Expire();
					}
					return null;
				}
			/*	dump	*/
				// public IEnumeratorMock WaitForRevertDone(){
				// 	bool done = true;
				// 	done &= m_pickedSBDoneTransaction;
				// 	done &= m_selectedSBDoneTransaction;
				// 	done &= m_origSGDoneTransaction;
				// 	done &= m_selectedSGDoneTransaction;
				// 	if(done)
				// 		this.CurProcess.Expire();
				// 	return null;
				// }
				// public IEnumeratorMock WaitForFillDone(){
				// 	bool done = true;
				// 	done &= m_pickedSBDoneTransaction;
				// 	done &= m_selectedSBDoneTransaction;
				// 	done &= m_origSGDoneTransaction;
				// 	done &= m_selectedSGDoneTransaction;
				// 	if(done)
				// 		this.CurProcess.Expire();
				// 	return null;
				// }
				// public IEnumeratorMock WaitForFillEquipDone(){
				// 	bool done = true;
				// 	done &= m_pickedSBDoneTransaction;
				// 	done &= m_selectedSBDoneTransaction;
				// 	done &= m_origSGDoneTransaction;
				// 	done &= m_selectedSGDoneTransaction;
				// 	if(done)
				// 		this.CurProcess.Expire();
				// 	return null;
				// }
				// public IEnumeratorMock WaitForUnequipDone(){
				// 	bool done = true;
				// 	done &= m_pickedSBDoneTransaction;
				// 	done &= m_selectedSBDoneTransaction;
				// 	done &= m_origSGDoneTransaction;
				// 	done &= m_selectedSGDoneTransaction;
				// 	if(done)
				// 		this.CurProcess.Expire();
				// 	return null;
				// }
				// public IEnumeratorMock WaitForSwapDone(){
				// 	bool done = true;
				// 	done &= m_pickedSBDoneTransaction;
				// 	done &= m_selectedSBDoneTransaction;
				// 	done &= m_origSGDoneTransaction;
				// 	done &= m_selectedSGDoneTransaction;
				// 	if(done)
				// 		this.CurProcess.Expire();
				// 	return null;
				// }
				// public IEnumeratorMock WaitForSortingDone(){
				// 	bool done = true;
				// 	done &= m_pickedSBDoneTransaction;
				// 	done &= m_selectedSBDoneTransaction;
				// 	done &= m_origSGDoneTransaction;
				// 	done &= m_selectedSGDoneTransaction;
				// 	if(done)
				// 		this.CurProcess.Expire();
				// 	return null;
				// }
				// public IEnumeratorMock WaitForReorderDone(){
				// 	bool done = true;
				// 	done &= m_pickedSBDoneTransaction;
				// 	done &= m_selectedSBDoneTransaction;
				// 	done &= m_origSGDoneTransaction;
				// 	done &= m_selectedSGDoneTransaction;
				// 	if(done)
				// 		this.CurProcess.Expire();
				// 	return null;
				// }
		/*	states	*/
			public void SetState(SGMState sgmState){
				if(CurState != sgmState){
					m_prevState = CurState;
					m_prevState.ExitState(this);
					m_curState = sgmState;
					CurState.EnterState(this);
				}
			}
			SGMState m_curState;
				public SGMState CurState{
					get{
						if(m_curState == null)
							m_curState = SlotGroupManager.DeactivatedState;
						return m_curState;}
				}
			SGMState m_prevState;
				public SGMState PrevState{
					get{
						if(m_prevState == null)
							m_prevState = SlotGroupManager.DeactivatedState;
						return m_prevState;
					}
				}
			static SGMState m_deactivatedState = new SGMDeactivatedState();
				public static SGMState DeactivatedState{
					get{return m_deactivatedState;}
				}
			static SGMState m_defocusedState = new SGMDefocusedState();
				public static SGMState DefocusedState{
					get{return m_defocusedState;}
				}
			static SGMState m_focusedState = new SGMFocusedState();
				public static SGMState FocusedState{
					get{return m_focusedState;}
				}
			
			static SGMState m_probingState = new SGMProbingState();
				public static SGMState ProbingState{
					get{return m_probingState;}
				}
			static SGMState m_performingTransactionState = new SGMPerformingTransactionState();
				public static SGMState PerformingTransactionState{
					get{return m_performingTransactionState;}
				}
		
		/*	public field	*/
			public Slottable HoveredSB{
				get{return m_hoveredSB;}
				set{m_hoveredSB = value;}
				}Slottable m_hoveredSB;
			public SlotGroup HoveredSG{
				get{return m_hoveredSG;}
				set{m_hoveredSG = value;}
				}SlotGroup m_hoveredSG;
			public void SetHovered(Slottable sb, SlotGroup sg){
				HoveredSB = sb; HoveredSG = sg;
			}
			public Slottable SelectedSB{
				get{
					Slottable sb = null;
					if(Transaction != null)
						sb = Transaction.SelectedSB;
					return sb;
				}
				}Slottable m_selectedSB;
				public void SetSelectedSB(Slottable sb){/* obsolete */
					if(m_selectedSB != sb){
						m_selectedSB = sb;
					}
				}
			SlotGroup m_selectedSG;
				public SlotGroup SelectedSG{
					get{return m_selectedSG;}
				}
				public void SetSelectedSG(SlotGroup sg){
					if(m_selectedSG != sg){
						m_selectedSG = sg;
						// UpdateTransaction();
						// if(sg != null){
						// 	m_selectedSGDoneTransaction = false;
						// }else{
						// 	m_selectedSGDoneTransaction = true;
						// }
					}
				}
			Slottable m_pickedSB;
				public Slottable PickedSB{
					get{return m_pickedSB;}
				}
				public void SetPickedSB(Slottable sb){
					this.m_pickedSB = sb;
					// if(sb != null){
					// 	m_pickedSBDoneTransaction = false;
					// 	m_origSGDoneTransaction = false;
					// }else{
					// 	m_pickedSBDoneTransaction = true;
					// 	m_origSGDoneTransaction = true;
					// }
				}
			InventoryManagerPage m_rootPage;
				public InventoryManagerPage RootPage{
					get{return m_rootPage;}
				}
				public void SetRootPage(InventoryManagerPage rootPage){
					m_rootPage = rootPage;
					m_rootPage.SGM = this;
				}
			//
			public List<SlotGroup> AllSGs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					result.AddRange(AllSGPs);
					result.AddRange(AllSGEs);
					return result;
				}
			}
			public List<SlotGroup> AllSGPs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					foreach(SlotSystemElement ele in RootPage.PoolBundle.Elements){
						result.Add((SlotGroup)ele);
					}
					return result;
				}
			}
			public List<SlotGroup> AllSGEs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					foreach(SlotSystemElement ele in RootPage.EquipBundle.Elements){
						EquipmentSet equiSet = (EquipmentSet)ele;
						foreach(SlotSystemElement ele2 in equiSet.Elements){
							SlotGroup sge = (SlotGroup)ele2;
							result.Add(sge);
						}
					}
					return result;
				}
			}
			public SlotGroup FocusedSGP{
				get{
					SlotSystemElement focusedEle = RootPage.PoolBundle.GetFocusedBundleElement();
					return (SlotGroup)focusedEle;
				}
			}
			public EquipmentSet FocusedEqSet{
				get{
					return (EquipmentSet)RootPage.EquipBundle.GetFocusedBundleElement();
				}
			}
			public List<SlotGroup> FocusedSGEs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					EquipmentSet focusedEquipSet = FocusedEqSet;
					foreach(SlotSystemElement ele in focusedEquipSet.Elements){
						result.Add((SlotGroup)ele);
					}
					return result;
				}
			}
			public List<SlotGroup> FocusedSGs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					result.Add(FocusedSGP);
					result.AddRange(FocusedSGEs);
					return result;
				}
			}
			public List<EquipmentSet> EquipmentSets{
				get{
					List<EquipmentSet> result = new List<EquipmentSet>();
					foreach(SlotSystemElement ele in RootPage.EquipBundle.Elements){
						result.Add((EquipmentSet)ele);
					}
					return result;
				}
			}
			public PoolInventory PoolInv{
				get{
					return (PoolInventory)FocusedSGP.Inventory;
				}
			}
			public BowInstanceMock EquippedBowInst{
				get{
					foreach(SlotGroup sge in FocusedSGEs){
						if(sge.Filter is SGBowFilter)
							return (BowInstanceMock)sge.Slots[0].Sb.ItemInst;
					}
					return null;
				}
			}
			public WearInstanceMock EquippedWearInst{
				get{
					foreach(SlotGroup sge in FocusedSGEs){
						if(sge.Filter is SGWearFilter)
							return (WearInstanceMock)sge.Slots[0].Sb.ItemInst;
					}
					return null;
				}
			}
			public List<CarriedGearInstanceMock> EquippedCarriedGears{
				get{
					List<CarriedGearInstanceMock> result = new List<CarriedGearInstanceMock>();
					foreach(SlotGroup sge in FocusedSGEs){
						if(sge.Filter is SGCGearsFilter){
							foreach(Slottable sb in sge.Slottables){
								if(sb != null)
									result.Add((CarriedGearInstanceMock)sb.ItemInst);
							}
						}
					}
					return result;
				}
			}
			public List<PartsInstanceMock> EquippedParts{
				get{return null;}
			}
			/*	dump	*/
				// List<SlotGroup> m_slotGroups;
				// public List<SlotGroup> SlotGroups{
				// 	get{return m_slotGroups;}
				// }
				// SlotGroup m_initiallyFocusedSG;
				// public SlotGroup InitiallyFocusedSG{
				// 	get{return m_initiallyFocusedSG;}
				// 	set{m_initiallyFocusedSG = value;}
				// }
				/*	methods	*/
		/*	methods	*/
			public void Initialize(){
				m_rootPage.Activate();
				UpdateEquipStatesOnAll();
			}
			public void Focus(){
				SetCurSGM();
				if(CurState == SlotGroupManager.FocusedState){
					RootPage.Focus();
				}else
					this.SetState(SlotGroupManager.FocusedState);
			}
			public void Defocus(){
				if(CurState == SlotGroupManager.DefocusedState){
					RootPage.Defocus();
				}
				this.SetState(SlotGroupManager.DefocusedState);
			}
			public SlotGroup GetSlotGroup(Slottable sb){
				return this.RootPage.GetSlotGroup(sb);	
			}
			public void ClearAndReset(){
				this.SetTransaction(null);
				this.SetState(SlotGroupManager.FocusedState);
				this.ClearFields();
			}
			public void ClearFields(){
				m_selectedSB = null;
				if(m_selectedSG != null && m_selectedSG.CurSelState != SlotGroup.FocusedState)
					m_selectedSG.SetSelState(SlotGroup.FocusedState);
				m_selectedSG = null;
				m_pickedSB = null;
				
				m_pickedSBDoneTransaction = true;
				m_pickedSBForTS = null;
				m_selectedSBDoneTransaction = true;
				m_selectedSBForTS = null;
				m_origSGDoneTransaction = true;
				m_origSGForTS = null;
				m_selectedSGDoneTransaction = true;
				m_selectedSBForTS = null;
			}
			public void Deactivate(){
				SetState(SlotGroupManager.DeactivatedState);
				m_rootPage.Deactivate();
			}
			public void SetFocusedPoolSG(SlotGroup sg){
				RootPage.PoolBundle.SetFocusedBundleElement(sg);
				Focus();
			}
			public void SetFocusedEquipmentSet(EquipmentSet eSet){
				RootPage.EquipBundle.SetFocusedBundleElement(eSet);
				Focus();
			}
			public void DestroyDraggedIcon(){}
			public void UpdateEquipStatesOnAll(){
				/*	
					ItemInst of spe is marked the equipped one
					all sbs compare its iteminst with it and Equip or Unequip according to the result
				*/
				foreach(InventoryItemInstanceMock itemInst in PoolInv.Items){
					if(itemInst is BowInstanceMock)
						itemInst.IsEquipped = itemInst == EquippedBowInst;
					else if (itemInst is WearInstanceMock)
						itemInst.IsEquipped = itemInst == EquippedWearInst;
					else if(itemInst is CarriedGearInstanceMock)
						itemInst.IsEquipped = EquippedCarriedGears != null && EquippedCarriedGears.Contains((CarriedGearInstanceMock)itemInst);
					else if(itemInst is PartsInstanceMock)
						itemInst.IsEquipped = EquippedParts != null && EquippedParts.Contains((PartsInstanceMock)itemInst);
				}
				foreach(SlotGroup sg in AllSGs){
					foreach(Slottable sb in sg.Slottables){
						if(sb!= null)
							sb.UpdateEquipState();
					}
				}
			}
			public void SortSG(SlotGroup sg, SGSorter sorter){
				sg.SetSorter(sorter);
				SlotSystemTransaction sortTransaction = new SortTransaction(sg, sorter);
				SetTransaction(sortTransaction);
				Transaction.Execute();
			}
			TransactionResults TransactionResults{
				get{return m_transactionResults;}
				set{
					m_transactionResults = value;
				}
				}TransactionResults m_transactionResults;
			public void CreateTransactionResults(){
				/*	Create TransactionResult class instance with Transaction and SelectedSG, SelectedSB
					and store them in a list for lookup
					Filter all sbs and sgs according to the transaction result
					make sure to initialize with RevertTransaction with orig sg and pickedSB
					Perform Transaction update at SimHover looing up the list
				*/
				TransactionResults transactionResults = new TransactionResults();
				foreach(SlotGroup sg in FocusedSGs){
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(PickedSB, null, sg);
					TransactionResult tr = new TransactionResult(null, sg, ta);
					transactionResults.AddTransactionResult(tr);
					if(ta is RevertTransaction)
						sg.Defocus();
					else
						sg.Focus();
					foreach(Slottable sb in sg.Slottables){
						if(sb != null){
							SlotSystemTransaction ta2 = AbsSlotSystemTransaction.GetTransaction(PickedSB, sb, null);
							TransactionResult tr2 = new TransactionResult(sb, null, ta);
							transactionResults.AddTransactionResult(tr2);
							if(ta2 is RevertTransaction)
								sb.Defocus();
							else
								sb.Focus();
						}
					}
				}
				this.TransactionResults = transactionResults;
			}
			public void UpdateTransaction(){
				SlotSystemTransaction ta = TransactionResults.GetTransaction(HoveredSB, HoveredSG);
				SetTransaction(ta);
			}
		/*	dump	*/
			// public SlotGroup GetFocusedCGearsSG(){
			// 	foreach(SlotSystemElement ele in GetFocusedEquipSet().Elements){
			// 		if(((SlotGroup)ele).Filter is SGCarriedGearFilter)
			// 			return (SlotGroup)ele;
			// 	}
			// 	return null;
			// }
			// public List<Slot> GetCGEmptySlots(){
			// 	List<Slot> result = new List<Slot>();
			// 	foreach(Slot slot in GetFocusedCGearsSG().Slots){
			// 		if(slot.Sb == null)
			// 			result.Add(slot);
			// 	}
			// 	return result;
			// }
			// Slottable m_removedSb;
			// public Slottable removedSB{
			// 	set{
			// 		m_removedSb = value;
			// 	}
			// }
			// public void DestroyRemovedSB(){
			// 	if(m_removedSb != null){
			// 		GameObject go = m_removedSb.gameObject;
			// 		DestroyImmediate(m_removedSb);
			// 		DestroyImmediate(go);
			// 		m_removedSb = null;
			// 	}
			// }
			// public int GetInvInstID(InventoryItemInstanceMock invInst){
			// 	PoolInventory poolInv = (PoolInventory)GetFocusedPoolSG().Inventory;
			// 	List<InventoryItemInstanceMock> sameItemList = new List<InventoryItemInstanceMock>();
			// 	foreach(InventoryItemInstanceMock ii in poolInv.Items){
			// 		if(ii.Item == invInst.Item)
			// 			sameItemList.Add(ii);
			// 	}
			// 	return sameItemList.IndexOf(invInst);
			// }
			// public void SetSG(SlotGroup sg){
			// 	if(m_slotGroups == null)
			// 		m_slotGroups = new List<SlotGroup>();
			// 	m_slotGroups.Add(sg);
			// 	sg.SGM = this;
			// }
			// public void InitializeItems(){
			// 	foreach(SlotGroup sg in SlotGroups){
			// 		sg.InitializeItems();
			// 	}
			// }
			// public void SimSBHover(Slottable sb, PointerEventDataMock eventData){
			// 	if(CurState == SlotGroupManager.m_probingState){
			// 		if(sb != null){
			// 			if(SelectedSB != sb){
			// 				if(SelectedSB != null)
			// 					SelectedSB.OnHoverExitMock(eventData);
			// 				sb.OnHoverEnterMock(eventData);
			// 			}
			// 		}else{
			// 			if(SelectedSB != null){
			// 				SelectedSB.OnHoverExitMock(eventData);
			// 			}
			// 		}
			// 	}
			// 	UpdateTransaction();
			// }
			// public void SimSGHover(SlotGroup sg, PointerEventDataMock eventData){
			// 	if(CurState == SlotGroupManager.m_probingState){
			// 		if(sg != null){
			// 			if(SelectedSG != sg){
			// 				if(SelectedSG != null)
			// 					SelectedSG.OnHoverExitMock(eventData);
			// 				sg.OnHoverEnterMock(eventData);
			// 			}
			// 		}else{
			// 			if(SelectedSG != null){
			// 				SelectedSG.OnHoverExitMock(eventData);
			// 			}
			// 		}
			// 	}
			// 	UpdateTransaction();
			// }
	}

}
