﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlotSystem{
	public class SlotGroupManager : MonoBehaviour {
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
				if(sb == m_pickedSBForTS) m_pickedSBDoneTransaction = true;
				else if(sb == m_selectedSBForTS) m_selectedSBDoneTransaction = true;
				IEnumeratorMock tryInvoke = ((AbsSGMProcess)StateProcess).CoroutineMock();
			}
			public void CompleteTransactionOnSG(SlotGroup sg){
				if(m_selectedSGForTS != null && sg == m_selectedSGForTS)
					m_selectedSGDoneTransaction = true;
				else if(m_origSGForTS != null && sg == m_origSGForTS)
					m_origSGDoneTransaction = true;
				else if(sg.CurState == SlotGroup.PerformingTransactionState)
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

		/*	command	*/
			public void SetupCommands(){
				m_updateTransactionCommand = new UpdateTransactionCommand();
				m_postPickFilterCommand = new PostPickFilterCommand();
			}
			SGMCommand m_updateTransactionCommand;
				public SGMCommand UpdateTransactionCommand{
					get{return m_updateTransactionCommand;}
				}
				public void UpdateTransaction(){
					m_updateTransactionCommand.Execute(this);
				}
			SGMCommand m_postPickFilterCommand;
				public SGMCommand PostPickFilterCommand{
					get{return m_postPickFilterCommand;}
				}
				public void PostPickFilter(){
					if(m_postPickFilterCommand != null)
					m_postPickFilterCommand.Execute(this);
				}
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
			Slottable m_selectedSB;
				public Slottable SelectedSB{
					get{return m_selectedSB;}
				}
				public void SetSelectedSB(Slottable sb){
					if(m_selectedSB != sb){
						m_selectedSB = sb;
						// UpdateTransaction();
						// if(sb != null){
						// 	m_selectedSBDoneTransaction = false;
						// }else{
						// 	m_selectedSBDoneTransaction = true;
						// }
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
			public List<SlotGroup> FocusedSGEs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					EquipmentSet focusedEquipSet = GetFocusedEquipSet();
					foreach(SlotSystemElement ele in focusedEquipSet.Elements){
						result.Add((SlotGroup)ele);
					}
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
				UpdateEquipStatus();
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
			public EquipmentSet GetFocusedEquipSet(){
				return (EquipmentSet)RootPage.EquipBundle.GetFocusedBundleElement();
			}
			public BowInstanceMock GetEquippedBow(){
				// foreach(SlotGroup sg in SlotGroups){
				// 	if(!sg.IsPool && sg.Filter.GetType() == typeof(SGBowFilter))
				// 		return (BowInstanceMock)sg.Slots[0].Sb.Item;
				// }

				// return null;
				foreach(SlotSystemElement ele in GetFocusedEquipSet().Elements){
					SlotGroup sg = (SlotGroup)ele;
					if(sg.Filter is SGBowFilter)
						return (BowInstanceMock)sg.Slots[0].Sb.Item;
				}
				return null;
			}
			public WearInstanceMock GetEquippedWear(){
				// foreach(SlotGroup sg in SlotGroups){
				// 	if(!sg.IsPool && sg.Filter.GetType() == typeof(SGWearFilter))
				// 		return (WearInstanceMock)sg.Slots[0].Sb.Item;
				// }
				// return null;
				foreach(SlotSystemElement ele in GetFocusedEquipSet().Elements){
					SlotGroup sg = (SlotGroup)ele;
					if(sg.Filter is SGWearFilter)
						return (WearInstanceMock)sg.Slots[0].Sb.Item;
				}
				return null;
			}
			public List<CarriedGearInstanceMock> GetEquippedCarriedGears(){
				List<CarriedGearInstanceMock> result = new List<CarriedGearInstanceMock>();
				EquipmentSet equipSet = (EquipmentSet)RootPage.EquipBundle.GetFocusedBundleElement();
				foreach(SlotSystemElement ele in equipSet.Elements){
					SlotGroup sg = (SlotGroup)ele;
					if(sg.Filter is SGCGearsFilter){
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null)
								result.Add((CarriedGearInstanceMock)slot.Sb.Item);
						}
					}
				}
				return result;
			}
			public void ClearAndReset(){
				this.SetTransaction(null);
				this.SetState(SlotGroupManager.FocusedState);
				this.ClearFields();
			}
			public void ClearFields(){
				m_selectedSB = null;
				if(m_selectedSG != null && m_selectedSG.CurState != SlotGroup.FocusedState)
					m_selectedSG.SetState(SlotGroup.FocusedState);
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
			public SlotGroup GetFocusedPoolSG(){
				SlotSystemElement focusedEle = RootPage.PoolBundle.GetFocusedBundleElement();
				return (SlotGroup)focusedEle;
			}
			public void SetFocusedEquipmentSet(EquipmentSet eSet){
				RootPage.EquipBundle.SetFocusedBundleElement(eSet);
				Focus();
			}
			public void DestroyDraggedIcon(){}
			public void UpdateEquipStatus(){
				GetEquippedBow().IsEquipped = true;
				GetEquippedWear().IsEquipped = true;
				foreach(CarriedGearInstanceMock cGear in GetEquippedCarriedGears()){
					cGear.IsEquipped = true;
				}
				foreach(SlotSystemElement ele in RootPage.PoolBundle.Elements){
					SlotGroup sg = (SlotGroup)ele;
					foreach(InventoryItemInstanceMock item in sg.Inventory.Items){
						if(item.IsEquipped){
							if(item is BowInstanceMock){
								if(GetEquippedBow() != (BowInstanceMock)item)
									item.IsEquipped = false;
							}else if(item is WearInstanceMock){
								if(GetEquippedWear() != (WearInstanceMock)item)
									item.IsEquipped = false;
							}else if(item is CarriedGearInstanceMock){
								List<CarriedGearInstanceMock> cGears = GetEquippedCarriedGears();
								if(cGears.Count == 0)
									item.IsEquipped = false;
								else{
									bool found = false;
									foreach(CarriedGearInstanceMock cGear in cGears){
										if(cGear == (CarriedGearInstanceMock)item)
											found = true;
									}
									if(!found)
										item.IsEquipped = false;
								}
							}
						}
					}
				}
				SlotGroup poolSG = GetFocusedPoolSG();
				
			}
			public void SortSG(SlotGroup sg, SGSorter sorter){
				sg.SetSorter(sorter);
				SlotSystemTransaction sortTransaction = new SortTransaction(sg, sorter);
				SetTransaction(sortTransaction);
				Transaction.Execute();
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
