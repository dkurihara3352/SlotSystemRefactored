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
			public SlotSystemTransaction Transaction{
				get{return m_transaction;}
				}SlotSystemTransaction m_transaction;
				public void SetTransaction(SlotSystemTransaction transaction){
					if(m_transaction != transaction){
						m_transaction = transaction;
						if(m_transaction != null){
							m_transaction.Indicate();
						}
					}
				}
			// public void CompleteTransactionOnSB(Slottable sb){
				// 	if(pickedSB != null && sb == pickedSB) m_pickedSBDone = true;
				// 	else if(targetSB != null && sb == targetSB) m_targetSBDone = true;
				// 	IEnumeratorMock tryInvoke = ((AbsSGMProcess)ActionProcess).CoroutineMock();
				// }
			public void AcceptSGTAComp(SlotGroup sg){
				if(sg2 != null && sg == sg2) m_sg2Done = true;
				else if(sg1 != null && sg == sg1) m_sg1Done = true;
				IEnumeratorMock tryInvoke = ((AbsSGMProcess)ActionProcess).CoroutineMock();
			}
			public void AcceptDITAComp(DraggedIcon di){
				if(dIcon2 != null && di == dIcon2) m_dIcon2Done = true;
				else if(dIcon1 != null && di == dIcon1) m_dIcon1Done = true;
				IEnumeratorMock tryInvoke = ((AbsSGMProcess)ActionProcess).CoroutineMock();
			}
			public void OnAllTransactionComplete(){
				Transaction.OnComplete();
			}
		/*	states	*/
			SGMStateEngine SelStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SGMStateEngine(this);
					return m_selStateEngine;
				}
				}SGMStateEngine m_selStateEngine;
				public SGMSelectionState CurSelState{
					get{return (SGMSelectionState)SelStateEngine.curState;}
				}
				public SGMSelectionState PrevSelState{
					get{return (SGMSelectionState)SelStateEngine.prevState;}
				}
				public void SetSelState(SGMSelectionState state){
					SelStateEngine.SetState(state);
				}
			SGMStateEngine ActStateEngine{
				get{
					if(m_actStateEngine == null)
						m_actStateEngine = new SGMStateEngine(this);
					return m_actStateEngine;
				}
				}SGMStateEngine m_actStateEngine;
				public SGMActionState CurActState{
					get{return (SGMActionState)ActStateEngine.curState;}
				}
				public SGMActionState PrevActState{
					get{return (SGMActionState)ActStateEngine.prevState;}
				}
				public void SetActState(SGMActionState state){
					ActStateEngine.SetState(state);
				}
			/*	dump	*/
				// public void SetState(SGMState sgmState){
				// 	if(CurState != sgmState){
				// 		m_prevState = CurState;
				// 		m_prevState.ExitState(this);
				// 		m_curState = sgmState;
				// 		CurState.EnterState(this);
				// 	}
				// }
				// SGMState m_curState;
				// 	public SGMState CurState{
				// 		get{
				// 			if(m_curState == null)
				// 				m_curState = SlotGroupManager.DeactivatedState;
				// 			return m_curState;}
				// 	}
				// SGMState m_prevState;
				// 	public SGMState PrevState{
				// 		get{
				// 			if(m_prevState == null)
				// 				m_prevState = SlotGroupManager.DeactivatedState;
				// 			return m_prevState;
				// 		}
				// 	}
			/*	static states	*/
				/*	Selection State	*/
					public static SGMSelectionState DeactivatedState{
						get{return m_deactivatedState;}
						}static SGMSelectionState m_deactivatedState = new SGMDeactivatedState();
					public static SGMSelectionState DefocusedState{
						get{return m_defocusedState;}
						}static SGMSelectionState m_defocusedState = new SGMDefocusedState();
					public static SGMSelectionState FocusedState{
						get{return m_focusedState;}
						}static SGMSelectionState m_focusedState = new SGMFocusedState();
				/*	Action State	*/
					public static SGMActionState WaitForActionState{
						get{return m_WaitForActionState;}
						}static SGMActionState m_WaitForActionState = new SGMWaitForActionState();
					public static SGMActionState ProbingState{
						get{return m_probingState;}
						}static SGMActionState m_probingState = new SGMProbingState();
					public static SGMActionState PerformingTransactionState{
						get{return m_performingTransactionState;}
						}static SGMActionState m_performingTransactionState = new SGMPerformingTransactionState();
		/*	process	*/
			public SGMProcess SelectionProcess{
				get{return m_selectionProcess;}
				}SGMProcess m_selectionProcess;
				public void SetAndRunSelProcess(SGMProcess process){	
					if(SelectionProcess != null)
						SelectionProcess.Stop();
					m_selectionProcess = process;
					if(SelectionProcess != null)
						SelectionProcess.Start();
				}
			public SGMProcess ActionProcess{
				get{return m_actionProcess;}
				}SGMProcess m_actionProcess;
				public void SetAndRunActProcess(SGMProcess process){	
					if(ActionProcess != null)
						ActionProcess.Stop();
					m_actionProcess = process;
					if(ActionProcess != null)
						ActionProcess.Start();
				}
			/*	coroutines*/
				public IEnumeratorMock GreyoutCoroutine(){
					return null;
				}
				public IEnumeratorMock GreyinCoroutine(){
					return null;
				}
				public IEnumeratorMock ProbeCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForTransactionDone(){
					bool done = true;
					done &= m_dIcon1Done;
					done &= m_dIcon2Done;
					done &= m_sg1Done;
					done &= m_sg2Done;
					if(done){
						this.ActionProcess.Expire();
					}
					return null;
				}
			/*	dump	*/
				// public SGMProcess CachedProcess{
				// 	get{return m_cachedProcess;}
				// 	set{m_cachedProcess = value;}
				// 	}SGMProcess m_cachedProcess;
				// List<SGMProcess> m_runningProcess = new List<SGMProcess>();
				// 	public List<SGMProcess> RunningProcess{
				// 		get{
				// 			return m_runningProcess;
				// 		}
				// 	}
				// 	public void StopAllProcess(){
				// 		SetAndRunStateProcess(null);
				// 	}
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
		/*	public field	*/
			public Slottable pickedSB{
				get{return m_pickedSB;}
				}Slottable m_pickedSB;
				public void SetPickedSB(Slottable sb){
					this.m_pickedSB = sb;
				}
			public Slottable targetSB{
				get{return m_targetSB;}
				}Slottable m_targetSB;
				public void SetTargetSB(Slottable sb){
					if(sb == null || sb != targetSB){
						if(targetSB != null)
							targetSB.SetSelState(Slottable.FocusedState);
					}
					this.m_targetSB = sb;
					if(targetSB != null)
						targetSB.SetSelState(Slottable.SelectedState);
				}
			public SlotGroup sg1{
				get{return m_sg1;}
				}SlotGroup m_sg1;
				public void SetSG1(SlotGroup sg){
					if(sg == null || sg != sg1){
						if(sg1 != null)
							sg1.SetSelState(SlotGroup.FocusedState);
					}
					this.m_sg1 = sg;
					// if(sg1 != null)
					// 	sg1.SetSelState(SlotGroup.SelectedState);
					if(sg1 != null)
						m_sg1Done = false;
					else
						m_sg1Done = true;
				}
				public bool sg1Done{
				get{return m_sg1Done;}
				}bool m_sg1Done = true;
			public SlotGroup sg2{
				get{return m_sg2;}
				}SlotGroup m_sg2;
				public void SetSG2(SlotGroup sg){
					if(sg == null || sg != sg2){
						if(sg2 != null)
							sg2.SetSelState(SlotGroup.FocusedState);
					}
					this.m_sg2 = sg;
					if(sg2 != null)
						sg2.SetSelState(SlotGroup.SelectedState);
					if(sg2 != null)
						m_sg2Done = false;
					else
						m_sg2Done = true;
				}
				public bool sg2Done{
					get{return m_sg2Done;}
					}bool m_sg2Done = true;
			public DraggedIcon dIcon1{
				get{return m_dIcon1;}
				}DraggedIcon m_dIcon1;
				public void SetDIcon1(DraggedIcon di){
					m_dIcon1 = di;
					if(m_dIcon1 == null)
						m_dIcon1Done = true;
					else
						m_dIcon1Done = false;
				}
				public bool dIcon1Done{
				get{return m_dIcon1Done;}
				}bool m_dIcon1Done = true;
			public DraggedIcon dIcon2{
				get{return m_dIcon2;}
				}DraggedIcon m_dIcon2;
				public void SetDIcon2(DraggedIcon di){
					m_dIcon2 = di;
					if(m_dIcon2 == null)
						m_dIcon2Done = true;
					else
						m_dIcon2Done = false;
				}
				public bool dIcon2Done{
				get{return m_dIcon2Done;}
				}bool m_dIcon2Done = true;
			public Slottable HoveredSB{
				get{return m_hoveredSB;}
				}Slottable m_hoveredSB;
				public void SetHoveredSB(Slottable sb){
					if(sb == null || sb != HoveredSB){
						if(HoveredSB != null)
							HoveredSB.OnHoverExitMock();
					}
					m_hoveredSB = sb;
				}
			public SlotGroup HoveredSG{
				get{return m_hoveredSG;}
				}SlotGroup m_hoveredSG;
				public void SetHoveredSG(SlotGroup sg){
					if(sg == null || sg != HoveredSG){
						if(HoveredSG != null)
							HoveredSG.OnHoverExitMock();
					}
					m_hoveredSG = sg;
				}
			public InventoryManagerPage RootPage{
				get{return m_rootPage;}
				}InventoryManagerPage m_rootPage;
				public void SetRootPage(InventoryManagerPage rootPage){
					m_rootPage = rootPage;
					m_rootPage.SGM = this;
				}
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
					return (PoolInventory)FocusedSGP.inventory;
				}
			}
			public BowInstanceMock EquippedBowInst{
				get{
					foreach(SlotGroup sge in FocusedSGEs){
						if(sge.Filter is SGBowFilter)
							return (BowInstanceMock)sge.Slots[0].sb.ItemInst;
					}
					return null;
				}
			}
			public WearInstanceMock EquippedWearInst{
				get{
					foreach(SlotGroup sge in FocusedSGEs){
						if(sge.Filter is SGWearFilter)
							return (WearInstanceMock)sge.Slots[0].sb.ItemInst;
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
			public void Initialize(InventoryManagerPage invManPage){
				SetRootPage(invManPage);
				SelStateEngine.SetState(SlotGroupManager.DeactivatedState);
				ActStateEngine.SetState(SlotGroupManager.WaitForActionState);
				UpdateEquipStatesOnAll();
				//set all sgs and fields and initialize all sgs here
			}
			public void Focus(){
				SetCurSGM();
				SetSelState(SlotGroupManager.FocusedState);
				RootPage.Focus();
			}
			public void Defocus(){
				SetSelState(SlotGroupManager.DefocusedState);
				RootPage.Defocus();
			}
			public SlotGroup GetSG(Slottable sb){
				return this.RootPage.GetSlotGroup(sb);	
			}
			public void Reset(){
				SetActState(SlotGroupManager.WaitForActionState);
				ClearFields();
			}
			public void ResetAndFocus(){
				Reset();
				Focus();
			}
			public void ClearFields(){
				SetPickedSB(null);
				SetTargetSB(null);
				SetSG1(null);
				SetSG2(null);
				SetHoveredSB(null);
				SetHoveredSG(null);
				SetDIcon1(null);
				SetDIcon2(null);
				SetTransaction(null);
			}
			public void Deactivate(){
				SetSelState(SlotGroupManager.DeactivatedState);
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
				// sg.SetSorter(sorter);
				SlotSystemTransaction sortTransaction = new SortTransaction(sg, sorter);
				SetTargetSB(sortTransaction.targetSB);
				SetSG1(sortTransaction.sg1);
				SetTransaction(sortTransaction);
				Transaction.Execute();
			}
			public TransactionResults TransactionResults{
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
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, null, sg);
					TransactionResult tr = new TransactionResult(null, sg, ta);
					transactionResults.AddTransactionResult(tr);
					if(ta is RevertTransaction)
						sg.DefocusSelf();
					else
						sg.FocusSelf();
					foreach(Slottable sb in sg.Slottables){
						if(sb != null){
							SlotSystemTransaction ta2 = AbsSlotSystemTransaction.GetTransaction(pickedSB, sb, null);
							TransactionResult tr2 = new TransactionResult(sb, null, ta2);
							transactionResults.AddTransactionResult(tr2);
							if(ta2 is RevertTransaction || ta2 is FillTransaction)
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
				SetTargetSB(ta.targetSB);
				SetSG1(ta.sg1);
				SetSG2(ta.sg2);
				SetTransaction(ta);
			}
			public SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotGroup targetSG, Slottable targetSB){
            Slottable prevPickedSB = this.pickedSB;
				SetPickedSB(pickedSB);
				SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, targetSB, targetSG);
				SetPickedSB(prevPickedSB);
				return ta;
			}
		/*	dump	*/
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
