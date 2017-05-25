﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace SlotSystem{
	public class SlotSystemClasses{
	}
		/*	test classes
		*/
			public class IEnumeratorMock{}
			public class PointerEventDataMock{
				public GameObject pointerDrag;
			}
	/*	SGM classes
	*/
		/*	transaction
		*/
			public interface SlotSystemTransaction{
				void Indicate();
				void Execute();
				void OnComplete();
			}
			public class RevertTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				SlotGroupManager sgm;
				public RevertTransaction(Slottable sb){
					this.pickedSB = sb;
					this.sgm = sb.SGM;
				}
				public void Indicate(){
					//implement revert indication here
				}
				public void Execute(){
					//implement reverting here
					SlotGroup sg = sgm.GetSlotGroup(pickedSB);
					Slot slot = sg.GetSlot(pickedSB);
					pickedSB.MoveDraggedIcon(sg, slot);
					pickedSB.SetState(Slottable.UnpickingState);
					SGMRevertTransactionProcess revertProcess = new SGMRevertTransactionProcess(sgm, sgm.WaitForRevertDone);
					sgm.SetAndRun(revertProcess);
					sgm.SetState(SlotGroupManager.PerformingTransactionState);
				}
				public void OnComplete(){
					if(pickedSB.IsEquipped)
						pickedSB.SetState(Slottable.EquippedAndDeselectedState);
					else
						pickedSB.SetState(Slottable.FocusedState);
					pickedSB.PickedAmount = 0;
					sgm.ClearAndReset();
				}
			}
			public class ReorderTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				SlotGroup sg;
				SlotGroupManager sgm;
				public ReorderTransaction(Slottable picked, Slottable selected){
					this.pickedSB = picked;
					this.selectedSB = selected;
					this.sgm = picked.SGM;
					this.sg = sgm.GetSlotGroup(pickedSB);
				}
				public void Indicate(){}
				public void Execute(){
					SGMReorderProcess process = new SGMReorderProcess(sgm, sgm.WaitForReorderDone);
					sgm.SetAndRun(process);
					
					/*	hijack orderedSBs here */
						sg.SetReorderedSBs(pickedSB, selectedSB);

					sgm.SetState(SlotGroupManager.PerformingTransactionState);
					sg.TransactionUpdateV2(null, null);

					Slot slot = sg.GetSlot(selectedSB);
					pickedSB.MoveDraggedIcon(sg, slot);

					pickedSB.SetState(Slottable.MovingState);
					// selectedSB.SetState(Slottable.MovingState);
				}
				public void OnComplete(){
					sgm.DestroyDraggedIcon();
					sgm.ClearAndReset();
				}
			}
			public class StackTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				public StackTransaction(Slottable picked, Slottable selected){
					this.pickedSB = picked;
					this.selectedSB = selected;
				}
				public void Indicate(){}
				public void Execute(){
					pickedSB.SGM.ClearAndReset();
				}
				public void OnComplete(){}
			}
			public class SwapTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup origSG;
				Slottable selectedSB;
				SlotGroup selectedSG;
				SlotGroupManager sgm;
				public SwapTransaction(Slottable picked, Slottable selected){
					this.pickedSB = picked;
					this.selectedSB = selected;
					this.sgm = picked.SGM;
					this.origSG = sgm.GetSlotGroup(picked);
					this.selectedSG = sgm.GetSlotGroup(selected);
				}
				public void Indicate(){}
				public void CheckFields(out Slottable pickedSB, out SlotGroup origSG, out Slottable selectedSB, out SlotGroup selectedSG){
					pickedSB = this.pickedSB;
					origSG = this.origSG;
					selectedSB = this.selectedSB;
					selectedSG = this.selectedSG;
				}
				public void Execute(){
					SGMSwapTransactionProcess process = new SGMSwapTransactionProcess(sgm, sgm.WaitForSwapDone);
					sgm.SetAndRun(process);
					sgm.SetState(SlotGroupManager.PerformingTransactionState);
					
					origSG.TransactionUpdateV2(selectedSB, pickedSB);
					selectedSG.TransactionUpdateV2(pickedSB, selectedSB);

					Slot selectedSBSlot = selectedSG.GetSlot(selectedSB);
					Slot pickedSBSlot = origSG.GetSlot(pickedSB);

					selectedSB.MoveDraggedIcon(origSG, pickedSBSlot);
					pickedSB.MoveDraggedIcon(selectedSG, selectedSBSlot);
					
					selectedSB.SetState(Slottable.MovingState);
					pickedSB.SetState(Slottable.MovingState);
				}
				public void OnComplete(){
					sgm.DestroyDraggedIcon();
					sgm.DestroyRemovedSB();
					sgm.UpdateEquipStatus();
					sgm.ClearAndReset();
				}
			}
			public class FillTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup origSG;
				SlotGroup selectedSG;
				SlotGroupManager sgm;
				public FillTransaction(Slottable picked, SlotGroup selSG){
					this.pickedSB = picked;
					this.selectedSG = selSG;
					this.sgm = picked.SGM;
					this.origSG = sgm.GetSlotGroup(picked
					);
				}
				public void Indicate(){}
				public void Execute(){

					Slot slot = selectedSG.GetNextEmptySlot();
					/*	precondition for GetNext...
							1. it has an empty slot OR isExpandable
							2. does not have the same stackable item
					*/
					

					origSG.TransactionUpdate(null/*added*/, pickedSB/*removed*/);
					selectedSG.TransactionUpdate(pickedSB, null);
					/*	perform focusing of scroller if something is to be added
						perform Inventory update, sorting ,filtering and updating slots and sbs, and moving of all the elements it contains
					*/
					pickedSB.MoveDraggedIcon(selectedSG, slot);
					SGMFillTransactionProcess fillProcess = new SGMFillTransactionProcess(sgm, sgm.WaitForFillDone);
					sgm.SetAndRun(fillProcess);
					sgm.SetState(SlotGroupManager.PerformingTransactionState);
				}
				public void OnComplete(){
					sgm.DestroyDraggedIcon();
					//Update equipstatus
					// origSG.UpdateEquipStatus();
					// selectedSG.UpdateEquipStatus();
					sgm.UpdateEquipStatus();
					sgm.ClearAndReset();//Focus should take care of clearing the processes
				}
			}
			public class UnequipTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup selectedSG;
				SlotGroup origSG;
				SlotGroupManager sgm;
				public UnequipTransaction(Slottable picked, SlotGroup selSG){
					this.pickedSB = picked;
					this.selectedSG = selSG;
					this.sgm = picked.SGM;
					this.origSG = sgm.GetSlotGroup(picked);
				}
				public void Indicate(){}
				public void Execute(){

					SGMUnequipTransactionProcess process = new SGMUnequipTransactionProcess(sgm, sgm.WaitForUnequipDone);
					sgm.SetAndRun(process);
					sgm.SetState(SlotGroupManager.PerformingTransactionState);

					// origSG.TransactionUpdate(null, pickedSB);
					// selectedSG.TransactionUpdate(pickedSB, null);
					origSG.TransactionUpdateV2(null, pickedSB);
					selectedSG.TransactionUpdateV2(pickedSB, null);

					Slot slot = selectedSG.GetSlot(selectedSG.GetSlottable((InventoryItemInstanceMock)pickedSB.Item));
					pickedSB.MoveDraggedIcon(selectedSG, slot);
					// pickedSB.SetState(Slottable.RemovingState);
					pickedSB.SetState(Slottable.MovingState);
				}
				public void OnComplete(){
					sgm.DestroyDraggedIcon();
					sgm.DestroyRemovedSB();
					/*	TransactionUpdate HIDES (not removes) the pickedSB the moment the transaction is 	executed (only dragged icon persists)
						performing the equip state update checks all the equipped sbs in pool to see if there's matching sb in one of equip egs, unequip if there's none
					*/	
					// origSG.RemoveSB(pickedSB);
					sgm.UpdateEquipStatus();
					sgm.ClearAndReset();
				}
			}
			public class FillEquipTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup selectedSG;
				SlotGroup origSG;
				SlotGroupManager sgm;
				public FillEquipTransaction(Slottable picked, SlotGroup selSG){
					this.pickedSB = picked;
					this.selectedSG = selSG;
					this.sgm = picked.SGM;
					this.origSG = sgm.GetSlotGroup(picked);
				}
				public void Indicate(){}
				public void Execute(){
					Slot slot = selectedSG.GetNextEmptySlot();

					SGMFillEquipTransactionProcess process = new SGMFillEquipTransactionProcess(sgm, sgm.WaitForFillEquipDone);
					sgm.SetAndRun(process);
					sgm.SetState(SlotGroupManager.PerformingTransactionState);

					origSG.TransactionUpdateV2(null, pickedSB);
					selectedSG.TransactionUpdateV2(pickedSB, null);

					pickedSB.MoveDraggedIcon(selectedSG, slot);
					pickedSB.SetState(Slottable.MovingState);
				}
				public void OnComplete(){
					sgm.DestroyDraggedIcon();
					sgm.UpdateEquipStatus();
					sgm.ClearAndReset();
				}
			}
			public class SortTransaction: SlotSystemTransaction{
				SlotGroup m_sg;
				SGSorter m_sorter;
				SlotGroupManager sgm;
				public SortTransaction(SlotGroup sg, SGSorter sorter){
					m_sg = sg;
					m_sorter = sorter;
					sgm = sg.SGM;
				}
				public void Indicate(){}
				public void Execute(){
					SGMSortingProcess process = new SGMSortingProcess(sgm, sgm.WaitForSortingDone);
					sgm.SetAndRun(process);
					sgm.SetState(SlotGroupManager.PerformingTransactionState);
					// m_sg.TransactionUpdate(null, null);
					m_sg.TransactionUpdateV2(null, null);
				}
				public void OnComplete(){
					//enable imput
					sgm.ClearAndReset();
				}
			}
		/*	commands
		*/
			public interface SGMCommand{
				void Execute(SlotGroupManager sgm);
			}
			public class UpdateTransactionCommand: SGMCommand{
				
				public void Execute(SlotGroupManager sgm){
					Slottable pickedSB = sgm.PickedSB;
					Slottable selectedSB = sgm.SelectedSB;
					SlotGroup selectedSG = sgm.SelectedSG;
					SlotGroup origSG = sgm.GetSlotGroup(pickedSB);
					if(pickedSB != null){
						if(selectedSB == null){
							if(selectedSG == null || selectedSG == origSG || !origSG.IsShrinkable){
								SlotSystemTransaction revertTs = new RevertTransaction(pickedSB);
								sgm.SetTransaction(revertTs);
							}else{
								/*	selectedSG != null && != origSG
									there's at least one vacant slot OR there's a sb of a same stackable item
								*/
								if(selectedSG.HasItem((InventoryItemInstanceMock)pickedSB.Item)){
									if(sgm.RootPage.PoolBundle.ContainsElement(selectedSG)){
										UnequipTransaction unequipTs = new UnequipTransaction(pickedSB, selectedSG);
										sgm.SetTransaction(unequipTs);
									}else{
										StackTransaction stackTs = new StackTransaction(pickedSB, selectedSB);
										sgm.SetTransaction(stackTs);
									}
								}else{
									EquipmentSet focusedEquipSet = (EquipmentSet)sgm.RootPage.EquipBundle.GetFocusedBundleElement();
									if(focusedEquipSet.ContainsElement(selectedSG)){
										if(selectedSG.Filter is SGCarriedGearFilter){
											FillEquipTransaction fillEquipTs = new FillEquipTransaction(pickedSB, selectedSG);
											sgm.SetTransaction(fillEquipTs);
										}else{
											sgm.SetSelectedSB(selectedSG.Slots[0].Sb);
											SwapTransaction swapTs = new SwapTransaction(pickedSB, selectedSG.Slots[0].Sb);
											sgm.SetTransaction(swapTs);
										}
									}else{
										FillTransaction fillTs = new FillTransaction(pickedSB, selectedSG);
										sgm.SetTransaction(fillTs);
									}
								}
							}

						}else{// selectedSB != null
							if(pickedSB == selectedSB){
								SlotSystemTransaction revertTs = new RevertTransaction(pickedSB);
								sgm.SetTransaction(revertTs);
							}else{
								if(sgm.GetSlotGroup(selectedSB) == sgm.GetSlotGroup(pickedSB)){
									if(!sgm.GetSlotGroup(pickedSB).IsAutoSort){
										SlotSystemTransaction reorderTs = new ReorderTransaction(pickedSB, selectedSB);
										sgm.SetTransaction(reorderTs);
									}
								}else{// different SGs
									if(pickedSB.Item == selectedSB.Item){
										if(pickedSB.IsEquipped){
											UnequipTransaction unequipTs = new UnequipTransaction(pickedSB, sgm.GetSlotGroup(selectedSB));
											sgm.SetTransaction(unequipTs);
										}else{
											StackTransaction stackTs = new StackTransaction(pickedSB, selectedSB);
											sgm.SetTransaction(stackTs);
										}
									}else{
										SwapTransaction swapTs = new SwapTransaction(pickedSB, selectedSB);
										sgm.SetTransaction(swapTs);
									}
								}
							}
						}
					}
				}
			}
			/*	use this */
			public class PostPickFilterV3Command: SGMCommand{
				public void Execute(SlotGroupManager sgm){
					Slottable pickedSb = sgm.PickedSB;
					SlotGroup origSG = sgm.SelectedSG;
					SlotSystemBundle poolBundle = sgm.RootPage.PoolBundle;
					SlotSystemBundle equipBundle = sgm.RootPage.EquipBundle;

					if(poolBundle.ContainsElement(origSG)){// pickedSb in the pool
						foreach(Slot slot in origSG.Slots){
							if(slot.Sb != null && slot.Sb != pickedSb){
								if(origSG.IsAutoSort){
									// if(slot.Sb.IsEquipped)
									// 	slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
									// else
									// 	slot.Sb.SetState(Slottable.DefocusedState);
									slot.Sb.Defocus();
								}else{
									// if(slot.Sb.IsEquipped)
									// 	slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
									// else
									// 	slot.Sb.SetState(Slottable.FocusedState);
									slot.Sb.Focus();
								}
							}
						}
						EquipmentSet focusedEquipmentSet = (EquipmentSet)equipBundle.GetFocusedBundleElement();
						foreach(SlotSystemElement ele in focusedEquipmentSet.Elements){
							SlotGroup sg = (SlotGroup)ele;
							if(sg.AcceptsFilter(pickedSb)){
								if(sg.Filter is SGCarriedGearFilter && sg.GetNextEmptySlot()==null)
									sg.SetState(SlotGroup.DefocusedState);
								else
									sg.SetState(SlotGroup.FocusedState);
								foreach(Slot slot in sg.Slots){
									if(slot.Sb != null){
										// if(slot.Sb.IsEquipped){
										// 	slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
										// }else{
										// 	slot.Sb.SetState(Slottable.FocusedState);
										// }
										slot.Sb.Focus();
									}
								}
							}else{// sg filtered out
								sg.SetState(SlotGroup.DefocusedState);
								foreach(Slot slot in sg.Slots){
									if(slot.Sb != null){
										// if(slot.Sb.IsEquipped)
										// 	slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
										// else
										// 	slot.Sb.SetState(Slottable.DefocusedState);
										slot.Sb.Defocus();
									}
								}
							}
						}
					}else{// if pickedSB.IsEquipped
						
						foreach(SlotSystemElement ele in equipBundle.Elements){
							EquipmentSet equipSet = (EquipmentSet)ele;
							if(equipSet.ContainsElement(origSG)){
								foreach(SlotSystemElement nestedEle in equipSet.Elements){
									SlotGroup sg = (SlotGroup)nestedEle;
									if(sg != origSG){
										if(sg.AcceptsFilter(pickedSb)){
											sg.SetState(SlotGroup.FocusedState);
											foreach(Slot slot in sg.Slots){
												if(slot.Sb != null)
													// slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
													slot.Sb.Focus();
											}
										}else{
											sg.SetState(SlotGroup.DefocusedState);
											foreach(Slot slot in sg.Slots){
												if(slot.Sb != null)
													// slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
													slot.Sb.Defocus();
											}
										}
									}else{// sg == origSG, the state is Selected
										foreach(Slot slot in sg.Slots){
											if(slot.Sb != null){
												if(slot.Sb != pickedSb){
													if(!sg.IsAutoSort)
														slot.Sb.Focus();
													else
														slot.Sb.Defocus();
												}
											}
										}
									}
								}
							}
						}
						SlotGroup focusedPoolSG = sgm.GetFocusedPoolSG();
						foreach(Slot slot in focusedPoolSG.Slots){
							if(slot.Sb != null){
								if(SlotSystem.Utility.HaveCommonItemFamily(slot.Sb, pickedSb)){
									if(object.ReferenceEquals(slot.Sb.Item, pickedSb.Item)){
										if(origSG.IsShrinkable)// unequip
											// slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
											slot.Sb.Focus();
										else
											// slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
											slot.Sb.Defocus();
									}
									else{
										if(slot.Sb.IsEquipped)
											// slot.Sb.SetState(/*Slottable.EquippedAndDeselectedState*/Slottable.EquippedAndDefocusedState);
											slot.Sb.Defocus();
										else
											// slot.Sb.SetState(Slottable.FocusedState);
											slot.Sb.Focus();
									}
								}else{	// different item family
									// if(slot.Sb.IsEquipped)
									// 	slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
									// else
									// 	slot.Sb.SetState(Slottable.DefocusedState);
									slot.Sb.Defocus();
								}
							}
						}
					}
				}
			}
			public class PostPickFilterCommand: SGMCommand{
				public void Execute(SlotGroupManager sgm){
					if(sgm.PickedSB != null){
						
						if(sgm.PickedSB.Item is BowInstanceMock){
							foreach(SlotGroup sg in sgm.SlotGroups){
								if(sg.CurState != SlotGroup.SelectedState){
									if(sg.CurState == SlotGroup.FocusedState){
										if(!(sg.Filter is SGNullFilter) && !(sg.Filter is SGBowFilter))
											sg.SetState(SlotGroup.DefocusedState);
									}
								}
							}
						}else if(sgm.PickedSB.Item is WearInstanceMock){
							foreach(SlotGroup sg in sgm.SlotGroups){
								if(sg.CurState != SlotGroup.SelectedState){
									if(sg.CurState == SlotGroup.FocusedState){
										if(!(sg.Filter is SGNullFilter) && !(sg.Filter is SGWearFilter))
											sg.SetState(SlotGroup.DefocusedState);
									}
								}
							}
						}else if(sgm.PickedSB.Item is PartsInstanceMock){
							foreach(SlotGroup sg in sgm.SlotGroups){
								if(sg.CurState != SlotGroup.SelectedState){
									if(sg.CurState == SlotGroup.FocusedState){
										if(!(sg.Filter is SGNullFilter) && !(sg.Filter is SGPartsFilter))
											sg.SetState(SlotGroup.DefocusedState);
									}
								}
							}
						}
						foreach(SlotGroup sg in sgm.SlotGroups){
							if(sg.CurState == SlotGroup.FocusedState || sg.CurState == SlotGroup.SelectedState){
								// if(sg.Filter is SGNullFilter)
									// sg.UpdateSbState();
							}

						}
					}
				}
			}
			
			public class PostPickFilterV2Command: SGMCommand{
				public void Execute(SlotGroupManager sgm){
					Slottable pickedSb = sgm.PickedSB;
					SlotGroup origSG = sgm.SelectedSG;
					SlotSystemBundle poolBundle = sgm.RootPage.PoolBundle;
					SlotSystemBundle equipBundle = sgm.RootPage.EquipBundle;

					if(poolBundle.ContainsElement(origSG)){
						foreach(Slot slot in origSG.Slots){
							if(slot.Sb != null && slot.Sb != pickedSb){
								if(origSG.IsAutoSort){
									if(slot.Sb.IsEquipped)
										slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
									else
										slot.Sb.SetState(Slottable.DefocusedState);
								}else{
									if(slot.Sb.IsEquipped)
										slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
									else
										slot.Sb.SetState(Slottable.FocusedState);
								}
							}
						}
					}else{
						foreach(SlotSystemElement ele in equipBundle.Elements){
							EquipmentSet equipSet = (EquipmentSet)ele;
							if(equipSet.ContainsElement(origSG)){
								foreach(SlotSystemElement nestedEle in equipSet.Elements){
									SlotGroup sg = (SlotGroup)nestedEle;
									if(sg != origSG){
										if(sg.AcceptsFilter(pickedSb)){
											sg.SetState(SlotGroup.FocusedState);
											sg.Slots[0].Sb.SetState(Slottable.EquippedAndDeselectedState);
										}else{
											sg.SetState(SlotGroup.DefocusedState);
											sg.Slots[0].Sb.SetState(Slottable.EquippedAndDefocusedState);
										}
									}
								}
							}
						}
					}
				}
			}
			
		/*	process
		*/
			public interface SGMProcess{
				bool IsRunning{get;}
				bool IsExpired{get;}
				System.Func<IEnumeratorMock> CoroutineMock{set;}
				SlotGroupManager SGM{set;}
				void Start();
				void Stop();
				void Expire();
			}
			public abstract class AbsSGMProcess: SGMProcess{
				
				System.Func<IEnumeratorMock> m_coroutineMock;
				public System.Func<IEnumeratorMock> CoroutineMock{
					get{return m_coroutineMock;}
					set{m_coroutineMock = value;}
				}
				SlotGroupManager m_sgm;
				public SlotGroupManager SGM{
					get{return m_sgm;}
					set{m_sgm = value;}
				}
				bool m_isRunning;
				public bool IsRunning{get{return m_isRunning;}}
				bool m_isExpired;
				public bool IsExpired{get{return m_isExpired;}}
				public virtual void Start(){
					//call StartCoroutine(m_coroutine);
					m_isRunning = true;
					m_isExpired = false;
					m_coroutineMock();
				}
				public void Stop(){
					//call StopCoroutine(m_coroutine);
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = false;
					}
				}
				public virtual void Expire(){
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = true;
					}
				}
				
			}
			public class SGMProbingStateProcess: AbsSGMProcess{
				public SGMProbingStateProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SGMRevertTransactionProcess: AbsSGMProcess{
				public SGMRevertTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Start(){
					SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
					SGM.SetSelectedSBDoneTransaction(null, true);
					SGM.SetOrigSGDoneTransaction(null, true);
					SGM.SetSelectedSGDoneTransaction(null, true);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.CompleteAllTransaction();
				}
			}
			public class SGMFillTransactionProcess: AbsSGMProcess{
				public SGMFillTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Start(){
					SGM.SetPickedSBDoneTransaction(SGM.PickedSB,false);
					SGM.SetSelectedSBDoneTransaction(null,true);
					SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB), false);
					SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.CompleteAllTransaction();
				}
			}
			public class SGMFillEquipTransactionProcess: AbsSGMProcess{
				public SGMFillEquipTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Start(){
					SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
					SGM.SetSelectedSBDoneTransaction(null, true);
					SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB),false);
					SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.CompleteAllTransaction();
				}
			}
			public class SGMUnequipTransactionProcess: AbsSGMProcess{
				public SGMUnequipTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Start(){
					SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
					SGM.SetSelectedSBDoneTransaction(null, true);
					SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB),false);
					SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.CompleteAllTransaction();
				}
			}
			public class SGMSwapTransactionProcess: AbsSGMProcess{
				public SGMSwapTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Start(){
					SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
					SGM.SetSelectedSBDoneTransaction(SGM.SelectedSB, false);
					SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB),false);
					SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
					
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.CompleteAllTransaction();
				}
			}
			public class SGMSortingProcess: AbsSGMProcess{
				public SGMSortingProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Start(){
					SGM.SetPickedSBDoneTransaction(null, true);
					SGM.SetSelectedSBDoneTransaction(null, true);
					SGM.SetOrigSGDoneTransaction(null ,true);
					SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.CompleteAllTransaction();
				}
			}
			public class SGMReorderProcess: AbsSGMProcess{
				public SGMReorderProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Start(){
					SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
					SGM.SetSelectedSBDoneTransaction(null, true);
					SGM.SetOrigSGDoneTransaction(null ,true);
					SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.CompleteAllTransaction();
				}

			}
		/*	state
		*/
			public interface SGMState{
				void EnterState(SlotGroupManager sgm);	
				void ExitState(SlotGroupManager sgm);	
			}
			public class SGMDeactivatedState: SGMState{
				public void EnterState(SlotGroupManager sgm){
					sgm.SetAndRun(null);
				}
				public void ExitState(SlotGroupManager sgm){}
			}
			public class SGMDefocusedState: SGMState{
				public void EnterState(SlotGroupManager sgm){
					sgm.SetAndRun(null);
					sgm.RootPage.Defocus();
				}
				public void ExitState(SlotGroupManager sgm){}
			}
			public class SGMFocusedState: SGMState{
				public void EnterState(SlotGroupManager sgm){
					sgm.SetAndRun(null);
					sgm.RootPage.Focus();
				}
				public void ExitState(SlotGroupManager sgm){}

			}
			public class SGMProbingState: SGMState{
				public void EnterState(SlotGroupManager sgm){
					SGMProcess probingStateProcess = new SGMProbingStateProcess(sgm, sgm.ProbingStateCoroutine);
					sgm.SetAndRun(probingStateProcess);
				}
				public void ExitState(SlotGroupManager sgm){
				}
			}
			public class SGMPerformingTransactionState: SGMState{
				public void EnterState(SlotGroupManager sgm){
				}
				public void ExitState(SlotGroupManager sgm){
				}
			}
	/*	SlotGroup Classes
	*/
		/*	states
		*/
			public interface SlotGroupState{
				void EnterState(SlotGroup sg);
				void ExitState(SlotGroup sg);
				void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData);
				void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData);
				void Focus(SlotGroup sg);
				void Defocus(SlotGroup sg);
			}
			public class SGDeactivatedState : SlotGroupState{
				public void EnterState(SlotGroup sg){
					sg.SetAndRun(null);
				}
				public void ExitState(SlotGroup sg){
				}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void Focus(SlotGroup sg){
					sg.SetState(SlotGroup.FocusedState);
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					sg.SetState(SlotGroup.DefocusedState);
					sg.DefocusSBs();
				}
			}
			public class SGDefocusedState: SlotGroupState{
				public void EnterState(SlotGroup sg){
					SGProcess process = null;

					if(sg.PrevState == SlotGroup.DeactivatedState)
						process = new SGInstantGreyoutProcess(sg, sg.InstantGreyoutCoroutine);
					else if(sg.PrevState == SlotGroup.FocusedState)
						process = new SGGreyoutProcess(sg, sg.GreyoutCoroutine);
					else if(sg.PrevState == SlotGroup.SelectedState)
						process = new SGDehighlightProcess(sg, sg.DehighlightCoroutine);
					if(process != null)
						sg.SetAndRun(process);

				}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void Focus(SlotGroup sg){
					sg.SetState(SlotGroup.FocusedState);
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					sg.SetState(SlotGroup.DefocusedState);
					sg.DefocusSBs();
				}
			}
			public class SGFocusedState: SlotGroupState{
				public void EnterState(SlotGroup sg){
					SGProcess process = null;
					if(sg.PrevState == SlotGroup.DeactivatedState)
						process = new SGInstantGreyinProcess(sg, sg.InstantGreyinCoroutine);
					else if(sg.PrevState == SlotGroup.DefocusedState)
						process = new SGGreyinProcess(sg, sg.GreyinCoroutine);
					else if(sg.PrevState == SlotGroup.SelectedState)
						process = new SGDehighlightProcess(sg, sg.DehighlightCoroutine);
					else if(sg.PrevState == SlotGroup.PerformingTransactionState){
						process = null;
						sg.SetAndRun(process);
					}
					if(process != null)
						sg.SetAndRun(process);
				}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){
					sg.SGM.SetSelectedSG(sg);
					sg.SetState(SlotGroup.SelectedState);
				}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
					
				}
				public void Focus(SlotGroup sg){
					sg.SetState(SlotGroup.FocusedState);
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					sg.SetState(SlotGroup.DefocusedState);
					sg.DefocusSBs();
				}
			}
			public class SGSelectedState: SlotGroupState{
				public void EnterState(SlotGroup sg){
					SGProcess process = null;
					if(sg.PrevState == SlotGroup.FocusedState)
						process = new SGHighlightProcess(sg, sg.HighlightCoroutine);
					if(process != null)
						sg.SetAndRun(process);
				}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
					if(sg.SGM.SelectedSG == sg){
						sg.SGM.SetSelectedSG(null);
					}
					sg.SetState(SlotGroup.FocusedState);
				}
				public void Focus(SlotGroup sg){
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					sg.SetState(SlotGroup.DefocusedState);
					sg.DefocusSBs();
				}
			}
			public class SGPerformingTransactionState: SlotGroupState{
				public void EnterState(SlotGroup sg){
					SGProcess process = new SGUpdateTransactionProcess(sg, sg.UpdateTransactionCoroutine);
					
					sg.SetAndRun(process);
				}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void Focus(SlotGroup sg){
					sg.SetState(SlotGroup.FocusedState);
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
				}
			}
			public class SGSortingState: SlotGroupState{
				// SlotGroupState prevState;
				public void EnterState(SlotGroup sg){
					// prevState = sg.PrevState;
					SGProcess process = new SGSortingProcess(sg, sg.WaitForAllSlotMovementsDone);
					sg.SetAndRun(process);
					/*	implement SlotMovements creation and stuff in the process
					*/
				}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void Focus(SlotGroup sg){
					sg.SetState(SlotGroup.FocusedState);
					sg.FocusSBs();
					// sg.SetState(prevState);
				}
				public void Defocus(SlotGroup sg){
				}
			}
		/*	process
		*/
			public interface SGProcess{
				bool IsRunning{get;}
				bool IsExpired{get;}
				System.Func<IEnumeratorMock> CoroutineMock{set;}
				SlotGroup SG{set;}
				void Start();
				void Stop();
				void Expire();
			}
			public abstract class AbsSGProcess: SGProcess{
				System.Func<IEnumeratorMock> m_coroutineMock;
				public System.Func<IEnumeratorMock> CoroutineMock{
					get{return m_coroutineMock;}
					set{m_coroutineMock = value;}
				}
				SlotGroup m_sg;
				public SlotGroup SG{
					get{return m_sg;}
					set{m_sg = value;}
				}
				bool m_isRunning;
				public bool IsRunning{get{return m_isRunning;}}
				bool m_isExpired;
				public bool IsExpired{get{return m_isExpired;}}
				public virtual void Start(){
					m_isRunning = true;
					m_isExpired = false;
					m_coroutineMock();
				}
				public virtual void Stop(){
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = false;
					}
				}
				public virtual void Expire(){
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = true;
					}
				}
			}
			public class SGFocusedProcess: AbsSGProcess{
				public SGFocusedProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
					}
			}
			public class SGGreyinProcess: AbsSGProcess{
				public SGGreyinProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
					}
			}
			public class SGGreyoutProcess: AbsSGProcess{
				public SGGreyoutProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
					}
			}
			public class SGHighlightProcess: AbsSGProcess{
				public SGHighlightProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
					}
			}
			public class SGDehighlightProcess: AbsSGProcess{
				public SGDehighlightProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
					}
			}
			public class SGInstantGreyoutProcess: AbsSGProcess{
				public SGInstantGreyoutProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
					}
			}
			public class SGInstantGreyinProcess: AbsSGProcess{
				public SGInstantGreyinProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
					}
			}
			public class SlotMovement{
				SlotGroup m_sg;
				Slottable m_sb;
				public Slottable SB{
					get{return m_sb;}
				}
				int m_curSlotID;
				int m_newSlotID;
				public SlotMovement(SlotGroup sg, Slottable sb, int curSlotID, int newSlotID){
					m_sg = sg;
					m_sb = sb;
					m_curSlotID = curSlotID;
					m_newSlotID = newSlotID;
					m_sg.AddSlotMovement(this);
				}
				bool m_completed = false;
				public bool Completed{
					get{return m_completed;}
				}
				public void Execute(){
					if(m_curSlotID == m_newSlotID)
						m_completed = true;
					else
						m_completed = false;
				}
				public void Complete(){
					m_completed = true;
					IEnumeratorMock tryInvoke = m_sg.WaitForAllSlotMovementsDone();
				}
				public void GetIndex(out int curId, out int newId){
					curId = m_curSlotID;
					newId = m_newSlotID;
				}
			}
			public class SGUpdateTransactionProcess: AbsSGProcess{
				public SGUpdateTransactionProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				
				/*	overridden functions
				*/
					public override void Start(){
						base.Start();
						
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						base.Expire();
						SG.SGM.CompleteTransactionOnSG(SG);
					}
			}
			public class SGSortingProcess: AbsSGProcess{
				public SGSortingProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				/*	overridden functions
				*/
					public override void Start(){
						// base.Start();
						/*	implement SlotMovements here
								Create SlotMovement instance for every slottables
									1. Get current Slot index
									2. Sort SG
									3. Get new Slot index
									4. pass 1, 3, and the slottable into it
									5. and execute it
									6. store each SlotMovement into sg's slotMovements list
						*/
						List<Slottable> newSlotOrderList = SG.OrderedSbs();
						for(int i = 0; i < SG.Slots.Count; i++){
							if(SG.Slots[i].Sb != null){
								Slottable sb = SG.Slots[i].Sb;
								int curSlotID = sb.SlotID;
								int newSlotID = newSlotOrderList.IndexOf(sb);
								SlotMovement slotMovement = new SlotMovement(SG, sb, curSlotID, newSlotID);
								// slotMovement.Execute();
							}
						}
						foreach(SlotMovement sm in SG.SlotMovements){
							sm.Execute();
						}
						SG.WaitForAllSlotMovementsDone();
						base.Start();
					}
					public override void Stop(){
						base.Stop();
					}
					public override void Expire(){
						SG.InstantSort();
						base.Expire();
						SG.SGM.CompleteTransactionOnSG(SG);
					}
			}

		/*	commands
		*/
			public interface SlotGroupCommand{
				void Execute(SlotGroup Sg);
			}

			public class SGWakeupCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					if(sg.Scroller == null){
						sg.SetState(SlotGroup.FocusedState);
					}else{
						if(sg == sg.SGM.InitiallyFocusedSG)
							sg.Focus();
					}
					// sg.UpdateSbState();
				}
			}
			public class UpdateSbStateCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					SlotGroupManager sgm = sg.SGM;
					if(sg.CurState == SlotGroup.DefocusedState){
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null){
								InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
								if(invItem.IsEquipped){
									slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
								}else
									slot.Sb.SetState(Slottable.DefocusedState);
							}
						}
					}else if(sg.CurState == SlotGroup.FocusedState){
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null){
								if(slot.Sb != sgm.PickedSB){

									InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
									if(invItem.IsEquipped){
										if(sgm.PickedSB == null)
											slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
										else if(slot.Sb.Item is BowInstanceMock){
											if(sgm.PickedSB.Item is BowInstanceMock){
												if(object.ReferenceEquals(sgm.PickedSB.Item, slot.Sb.Item))
													slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
												else
													slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
											}
											else
												slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
										}else if(slot.Sb.Item is WearInstanceMock){
											if(sgm.PickedSB.Item is WearInstanceMock)
												if(object.ReferenceEquals(sgm.PickedSB.Item, slot.Sb.Item))
													slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
												else
													slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
											else
												slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
										}else
											slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
									}
									else{
										if(!(sg.Filter is SGPartsFilter) && (invItem is PartsInstanceMock))
											slot.Sb.SetState(Slottable.DefocusedState);
										else{
											if(sgm.PickedSB == null)
												slot.Sb.SetState(Slottable.FocusedState);
											else{
												if(invItem is BowInstanceMock){
													if(sgm.PickedSB.Item is BowInstanceMock)
														slot.Sb.SetState(Slottable.FocusedState);
													else
														slot.Sb.SetState(Slottable.DefocusedState);
												}else if(invItem is WearInstanceMock){
													if(sgm.PickedSB.Item is WearInstanceMock)
														slot.Sb.SetState(Slottable.FocusedState);
													else
														slot.Sb.SetState(Slottable.DefocusedState);
												}else if(invItem is PartsInstanceMock){
													if(sgm.PickedSB.Item is PartsInstanceMock)
														slot.Sb.SetState(Slottable.FocusedState);
													else
														slot.Sb.SetState(Slottable.DefocusedState);
												}else{
													slot.Sb.SetState(Slottable.DefocusedState);
												}
											}
										}
									}
								}
							}
						}
					}else if(sg.CurState == SlotGroup.SelectedState){
						//pickedSB != null;
						foreach(Slot slot in sg.Slots){
							if(slot.Sb != null){
								if(sgm.PickedSB != slot.Sb){
									BowInstanceMock equippedBow = sgm.GetEquippedBow();
									WearInstanceMock equippedWear = sgm.GetEquippedWear();
									if(sg.IsAutoSort){
										if(object.ReferenceEquals(slot.Sb.Item, equippedBow) || (object.ReferenceEquals(slot.Sb.Item, equippedWear)))
											slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
										else
											slot.Sb.SetState(Slottable.DefocusedState);
									}else{
										if(object.ReferenceEquals(slot.Sb.Item, equippedBow) || (object.ReferenceEquals(slot.Sb.Item, equippedWear)))
											slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
										else
											slot.Sb.SetState(Slottable.FocusedState);
									}
								}
							}
						}
					}
				}
			}
			public class UpdateSbStateCommandV2: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					/*	call Focus on all sbs?
					*/
				}
			}
			public class SGInitItemsCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.FilterItems();//setup Items list
					// sg.SortItems();//sort Items
					sg.CreateSlots();
					sg.CreateSlottables();
					if(sg.IsAutoSort)
						sg.InstantSort();
					// sg.UpdateEquipStatus();
				}
			}
			public class ConcCreateSlotsCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					List<Slot> newList = new List<Slot>();
					if(sg.Slots == null)
						sg.Slots = new List<Slot>();
					if(sg.IsExpandable){
						foreach(SlottableItem item in sg.FilteredItems){
							Slot slot = new Slot();
							slot.Position = Vector2.zero;
							newList.Add(slot);
						}
						sg.Slots = newList;
					}
				}
			}
			public class ConcCreateSbsCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					for(int i = 0; i < sg.FilteredItems.Count; i++){
						GameObject go = new GameObject("SlottablePrefab");
						Slottable sb = go.AddComponent<Slottable>();
						InventoryItemInstanceMock item = (InventoryItemInstanceMock)sg.FilteredItems[i];
						sb.Initialize(sg.SGM, true, item);
						sg.Slots[i].Sb = sb;
					}
				}
			}
			public class UpdateEquipStatusForPoolCommmand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null){
							if(slot.Sb.IsEquipped){
								if(slot.Sb.Item is BowInstanceMock){
									BowInstanceMock equippedBow = sg.SGM.GetEquippedBow();
									if(equippedBow != (BowInstanceMock)slot.Sb.Item)
										slot.Sb.Unequip();
								}else if(slot.Sb.Item is WearInstanceMock){
									WearInstanceMock equippedWear = sg.SGM.GetEquippedWear();
									if(equippedWear != (WearInstanceMock)slot.Sb.Item)
										slot.Sb.Unequip();
								}else if(slot.Sb.Item is CarriedGearInstanceMock){
									List<CarriedGearInstanceMock> cGears = sg.SGM.GetEquippedCarriedGears();
									if(cGears.Count == 0)
										slot.Sb.Unequip();
									else{
										bool found = false;
										foreach(CarriedGearInstanceMock cGear in cGears){
											if(cGear == (CarriedGearInstanceMock)slot.Sb.Item)
												found = true;
										}
										if(!found)
											slot.Sb.Unequip();
									}
								}
							}
						}
					}
				}
			}
			public class UpdateEquipStatusForEquipSGCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null){
							InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
							invItem.IsEquipped = true;
						}
					}
				}
			}
			public class SGFocusCommandV2: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.CurState.Focus(sg);
				}
			}
			public class SGDefocusCommandV2: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.CurState.Defocus(sg);
				}
			}
			
		/*	filters
		*/
			public interface SGFilter{
				void Execute(SlotGroup sg);
				List<InventoryItemInstanceMock> filteredItemInstances(List<InventoryItemInstanceMock> prefilteredList);
			}
			public class SGNullFilter: SGFilter{
				public void Execute(SlotGroup sg){

					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						filteredItems.Add(item);
					}
					sg.SetFilteredItems(filteredItems);
				}
				public List<InventoryItemInstanceMock> filteredItemInstances(List<InventoryItemInstanceMock> prefilteredList){
					return prefilteredList;
				}
			}
			public class SGBowFilter: SGFilter{
				public void Execute(SlotGroup sg){
					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						if(item is BowInstanceMock)
							filteredItems.Add(item);
					}
					sg.SetFilteredItems(filteredItems);
				}
				public List<InventoryItemInstanceMock> filteredItemInstances(List<InventoryItemInstanceMock> prefilteredList){
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
					foreach(InventoryItemInstanceMock itemInst in prefilteredList){
						if(itemInst is BowInstanceMock)
							result.Add(itemInst);
					}
					return result;
				}
			}
			public class SGWearFilter: SGFilter{
				public void Execute(SlotGroup sg){
					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						if(item is WearInstanceMock)
							filteredItems.Add(item);
					}
					sg.SetFilteredItems(filteredItems);
				}
				public List<InventoryItemInstanceMock> filteredItemInstances(List<InventoryItemInstanceMock> prefilteredList){
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
					foreach(InventoryItemInstanceMock itemInst in prefilteredList){
						if(itemInst is WearInstanceMock)
							result.Add(itemInst);
					}
					return result;
				}
			}
			public class SGCarriedGearFilter: SGFilter{
				public void Execute(SlotGroup sg){
					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						if(item is CarriedGearInstanceMock)
							filteredItems.Add(item);
					}
					sg.SetFilteredItems(filteredItems);
				}
				public List<InventoryItemInstanceMock> filteredItemInstances(List<InventoryItemInstanceMock> prefilteredList){
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
					foreach(InventoryItemInstanceMock itemInst in prefilteredList){
						if(itemInst is CarriedGearInstanceMock)
							result.Add(itemInst);
					}
					return result;
				}
			}
			public class SGPartsFilter: SGFilter{
				public void Execute(SlotGroup sg){
					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						if(item is PartsInstanceMock)
							filteredItems.Add(item);
					}
					sg.SetFilteredItems(filteredItems);
				}
				public List<InventoryItemInstanceMock> filteredItemInstances(List<InventoryItemInstanceMock> prefilteredList){
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
					foreach(InventoryItemInstanceMock itemInst in prefilteredList){
						if(itemInst is PartsInstanceMock)
							result.Add(itemInst);
					}
					return result;
				}
			}
			
		/*	sorters
		*/
			public interface SGSorter{
				List<Slottable> OrderedSbs(SlotGroup sg);
			}
			
			/*	non-instant sorting and AutoSorting
					SlotGroup manages its entire slottables movement in a corrdinated way
					SlotGroup creates SlotMovement class for each slottable and executes its Move command
					Each SlotMovement notifies SlotGroup its completion
					SlotGroup keeps a tally of every SlotMovements's execution status
					SlotGroup self invokes a CompleteAllMovements method upon completion of every movement's termination

					SlotMovement's motion is independent of each slottable's state and processes
					
					SlotGroup's SGUpdateTransactionProcess expires within CompleteAllMovements method
					NOT SlotGroup calls SGM's CompleteTransactionOnSG in its CompleteAllMovements method

				Voluntary Sorting
					SGM creates SortTransaction with the target SG (and only the target SG)
					SortTransaction.Execute triggers the sorting
			*/
				
			public class　SGItemIDSorter: SGSorter{
				public List<Slottable> OrderedSbs(SlotGroup sg){
					
					List<Slottable> sbList = new List<Slottable>();
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null)
							sbList.Add(slot.Sb);
					}
					sbList.Sort();
					return sbList;
				}
			}
			public class SGInverseItemIDSorter: SGSorter{
				public List<Slottable> OrderedSbs(SlotGroup sg){
					List<Slottable> sbList = new List<Slottable>();
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null)
							sbList.Add(slot.Sb);
					}
					sbList.Sort();
					sbList.Reverse();
					return sbList;
				}
			}
			public class SGAcquisitionOrderSorter: SGSorter{
				
				public List<Slottable> OrderedSbs(SlotGroup sg){
					List<Slottable> sbList = new List<Slottable>();
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null)
							sbList.Add(slot.Sb);
						// slot.Sb = null;
					}
					
					List<Slottable> temp = new List<Slottable>();
					Slottable addedMax = null;
					while(temp.Count < sbList.Count){
						int indexAtMin = -1;
						int addedAO;
						if(addedMax == null) addedAO = -1;
						else addedAO = ((InventoryItemInstanceMock)addedMax.Item).AcquisitionOrder;

						for(int i = 0; i < sbList.Count; i++){
							InventoryItemInstanceMock inst = (InventoryItemInstanceMock)sbList[i].Item;
							if(inst.AcquisitionOrder > addedAO){
								if(indexAtMin == -1 || inst.AcquisitionOrder < ((InventoryItemInstanceMock)sbList[indexAtMin].Item).AcquisitionOrder){
									indexAtMin = i;
								}
							}
						}
						Slottable added = sbList[indexAtMin];
						temp.Add(added);
						addedMax = added;
					}
					return temp;
				}
			}
	/*	Slottable Classses
	*/
		/*	process
		*/
			public interface SBProcess{
				bool IsRunning{get;}
				bool IsExpired{get;}
				System.Func<IEnumeratorMock> CoroutineMock{set;}
				Slottable SB{set;}
				void Start();
				void Stop();
				void Expire();
			}
			public abstract class AbsSBProcess: SBProcess{
				System.Func<IEnumeratorMock> m_coroutineMock;
				public System.Func<IEnumeratorMock> CoroutineMock{
					get{return m_coroutineMock;}
					set{m_coroutineMock = value;}
				}
				Slottable m_sb;
				public Slottable SB{
					get{return m_sb;}
					set{m_sb = value;}
				}
				bool m_isRunning;
				public bool IsRunning{get{return m_isRunning;}}
				bool m_isExpired;
				public bool IsExpired{get{return m_isExpired;}}
				public void Start(){
					//call StartCoroutine(m_coroutine);
					m_isRunning = true;
					m_isExpired = false;
					m_coroutineMock();
				}
				public void Stop(){
					//call StopCoroutine(m_coroutine);
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = false;
					}
				}
				public virtual void Expire(){
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = true;
					}
				}
			}
			public class SBGreyoutProcess: AbsSBProcess{
				public SBGreyoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}	
			}
			public class SBUnequipAndGreyoutProcess: AbsSBProcess{
				public SBUnequipAndGreyoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}	
			}
			public class SBEquipAndGreyoutProcess: AbsSBProcess{
				public SBEquipAndGreyoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}	
			}
			
			public class SBGreyinProcess: AbsSBProcess{
				public SBGreyinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SBUnequipAndGreyinProcess: AbsSBProcess{
				public SBUnequipAndGreyinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SBEquipAndGreyinProcess: AbsSBProcess{
				public SBEquipAndGreyinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SBHighlightProcess: AbsSBProcess{
				public SBHighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SBDehighlightProcess: AbsSBProcess{
				public SBDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SBUnequipAndDehighlightProcess: AbsSBProcess{
				public SBUnequipAndDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SBEquipAndDehighlightProcess: AbsSBProcess{
				public SBEquipAndDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class WaitForPointerUpProcess: AbsSBProcess{
				public WaitForPointerUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.SetState(Slottable.DefocusedState);
				}
			}
			public class WaitForPickUpProcess: AbsSBProcess{
				public WaitForPickUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.PickUp();
					// SB.SetState(Slottable.PickedUpAndSelectedState);
				}
			}
			public class SBPickUpProcess: AbsSBProcess{
				public SBPickUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class MoveProcess: AbsSBProcess{
				public MoveProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					/*	destroy dragged icon
						complete translation

						make defocused non pool SGs focused
						make selectedSG focused
						then updateSBstates
					*/
					base.Expire();
					SB.ClearDraggedIconDestination();
					SB.SGM.CompleteTransactionOnSB(SB);
				}
			}
			public class WaitForNextTouchWhilePUProcess: AbsSBProcess{
				public WaitForNextTouchWhilePUProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.SGM.Transaction.Execute();
				}
			}
			public class WaitForNextTouchProcess: AbsSBProcess{
				public WaitForNextTouchProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.Tap();
					SB.SetState(Slottable.FocusedState);
				}
			}
			public class SBUnequipProcess: AbsSBProcess{
				public SBUnequipProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.Tap();
					SB.SetState(Slottable.FocusedState);
				}
			}
			public class SBUnpickProcess: AbsSBProcess{
				public SBUnpickProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.Tap();
					SB.SetState(Slottable.FocusedState);
				}
			}
			public class SBRemovingProcess: AbsSBProcess{
				public SBRemovingProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.ClearDraggedIconDestination();
					SB.SGM.CompleteTransactionOnSB(SB);
				}
			}
			public class SBEquippingProcess: AbsSBProcess{
				public SBEquippingProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.ClearDraggedIconDestination();
					SB.SGM.CompleteTransactionOnSB(SB);
				}
			}
			public class SBReorderingProcess: AbsSBProcess{
				public SBReorderingProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.ClearDraggedIconDestination();
					SB.SGM.CompleteTransactionOnSB(SB);
				}
			}
			public class SBUnpickingProcess: AbsSBProcess{
				public SBUnpickingProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.ClearDraggedIconDestination();
					SB.SGM.CompleteTransactionOnSB(SB);
				}
			}
			
		/*	states
		*/
			public interface SlottableState{
				void EnterState(Slottable sb);
				void ExitState(Slottable sb);
				void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock);
				void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock);
				void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock);
				void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock);
				void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock);
				void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock);
				void Focus(Slottable sb);
				void Defocus(Slottable sb);
			}
			public class DeactivatedState: SlottableState{
				public void EnterState(Slottable sb){
					sb.SetAndRun(null);
				}
				public void ExitState(Slottable sb){
				}
				public void Focus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDeselectedState);
					else
						sb.SetState(Slottable.FocusedState);

				}
				public void Defocus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDefocusedState);
					else
						sb.SetState(Slottable.DefocusedState);
				}
				/*	undef
				*/
					public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){
						
					}
					public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class DefocusedState: SlottableState{
				public void EnterState(Slottable slottable){
					SBProcess process = null;
					if(slottable.PrevState == Slottable.FocusedState){
						process = new SBGreyoutProcess(slottable, slottable.GreyoutCoroutine);
					}else if(slottable.PrevState == Slottable.EquippedAndDeselectedState){
						process = new SBUnequipAndGreyoutProcess(slottable, slottable.UnequipAndGreyoutCoroutine);
					}else if(slottable.PrevState == Slottable.DeactivatedState){
						slottable.InstantGreyout();
					}else if(slottable.PrevState == Slottable.EquippedAndDefocusedState){
						process = new SBUnequipProcess(slottable, slottable.UnequipCoroutine);
					}else if(slottable.PrevState == Slottable.MovingState){
						process = new SBUnpickProcess(slottable, slottable.UnpickCoroutine);
					}else if(slottable.PrevState == Slottable.WaitForPointerUpState){
						process = null;
						slottable.SetAndRun(process);
					}else if(slottable.PrevState == Slottable.MovingState){
						process = null;
						slottable.SetAndRun(process);
					}
					if(process != null)
						slottable.SetAndRun(process);
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.SetState(Slottable.WaitForPointerUpState);
				}
				public void Focus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDeselectedState);
					else
						sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDefocusedState);
				}
				/*	undef
				*/
					public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
				/*	ignore
				*/
					public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class FocusedState: SlottableState{
				public void EnterState(Slottable slottable){
					SBProcess process = null;
					if(slottable.PrevState == Slottable.DefocusedState){
						process = new SBGreyinProcess(slottable, slottable.GreyinCoroutine);
					}else if(slottable.PrevState == Slottable.EquippedAndDefocusedState){
						process = new SBUnequipAndGreyinProcess(slottable, slottable.UnequipAndGreyinCoroutine);
					}else if(slottable.PrevState == Slottable.SelectedState){
						process = new SBDehighlightProcess(slottable, slottable.DehighlightCoroutine);
					}else if(slottable.PrevState == Slottable.EquippedAndSelectedState){
						process = new SBUnequipAndDehighlightProcess(slottable, slottable.UnequipAndDehighlightCoroutine);
					}else if(slottable.PrevState == Slottable.DeactivatedState){
						slottable.InstantGreyin();
					}else if(slottable.PrevState == Slottable.MovingState){
						// process = new SBUnpickProcess(slottable, slottable.UnpickCoroutine	
						process = null;
						slottable.SetAndRun(process);
					
					}
					if(process != null)
						slottable.SetAndRun(process);
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					
					slottable.SetState(Slottable.WaitForPickUpState);

				}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SGM.SetSelectedSB(sb);
					sb.SetState(Slottable.SelectedState);
					
				}
				public void Focus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDeselectedState);
				}
				public void Defocus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDefocusedState);
					else
						sb.SetState(Slottable.DefocusedState);
				}
				/*	undef
				*/
					public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class WaitForPointerUpState: SlottableState{
				public void EnterState(Slottable sb){
					
					SBProcess wfPtuProcess = new WaitForPointerUpProcess(sb, sb.WaitForPointerUpCoroutine);
					sb.SetAndRun(wfPtuProcess);
				}
				public void ExitState(Slottable sb){
				}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.Tap();
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDefocusedState);
					else
						sb.SetState(Slottable.DefocusedState);
					
				}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDefocusedState);
					else
						sb.SetState(Slottable.DefocusedState);
				}
				/*	undef
				*/
					public void Focus(Slottable sb){
					}
					public void Defocus(Slottable sb){
					}
					public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class WaitForPickUpState: SlottableState{
				public void EnterState(Slottable slottable){
					SBProcess wfpuProcess = new WaitForPickUpProcess(slottable, slottable.WaitForPickUpCoroutine);
					slottable.SetAndRun(wfpuProcess);
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
					if(slottable.Item.IsStackable)
						slottable.SetState(Slottable.WaitForNextTouchState);
					else{
						slottable.Tap();
						if(slottable.IsEquipped)
							slottable.SetState(Slottable.EquippedAndDeselectedState);
						else
							slottable.SetState(Slottable.FocusedState);
					}
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
					if(slottable.IsEquipped)
						slottable.SetState(Slottable.EquippedAndDeselectedState);
					else
						slottable.SetState(Slottable.FocusedState);
				}
				/*	undef
				*/
					public void Focus(Slottable sb){
					}
					public void Defocus(Slottable sb){
					}
					public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
						
					}
			}
			public class WaitForNextTouchState: SlottableState{
				public void EnterState(Slottable slottable){
					SBProcess wfntProcess = new WaitForNextTouchProcess(slottable, slottable.WaitForNextTouchCoroutine);
					slottable.SetAndRun(wfntProcess);
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					
					slottable.PickUp();
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
					
					slottable.SetState(Slottable.FocusedState);
				}
				/*	undef
				*/
					public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
					}
					public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void Focus(Slottable sb){
					}
					public void Defocus(Slottable sb){
					}
			}
			public class PickedUpAndSelectedState: SlottableState{
				/*	Execute transaction needs revision
					it should turn the state of sb not straight into RevertingState, but need to operate an alogorithm to decide whether it should turn sb state instead to WaitForNextTouch state before jumping into the conclusion
				*/
				public void EnterState(Slottable slottable){
					SBProcess process = null;
					if(slottable.PrevState == Slottable.WaitForPickUpState || slottable.PrevState == Slottable.WaitForNextTouchState){
						process = new SBPickUpProcess(slottable, slottable.PickUpCoroutine);
					}else if(slottable.PrevState == Slottable.PickedAndDeselectedState){
						process = new SBHighlightProcess(slottable, slottable.HighlightCoroutine);
					}
					if(process != null)
						slottable.SetAndRun(process);
					if(slottable.SGM.CurState != SlotGroupManager.ProbingState){
						slottable.SGM.SetState(SlotGroupManager.ProbingState);
						InitializeSGMFields(slottable);
						slottable.SGM.PostPickFilter();
					}
					
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
					if(slottable.Item.IsStackable)
						slottable.SetState(Slottable.WaitForNextTouchWhilePUState);
					else
						slottable.ExecuteTransaction();
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.ExecuteTransaction();
				}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SGM.SetPickedSB(sb);
					sb.SGM.SetSelectedSB(sb);
				}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb){
						sb.SGM.SetSelectedSB(null);
					}
					sb.SetState(Slottable.PickedAndDeselectedState);
				}
				public void Focus(Slottable sb){
				}
				public void Defocus(Slottable sb){
				}
				void InitializeSGMFields(Slottable slottable){
					slottable.SGM.SetSelectedSB(null);
					slottable.SGM.SetSelectedSG(null);
					slottable.SGM.SetPickedSB(slottable);//picked needs to be set prior to the other two in order to update transaction properly
					slottable.SGM.SetSelectedSB(slottable);
					SlotGroup sg = slottable.SGM.GetSlotGroup(slottable);
					slottable.SGM.SetSelectedSG(sg);
					sg.SetState(SlotGroup.SelectedState);
					slottable.SGM.UpdateTransaction();
					// slottable.SGM.SetSelectedSG(slottable.SGM.GetSlotGroup(slottable));
				}
			}
			public class PickedUpAndDeselectedState: SlottableState{
				public void EnterState(Slottable sb){
					SBDehighlightProcess gradDeHiProcess = new SBDehighlightProcess(sb, sb.DehighlightCoroutine);
					sb.SetAndRun(gradDeHiProcess);
				}
				public void ExitState(Slottable sb){
				}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.ExecuteTransaction();
				}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SetState(Slottable.PickedAndSelectedState);
					sb.SGM.SetSelectedSB(sb);
				}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void Focus(Slottable sb){
				}
				public void Defocus(Slottable sb){
				}
			}
			public class WaitForNextTouchWhilePUState: SlottableState{
				public void EnterState(Slottable slottable){
					
					SBProcess wfntwpuProcess = new WaitForNextTouchWhilePUProcess(slottable, slottable.WaitForNextTouchWhilePUCoroutine);
					slottable.SetAndRun(wfntwpuProcess);
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.Increment();
					slottable.SetState(Slottable.PickedAndSelectedState);
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.ExecuteTransaction();
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void Focus(Slottable sb){
				}
				public void Defocus(Slottable sb){
				}
			}
			public class MovingOutState: SlottableState{
				public void EnterState(Slottable slottable){
					MoveProcess moveProcess = new MoveProcess(slottable, slottable.MoveCoroutine);
					slottable.SetAndRun(moveProcess);

					SlotGroupManager sgm = slottable.SGM;
					SlotGroup sg = sgm.GetSlotGroup(slottable);
					if(sgm.Transaction.GetType() == typeof(ReorderTransaction)){
						SBReorderingProcess process = new SBReorderingProcess(slottable, slottable.ReorderingCoroutine);
						slottable.SetAndRun(process);
					}else{
						if(sgm.GetFocusedPoolSG() == sg){
							/*	Create and Run Equipping Process	*/
							SBEquippingProcess process = new SBEquippingProcess(slottable, slottable.EquippingCoroutine);
							slottable.SetAndRun(process);
						}else{
							/*	Create and Run Removing Process	*/
							SBRemovingProcess process = new SBRemovingProcess(slottable, slottable.RemovingCoroutine);
							slottable.SetAndRun(process);
						}
					}
				}
				public void ExitState(Slottable Slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void Focus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDeselectedState);
					else
						sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDefocusedState);
					else
						sb.SetState(Slottable.DefocusedState);
				}
			}
			public class EquippedAndDeselectedState: SlottableState{
				public void EnterState(Slottable slottable){
					SBProcess process = null;
					if(slottable.PrevState == Slottable.DefocusedState){
						process = new SBEquipAndGreyinProcess(slottable, slottable.EquipAndGreyinCoroutine);
					}else if(slottable.PrevState == Slottable.EquippedAndDefocusedState){
						process = new SBGreyinProcess(slottable, slottable.GreyinCoroutine);
					}else if(slottable.PrevState == Slottable.SelectedState){
						process = new SBEquipAndDehighlightProcess(slottable, slottable.EquipAndDehighlightCoroutine);
					}else if(slottable.PrevState == Slottable.EquippedAndSelectedState){
						process = new SBDehighlightProcess(slottable, slottable.DehighlightCoroutine);
					}else if(slottable.PrevState == Slottable.DeactivatedState){
						slottable.InstantEquipAndGreyin();
					}else if(slottable.PrevState == Slottable.MovingState){
						process = null;
						slottable.SetAndRun(process);
					}
					if(process != null)
						slottable.SetAndRun(process);

				}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					if(slottable.Delayed)
						slottable.SetState(Slottable.WaitForPickUpState);
					else
						slottable.SetState(Slottable.PickedAndSelectedState);
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SGM.SetSelectedSB(sb);
					sb.SetState(Slottable.EquippedAndSelectedState);
				}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void Focus(Slottable sb){
					if(!sb.IsEquipped)
						sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					if(!sb.IsEquipped)
						sb.SetState(Slottable.DefocusedState);
					else
						sb.SetState(Slottable.EquippedAndDefocusedState);
				}
			}
			public class EquippedAndSelectedState: SlottableState{
				public void EnterState(Slottable slottable){
					SBProcess process = null;
					if(slottable.PrevState == Slottable.EquippedAndDeselectedState){
						process = new SBHighlightProcess(slottable, slottable.HighlightCoroutine);
					}
					if(process != null)
						slottable.SetAndRun(process);
				}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(null);
					sb.SetState(Slottable.EquippedAndDeselectedState);
				}
				public void Focus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDeselectedState);
					else
						sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDefocusedState);
					else
						sb.SetState(Slottable.DefocusedState);

				}
			}
			public class EquippedAndDefocusedState: SlottableState{
				public void EnterState(Slottable slottable){
					SBProcess process = null;
					
					if(slottable.PrevState == Slottable.FocusedState){
						process = new SBEquipAndGreyoutProcess(slottable, slottable.EquipAndGreyoutCoroutine);
					}else if(slottable.PrevState == Slottable.EquippedAndDeselectedState){
						process = new SBGreyoutProcess(slottable, slottable.GreyoutCoroutine);
					}else if(slottable.PrevState == Slottable.DeactivatedState){
						slottable.InstantEquipAndGreyout();
					}else if(slottable.PrevState == Slottable.WaitForPointerUpState){
						process = null;
						slottable.SetAndRun(process);
					}else if(slottable.PrevState == Slottable.MovingState){
						process = null;
						slottable.SetAndRun(process);
					}else if(slottable.PrevState == Slottable.EquippingState){
						process = null;
						slottable.SetAndRun(process);
					}
					if(process != null)
						slottable.SetAndRun(process);
				}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.SetState(Slottable.WaitForPointerUpState);
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
				}
				public void Focus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDeselectedState);
					else
						sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					
					sb.SetState(Slottable.EquippedAndDefocusedState);
				}
			}
			public class SBSelectedState: SlottableState{
				public void EnterState(Slottable sb){
					if(sb.PrevState == Slottable.FocusedState){
						SBHighlightProcess gradHiProcess = new SBHighlightProcess(sb, sb.HighlightCoroutine);
						sb.SetAndRun(gradHiProcess);
					}
				}
				public void ExitState(Slottable sb){}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(null);
					sb.SetState(Slottable.FocusedState);
				}
				public void Focus(Slottable sb){
					if(sb.IsEquipped)
						sb.SetState(Slottable.EquippedAndDeselectedState);
					else
						sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
				}
			}
			public class SBRemovingState: SlottableState{
				public void EnterState(Slottable sb){
					SBRemovingProcess process = new SBRemovingProcess(sb, sb.RemovingCoroutine);
					sb.SetAndRun(process);
					
				}
				public void ExitState(Slottable sb){}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(null);
					sb.SetState(Slottable.FocusedState);
				}
				public void Focus(Slottable sb){
				}
				public void Defocus(Slottable sb){
				}
			}
			public class SBEquippingState: SlottableState{
				public void EnterState(Slottable sb){
					SBEquippingProcess process = new SBEquippingProcess(sb, sb.EquippingCoroutine);
					sb.SetAndRun(process);
					
				}
				public void ExitState(Slottable sb){}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(null);
					sb.SetState(Slottable.FocusedState);
				}
				public void Focus(Slottable sb){
				}
				public void Defocus(Slottable sb){
					sb.SetState(Slottable.EquippedAndDefocusedState);
				}
			}
			public class SBUnpickingState: SlottableState{
				public void EnterState(Slottable sb){
					SBUnpickingProcess process = new SBUnpickingProcess(sb, sb.UnpickingCoroutine);
					sb.SetAndRun(process);
				}
				public void ExitState(Slottable sb){}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(null);
					sb.SetState(Slottable.FocusedState);
				}
				public void Focus(Slottable sb){
				}
				public void Defocus(Slottable sb){
				}
			}

		/*	commands
		*/
			public interface SlottableCommand{
				void Execute(Slottable sb);
			}
			public class DefInstantDeactivateCommand: SlottableCommand{
				public void Execute(Slottable sb){
				}
			}
			public class SBTapCommand: SlottableCommand{
				public void Execute(Slottable sb){

				}
			}
		
	/*	Other Classes
		*/
			/*	Inventory Item
			*/
				public interface SlottableItem: IEquatable<SlottableItem>, IComparable<SlottableItem>, IComparable{

					int Quantity{get;}
					bool IsStackable{get;}
				}
				public class InventoryItemInstanceMock: SlottableItem{
					InventoryItemMock m_item;
					public InventoryItemMock Item{
						get{return m_item;}
						set{m_item = value;}
					}
					int m_quantity;
					public int Quantity{
						get{return m_quantity;}
						set{m_quantity = value;}
					}
					int m_acquisitionOrder;
					public int AcquisitionOrder{
						get{return m_acquisitionOrder;}
					}
					public void SetAcquisitionOrder(int id){
						m_acquisitionOrder = id;
					}
					bool m_isStackable;
					public bool IsStackable{
						get{
							return m_item.IsStackable;
						}
					}
					bool m_isEquipped = false;
					public bool IsEquipped{
						get{return m_isEquipped;}
						set{m_isEquipped = value;}
					}
					public override bool Equals(object other){
						if(!(other is InventoryItemInstanceMock))
							return false;
						return Equals((SlottableItem)other);
					}
					public bool Equals(SlottableItem other){
						if(!(other is InventoryItemInstanceMock))
							return false;
						InventoryItemInstanceMock otherInst = (InventoryItemInstanceMock)other;
						if(m_item.IsStackable)
							return m_item.Equals(otherInst.Item);
						else
							return object.ReferenceEquals(this, other);
					}
					public override int GetHashCode(){
						return m_item.ItemID.GetHashCode() + 31;
					}
					public static bool operator ==(InventoryItemInstanceMock a, InventoryItemInstanceMock b){
						return a.Equals(b);
					}
					public static bool operator != (InventoryItemInstanceMock a, InventoryItemInstanceMock b){
						if(object.ReferenceEquals(a, null)){
							return !object.ReferenceEquals(b, null);
						}
						if(object.ReferenceEquals(b, null)){
							return !object.ReferenceEquals(a, null);
						}
						return !(a == b);
					}
					int IComparable.CompareTo(object other){
						if(!(other is SlottableItem))
							throw new InvalidOperationException("System.Object.CompareTo: not a SlottableItem");
						return CompareTo((SlottableItem)other);
					}
					public int CompareTo(SlottableItem other){
						if(!(other is InventoryItemInstanceMock))
							throw new InvalidOperationException("System.Object.CompareTo: not an InventoryItemInstance");
						InventoryItemInstanceMock otherInst = (InventoryItemInstanceMock)other;

						int result = m_item.ItemID.CompareTo(otherInst.Item.ItemID);
						if(result == 0)
							result = this.AcquisitionOrder.CompareTo(otherInst.AcquisitionOrder);
						
						return result;
					}
				}
				public class InventoryItemMock: IEquatable<InventoryItemMock>, IComparable, IComparable<InventoryItemMock>{
					
					bool m_isStackable;
					public bool IsStackable{
						get{return m_isStackable;}
						set{m_isStackable = value;}
					}

					int m_itemId;
					public int ItemID{
						get{return m_itemId;}
						set{m_itemId = value;}
					}

					public override bool Equals(object other){
						if(!(other is InventoryItemMock)) return false;
						else
							return Equals((InventoryItemMock)other);
					}
					public bool Equals(InventoryItemMock other){
						return m_itemId == other.ItemID;
					}

					public override int GetHashCode(){
						return 31 + m_itemId.GetHashCode();
					}

					public static bool operator == (InventoryItemMock a, InventoryItemMock b){
						return a.ItemID == b.ItemID;
					}

					public static bool operator != (InventoryItemMock a, InventoryItemMock b){
						return a.ItemID != b.ItemID;
					}
					int IComparable.CompareTo(object other){
						if(!(other is InventoryItemMock))
							throw new InvalidOperationException("Compare To: not a InventoryItemMock");
						return CompareTo((InventoryItemMock)other);
					}
					public int CompareTo(InventoryItemMock other){
						if(!(other is InventoryItemMock))
							throw new InvalidOperationException("Compare To: not a InventoryItemMock");
						
						return this.m_itemId.CompareTo(other.ItemID);
					}
					public static bool operator > (InventoryItemMock a, InventoryItemMock b){
						return a.CompareTo(b) > 0;
					}
					public static bool operator < (InventoryItemMock a, InventoryItemMock b){
						return a.CompareTo(b) < 0;
					}
				}
			/*	Inventories
			*/
				public interface Inventory{
					List<SlottableItem> Items{get;}
					void AddItem(SlottableItem item);
					void RemoveItem(SlottableItem item);
				}
				public class PoolInventory: Inventory{
					List<SlottableItem> m_items = new List<SlottableItem>();
					public List<SlottableItem> Items{
						get{return m_items;}
					}
					public void AddItem(SlottableItem item){
						// m_items.Add(item);
						foreach(SlottableItem it in m_items){
							InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)it;
							InventoryItemInstanceMock addedInst = (InventoryItemInstanceMock)item;
							if(invInst == addedInst){
								invInst.Quantity += addedInst.Quantity;
								return;
							}
						}
						m_items.Add(item);
						IndexItems();
					}
					public void RemoveItem(SlottableItem item){
						SlottableItem itemToRemove = null;
						foreach(SlottableItem it in Items){
							InventoryItemInstanceMock checkedInst = (InventoryItemInstanceMock)it;
							InventoryItemInstanceMock removedInst = (InventoryItemInstanceMock)item;
							if(checkedInst == removedInst){
								if(!removedInst.IsStackable)
									itemToRemove = it;
								else{
									checkedInst.Quantity -= removedInst.Quantity;
									if(checkedInst.Quantity <= 0)
										itemToRemove = it;
								}
							}
						}
						if(itemToRemove != null)
							Items.Remove(itemToRemove);
						IndexItems();
					}

					void IndexItems(){
						for(int i = 0; i < m_items.Count; i ++){
							((InventoryItemInstanceMock)m_items[i]).SetAcquisitionOrder(i);
						}
					}
				}
				public class EquipmentSetInventory: Inventory{
					BowInstanceMock m_equippedBow;
					WearInstanceMock m_equippedWear;
					List<CarriedGearInstanceMock> m_equippedCGears = new List<CarriedGearInstanceMock>();
					int m_equippableCGearsCount;
					public int EquippableCGearsCount{
						get{return m_equippableCGearsCount;}
					}
					public void SetEquippableCGearsCount(int num){
						m_equippableCGearsCount = num;
					}
					
					public List<SlottableItem> Items{
						get{
							List<SlottableItem> result = new List<SlottableItem>();
							if(m_equippedBow != null)
								result.Add(m_equippedBow);
							if(m_equippedWear != null)
								result.Add(m_equippedWear);
							if(m_equippedCGears.Count != 0){
								foreach(CarriedGearInstanceMock inst in m_equippedCGears){
									result.Add((SlottableItem)inst);
								}
							}
							return result;
						}
					}
					public void AddItem(SlottableItem item){
						if(item != null){
							if(item is BowInstanceMock){
								BowInstanceMock bowInst = (BowInstanceMock)item;
								m_equippedBow = bowInst;
							}	
							else if(item is WearInstanceMock){
								WearInstanceMock wearInst = (WearInstanceMock)item;
								m_equippedWear = wearInst;
							}
							else if(item is CarriedGearInstanceMock){
								if(m_equippedCGears.Count < m_equippableCGearsCount)
									m_equippedCGears.Add((CarriedGearInstanceMock)item);
								else
									throw new InvalidOperationException("trying to add a CarriedGear exceeding the maximum allowed count");
							}
						}
					}
					public void RemoveItem(SlottableItem removedItem){
						if(removedItem != null){
							if(removedItem is BowInstanceMock){
								if((BowInstanceMock)removedItem == m_equippedBow)
									m_equippedBow = null;
							}else if(removedItem is WearInstanceMock){
								if((WearInstanceMock)removedItem == m_equippedWear)
									m_equippedWear = null;
							}else if(removedItem is CarriedGearInstanceMock){
								CarriedGearInstanceMock spottedOne = null;
								foreach(CarriedGearInstanceMock cgInst in m_equippedCGears){
									if((CarriedGearInstanceMock)removedItem == cgInst)
										spottedOne = cgInst;
								}
								if(spottedOne != null)
									m_equippedCGears.Remove(spottedOne);
							}
						}
					}
				}
			public class Slot{
				// SlottableItem m_item;
				// public SlottableItem Item{
				// 	get{return m_item;}
				// 	set{m_item = value;}
				// }
				Slottable m_sb;
				public Slottable Sb{
					get{return m_sb;}
					set{m_sb = value;}
				}
				Vector2 m_position;
				public Vector2 Position{
					get{return m_position;}
					set{m_position = value;}
				}
			}
			/*	mock items
			*/
				public class BowMock: InventoryItemMock{
					public BowMock(){
						IsStackable = false;
					}
					
				}
				public class WearMock: InventoryItemMock{
					public WearMock(){
						IsStackable = false;
					}
				}
				public abstract class CarriedGearMock: InventoryItemMock{

				}
				public class ShieldMock: CarriedGearMock{
					public ShieldMock(){
						IsStackable = false;
					}
				}
				public class MeleeWeaponMock: CarriedGearMock{
					public MeleeWeaponMock(){
						IsStackable = false;
					}
				}
				public class QuiverMock: CarriedGearMock{
					public QuiverMock(){
						IsStackable = false;
					}
				}
				public class PackMock: CarriedGearMock{
					public PackMock(){
						IsStackable = false;
					}
				}
				public class PartsMock: InventoryItemMock{
					public PartsMock(){
						IsStackable = true;
					}
				}
				
			/*	mock instances
			*/
				public class BowInstanceMock: InventoryItemInstanceMock{
					public BowInstanceMock(){
						this.Quantity = 1;
					}
				}
				public class WearInstanceMock: InventoryItemInstanceMock{
					public WearInstanceMock(){
						this.Quantity = 1;
					}
				}
				public class CarriedGearInstanceMock: InventoryItemInstanceMock{
					
				}
				public class ShieldInstanceMock: CarriedGearInstanceMock{
					public ShieldInstanceMock(){
						this.Quantity = 1;
					}
				}
				public class MeleeWeaponInstanceMock: CarriedGearInstanceMock{
					public MeleeWeaponInstanceMock(){
						this.Quantity = 1;
					}
				}
				public class QuiverInstanceMock: CarriedGearInstanceMock{
					public QuiverInstanceMock(){
						this.Quantity = 1;
					}
				}
				public class PackInstanceMock: CarriedGearInstanceMock{
					public PackInstanceMock(){
						this.Quantity = 1;
					}
				}
				public class PartsInstanceMock: InventoryItemInstanceMock{

				}

			/*
			*/
				public interface SlotSystemElement{
					void Activate();
					void Deactivate();
					void Focus();
					void Defocus();
					SlotGroupManager SGM{get; set;}
					SlotGroup GetSlotGroup(Slottable sb);
					bool ContainsElement(SlotSystemElement element);
					
				}
				public abstract class AbsSlotSysElement: SlotSystemElement{
					public virtual List<SlotSystemElement> Elements{
						get{return null;}
					}
					public virtual bool ContainsElement(SlotSystemElement element){
						foreach(SlotSystemElement ele in Elements){
							if(ele == element)
								return true;
						}
						return false;
					}
					public virtual void Activate(){
						foreach(SlotSystemElement ele in Elements){
							ele.Activate();
						}
					}
					public virtual void Deactivate(){
						foreach(SlotSystemElement ele in Elements){
							ele.Deactivate();
						}
					}
					public virtual void Focus(){
						foreach(SlotSystemElement ele in Elements){
							ele.Focus();
						}
					}
					public virtual void Defocus(){
						foreach(SlotSystemElement ele in Elements){
							ele.Defocus();
						}
					}
					SlotGroupManager m_sgm;
					public SlotGroupManager SGM{
						get{return m_sgm;}
						set{
							m_sgm = value;
							foreach(SlotSystemElement ele in Elements){
								ele.SGM = value;
							}
						}
					}
					public SlotGroup GetSlotGroup(Slottable sb){
						foreach(SlotSystemElement ele in this.Elements){
							if(ele is SlotGroup){
								SlotGroup sg = (SlotGroup)ele;
								foreach(Slot slot in sg.Slots){
									if(slot.Sb != null)
										if(slot.Sb == sb)
											return sg;
								}
								// return null;
								continue;
							}else{
								if(ele.GetSlotGroup(sb) == null)
									continue;
								else
									return ele.GetSlotGroup(sb);
							}
						}
						return null;
					}
				}
				public class InventoryManagerPage: AbsSlotSysElement{
					SlotSystemBundle m_poolBundle;
					public SlotSystemBundle PoolBundle{
						get{return m_poolBundle;}
					}
					SlotSystemBundle m_equipBundle;
					public SlotSystemBundle EquipBundle{
						get{return m_equipBundle;}
					}
					public InventoryManagerPage(SlotSystemBundle poolBundle, SlotSystemBundle equipBundle){
						this.m_poolBundle = poolBundle;
						this.m_equipBundle = equipBundle;
					}
					public override List<SlotSystemElement> Elements{
						get{
							List<SlotSystemElement> pageElements = new List<SlotSystemElement>();
							pageElements.Add(m_poolBundle);
							pageElements.Add(m_equipBundle);
							return pageElements;
						}
					}
					public override void Focus(){
						base.Focus();
					}
				}	
				public class EquipmentSet: AbsSlotSysElement{
					SlotGroup m_bowSG;
					SlotGroup m_wearSG;
					SlotGroup m_cGearsSG;
					List<SlotSystemElement> m_pageElements;
					public EquipmentSet(SlotGroup bowSG, SlotGroup wearSG, SlotGroup cGearsSG){
						m_bowSG = bowSG;
						m_wearSG = wearSG;
						m_cGearsSG = cGearsSG;
					}
					public override List<SlotSystemElement> Elements{
						get{
							m_pageElements = new List<SlotSystemElement>();
							m_pageElements.Add(m_bowSG);
							m_pageElements.Add(m_wearSG);
							m_pageElements.Add(m_cGearsSG);
							return m_pageElements;
						}
					}
				}
				public class SlotSystemBundle: AbsSlotSysElement{
					List<SlotSystemElement> m_elements = new List<SlotSystemElement>();
					public override List<SlotSystemElement> Elements{
						get{return m_elements;}
					}
					SlotSystemElement m_focusedElement;
						public void SetFocusedBundleElement(SlotSystemElement element){
							// if(m_focusedElement != element){
							// 	if(element == null){
							// 		// if(m_focusedElement != null) => always true
							// 		m_focusedElement.Defocus();
							// 		m_focusedElement = null;
							// 	}else{
							// 		if(m_focusedElement != null)
							// 			m_focusedElement.Defocus();
							// 		m_focusedElement = element;
							// 	}
							// }
							if(ContainsElement(element))
								m_focusedElement = element;
							else
								throw new InvalidOperationException("trying to set focsed element that is not one of its members");
							// Focus();
						}
						public SlotSystemElement GetFocusedBundleElement(){
							return m_focusedElement;
						}
					public override void Focus(){
						if(m_focusedElement != null)
							m_focusedElement.Focus();
						foreach(SlotSystemElement ele in Elements){
							if(ele != m_focusedElement)
							ele.Defocus();
						}
					}
					public override void Defocus(){
						foreach(SlotSystemElement ele in Elements){
							ele.Defocus();
						}
					}	
				}
				
				
	/*	utility
	*/	
		public static class Utility{
			public static bool HaveCommonItemFamily(Slottable sb, Slottable other){
				if(sb.Item is BowInstanceMock)
					return (other.Item is BowInstanceMock);
				else if(sb.Item is WearInstanceMock)
					return (other.Item is WearInstanceMock);
				else if(sb.Item is CarriedGearInstanceMock)
					return (other.Item is CarriedGearInstanceMock);
				else if(sb.Item is PartsInstanceMock)
					return (other.Item is PartsInstanceMock);
				else
					return false;
			}

		}
}
