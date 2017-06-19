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
			public void AcceptSGTAComp(SlotGroup sg){
				if(sg2 != null && sg == sg2) m_sg2Done = true;
				else if(sg1 != null && sg == sg1) m_sg1Done = true;
				if(CurActState == SlotGroupManager.PerformingTransactionState){
					IEnumeratorMock tryInvoke = ((AbsSGMProcess)ActionProcess).CoroutineMock();
				}
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
			public Slottable hoveredSB{
				get{return m_hoveredSB;}
				}Slottable m_hoveredSB;
				public void SetHoveredSB(Slottable sb){
					if(sb == null || sb != hoveredSB){
						if(hoveredSB != null)
							hoveredSB.OnHoverExitMock();
					}
					m_hoveredSB = sb;
				}
			public SlotGroup hoveredSG{
				get{return m_hoveredSG;}
				}SlotGroup m_hoveredSG;
				public void SetHoveredSG(SlotGroup sg){
					if(sg == null || sg != hoveredSG){
						if(hoveredSG != null)
							hoveredSG.OnHoverExitMock();
					}
					m_hoveredSG = sg;
				}
			public InventoryManagerPage rootPage{
				get{return m_rootPage;}
				}InventoryManagerPage m_rootPage;
				public void SetRootPage(InventoryManagerPage rootPage){
					m_rootPage = rootPage;
					rootPage.SetSGMRecursively(this);
				}
			public List<SlotGroup> allSGs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					result.AddRange(allSGPs);
					result.AddRange(allSGEs);
					return result;
				}
			}
			public List<SlotGroup> allSGPs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					foreach(SlotSystemElement ele in rootPage.PoolBundle){
						result.Add((SlotGroup)ele);
					}
					return result;
				}
			}
			public List<SlotGroup> allSGEs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					foreach(SlotSystemElement ele in rootPage.EquipBundle){
						EquipmentSet equiSet = (EquipmentSet)ele;
						foreach(SlotSystemElement ele2 in equiSet){
							SlotGroup sge = (SlotGroup)ele2;
							result.Add(sge);
						}
					}
					return result;
				}
			}
			public SlotGroup focusedSGEBow{
				get{
					foreach(SlotGroup sge in focusedSGEs){
						if(sge.Filter is SGBowFilter)
							return sge;
					}
					return null;
				}
			}
			public SlotGroup focusedSGEWear{
				get{
					foreach(SlotGroup sge in focusedSGEs){
						if(sge.Filter is SGWearFilter)
							return sge;
					}
					return null;
				}
			}
			public SlotGroup focusedSGECGears{
				get{
					foreach(SlotGroup sge in focusedSGEs){
						if(sge.Filter is SGCGearsFilter)
							return sge;
					}
					return null;
				}
			}
			public List<InventoryItemInstanceMock> actualEquippedItems{
				get{
					List<InventoryItemInstanceMock> items = new List<InventoryItemInstanceMock>();
					items.Add(equippedBowInst);
					items.Add(equippedWearInst);
					foreach(CarriedGearInstanceMock cgItem in equippedCarriedGears){
						items.Add(cgItem);
					}
					return items;
				}
			}
			public SlotGroup focusedSGP{
				get{
					SlotSystemElement focusedEle = rootPage.PoolBundle.focusedElement;
					return (SlotGroup)focusedEle;
				}
			}
			public EquipmentSet focusedEqSet{
				get{
					return (EquipmentSet)rootPage.EquipBundle.focusedElement;
				}
			}
			public List<SlotGroup> focusedSGEs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					EquipmentSet focusedEquipSet = focusedEqSet;
					foreach(SlotSystemElement ele in focusedEquipSet){
						result.Add((SlotGroup)ele);
					}
					return result;
				}
			}
			public List<SlotGroup> focusedSGs{
				get{
					List<SlotGroup> result = new List<SlotGroup>();
					result.Add(focusedSGP);
					result.AddRange(focusedSGEs);
					return result;
				}
			}
			public List<EquipmentSet> equipmentSets{
				get{
					List<EquipmentSet> result = new List<EquipmentSet>();
					foreach(SlotSystemElement ele in rootPage.EquipBundle){
						result.Add((EquipmentSet)ele);
					}
					return result;
				}
			}
			public PoolInventory poolInv{
				get{
					return (PoolInventory)focusedSGP.inventory;
				}
			}
			public EquipmentSetInventory equipInv{
				get{
					return (EquipmentSetInventory)focusedSGEs[0].inventory;
				}
			}
			public BowInstanceMock equippedBowInst{
				get{
					foreach(SlotGroup sge in focusedSGEs){
						if(sge.Filter is SGBowFilter)
							return (BowInstanceMock)sge.slots[0].sb.itemInst;
					}
					return null;
				}
			}
			public WearInstanceMock equippedWearInst{
				get{
					foreach(SlotGroup sge in focusedSGEs){
						if(sge.Filter is SGWearFilter)
							return (WearInstanceMock)sge.slots[0].sb.itemInst;
					}
					return null;
				}
			}
			public List<CarriedGearInstanceMock> equippedCarriedGears{
				get{
					List<CarriedGearInstanceMock> result = new List<CarriedGearInstanceMock>();
					foreach(SlotGroup sge in focusedSGEs){
						if(sge.Filter is SGCGearsFilter){
							foreach(Slottable sb in sge){
								if(sb != null)
									result.Add((CarriedGearInstanceMock)sb.itemInst);
							}
						}
					}
					return result;
				}
			}
			public List<PartsInstanceMock> equippedParts{
				get{
					List<PartsInstanceMock> items = new List<PartsInstanceMock>();
					return items;
				}
			}
		/*	methods	*/
			public void Initialize(InventoryManagerPage invManPage){
				SetRootPage(invManPage);
				SelStateEngine.SetState(SlotGroupManager.DeactivatedState);
				ActStateEngine.SetState(SlotGroupManager.WaitForActionState);
			}
			public void Activate(){
				SetCurSGM();
				UpdateEquipStatesOnAll();
				Focus();
			}
			public void Focus(){
				SetSelState(SlotGroupManager.FocusedState);
				rootPage.Focus();
			}
			public void Defocus(){
				SetSelState(SlotGroupManager.DefocusedState);
				rootPage.Defocus();
			}
			public SlotGroup GetSG(Slottable sb){
				return (SlotGroup)rootPage.DirectParent(sb);
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
				rootPage.PoolBundle.SetFocusedBundleElement(sg);
				Focus();
			}
			public void SetFocusedEquipmentSet(EquipmentSet eSet){
				rootPage.EquipBundle.SetFocusedBundleElement(eSet);
				Focus();
			}
			public void UpdateEquipStatesOnAll(){
				// /*	update equip inventory	*/
					/*	remove	*/
						List<InventoryItemInstanceMock> removed = new List<InventoryItemInstanceMock>();
						foreach(InventoryItemInstanceMock itemInInv in equipInv){
							if(!actualEquippedItems.Contains(itemInInv))
								removed.Add(itemInInv);
						}
						foreach(InventoryItemInstanceMock item in removed){
							equipInv.Remove(item);
						}
					/*	add	*/
						List<InventoryItemInstanceMock> added = new List<InventoryItemInstanceMock>();
						foreach(InventoryItemInstanceMock itemInAct in actualEquippedItems){
							if(!equipInv.Contains(itemInAct))
								added.Add(itemInAct);
						}
						foreach(InventoryItemInstanceMock item in added){
							equipInv.Add(item);
						}
				/*	update all itemInst's isEquipped status	*/
				foreach(InventoryItemInstanceMock itemInst in poolInv){
					if(itemInst is BowInstanceMock)
						itemInst.isEquipped = itemInst == equippedBowInst;
					else if (itemInst is WearInstanceMock)
						itemInst.isEquipped = itemInst == equippedWearInst;
					else if(itemInst is CarriedGearInstanceMock)
						itemInst.isEquipped = equippedCarriedGears != null && equippedCarriedGears.Contains((CarriedGearInstanceMock)itemInst);
					else if(itemInst is PartsInstanceMock)
						itemInst.isEquipped = equippedParts != null && equippedParts.Contains((PartsInstanceMock)itemInst);
				}
				/*	set sbs equip states	*/
				foreach(SlotGroup sg in allSGs){
					foreach(Slottable sb in sg){
						if(sb!= null)
							sb.UpdateEquipState();
					}
				}
			}
			public void SortSG(SlotGroup sg, SGSorter sorter){
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
				foreach(SlotGroup sg in focusedSGs){
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, null, sg);
					TransactionResult tr = new TransactionResult(null, sg, ta);
					transactionResults.AddTransactionResult(tr);
					if(ta is RevertTransaction)
						sg.DefocusSelf();
					else
						sg.FocusSelf();
					foreach(Slottable sb in sg){
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
				SlotSystemTransaction ta = TransactionResults.GetTransaction(hoveredSB, hoveredSG);
				SetTargetSB(ta.targetSB);
				SetSG1(ta.sg1);
				SetSG2(ta.sg2);
				SetTransaction(ta);
			}
			public SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotGroup targetSG, Slottable targetSB){
				return AbsSlotSystemTransaction.GetTransaction(pickedSB, targetSB, targetSG);
			}
			public void ChangeEquippableCGearsCount(int i, SlotGroup targetSG){
				if(!targetSG.isExpandable){
					if(targetSG.CurSelState == SlotGroup.FocusedState ||
						targetSG.CurSelState == SlotGroup.DefocusedState){
							equipInv.SetEquippableCGearsCount(i);
							targetSG.InitializeItems();
							UpdateEquipStatesOnAll();
							ResetAndFocus();
						}
				}else{
					throw new System.InvalidOperationException("SlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable");
				}
			}
			public void MarkEquippedInPool(InventoryItemInstanceMock item, bool equipped){
				foreach(InventoryItemInstanceMock itemInInv in poolInv){
					if(itemInInv == item)
						itemInInv.isEquipped = equipped;
				}
			}
			public void SetEquippedOnAllSBs(InventoryItemInstanceMock item, bool equipped){
				if(equipped)
					rootPage.PerformInHierarchy(Equip, item);
				else
					rootPage.PerformInHierarchy(Unequip, item);
			}
			public void Equip(SlotSystemElement ele, object obj){
				if(ele is Slottable){
					InventoryItemInstanceMock item = (InventoryItemInstanceMock)obj;
					Slottable sb = (Slottable)ele;
					/*	assume all sbs are properly set in slottables, not int newSBs	*/
					if(sb.itemInst == item){
						if(sb.sg.isFocusedInBundle){/*	focused sgp or sge	*/
							if(sb.newSlotID != -1)/*	not being removed	*/
								sb.SetEqpState(Slottable.EquippedState);
						}else if(sb.sg.isPool){/*	defocused sgp	*/
							sb.SetEqpState(null);
							sb.SetEqpState(Slottable.EquippedState);
						}
					}
				}
			}
			public void Unequip(SlotSystemElement ele, object obj){
				if(ele is Slottable){
					InventoryItemInstanceMock item = (InventoryItemInstanceMock)obj;
					Slottable sb = (Slottable)ele;
					/*	assume all sbs are properly set in slottables, not int newSBs	*/
					if(sb.itemInst == item){
						if(sb.sg.isFocusedInBundle){
							if(sb.slotID != -1)/*	not being added	*/
								sb.SetEqpState(Slottable.UnequippedState);
						}else if(sb.sg.isPool){/*	defocused sgp	*/
							sb.SetEqpState(null);
							sb.SetEqpState(Slottable.UnequippedState);
						}
					}
				}
			}
	}
}
