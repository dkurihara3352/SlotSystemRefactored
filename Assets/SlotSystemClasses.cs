using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class SlotSystemClasses{
	}
		/*	test classes	*/
			public class IEnumeratorMock{}
			public class PointerEventDataMock{
				public GameObject pointerDrag;
			}
	/*	SGM classes	*/
		/*	transaction	*/
			public interface SlotSystemTransaction{
				Slottable TargetSB{get;}
				SlotGroup TargetSG{get;}
				void Indicate();
				void Execute();
				void OnComplete();
			}
			public abstract class AbsSlotSystemTransaction: SlotSystemTransaction{
				public static SlotSystemTransaction GetTransaction(Slottable pickedSB, Slottable targetSB, SlotGroup targetSG){
					/*	notes	*/
						/*	Postpick Filter evaluation	*/
							/*	based on the possible outcome Transaction output if the PickedSB is picked 	and put under the cursor
								Defocus if the result is Revert , Focus otherwise
								SBs and SGs are evaluated independently
									SGs -> set hoveredSB null and evaluate
									SBs -> set hoveredSG null and evaluate
							*/
						/*	Transaction cache and retrieval	*/
							/*	When a SB is picked, a list of TransacionCache is created
									Transaction cache contains 1)Transaction to be performed when executed, 2) SB and/or SG that needs to be under cursor at the time of execution
								Probing is performed only on Focused (postpick filtered) SBs and SGs
							*/
						/*	SGM selection fields evaluation	*/
							/*	also is based upon this returned value, not directly what is under cursor
								Transaction feeds what sb and sg are selected
									Revert:		sSB null,		sSG targetSG(orig)
									Fill:		sSB null,		sSG targetSG(non orig)
									Swap:		sSB targetSB/calced,	sSG targetSG(non orig)
									Reorder:	sSB targetSB,	sSG targetSG(orig)
									Insert:		sSB targetSB,	sSG targetSG(non orig)
									(Sort):		sSB null, 		sSG (specified)
									Stack:		sSB targetSB/calced,	sSG targetSG(non orig)
							*/
						/*	Precondition	*/
							/*	1)	pickedSB.IsPickable
								2)	the states of the rest is unknown
								3)	a. this method is performed upon All SBs and SGs in Focused SGP and Focused SGEs, not ones in defocused (although the state of target SB or SG is not necessarily focused due to prepick filtering)
									b. this means that those elements that are defocused before prepick filtering are not going to be accidentally focused
							*/

					if(!pickedSB.IsPickable){
						throw new System.InvalidOperationException("GetTransaction: pickedSB is NOT in a pickable state");
					}
					SlotGroup origSG = pickedSB.SG;

					if(targetSB != null){
						targetSG = targetSB.SG;
					}
					if(targetSG != null){
						if(targetSG.IsPool && targetSG != SlotGroupManager.CurSGM.FocusedSGP)
							throw new System.InvalidOperationException("GetTransaction: targetSG is poolSG but not focused");
						else if(targetSG.IsSGE && !SlotGroupManager.CurSGM.FocusedSGEs.Contains(targetSG))
							throw new System.InvalidOperationException("GetTransaction: targetSG is SGE but does not belong to the focused EquipmentSet");
					}
					if(targetSG == null){// meaning selectedSB is also null
						return new RevertTransaction();
					}else{// hoveredSB could be null
						if(targetSB == null){// on SG
							if(targetSG.AcceptsFilter(pickedSB)){
								if(targetSG != origSG && origSG.IsShrinkable){
									if(targetSG.HasItem(pickedSB.ItemInst) && pickedSB.ItemInst.Item.IsStackable)
										return new StackTransaction(targetSG.GetSB(pickedSB.ItemInst));
										
									if(targetSG.HasEmptySlot){
										if(!targetSG.HasItem(pickedSB.ItemInst))
											return new FillEquipTransaction(targetSG);
									}else{
										if(targetSG.SwappableSBs(pickedSB).Count == 1){
											Slottable calcedSB = targetSG.SwappableSBs(pickedSB)[0];
											if(calcedSB.ItemInst != pickedSB.ItemInst)
												return new SwapTransaction(calcedSB);
										}else{
											if(targetSG.IsExpandable)
												return new FillEquipTransaction(targetSG);
										}
									}
								}
							}
							return new RevertTransaction();
						}else{// targetSB specified, targetSG == targetSB.SG
							if(targetSG == origSG){//
								if(targetSB != pickedSB){
									if(!targetSG.IsAutoSort)
										return new ReorderTransaction(targetSB);
								}
							}else{
								if(targetSG.AcceptsFilter(pickedSB)){
									//swap or stack, else insert
									if(pickedSB.ItemInst == targetSB.ItemInst){
										if(targetSG.IsPool && origSG.IsShrinkable)
											return new FillEquipTransaction(targetSG);
										if(pickedSB.ItemInst.Item.IsStackable)
											return new StackTransaction(targetSB);
									}else{
										if(targetSG.HasItem(pickedSB.ItemInst)){
											if(targetSG.IsPool){
												if(origSG.AcceptsFilter(targetSB))
													return new SwapTransaction(targetSB);
												if(origSG.IsShrinkable)
													return new FillEquipTransaction(targetSG);
											}
											if(!targetSG.IsAutoSort)
												return new ReorderInOtherSGTransaction(targetSB);
										}else{
											if(origSG.AcceptsFilter(targetSB))
												return new SwapTransaction(targetSB);
											if(targetSG.HasEmptySlot || targetSG.IsExpandable)
												return new FillEquipTransaction(targetSG);
											if(!targetSG.IsAutoSort)
												return new InsertTransaction(targetSB);
										}
									}
								}
							}
							return new RevertTransaction();
						}
					}
				}
				protected SlotGroupManager sgm = SlotGroupManager.CurSGM;
				// protected void CacheProcessAndSwitchState(Slottable pickedSB, Slottable selectedSB, SlotGroup pickedSG, SlotGroup selectedSG){
				// 	sgm.CachedProcess = new SGMTransactionProcess(sgm, pickedSB, selectedSB, pickedSG, selectedSG);
				// 	sgm.SetActState(SlotGroupManager.PerformingTransactionState);
				// }
				public abstract Slottable TargetSB{get;}
				public abstract SlotGroup TargetSG{get;}
				public abstract void Indicate();
				public abstract void Execute();
				public abstract void OnComplete();
			}
			public class RevertTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				public RevertTransaction(){
					this.pickedSB = sgm.PickedSB;
				}
				public override Slottable TargetSB{get{return null;}}
				public override SlotGroup TargetSG{get{return null;}}
				public override void Indicate(){}
				public override void Execute(){
					// CacheProcessAndSwitchState(pickedSB, null, null, null);
					sgm.CompleteTransactionOnSG(pickedSB.SG);
					SlotGroup sg = sgm.GetSG(pickedSB);
					Slot slot = sg.GetSlot(pickedSB);
					pickedSB.MoveDraggedIcon(sg, slot);
					pickedSB.SetActState(Slottable.RevertingState);
				}
				public override void OnComplete(){
					sgm.DestroyDraggedIcon();
					sgm.ClearAndReset();
				}
			}
			public class ReorderTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				SlotGroup origSG;
				public ReorderTransaction(Slottable selected){
					this.pickedSB = sgm.PickedSB;
					this.selectedSB = selected;
					this.origSG = pickedSB.SG;
				}
				public override Slottable TargetSB{get{return selectedSB;}}
				public override SlotGroup TargetSG{get{return null;}}
				public override void Indicate(){}
				public override void Execute(){
					// CacheProcessAndSwitchState(pickedSB, null, null, selectedSG);
					origSG.SetAndRunSlotMovementsForReorder(pickedSB, selectedSB);
					origSG.ActionProcess.Start();

					Slot slot = origSG.GetSlot(selectedSB);
					pickedSB.MoveDraggedIcon(origSG, slot);
					pickedSB.SetActState(Slottable.RevertingState);

					origSG.CheckCompletion();
				}
				public override void OnComplete(){
					origSG.OnCompleteSlotMovements();
					sgm.DestroyDraggedIcon();
					sgm.ClearAndReset();
				}
			}
			public class ReorderInOtherSGTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				SlotGroup origSG;
				SlotGroup selectedSG;
				public ReorderInOtherSGTransaction(Slottable selected){
					this.pickedSB = sgm.PickedSB;
					this.selectedSB = selected;
					this.selectedSG = this.selectedSB.SG;
				}
				public override Slottable TargetSB{get{return selectedSB;}}
				public override SlotGroup TargetSG{get{return selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					
					// CacheProcessAndSwitchState(pickedSB, null, null, selectedSG);
					sgm.CompleteTransactionOnSG(origSG);
					selectedSG.SetAndRunSlotMovementsForReorder(selectedSG.GetSB(pickedSB.ItemInst), selectedSB);
					selectedSG.ActionProcess.Start();

					Slot slot = pickedSB.SG.GetSlot(pickedSB);
					pickedSB.MoveDraggedIcon(pickedSB.SG, slot);
					pickedSB.SetActState(Slottable.RevertingState);

					selectedSG.CheckCompletion();
				}
				public override void OnComplete(){
					selectedSG.OnCompleteSlotMovements();
					sgm.DestroyDraggedIcon();
					sgm.ClearAndReset();
				}
			}
			public class StackTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				public StackTransaction(Slottable selected){
					this.pickedSB = sgm.PickedSB;
					this.selectedSB = selected;
				}
				public override Slottable TargetSB{get{return selectedSB;}}
				public override SlotGroup TargetSG{get{return selectedSB.SG;}}
				public override void Indicate(){}
				public override void Execute(){
					sgm.CompleteTransactionOnSG(pickedSB.SG);
					sgm.ClearAndReset();
				}
				public override void OnComplete(){}
			}
			public class SwapTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup origSG;
				Slottable selectedSB;
				SlotGroup selectedSG;
				List<InventoryItemInstanceMock> picked = new List<InventoryItemInstanceMock>();
				List<InventoryItemInstanceMock> hovered = new List<InventoryItemInstanceMock>();
				public SwapTransaction(Slottable selected){
					this.pickedSB = sgm.PickedSB;
					this.selectedSB = selected;
					this.origSG = pickedSB.SG;
					this.selectedSG = selectedSB.SG;
					this.picked.Add(pickedSB.ItemInst);
					this.hovered.Add(selectedSB.ItemInst);
				}
				public override Slottable TargetSB{get{return selectedSB;}}
				public override SlotGroup TargetSG{get{return selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					
					// CacheProcessAndSwitchState(pickedSB, selectedSB, origSG.IsPool?null: origSG, selectedSG.IsPool?null: selectedSG);
					
					if(!origSG.IsPool){
						origSG.SetAndRunSlotmovementsForSwap(pickedSB, selectedSB);
						origSG.ActionProcess.Start();
					}
					if(!selectedSG.IsPool){
						selectedSG.SetAndRunSlotmovementsForSwap(selectedSB, pickedSB);
						selectedSG.ActionProcess.Start();
					}
					
					Slot selectedSBSlot = selectedSG.GetSlotForAdded(pickedSB);
					Slot pickedSBSlot = origSG.GetSlotForAdded(selectedSB);

					selectedSB.MoveDraggedIcon(origSG, pickedSBSlot);
					pickedSB.MoveDraggedIcon(selectedSG, selectedSBSlot);
					
					if(origSG.IsPool){
						pickedSB.SetActState(Slottable.MovingOutState);
						Slottable swappedDest = origSG.GetSB(selectedSB.ItemInst);
						swappedDest.SetActState(Slottable.MovingInState);
					}
					if(selectedSG.IsPool){
						selectedSB.SetActState(Slottable.MovingInState);
						Slottable swappedDest = selectedSG.GetSB(pickedSB.ItemInst);
						swappedDest.SetActState(Slottable.MovingOutState);
					}

					origSG.CheckCompletion();
					selectedSG.CheckCompletion();
				}
				public override void OnComplete(){
					sgm.DestroyDraggedIcon();
					if(!origSG.IsPool)
						origSG.OnCompleteSlotMovements();
					if(!selectedSG.IsPool)
						selectedSG.OnCompleteSlotMovements();
					sgm.UpdateEquipStatesOnAll();
					sgm.ClearAndReset();
				}
			}
			public class FillTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup origSG;
				SlotGroup selectedSG;
				public FillTransaction(SlotGroup selected){
					this.pickedSB = sgm.PickedSB;
					this.selectedSG = selected;
					this.origSG = pickedSB.SG;
				}
				public override Slottable TargetSB{get{return null;}}
				public override SlotGroup TargetSG{get{return selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){

					Slot slot = selectedSG.GetNextEmptySlot();
					/*	precondition for GetNext...
							1. it has an empty slot OR isExpandable
							2. does not have the same stackable item
					*/

					/*	perform focusing of scroller if something is to be added
						perform Inventory update, sorting ,filtering and updating slots and sbs, and moving of all the elements it contains
					*/
					pickedSB.MoveDraggedIcon(selectedSG, slot);
					// CacheProcessAndSwitchState(pickedSB, null, origSG, selectedSG);
				}
				public override void OnComplete(){
					sgm.DestroyDraggedIcon();
					sgm.UpdateEquipStatesOnAll();
					sgm.ClearAndReset();//Focus should take care of clearing the processes
				}
			}
			public class FillEquipTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup selectedSG;
				SlotGroup origSG;
				List<InventoryItemInstanceMock> moved = new List<InventoryItemInstanceMock>();
				
				public FillEquipTransaction(SlotGroup selected){
					this.pickedSB = sgm.PickedSB;
					this.selectedSG = selected;
					this.origSG = pickedSB.SG;
					this.moved.Add(pickedSB.ItemInst);
				}
				public override Slottable TargetSB{get{return null;}}
				public override SlotGroup TargetSG{get{return selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					// CacheProcessAndSwitchState(pickedSB, null, origSG.IsPool? null: origSG, selectedSG.IsPool? null: selectedSG);

					if(!origSG.IsPool){
						origSG.SetAndRunSlotMovements(moved, null);
						origSG.ActionProcess.Start();
					}else
						sgm.CompleteTransactionOnSG(origSG);

					if(!selectedSG.IsPool){
						selectedSG.SetAndRunSlotMovements(null, moved);
						selectedSG.ActionProcess.Start();
					}else
						sgm.CompleteTransactionOnSG(selectedSG);

					Slot slot = selectedSG.GetSlotForAdded(pickedSB);
					pickedSB.MoveDraggedIcon(selectedSG, slot);
					pickedSB.SetActState(Slottable.MovingOutState);
					
					if(selectedSG.IsPool){
						Slottable targetSB = selectedSG.GetSB(pickedSB.ItemInst);
						targetSB.SetActState(Slottable.MovingInState);
					}

					if(!origSG.IsPool)
						origSG.CheckCompletion();
					if(!selectedSG.IsPool)
						selectedSG.CheckCompletion();
				}
				public override void OnComplete(){
					sgm.DestroyDraggedIcon();
					if(!origSG.IsPool)
						origSG.OnCompleteSlotMovements();
					if(!selectedSG.IsPool)
						selectedSG.OnCompleteSlotMovements();

					sgm.UpdateEquipStatesOnAll();
					sgm.ClearAndReset();
				}
			}
			public class SortTransaction: AbsSlotSystemTransaction{
				SlotGroup selectedSG;
				SGSorter sorter;
				public SortTransaction(SlotGroup sg, SGSorter sorter){
					selectedSG = sg;
					this.sorter = sorter;
				}
				public override Slottable TargetSB{get{return null;}}
				public override SlotGroup TargetSG{get{return selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					sgm.SetActState(SlotGroupManager.PerformingTransactionState);
					selectedSG.SetAndRunSlotMovementsForSort();
					selectedSG.SetActState(SlotGroup.PerformingTransactionState);
					selectedSG.ActionProcess.Start();
					selectedSG.CheckCompletion();
				}
				public override void OnComplete(){
					selectedSG.OnCompleteSlotMovements();
					sgm.ClearAndReset();
				}
			}
			public class InsertTransaction: AbsSlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				SlotGroup selectedSG;
				public InsertTransaction(Slottable sb){
					this.pickedSB = sgm.PickedSB;
					this.selectedSB = sb;
					this.selectedSG = sb.SG;
				}
				public override Slottable TargetSB{get{return selectedSB;}}
				public override SlotGroup TargetSG{get{return selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					sgm.CompleteTransactionOnSG(pickedSB.SG);
				}
				public override void OnComplete(){
					
					sgm.ClearAndReset();
				}
			}
			/*	dump	*/
				// public class ComplexTransaction: SlotSystemTransaction{
					// 	List<InventoryItemInstanceMock> m_removed;
					// 	List<InventoryItemInstanceMock> m_added;
					// 	SlotGroup m_sg;
					// 	SlotGroupManager m_sgm;
					// 	public ComplexTransaction(List<InventoryItemInstanceMock> removedList, List<InventoryItemInstanceMock> addedList, SlotGroup sg){
					// 		m_removed = removedList;
					// 		m_added = addedList;
					// 		m_sg = sg;
					// 		m_sgm = sg.SGM;
					// 	}
					// 	public void Indicate(){}
					// 	public void Execute(){
							
					// 		m_sgm.SetAndRunTransactionProcess(null, null, null, m_sg);
							
					// 		m_sg.SetAndRunSlotMovements(m_removed, m_added);
					// 		m_sg.SetState(SlotGroup.PerformingTransactionState);
					// 	}
					// 	public void OnComplete(){
					// 		m_sg.OnCompleteSlotMovements();
					// 		m_sgm.ClearAndReset();
					// 	}
					// }
				// public class UnequipTransaction: SlotSystemTransaction{
					// 	Slottable pickedSB;
					// 	SlotGroup selectedSG;
					// 	SlotGroup origSG;
					// 	SlotGroupManager sgm;
					// 	public UnequipTransaction(Slottable picked, SlotGroup selSG){
					// 		this.pickedSB = picked;
					// 		this.selectedSG = selSG;
					// 		this.sgm = picked.SGM;
					// 		this.origSG = sgm.GetSlotGroup(picked);
					// 	}
					// 	public void Indicate(){}
					// 	public void Execute(){

					// 		sgm.SetAndRunTransactionProcess(pickedSB, null, origSG, selectedSG);

					// 		origSG.TransactionUpdateV2(null, pickedSB);
					// 		selectedSG.TransactionUpdateV2(pickedSB, null);

					// 		Slot slot = selectedSG.GetSlot(selectedSG.GetSlottable((InventoryItemInstanceMock)pickedSB.Item));
					// 		pickedSB.MoveDraggedIcon(selectedSG, slot);
					// 		pickedSB.SetState(Slottable.MovingState);
					// 	}
					// 	public void OnComplete(){
					// 		sgm.DestroyDraggedIcon();
					// 		sgm.DestroyRemovedSB();
					// 		/*	TransactionUpdate HIDES (not removes) the pickedSB the moment the transaction is 	executed (only dragged icon persists)
					// 			performing the equip state update checks all the equipped sbs in pool to see if there's matching sb in one of equip egs, unequip if there's none
					// 		*/
					// 		sgm.UpdateEquipStatus();
					// 		sgm.ClearAndReset();
					// 	}
					// }

		/*	commands	*/
			public interface SGMCommand{
				void Execute(SlotGroupManager sgm);
			}
			/*	dump	*/
				// public class UpdateTransactionCommand: SGMCommand{
				// 	public void Execute(SlotGroupManager sgm){
				// 		Slottable pickedSB = sgm.PickedSB;
				// 		Slottable selectedSB = sgm.SelectedSB;
				// 		SlotGroup selectedSG = sgm.SelectedSG;
				// 		SlotGroup origSG = pickedSB.SG;
				// 		if(pickedSB != null){
				// 			if(selectedSB == null){// drop on SG
				// 				if(selectedSG == null || selectedSG == origSG || !origSG.IsShrinkable){
				// 					SlotSystemTransaction revertTs = new RevertTransaction();
				// 					sgm.SetTransaction(revertTs);
				// 				}else{
				// 					/*	selectedSG != null && != origSG
				// 						there's at least one vacant slot OR there's a sb of a same stackable item
				// 					*/
				// 					if(selectedSG.HasItem((InventoryItemInstanceMock)pickedSB.Item)){
				// 						if(selectedSG.IsPool){
				// 							FillEquipTransaction ta = new FillEquipTransaction(selectedSG);
				// 							sgm.SetTransaction(ta);
				// 						}else{
				// 							StackTransaction stackTs = new StackTransaction(selectedSB);
				// 							sgm.SetTransaction(stackTs);
				// 						}
				// 					}else{
				// 						EquipmentSet focusedEquipSet = (EquipmentSet)sgm.RootPage.EquipBundle.GetFocusedBundleElement();
				// 						if(focusedEquipSet.ContainsElement(selectedSG)){
				// 							if(selectedSG.Filter is SGCGearsFilter){
				// 								FillEquipTransaction fillEquipTs = new FillEquipTransaction(selectedSG);
				// 								sgm.SetTransaction(fillEquipTs);
				// 							}else{
				// 								sgm.SetSelectedSB(selectedSG.Slots[0].Sb);
				// 								SwapTransaction swapTs = new SwapTransaction(selectedSG.Slots[0].Sb);
				// 								sgm.SetTransaction(swapTs);
				// 							}
				// 						}else{
				// 							FillTransaction fillTs = new FillTransaction(selectedSG);
				// 							sgm.SetTransaction(fillTs);
				// 						}
				// 					}
				// 				}

				// 			}else{// selectedSB != null
				// 				if(pickedSB == selectedSB){
				// 					SlotSystemTransaction revertTs = new RevertTransaction();
				// 					sgm.SetTransaction(revertTs);
				// 				}else{
				// 					if(sgm.GetSlotGroup(selectedSB) == sgm.GetSlotGroup(pickedSB)){
				// 						if(!sgm.GetSlotGroup(pickedSB).IsAutoSort){
				// 							SlotSystemTransaction reorderTs = new ReorderTransaction(selectedSB);
				// 							sgm.SetTransaction(reorderTs);
				// 						}
				// 					}else{// different SGs
				// 						if(pickedSB.Item == selectedSB.Item){
				// 							if(pickedSB.IsEquipped){
												
				// 								FillEquipTransaction ta = new FillEquipTransaction(selectedSB.SG);
				// 								sgm.SetTransaction(ta);
				// 							}else{
				// 								StackTransaction stackTs = new StackTransaction(selectedSB);
				// 								sgm.SetTransaction(stackTs);
				// 							}
				// 						}else{
				// 							SwapTransaction swapTs = new SwapTransaction(selectedSB);
				// 							sgm.SetTransaction(swapTs);
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}
				// 	}
				// }
				// public class PostPickFilterCommand: SGMCommand{
				// 	public void Execute(SlotGroupManager sgm){
				// 		Slottable pickedSb = sgm.PickedSB;
				// 		SlotGroup origSG = sgm.SelectedSG;
				// 		SlotSystemBundle poolBundle = sgm.RootPage.PoolBundle;
				// 		SlotSystemBundle equipBundle = sgm.RootPage.EquipBundle;

				// 		if(poolBundle.ContainsElement(origSG)){// pickedSb in the pool
				// 			foreach(Slot slot in origSG.Slots){
				// 				if(slot.Sb != null && slot.Sb != pickedSb){
				// 					if(origSG.IsAutoSort){
				// 						slot.Sb.Defocus();
				// 					}else{
				// 						slot.Sb.Focus();
				// 					}
				// 				}
				// 			}
				// 			EquipmentSet focusedEquipmentSet = (EquipmentSet)equipBundle.GetFocusedBundleElement();
				// 			foreach(SlotSystemElement ele in focusedEquipmentSet.Elements){
				// 				SlotGroup sg = (SlotGroup)ele;
				// 				if(sg.AcceptsFilter(pickedSb)){
				// 					if(sg.Filter is SGCGearsFilter && sg.GetNextEmptySlot()==null)
				// 						sg.SetSelState(SlotGroup.DefocusedState);
				// 					else
				// 						sg.SetSelState(SlotGroup.FocusedState);
				// 					foreach(Slot slot in sg.Slots){
				// 						if(slot.Sb != null){
				// 							slot.Sb.Focus();
				// 						}
				// 					}
				// 				}else{// sg filtered out
				// 					sg.SetSelState(SlotGroup.DefocusedState);
				// 					foreach(Slot slot in sg.Slots){
				// 						if(slot.Sb != null){
				// 							slot.Sb.Defocus();
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}else{// if pickedSB in sge
				// 			SlotGroup focSGP = sgm.FocusedSGP;
				// 			if(focSGP.AcceptsFilter(pickedSb)){
				// 				foreach(Slottable sbp in focSGP.Slottables){
				// 					if(sbp != null){
				// 						if(Util.HaveCommonItemFamily(sbp, pickedSb)){
				// 							if(object.ReferenceEquals(sbp.Item, pickedSb.Item)){
				// 								if(origSG.IsShrinkable)// unequip
				// 									sbp.Focus();
				// 								else
				// 									sbp.Defocus();
				// 							}
				// 							else{
				// 								if(sbp.IsEquipped)
				// 									sbp.Defocus();
				// 								else
				// 									sbp.Focus();
				// 							}
				// 						}else{	// different item family
				// 							sbp.Defocus();
				// 						}
				// 					}
				// 				}
				// 			}else{
				// 				focSGP.Defocus();
				// 			}
				// 			foreach(SlotGroup sge in sgm.FocusedSGEs){
				// 				if(sge != origSG){
				// 					if(sge.AcceptsFilter(pickedSb)){
				// 						sge.SetSelState(SlotGroup.FocusedState);
				// 						foreach(Slot slot in sge.Slots){
				// 							if(slot.Sb != null)
				// 								slot.Sb.Focus();
				// 						}
				// 					}else{
				// 						sge.SetSelState(SlotGroup.DefocusedState);
				// 						foreach(Slot slot in sge.Slots){
				// 							if(slot.Sb != null)
				// 								slot.Sb.Defocus();
				// 						}
				// 					}
				// 				}else{// sge == origSG, the state is Selected
				// 					foreach(Slot slot in sge.Slots){
				// 						if(slot.Sb != null){
				// 							if(slot.Sb != pickedSb){
				// 								if(!sge.IsAutoSort)
				// 									slot.Sb.Focus();
				// 								else
				// 									slot.Sb.Defocus();
				// 							}
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}
				// 	}
				// }
				// public class PostPickFilterCommand: SGMCommand{
				// 	public void Execute(SlotGroupManager sgm){
				// 		if(sgm.PickedSB != null){
							
				// 			if(sgm.PickedSB.Item is BowInstanceMock){
				// 				foreach(SlotGroup sg in sgm.SlotGroups){
				// 					if(sg.CurState != SlotGroup.SelectedState){
				// 						if(sg.CurState == SlotGroup.FocusedState){
				// 							if(!(sg.Filter is SGNullFilter) && !(sg.Filter is SGBowFilter))
				// 								sg.SetState(SlotGroup.DefocusedState);
				// 						}
				// 					}
				// 				}
				// 			}else if(sgm.PickedSB.Item is WearInstanceMock){
				// 				foreach(SlotGroup sg in sgm.SlotGroups){
				// 					if(sg.CurState != SlotGroup.SelectedState){
				// 						if(sg.CurState == SlotGroup.FocusedState){
				// 							if(!(sg.Filter is SGNullFilter) && !(sg.Filter is SGWearFilter))
				// 								sg.SetState(SlotGroup.DefocusedState);
				// 						}
				// 					}
				// 				}
				// 			}else if(sgm.PickedSB.Item is PartsInstanceMock){
				// 				foreach(SlotGroup sg in sgm.SlotGroups){
				// 					if(sg.CurState != SlotGroup.SelectedState){
				// 						if(sg.CurState == SlotGroup.FocusedState){
				// 							if(!(sg.Filter is SGNullFilter) && !(sg.Filter is SGPartsFilter))
				// 								sg.SetState(SlotGroup.DefocusedState);
				// 						}
				// 					}
				// 				}
				// 			}
				// 			foreach(SlotGroup sg in sgm.SlotGroups){
				// 				if(sg.CurState == SlotGroup.FocusedState || sg.CurState == SlotGroup.SelectedState){
				// 					// if(sg.Filter is SGNullFilter)
				// 						// sg.UpdateSbState();
				// 				}

				// 			}
				// 		}
				// 	}
				// }
				
				// public class PostPickFilterV2Command: SGMCommand{
				// 	public void Execute(SlotGroupManager sgm){
				// 		Slottable pickedSb = sgm.PickedSB;
				// 		SlotGroup origSG = sgm.SelectedSG;
				// 		SlotSystemBundle poolBundle = sgm.RootPage.PoolBundle;
				// 		SlotSystemBundle equipBundle = sgm.RootPage.EquipBundle;

				// 		if(poolBundle.ContainsElement(origSG)){
				// 			foreach(Slot slot in origSG.Slots){
				// 				if(slot.Sb != null && slot.Sb != pickedSb){
				// 					if(origSG.IsAutoSort){
				// 						if(slot.Sb.IsEquipped)
				// 							slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 						else
				// 							slot.Sb.SetState(Slottable.DefocusedState);
				// 					}else{
				// 						if(slot.Sb.IsEquipped)
				// 							slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
				// 						else
				// 							slot.Sb.SetState(Slottable.FocusedState);
				// 					}
				// 				}
				// 			}
				// 		}else{
				// 			foreach(SlotSystemElement ele in equipBundle.Elements){
				// 				EquipmentSet equipSet = (EquipmentSet)ele;
				// 				if(equipSet.ContainsElement(origSG)){
				// 					foreach(SlotSystemElement nestedEle in equipSet.Elements){
				// 						SlotGroup sg = (SlotGroup)nestedEle;
				// 						if(sg != origSG){
				// 							if(sg.AcceptsFilter(pickedSb)){
				// 								sg.SetState(SlotGroup.FocusedState);
				// 								sg.Slots[0].Sb.SetState(Slottable.EquippedAndDeselectedState);
				// 							}else{
				// 								sg.SetState(SlotGroup.DefocusedState);
				// 								sg.Slots[0].Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 							}
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}
				// 	}
				// }
		/*	state	*/
			/*	superclass */
				public class SGMStateEngine: SwitchableStateEngine{
					public SGMStateEngine(SlotGroupManager sgm){
						this.handler = sgm;
					}
					public void SetState(SGMState sgmState){
						base.SetState(sgmState);
					}
				}
				public abstract class SGMState: SwitchableState{
					protected SlotGroupManager sgm;
					public virtual void EnterState(StateHandler handler){
						sgm = (SlotGroupManager)handler;
					}
					public virtual void ExitState(StateHandler handler){
						sgm = (SlotGroupManager)handler;
					}
				}
				public abstract class SGMSelectionState: SGMState{
				}
				public abstract class SGMActionState: SGMState{
				}
			/*	Selection state	*/
				public class SGMDeactivatedState: SGMSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sgm.SetAndRunSelProcess(null);
						sgm.SetActState(SlotGroupManager.WaitForActionState);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGMDefocusedState: SGMSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sgm.PrevSelState == SlotGroupManager.FocusedState)
							sgm.SetAndRunSelProcess(new SGMGreyoutProcess(sgm, sgm.GreyoutCoroutine));
						sgm.RootPage.Defocus();
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGMFocusedState: SGMSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sgm.PrevSelState == SlotGroupManager.DefocusedState)
							sgm.SetAndRunSelProcess(new SGMGreyinProcess(sgm, sgm.GreyinCoroutine));
						sgm.RootPage.Focus();
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	Action state	*/
				public class SGMWaitForActionState: SGMActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sgm.SetAndRunActProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGMProbingState: SGMActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sgm.PrevActState == SlotGroupManager.PerformingTransactionState)
							sgm.SetAndRunActProcess(new SGMProbeProcess(sgm, sgm.ProbeCoroutine));
						else
							throw new System.InvalidOperationException("SGMProbingState: Entering from an invalid state");
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGMPerformingTransactionState: SGMActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sgm.SetAndRunActProcess(new SGMTransactionProcess(sgm));
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				/*	dump	*/
					// public interface SGMState{
					// 	void EnterState(SlotGroupManager sgm);	
					// 	void ExitState(SlotGroupManager sgm);	
					// }
		/*	process	*/
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
				public System.Func<IEnumeratorMock> CoroutineMock{
					get{return m_coroutineMock;}
					set{m_coroutineMock = value;}
					}System.Func<IEnumeratorMock> m_coroutineMock;
				public SlotGroupManager SGM{
					get{return m_sgm;}
					set{m_sgm = value;}
					}SlotGroupManager m_sgm;
				public bool IsRunning{get{return m_isRunning;}
					}bool m_isRunning;
				public bool IsExpired{get{return m_isExpired;}
					}bool m_isExpired;
				public virtual void Start(){
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
			public class SGMGreyinProcess: AbsSGMProcess{
				public SGMGreyinProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SGMGreyoutProcess: AbsSGMProcess{
				public SGMGreyoutProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SGMProbeProcess: AbsSGMProcess{
				public SGMProbeProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class SGMTransactionProcess: AbsSGMProcess{
				public SGMTransactionProcess(SlotGroupManager sgm){
					this.SGM = sgm;
					this.CoroutineMock = sgm.WaitForTransactionDone;
				}
				// Slottable m_pickedSB;
				// Slottable m_selectedSB;
				// SlotGroup m_origSG;
				// SlotGroup m_selectedSG;
				// public SGMTransactionProcess(SlotGroupManager sgm ,Slottable pickedSB, Slottable selectedSB, SlotGroup origSG, SlotGroup selectedSG){
				// 	this.m_pickedSB = pickedSB;
				// 	this.m_selectedSB = selectedSB;
				// 	this.m_origSG = origSG;
				// 	this.m_selectedSG = selectedSG;
				// 	this.SGM = sgm;
				// 	this.CoroutineMock = SGM.WaitForTransactionDone;
				// }
				public override void Start(){
					// SGM.SetTransactionFields(m_pickedSB, m_selectedSB, m_origSG, m_selectedSG);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.OnAllTransactionComplete();
				}
			}
			/*	dump	*/
				// public class SGMRevertTransactionProcess: AbsSGMProcess{
				// 	public SGMRevertTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
				// 		SGM.SetSelectedSBDoneTransaction(null, true);
				// 		SGM.SetOrigSGDoneTransaction(null, true);
				// 		SGM.SetSelectedSGDoneTransaction(null, true);
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
				// public class SGMFillTransactionProcess: AbsSGMProcess{
				// 	public SGMFillTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(SGM.PickedSB,false);
				// 		SGM.SetSelectedSBDoneTransaction(null,true);
				// 		SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB), false);
				// 		SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
				// public class SGMFillEquipTransactionProcess: AbsSGMProcess{
				// 	public SGMFillEquipTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
				// 		SGM.SetSelectedSBDoneTransaction(null, true);
				// 		SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB),false);
				// 		SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
				// public class SGMUnequipTransactionProcess: AbsSGMProcess{
				// 	public SGMUnequipTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
				// 		SGM.SetSelectedSBDoneTransaction(null, true);
				// 		SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB),false);
				// 		SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
				// public class SGMSwapTransactionProcess: AbsSGMProcess{
				// 	public SGMSwapTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
				// 		SGM.SetSelectedSBDoneTransaction(SGM.SelectedSB, false);
				// 		SGM.SetOrigSGDoneTransaction(SGM.GetSlotGroup(SGM.PickedSB),false);
				// 		SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
						
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
				// public class SGMSortTransactionProcess: AbsSGMProcess{
				// 	public SGMSortTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(null, true);
				// 		SGM.SetSelectedSBDoneTransaction(null, true);
				// 		SGM.SetOrigSGDoneTransaction(null ,true);
				// 		SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
				// public class SGMComplexTransactionProcess: AbsSGMProcess{
				// 	public SGMComplexTransactionProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(null, true);
				// 		SGM.SetSelectedSBDoneTransaction(null, true);
				// 		SGM.SetOrigSGDoneTransaction(null ,true);
				// 		SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
				// public class SGMReorderProcess: AbsSGMProcess{
				// 	public SGMReorderProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SGM = sgm;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Start(){
				// 		SGM.SetPickedSBDoneTransaction(SGM.PickedSB, false);
				// 		SGM.SetSelectedSBDoneTransaction(null, true);
				// 		SGM.SetOrigSGDoneTransaction(null ,true);
				// 		SGM.SetSelectedSGDoneTransaction(SGM.SelectedSG, false);
				// 		base.Start();
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SGM.CompleteAllTransaction();
				// 	}
				// }
	/*	SlotGroup Classes	*/
		/*	states	*/
			/*	superclasses	*/
				public class SGStateEngine: SwitchableStateEngine{
					public SGStateEngine(SlotGroup sg){
						this.handler = sg;
					}
					public void SetState(SGState sgState){
						base.SetState(sgState);
					}
				}
				public abstract class SGState: SwitchableState{
					protected SlotGroup sg;
					public virtual void EnterState(StateHandler handler){
						sg = (SlotGroup)handler;
					}
					public virtual void ExitState(StateHandler handler){
						sg = (SlotGroup)handler;
					}
				}
				public abstract class SGSelectionState: SGState{
					public virtual void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventDataMock){
						sg.SGM.SetHoveredSG(sg);
					}
					public virtual void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventDataMock){

					}
				}
				public abstract class SGActionState: SGState{
				}
			/*	Selection States	*/
				public class SGDeactivatedState : SGSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sg.SetAndRunSelProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGDefocusedState: SGSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SGProcess process = null;
						if(sg.PrevSelState == SlotGroup.DeactivatedState){
							process = null;
							sg.InstantGreyout();
						}else if(sg.PrevSelState == SlotGroup.FocusedState)
							process = new SGGreyoutProcess(sg, sg.GreyoutCoroutine);
						else if(sg.PrevSelState == SlotGroup.SelectedState)
							process = new SGDehighlightProcess(sg, sg.GreyoutCoroutine);
						sg.SetAndRunSelProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGFocusedState: SGSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SGProcess process = null;
						if(sg.PrevSelState == SlotGroup.DeactivatedState){
							process = null;
							sg.InstantGreyin();
						}
						else if(sg.PrevSelState == SlotGroup.DefocusedState)
							process = new SGGreyinProcess(sg, sg.GreyinCoroutine);
						else if(sg.PrevSelState == SlotGroup.SelectedState)
							process = new SGDehighlightProcess(sg, sg.DehighlightCoroutine);
						sg.SetAndRunSelProcess(process);
					}
					
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGSelectedState: SGSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SGProcess process = null;
						if(sg.PrevSelState == SlotGroup.DeactivatedState){
							sg.InstantHighlight();
						}else if(sg.PrevSelState == SlotGroup.DefocusedState)
							process = new SGHighlightProcess(sg, sg.HighlightCoroutine);
						else if(sg.PrevSelState == SlotGroup.FocusedState)
							process = new SGHighlightProcess(sg, sg.HighlightCoroutine);
						sg.SetAndRunSelProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	Action State	*/
				public class SGWaitForActionState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sg.SetAndRunActProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGPerformingTransactionState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	dump	*/
				// public class SGSortingState: SlotGroupState{
				// 	public void EnterState(SlotGroup sg){
				// 		SGProcess process = new SGSortingProcess(sg, sg.WaitForAllSlotMovementsDone);
				// 		sg.SetAndRun(process);
				// 		/*	implement SlotMovements creation and stuff in the process
				// 		*/
				// 	}
				// 	public void ExitState(SlotGroup sg){}
				// 	public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){}
				// 	public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				// 	}
				// 	public void Focus(SlotGroup sg){
				// 		sg.SetState(SlotGroup.FocusedState);
				// 		sg.FocusSBs();
				// 		// sg.SetState(prevState);
				// 	}
				// 	public void Defocus(SlotGroup sg){
				// 	}
				// }
				// public class SGComplexTransactionState: SlotGroupState{
				// 	public void EnterState(SlotGroup sg){
				// 		SGProcess process = new SGSortingProcess(sg, sg.WaitForAllSlotMovementsDone);
				// 		sg.SetAndRun(process);
				// 	}
				// 	public void ExitState(SlotGroup sg){}
				// 	public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){}
				// 	public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				// 	}
				// 	public void Focus(SlotGroup sg){
				// 		sg.SetState(SlotGroup.FocusedState);
				// 		sg.FocusSBs();
				// 		// sg.SetState(prevState);
				// 	}
				// 	public void Defocus(SlotGroup sg){
				// 	}
				// }
		/*	process	*/
			public interface SGProcess{
				bool IsRunning{get;}
				bool IsExpired{get;}
				System.Func<IEnumeratorMock> CoroutineMock{set;}
				SlotGroup SG{set;}
				void Start();
				void Stop();
				void Expire();
				void Check();
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
					// SG.RunningProcess.Add(this);
					m_coroutineMock();
				}
				public virtual void Stop(){
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = false;
						// SG.RunningProcess.Remove(this);
					}
				}
				public virtual void Expire(){
					if(m_isRunning){
						m_isRunning = false;
						m_isExpired = true;
						// SG.RunningProcess.Remove(this);
					}
				}
				public void Check(){
					m_coroutineMock();
				}
			}
			public class SGGreyinProcess: AbsSGProcess{
				public SGGreyinProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
			}
			public class SGGreyoutProcess: AbsSGProcess{
				public SGGreyoutProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
			}
			public class SGHighlightProcess: AbsSGProcess{
				public SGHighlightProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
			}
			public class SGDehighlightProcess: AbsSGProcess{
				public SGDehighlightProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
			}
			public class SGTransactionProcess: AbsSGProcess{
				public SGTransactionProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
					this.SG = sg;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SG.SGM.CompleteTransactionOnSG(SG);
				}
			}
			public class SlotMovement{
				Slottable m_sb;
				public Slottable SB{
					get{return m_sb;}
				}
				SlotGroup m_sg;
				int m_curSlotID;
				int m_newSlotID;
				public SlotMovement(SlotGroup sg, Slottable sb, int curSlotID, int newSlotID){
					m_sg = sg;
					m_sb = sb;
					m_curSlotID = curSlotID;
					m_newSlotID = newSlotID;
					m_sg.AddSlotMovement(this);
				}
				public void Execute(){
					StateTransit();
				}
				public void GetIndex(out int curId, out int newId){
					curId = m_curSlotID;
					newId = m_newSlotID;
				}
				public void StateTransit(){
					if(m_curSlotID == -1)
						m_sb.SetActState(Slottable.AddedState);
					else if(m_curSlotID == -2)
						m_sb.SetActState(Slottable.MovingInState);
					else if(m_newSlotID == -1)
						m_sb.SetActState(Slottable.RemovedState);
					else if(m_newSlotID == -2)
						m_sb.SetActState(Slottable.MovingOutState);
					else{
						m_sb.SetActState(Slottable.MovingInSGState);
					}

				}
			}
			/*	dump	*/
				// public class SGInstantGreyoutProcess: AbsSGProcess{
				// 	public SGInstantGreyoutProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SG = sg;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	/*	overridden functions
				// 	*/
				// 		public override void Start(){
				// 			base.Start();
				// 		}
				// 		public override void Stop(){
				// 			base.Stop();
				// 		}
				// 		public override void Expire(){
				// 			base.Expire();
				// 		}
				// }
				// public class SGInstantGreyinProcess: AbsSGProcess{
				// 	public SGInstantGreyinProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SG = sg;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	/*	overridden functions
				// 	*/
				// 		public override void Start(){
				// 			base.Start();
				// 		}
				// 		public override void Stop(){
				// 			base.Stop();
				// 		}
				// 		public override void Expire(){
				// 			base.Expire();
				// 		}
				// }
				// public class SGUpdateTransactionProcess: AbsSGProcess{
				// 	public SGUpdateTransactionProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SG = sg;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
					
				// 	/*	overridden functions
				// 	*/
				// 		public override void Start(){
				// 			base.Start();
							
				// 		}
				// 		public override void Stop(){
				// 			base.Stop();
				// 		}
				// 		public override void Expire(){
				// 			base.Expire();
				// 			SG.SGM.CompleteTransactionOnSG(SG);
				// 		}
				// }
				// public class SGSortingProcess: AbsSGProcess{
				// 	public SGSortingProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SG = sg;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	/*	overridden functions
				// 	*/
				// 		public override void Start(){
				// 			// base.Start();
				// 			/*	implement SlotMovements here
				// 					Create SlotMovement instance for every slottables
				// 						1. Get current Slot index
				// 						2. Sort SG
				// 						3. Get new Slot index
				// 						4. pass 1, 3, and the slottable into it
				// 						5. and execute it
				// 						6. store each SlotMovement into sg's slotMovements list
				// 			*/
				// 			List<Slottable> newSlotOrderList = SG.OrderedSbs();
				// 			for(int i = 0; i < SG.Slots.Count; i++){
				// 				if(SG.Slots[i].Sb != null){
				// 					Slottable sb = SG.Slots[i].Sb;
				// 					int curSlotID = sb.SlotID;
				// 					int newSlotID = newSlotOrderList.IndexOf(sb);
				// 					SlotMovement slotMovement = new SlotMovement(SG, sb, curSlotID, newSlotID);
				// 					// slotMovement.Execute();
				// 				}
				// 			}
				// 			SG.ExecuteSlotMovements();
				// 			base.Start();
				// 		}
				// 		public override void Stop(){
				// 			base.Stop();
				// 		}
				// 		public override void Expire(){
				// 			SG.InstantSort();
				// 			base.Expire();
				// 			SG.SGM.CompleteTransactionOnSG(SG);
				// 		}
				// }
				// public class SGFocusedProcess: AbsSGProcess{
				// 	public SGFocusedProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SG = sg;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	/*	overridden functions
				// 	*/
				// 		public override void Start(){
				// 			base.Start();
				// 		}
				// 		public override void Stop(){
				// 			base.Stop();
				// 		}
				// 		public override void Expire(){
				// 			base.Expire();
				// 		}
				// }
		/*	commands	*/
			public interface SlotGroupCommand{
				void Execute(SlotGroup Sg);
			}
			public class SGInitItemsCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.FilterItems();//setup Items list
					sg.CreateSlots();
					sg.CreateSlottables();
					if(sg.IsAutoSort)
						sg.InstantSort();
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
						sb.Initialize(sg.SGM, sg, true, item);
						sg.Slots[i].Sb = sb;
					}
				}
			}
			/*	dump	*/
				// public class SGFocusCommandV2: SlotGroupCommand{
				// 	public void Execute(SlotGroup sg){
				// 		sg.CurState.Focus(sg);
				// 	}
				// }
				// public class SGDefocusCommandV2: SlotGroupCommand{
				// 	public void Execute(SlotGroup sg){
				// 		sg.CurState.Defocus(sg);
				// 	}
				// }
				// public class SGWakeupCommand: SlotGroupCommand{
				// 	public void Execute(SlotGroup sg){
				// 		if(sg.Scroller == null){
				// 			sg.SetState(SlotGroup.FocusedState);
				// 		}else{
				// 			if(sg == sg.SGM.InitiallyFocusedSG)
				// 				sg.Focus();
				// 		}
				// 		// sg.UpdateSbState();
				// 	}
				// }
				// public class UpdateEquipStatusForPoolCommmand: SlotGroupCommand{
				// 	public void Execute(SlotGroup sg){
				// 		foreach(Slot slot in sg.Slots){
				// 			if(slot.Sb != null){
				// 				if(slot.Sb.IsEquipped){
				// 					if(slot.Sb.Item is BowInstanceMock){
				// 						BowInstanceMock equippedBow = sg.SGM.GetEquippedBow();
				// 						if(equippedBow != (BowInstanceMock)slot.Sb.Item)
				// 							slot.Sb.Unequip();
				// 					}else if(slot.Sb.Item is WearInstanceMock){
				// 						WearInstanceMock equippedWear = sg.SGM.GetEquippedWear();
				// 						if(equippedWear != (WearInstanceMock)slot.Sb.Item)
				// 							slot.Sb.Unequip();
				// 					}else if(slot.Sb.Item is CarriedGearInstanceMock){
				// 						List<CarriedGearInstanceMock> cGears = sg.SGM.GetEquippedCarriedGears();
				// 						if(cGears.Count == 0)
				// 							slot.Sb.Unequip();
				// 						else{
				// 							bool found = false;
				// 							foreach(CarriedGearInstanceMock cGear in cGears){
				// 								if(cGear == (CarriedGearInstanceMock)slot.Sb.Item)
				// 									found = true;
				// 							}
				// 							if(!found)
				// 								slot.Sb.Unequip();
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}
				// 	}
				// }
				// public class UpdateEquipStatusForEquipSGCommand: SlotGroupCommand{
				// 	public void Execute(SlotGroup sg){
				// 		foreach(Slot slot in sg.Slots){
				// 			if(slot.Sb != null){
				// 				InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
				// 				invItem.IsEquipped = true;
				// 			}
				// 		}
				// 	}
				// }
				// public class UpdateSbStateCommand: SlotGroupCommand{
				// 	public void Execute(SlotGroup sg){
				// 		SlotGroupManager sgm = sg.SGM;
				// 		if(sg.CurState == SlotGroup.DefocusedState){
				// 			foreach(Slot slot in sg.Slots){
				// 				if(slot.Sb != null){
				// 					InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
				// 					if(invItem.IsEquipped){
				// 						slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 					}else
				// 						slot.Sb.SetState(Slottable.DefocusedState);
				// 				}
				// 			}
				// 		}else if(sg.CurState == SlotGroup.FocusedState){
				// 			foreach(Slot slot in sg.Slots){
				// 				if(slot.Sb != null){
				// 					if(slot.Sb != sgm.PickedSB){

				// 						InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
				// 						if(invItem.IsEquipped){
				// 							if(sgm.PickedSB == null)
				// 								slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
				// 							else if(slot.Sb.Item is BowInstanceMock){
				// 								if(sgm.PickedSB.Item is BowInstanceMock){
				// 									if(object.ReferenceEquals(sgm.PickedSB.Item, slot.Sb.Item))
				// 										slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 									else
				// 										slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
				// 								}
				// 								else
				// 									slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 							}else if(slot.Sb.Item is WearInstanceMock){
				// 								if(sgm.PickedSB.Item is WearInstanceMock)
				// 									if(object.ReferenceEquals(sgm.PickedSB.Item, slot.Sb.Item))
				// 										slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 									else
				// 										slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
				// 								else
				// 									slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 							}else
				// 								slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 						}
				// 						else{
				// 							if(!(sg.Filter is SGPartsFilter) && (invItem is PartsInstanceMock))
				// 								slot.Sb.SetState(Slottable.DefocusedState);
				// 							else{
				// 								if(sgm.PickedSB == null)
				// 									slot.Sb.SetState(Slottable.FocusedState);
				// 								else{
				// 									if(invItem is BowInstanceMock){
				// 										if(sgm.PickedSB.Item is BowInstanceMock)
				// 											slot.Sb.SetState(Slottable.FocusedState);
				// 										else
				// 											slot.Sb.SetState(Slottable.DefocusedState);
				// 									}else if(invItem is WearInstanceMock){
				// 										if(sgm.PickedSB.Item is WearInstanceMock)
				// 											slot.Sb.SetState(Slottable.FocusedState);
				// 										else
				// 											slot.Sb.SetState(Slottable.DefocusedState);
				// 									}else if(invItem is PartsInstanceMock){
				// 										if(sgm.PickedSB.Item is PartsInstanceMock)
				// 											slot.Sb.SetState(Slottable.FocusedState);
				// 										else
				// 											slot.Sb.SetState(Slottable.DefocusedState);
				// 									}else{
				// 										slot.Sb.SetState(Slottable.DefocusedState);
				// 									}
				// 								}
				// 							}
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}else if(sg.CurState == SlotGroup.SelectedState){
				// 			//pickedSB != null;
				// 			foreach(Slot slot in sg.Slots){
				// 				if(slot.Sb != null){
				// 					if(sgm.PickedSB != slot.Sb){
				// 						BowInstanceMock equippedBow = sgm.GetEquippedBow();
				// 						WearInstanceMock equippedWear = sgm.GetEquippedWear();
				// 						if(sg.IsAutoSort){
				// 							if(object.ReferenceEquals(slot.Sb.Item, equippedBow) || (object.ReferenceEquals(slot.Sb.Item, equippedWear)))
				// 								slot.Sb.SetState(Slottable.EquippedAndDefocusedState);
				// 							else
				// 								slot.Sb.SetState(Slottable.DefocusedState);
				// 						}else{
				// 							if(object.ReferenceEquals(slot.Sb.Item, equippedBow) || (object.ReferenceEquals(slot.Sb.Item, equippedWear)))
				// 								slot.Sb.SetState(Slottable.EquippedAndDeselectedState);
				// 							else
				// 								slot.Sb.SetState(Slottable.FocusedState);
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}
				// 	}
				// }
				// public class UpdateSbStateCommandV2: SlotGroupCommand{
				// 	public void Execute(SlotGroup sg){
				// 		/*	call Focus on all sbs?
				// 		*/
				// 	}
				// }
		/*	filters	*/
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
			public class SGCGearsFilter: SGFilter{
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
			
		/*	sorters	*/
			public interface SGSorter{
				List<Slottable> OrderedSBs(SlotGroup sg);
				void OrderSBs(ref List<Slottable> sbs);
				void OrderSBsWOSpace(ref List<Slottable> sbs);
			}	
			public class　SGItemIDSorter: SGSorter{
				public List<Slottable> OrderedSBs(SlotGroup sg){
					
					List<Slottable> sbList = new List<Slottable>();
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null)
							sbList.Add(slot.Sb);
					}
					sbList.Sort();
					return sbList;
				}
				public void OrderSBs(ref List<Slottable> sbs){
					int origCount = sbs.Count;
					List<Slottable> trimmed = sbs;
					this.OrderSBsWOSpace(ref trimmed);
					while(trimmed.Count < origCount){
						trimmed.Add(null);
					}
					sbs = trimmed;
				}
				public void OrderSBsWOSpace(ref List<Slottable> sbs){
					List<Slottable> trimmed = new List<Slottable>();
					foreach(Slottable sb in sbs){
						if(sb != null)
							trimmed.Add(sb);
					}
					trimmed.Sort();
					sbs = trimmed;
				}
			}
			public class SGInverseItemIDSorter: SGSorter{
				public List<Slottable> OrderedSBs(SlotGroup sg){
					List<Slottable> sbList = new List<Slottable>();
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null)
							sbList.Add(slot.Sb);
					}
					sbList.Sort();
					sbList.Reverse();
					return sbList;
				}
				public void OrderSBs(ref List<Slottable> sbs){
					int origCount = sbs.Count;
					List<Slottable> trimmed = sbs;
					this.OrderSBsWOSpace(ref trimmed);
					while(trimmed.Count < origCount){
						trimmed.Add(null);
					}
					sbs = trimmed;
				}
				public void OrderSBsWOSpace(ref List<Slottable> sbs){
					List<Slottable> trimmed = new List<Slottable>();
					foreach(Slottable sb in sbs){
						if(sb != null)
							trimmed.Add(sb);
					}
					trimmed.Sort();
					trimmed.Reverse();
					sbs = trimmed;
				}
			}
			public class SGAcquisitionOrderSorter: SGSorter{
				
				public List<Slottable> OrderedSBs(SlotGroup sg){
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
				public void OrderSBs(ref List<Slottable> sbs){
					int origCount = sbs.Count;
					List<Slottable> trimmed = sbs;
					this.OrderSBsWOSpace(ref trimmed);
					while(trimmed.Count < origCount){
						trimmed.Add(null);
					}
					sbs = trimmed;
				}
				public void OrderSBsWOSpace(ref List<Slottable> sbs){
					List<Slottable> trimmed = new List<Slottable>();
					foreach(Slottable sb in sbs){
						if(sb != null)
							trimmed.Add(sb);
					}
					List<Slottable> temp = new List<Slottable>();
					Slottable addedMax = null;
					while(temp.Count < trimmed.Count){
						int indexAtMin = -1;
						int addedAO;
						if(addedMax == null) addedAO = -1;
						else addedAO = ((InventoryItemInstanceMock)addedMax.Item).AcquisitionOrder;

						for(int i = 0; i < trimmed.Count; i++){
							InventoryItemInstanceMock inst = (InventoryItemInstanceMock)trimmed[i].Item;
							if(inst.AcquisitionOrder > addedAO){
								if(indexAtMin == -1 || inst.AcquisitionOrder < ((InventoryItemInstanceMock)trimmed[indexAtMin].Item).AcquisitionOrder){
									indexAtMin = i;
								}
							}
						}
						Slottable added = trimmed[indexAtMin];
						temp.Add(added);
						addedMax = added;
					}
					sbs = temp;
				}
			}
	/*	Slottable Classses	*/
		/*	process	*/
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
			/*	Selecttion process	*/
				public class SBGreyoutProcess: AbsSBProcess{
					public SBGreyoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
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
			/*	Action process	*/
				public class WaitForPointerUpProcess: AbsSBProcess{
					public WaitForPointerUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						this.SB = sb;
						this.CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						SB.SetSelState(Slottable.DefocusedState);
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
				public class PickedUpProcess: AbsSBProcess{
					public PickedUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						this.SB = sb;
						this.CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
					}
				}
				public class WaitForNextTouchProcess: AbsSBProcess{
					public WaitForNextTouchProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						this.SB = sb;
						this.CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						if(!SB.IsPickedUp){
							SB.Tap();
							SB.Focus();
						}else{
							SB.ExecuteTransaction();
						}
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
						SB.SetSelState(Slottable.FocusedState);
					}
				}
				public class SBRemovedProcess: AbsSBProcess{
					public SBRemovedProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						SB.ClearDraggedIconDestination();
					}
				}
				public class SBAddedProcess: AbsSBProcess{
					public SBAddedProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						SB.ClearDraggedIconDestination();
					}
				}
				public class SBMoveInSGProcess: AbsSBProcess{
					public SBMoveInSGProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						SB.ClearDraggedIconDestination();
					}
				}
				public class SBRevertProcess: AbsSBProcess{
					public SBRevertProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						SB.ClearDraggedIconDestination();
						SB.SGM.CompleteTransactionOnSB(SB);
					}
				}
				public class SBMoveOutProcess: AbsSBProcess{
					public SBMoveOutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						SB.ClearDraggedIconDestination();
						SB.SGM.CompleteTransactionOnSB(SB);
					}
				}
				public class SBMoveInProcess: AbsSBProcess{
					public SBMoveInProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						SB.ClearDraggedIconDestination();
						SB.SGM.CompleteTransactionOnSB(SB);
					}
				}
			/*	Equip process*/
				public class SBUnequipProcess: AbsSBProcess{
					public SBUnequipProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						this.SB = sb;
						this.CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						// SB.Tap();
						// SB.SetSelState(Slottable.FocusedState);
					}
				}
				public class SBEquipProcess: AbsSBProcess{
					public SBEquipProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						// SB.ClearDraggedIconDestination();
						// SB.SGM.CompleteTransactionOnSB(SB);
					}
				}
			/*	dump	*/
				// public class SBUnequipAndGreyoutProcess: AbsSBProcess{
				// 	public SBUnequipAndGreyoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		SB = sb;
				// 		CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 	}	
				// }
				// public class SBEquipAndGreyoutProcess: AbsSBProcess{
				// 	public SBEquipAndGreyoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		SB = sb;
				// 		CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 	}	
				// }			
				// public class SBUnequipAndGreyinProcess: AbsSBProcess{
				// 	public SBUnequipAndGreyinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SB = sb;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 	}
				// }
				// public class SBEquipAndGreyinProcess: AbsSBProcess{
				// 	public SBEquipAndGreyinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SB = sb;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 	}
				// }
				// public class SBUnequipAndDehighlightProcess: AbsSBProcess{
				// 	public SBUnequipAndDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SB = sb;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 	}
				// }
				// public class SBEquipAndDehighlightProcess: AbsSBProcess{
				// 	public SBEquipAndDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SB = sb;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 	}
				// }
				// public class WaitForNextTouchWhilePUProcess: AbsSBProcess{
				// 	public WaitForNextTouchWhilePUProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SB = sb;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SB.SGM.Transaction.Execute();
				// 	}
				// }
				// public class SBRemovingProcess: AbsSBProcess{
				// 	public SBRemovingProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		SB = sb;
				// 		CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SB.ClearDraggedIconDestination();
				// 		SB.SGM.CompleteTransactionOnSB(SB);
				// 	}
				// }
				// public class SBReorderingProcess: AbsSBProcess{
				// 	public SBReorderingProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		SB = sb;
				// 		CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SB.ClearDraggedIconDestination();
				// 		SB.SGM.CompleteTransactionOnSB(SB);
				// 	}
				// }
				// public class SBUnpickingProcess: AbsSBProcess{
				// 	public SBUnpickingProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		SB = sb;
				// 		CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		base.Expire();
				// 		SB.ClearDraggedIconDestination();
				// 		SB.SGM.CompleteTransactionOnSB(SB);
				// 	}
				// }
				// public class MoveProcess: AbsSBProcess{
				// 	public MoveProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
				// 		this.SB = sb;
				// 		this.CoroutineMock = coroutineMock;
				// 	}
				// 	public override void Expire(){
				// 		/*	destroy dragged icon
				// 			complete translation

				// 			make defocused non pool SGs focused
				// 			make selectedSG focused
				// 			then updateSBstates
				// 		*/
				// 		base.Expire();
				// 		SB.ClearDraggedIconDestination();
				// 		SB.SGM.CompleteTransactionOnSB(SB);
				// 	}
				// }
		/*	states	*/
			/*	superclasses	*/
				public class SBStateEngine: SwitchableStateEngine{
					public SBStateEngine(Slottable sb){
						this.handler = sb;
					}
					public void SetState(SBState sbState){
						base.SetState(sbState);
					}
				}
				public abstract class SBState: SwitchableState{
					protected Slottable sb;
					public virtual void EnterState(StateHandler handler){
						sb = (Slottable)handler;
					}
					public virtual void ExitState(StateHandler handler){
						sb = (Slottable)handler;
					}

				}
				public abstract class SBSelectionState: SBState{
					public virtual void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.SGM.SetHoveredSB(sb);
					}
					public virtual void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					}
				}
				public abstract class SBActionState: SBState{
					public abstract void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock);
					public abstract void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock);
					public abstract void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock);
					public abstract void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock);
				}
				public abstract class SBEquipState: SBState{

				}
			/*	SB Selection States	*/
				public class SBDeactivatedState: SBSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sb.SetAndRunSelectionProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBDefocusedState: SBSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBProcess process = null;
						if(sb.CurSelState == Slottable.DeactivatedState){
							sb.InstantGreyout();
							process = null;
						}else if(sb.PrevSelState == Slottable.FocusedState){
							process = new SBGreyoutProcess(sb, sb.GreyoutCoroutine);
						}else if(sb.PrevSelState == Slottable.SelectedState){
							process = new SBGreyoutProcess(sb, sb.GreyoutCoroutine);
						}
						sb.SetAndRunSelectionProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBFocusedState: SBSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBProcess process = null;
						if(sb.PrevSelState == Slottable.DeactivatedState){
							sb.InstantGreyin();
						}else if(sb.PrevSelState == Slottable.DefocusedState){
							process = new SBGreyinProcess(sb, sb.GreyinCoroutine);
						}else if(sb.PrevSelState == Slottable.SelectedState){
							process = new SBDehighlightProcess(sb, sb.DehighlightCoroutine);
						}
						sb.SetAndRunSelectionProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBSelectedState: SBSelectionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBProcess process = null;
						if(sb.PrevSelState == Slottable.DeactivatedState){
							sb.InstantHighlight();
						}else if(sb.PrevSelState == Slottable.DefocusedState){
							process = new SBHighlightProcess(sb, sb.HighlightCoroutine);
						}else if(sb.PrevSelState == Slottable.FocusedState){
							process = new SBHighlightProcess(sb, sb.HighlightCoroutine);
						}
						sb.SetAndRunSelectionProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	SB Action States	*/
				public class WaitForActionState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBProcess process = null;
						sb.SetAndRunActionProcess(process);
					}
					public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Tap();
						sb.Defocus();
					}
					public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Defocus();
					}
					public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){
						if(sb.IsFocused){
							sb.SetSelState(Slottable.SelectedState);
							sb.SetActState(Slottable.WaitForPickUpState);
						}
						else
							sb.SetActState(Slottable.WaitForPointerUpState);
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class WaitForPointerUpState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBProcess wfPtuProcess = new WaitForPointerUpProcess(sb, sb.WaitForPointerUpCoroutine);
						sb.SetAndRunActionProcess(wfPtuProcess);
					}
					public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Tap();
						sb.Defocus();
					}
					public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Defocus();
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
					public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class WaitForPickUpState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBProcess wfpuProcess = new WaitForPickUpProcess(sb, sb.WaitForPickUpCoroutine);
						sb.SetAndRunActionProcess(wfpuProcess);
					}
					public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.SetActState(Slottable.WaitForNextTouchState);
						// if(sb.Item.IsStackable)
						// else{
						// 	sb.Tap();
						// 	sb.Focus();
						// }
					}
					public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Focus();
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
					public override void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
					public override void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				}
				public class WaitForNextTouchState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBProcess wfntProcess = new WaitForNextTouchProcess(sb, sb.WaitForNextTouchCoroutine);
						sb.SetAndRunActionProcess(wfntProcess);
					}
					public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){
						if(!sb.IsPickedUp)
							sb.PickUp();
						else
							sb.SetActState(Slottable.PickedUpState);
							// sb.Increment();
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Focus();
					}
					/*	undef	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class PickedUpState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sb.SGM.SetPickedSB(sb);
						sb.SGM.CreateTransactionResults();
						// sb.SGM.SetHovered(sb, null);
						sb.OnHoverEnterMock();
						sb.SGM.UpdateTransaction();
						SBProcess pickedUpProcess = new PickedUpProcess(sb, sb.PickedUpCoroutine);
						sb.SetAndRunActionProcess(pickedUpProcess);
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Focus();
					}
					public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						if(sb.SGM.HoveredSB == sb)
							sb.SetActState(Slottable.WaitForNextTouchState);
						else
							sb.ExecuteTransaction();
					}
					public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.ExecuteTransaction();
					}
					/*	undef	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class SBRemovedState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBRemovedProcess process = new SBRemovedProcess(sb, sb.RemovedCoroutine);
						sb.SetAndRunActionProcess(process);
					}
					/*	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class SBAddedState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBAddedProcess process = new SBAddedProcess(sb, sb.AddedCoroutine);
						sb.SetAndRunActionProcess(process);
					}
					/*	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class SBMovingInSGState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBMoveInSGProcess process = new SBMoveInSGProcess(sb, sb.MoveInSGCoroutine);
						sb.SetAndRunActionProcess(process);
					}
					/*	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class SBRevertingState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBRevertProcess process = new SBRevertProcess(sb, sb.RevertCoroutine);
						sb.SetAndRunActionProcess(process);
					}
					/*	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class SBMovingOutState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBMoveOutProcess process = new SBMoveOutProcess(sb, sb.MoveOutCoroutine);
						sb.SetAndRunActionProcess(process);
					}
					/*	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
				public class SBMovingInState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBMoveInProcess process = new SBMoveInProcess(sb, sb.MoveInCoroutine);
						sb.SetAndRunActionProcess(process);
					}
					/*	*/
						public override void ExitState(StateHandler sh){
							base.ExitState(sh);
						}
						public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
						public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				}
			/*	SB Equip states	*/
				public class SBEquippedState: SBEquipState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						// sb.ItemInst.IsEquipped = true;
						if(sb.SG.IsPool){
							if(sb.PrevEqpState != null && sb.PrevEqpState == Slottable.UnequippedState){
								SBProcess process = new SBEquipProcess(sb, sb.EquipCoroutine);
								sb.SetAndRunEquipProcess(process);
							}
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBUnequippedState: SBEquipState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						// sb.ItemInst.IsEquipped = false;
						if(sb.SG.IsPool){
							if(sb.PrevEqpState != null && sb.PrevEqpState == Slottable.EquippedState){
								SBProcess process = new SBUnequipProcess(sb, sb.UnequipCoroutine);
								sb.SetAndRunEquipProcess(process);
							}
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	dump	*/
				// public class PickedUpAndSelectedState: SBState{
				// 	/*	Execute transaction needs revision
				// 		it should turn the state of sb not straight into RevertingState, but need to operate an alogorithm to decide whether it should turn sb state instead to WaitForNextTouch state before jumping into the conclusion
				// 	*/
				// 	public void EnterState(Slottable slottable){
				// 		SBProcess process = null;
				// 		if(slottable.PrevState == Slottable.WaitForPickUpState || slottable.PrevState == Slottable.WaitForNextTouchState){
				// 			process = new SBPickUpProcess(slottable, slottable.PickUpCoroutine);
				// 		}else if(slottable.PrevState == Slottable.PickedAndDeselectedState){
				// 			process = new SBHighlightProcess(slottable, slottable.HighlightCoroutine);
				// 		}
				// 		if(process != null)
				// 			slottable.SetAndRun(process);
				// 		if(slottable.SGM.CurState != SlotGroupManager.ProbingState){
				// 			slottable.SGM.SetState(SlotGroupManager.ProbingState);
				// 			InitializeSGMFields(slottable);
				// 			slottable.SGM.PostPickFilter();
				// 		}
						
				// 	}
				// 	public void ExitState(Slottable slottable){
				// 	}
				// 	public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 		if(slottable.Item.IsStackable)
				// 			slottable.SetState(Slottable.WaitForNextTouchWhilePUState);
				// 		else
				// 			slottable.ExecuteTransaction();
				// 	}
				// 	public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 		slottable.ExecuteTransaction();
				// 	}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		sb.SGM.SetPickedSB(sb);
				// 		sb.SGM.SetSelectedSB(sb);
				// 	}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		if(sb.SGM.SelectedSB == sb){
				// 			sb.SGM.SetSelectedSB(null);
				// 		}
				// 		sb.SetState(Slottable.PickedAndDeselectedState);
				// 	}
				// 	// public void Focus(Slottable sb){
				// 	// }
				// 	// public void Defocus(Slottable sb){
				// 	// }
				// 	void InitializeSGMFields(Slottable slottable){
				// 		slottable.SGM.SetSelectedSB(null);
				// 		slottable.SGM.SetSelectedSG(null);
				// 		slottable.SGM.SetPickedSB(slottable);//picked needs to be set prior to the other two in order to update transaction properly
				// 		slottable.SGM.SetSelectedSB(slottable);
				// 		SlotGroup sg = slottable.SGM.GetSlotGroup(slottable);
				// 		slottable.SGM.SetSelectedSG(sg);
				// 		sg.SetState(SlotGroup.SelectedState);
				// 		slottable.SGM.UpdateTransaction();
				// 		// slottable.SGM.SetSelectedSG(slottable.SGM.GetSlotGroup(slottable));
				// 	}
				// }
				// public class PickedUpAndDeselectedState: SBState{
				// 	public void EnterState(Slottable sb){
				// 		SBDehighlightProcess gradDeHiProcess = new SBDehighlightProcess(sb, sb.DehighlightCoroutine);
				// 		sb.SetAndRun(gradDeHiProcess);
				// 	}
				// 	public void ExitState(Slottable sb){
				// 	}
				// 	public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 			sb.ExecuteTransaction();
				// 	}
				// 	public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		sb.SetState(Slottable.PickedAndSelectedState);
				// 		sb.SGM.SetSelectedSB(sb);
				// 	}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	// public void Focus(Slottable sb){
				// 	// }
				// 	// public void Defocus(Slottable sb){
				// 	// }
				// }
				// public class WaitForNextTouchWhilePUState: SBState{
				// 	public void EnterState(Slottable slottable){
						
				// 		SBProcess wfntwpuProcess = new WaitForNextTouchWhilePUProcess(slottable, slottable.WaitForNextTouchWhilePUCoroutine);
				// 		slottable.SetAndRun(wfntwpuProcess);
				// 	}
				// 	public void ExitState(Slottable slottable){
				// 	}
				// 	public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 		slottable.Increment();
				// 		slottable.SetState(Slottable.PickedAndSelectedState);
				// 	}
				// 	public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 		slottable.ExecuteTransaction();
				// 	}
				// 	public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	// public void Focus(Slottable sb){
				// 	// }
				// 	// public void Defocus(Slottable sb){
				// 	// }
				// }
				// public class EquippedAndDeselectedState: SBState{
				// 	public void EnterState(Slottable slottable){
				// 		SBProcess process = null;
				// 		if(slottable.PrevState == Slottable.DefocusedState){
				// 			process = new SBEquipAndGreyinProcess(slottable, slottable.EquipAndGreyinCoroutine);
				// 		}else if(slottable.PrevState == Slottable.EquippedAndDefocusedState){
				// 			process = new SBGreyinProcess(slottable, slottable.GreyinCoroutine);
				// 		}else if(slottable.PrevState == Slottable.SelectedState){
				// 			process = new SBEquipAndDehighlightProcess(slottable, slottable.EquipAndDehighlightCoroutine);
				// 		}else if(slottable.PrevState == Slottable.EquippedAndSelectedState){
				// 			process = new SBDehighlightProcess(slottable, slottable.DehighlightCoroutine);
				// 		}else if(slottable.PrevState == Slottable.DeactivatedState){
				// 			slottable.InstantEquipAndGreyin();
				// 		}else if(slottable.PrevState == Slottable.MovingInSGState){
				// 			process = null;
				// 			slottable.SetAndRun(process);
				// 		}
				// 		if(process != null)
				// 			slottable.SetAndRun(process);

				// 	}
				// 	public void ExitState(Slottable slottable){}
				// 	public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 		if(slottable.Delayed)
				// 			slottable.SetState(Slottable.WaitForPickUpState);
				// 		else
				// 			slottable.SetState(Slottable.PickedAndSelectedState);
				// 	}
				// 	public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		sb.SGM.SetSelectedSB(sb);
				// 		sb.SetState(Slottable.EquippedAndSelectedState);
				// 	}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	// public void Focus(Slottable sb){
				// 	// 	if(!sb.IsEquipped)
				// 	// 		sb.SetState(Slottable.FocusedState);
				// 	// }
				// 	// public void Defocus(Slottable sb){
				// 	// 	if(!sb.IsEquipped)
				// 	// 		sb.SetState(Slottable.DefocusedState);
				// 	// 	else
				// 	// 		sb.SetState(Slottable.EquippedAndDefocusedState);
				// 	// }
				// }
				// public class EquippedAndSelectedState: SBState{
				// 	public void EnterState(Slottable slottable){
				// 		SBProcess process = null;
				// 		if(slottable.PrevState == Slottable.EquippedAndDeselectedState){
				// 			process = new SBHighlightProcess(slottable, slottable.HighlightCoroutine);
				// 		}
				// 		if(process != null)
				// 			slottable.SetAndRun(process);
				// 	}
				// 	public void ExitState(Slottable slottable){}
				// 	public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		if(sb.SGM.SelectedSB == sb)
				// 			sb.SGM.SetSelectedSB(null);
				// 		sb.SetState(Slottable.EquippedAndDeselectedState);
				// 	}
				// 	// public void Focus(Slottable sb){
				// 	// 	if(sb.IsEquipped)
				// 	// 		sb.SetState(Slottable.EquippedAndDeselectedState);
				// 	// 	else
				// 	// 		sb.SetState(Slottable.FocusedState);
				// 	// }
				// 	// public void Defocus(Slottable sb){
				// 	// 	if(sb.IsEquipped)
				// 	// 		sb.SetState(Slottable.EquippedAndDefocusedState);
				// 	// 	else
				// 	// 		sb.SetState(Slottable.DefocusedState);

				// 	// }
				// }
				// public class EquippedAndDefocusedState: SBState{
				// 	public void EnterState(Slottable slottable){
				// 		SBProcess process = null;
						
				// 		if(slottable.PrevState == Slottable.FocusedState){
				// 			process = new SBEquipAndGreyoutProcess(slottable, slottable.EquipAndGreyoutCoroutine);
				// 		}else if(slottable.PrevState == Slottable.EquippedAndDeselectedState){
				// 			process = new SBGreyoutProcess(slottable, slottable.GreyoutCoroutine);
				// 		}else if(slottable.PrevState == Slottable.DeactivatedState){
				// 			slottable.InstantEquipAndGreyout();
				// 		}else if(slottable.PrevState == Slottable.WaitForPointerUpState){
				// 			process = null;
				// 			slottable.SetAndRun(process);
				// 		}else if(slottable.PrevState == Slottable.MovingInSGState){
				// 			process = null;
				// 			slottable.SetAndRun(process);
				// 		}else if(slottable.PrevState == Slottable.MovingOutState){
				// 			process = null;
				// 			slottable.SetAndRun(process);
				// 		}
				// 		if(process != null)
				// 			slottable.SetAndRun(process);
				// 	}
				// 	public void ExitState(Slottable slottable){}
				// 	public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 		slottable.SetState(Slottable.WaitForPointerUpState);
				// 	}
				// 	public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 	}
				// 	// public void Focus(Slottable sb){
				// 	// 	if(sb.IsEquipped)
				// 	// 		sb.SetState(Slottable.EquippedAndDeselectedState);
				// 	// 	else
				// 	// 		sb.SetState(Slottable.FocusedState);
				// 	// }
				// 	// public void Defocus(Slottable sb){
				// 	// 	if(sb.IsEquipped)	
				// 	// 		sb.SetState(Slottable.EquippedAndDefocusedState);
				// 	// 	else
				// 	// 		sb.SetState(Slottable.DefocusedState);
				// 	// }
				// }

				// public class MovingOutState: SlottableState{
				// 	public void EnterState(Slottable slottable){
				// 		MoveProcess moveProcess = new MoveProcess(slottable, slottable.MoveCoroutine);
				// 		slottable.SetAndRun(moveProcess);

				// 		SlotGroupManager sgm = slottable.SGM;
				// 		SlotGroup sg = sgm.GetSlotGroup(slottable);
				// 		if(sgm.Transaction.GetType() == typeof(ReorderTransaction)){
				// 			SBReorderingProcess process = new SBReorderingProcess(slottable, slottable.ReorderingCoroutine);
				// 			slottable.SetAndRun(process);
				// 		}else{
				// 			if(sgm.FocusedSGP == sg){
				// 				/*	Create and Run Equipping Process	*/
				// 				SBEquippingProcess process = new SBEquippingProcess(slottable, slottable.EquippingCoroutine);
				// 				slottable.SetAndRun(process);
				// 			}else{
				// 				/*	Create and Run Removing Process	*/
				// 				SBRemovingProcess process = new SBRemovingProcess(slottable, slottable.RemovingCoroutine);
				// 				slottable.SetAndRun(process);
				// 			}
				// 		}
				// 	}
				// 	public void ExitState(Slottable Slottable){
				// 	}
				// 	public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				// 	}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void Focus(Slottable sb){
				// 		if(sb.IsEquipped)
				// 			sb.SetState(Slottable.EquippedAndDeselectedState);
				// 		else
				// 			sb.SetState(Slottable.FocusedState);
				// 	}
				// 	public void Defocus(Slottable sb){
				// 		if(sb.IsEquipped)
				// 			sb.SetState(Slottable.EquippedAndDefocusedState);
				// 		else
				// 			sb.SetState(Slottable.DefocusedState);
				// 	}
				// }
				// public class SBRemovingState: SlottableState{
				// 	public void EnterState(Slottable sb){
				// 		SBRemovingProcess process = new SBRemovingProcess(sb, sb.RemovingCoroutine);
				// 		sb.SetAndRun(process);
						
				// 	}
				// 	public void ExitState(Slottable sb){}
				// 	public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		if(sb.SGM.SelectedSB == sb)
				// 			sb.SGM.SetSelectedSB(null);
				// 		sb.SetState(Slottable.FocusedState);
				// 	}
				// 	public void Focus(Slottable sb){
				// 	}
				// 	public void Defocus(Slottable sb){
				// 	}
				// }
				// public class SBEquippingState: SlottableState{
				// 	public void EnterState(Slottable sb){
				// 		SBEquippingProcess process = new SBEquippingProcess(sb, sb.EquippingCoroutine);
				// 		sb.SetAndRun(process);
						
				// 	}
				// 	public void ExitState(Slottable sb){}
				// 	public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		if(sb.SGM.SelectedSB == sb)
				// 			sb.SGM.SetSelectedSB(null);
				// 		sb.SetState(Slottable.FocusedState);
				// 	}
				// 	public void Focus(Slottable sb){
				// 		if(sb.IsEquipped) sb.SetState(Slottable.EquippedAndDeselectedState);
				// 		else sb.SetState(Slottable.FocusedState);
				// 	}
				// 	public void Defocus(Slottable sb){
				// 		if(sb.IsEquipped) sb.SetState(Slottable.EquippedAndDefocusedState);
				// 		else sb.SetState(Slottable.DefocusedState);
				// 	}
				// }
				// public class SBUnpickingState: SlottableState{
				// 	public void EnterState(Slottable sb){
				// 		SBUnpickingProcess process = new SBUnpickingProcess(sb, sb.UnpickingCoroutine);
				// 		sb.SetAndRun(process);
				// 	}
				// 	public void ExitState(Slottable sb){}
				// 	public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 	public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
				// 		if(sb.SGM.SelectedSB == sb)
				// 			sb.SGM.SetSelectedSB(null);
				// 		sb.SetState(Slottable.FocusedState);
				// 	}
				// 	public void Focus(Slottable sb){
				// 	}
				// 	public void Defocus(Slottable sb){
				// 	}
				// }
		/*	commands	*/
			public interface SlottableCommand{
				void Execute(Slottable sb);
			}
			public class SBTapCommand: SlottableCommand{
				public void Execute(Slottable sb){

				}
			}
			/*	dump	*/
				// public class DefInstantDeactivateCommand: SlottableCommand{
				// 	public void Execute(Slottable sb){
				// 	}
				// }
	/*	Other Classes	*/
		/*	TransactionResult	*/
			public class TransactionResult{
				SlotSystemTransaction ta;
				Slottable selectedSB;
				SlotGroup selectedSG;
				public TransactionResult(Slottable sb, SlotGroup sg, SlotSystemTransaction ta){
					this.ta = ta;
					selectedSB = sb;
					selectedSG = sg;
				}
				public SlotSystemTransaction TryGetTransaction(Slottable sb, SlotGroup sg){
					bool same = true;
					if(sb != null)
						same &= selectedSB == sb;
					else
						same &= selectedSB == null;
					if(sg != null)
						same &= selectedSG == sg;
					else
						same &= selectedSG == null;
					if(same)
						return ta;
					else
						return null;
				}
			}
			public class TransactionResults{
				List<TransactionResult> trs;
				public TransactionResults(){
					trs = new List<TransactionResult>();
				}
				public void AddTransactionResult(TransactionResult tr){
					this.trs.Add(tr);
				}
				public SlotSystemTransaction GetTransaction(Slottable sb, SlotGroup sg){
					SlotSystemTransaction result = null;
					foreach(TransactionResult tr in trs){
						result = tr.TryGetTransaction(sb, sg);
						if(result != null)
							return result;
					}
					return result;
				}
			}
		/*	Inventory Item	*/
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
		/*	Inventories	*/
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
		/*	mock items	*/
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
			
		/*	mock instances	*/
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

		/*	*/
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
	/*	utility	*/
		public static class Util{
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
			public static bool IsSwappable(Slottable pickedSB, Slottable otherSB){
				/*	precondition
						1) they do not share same SG
						2) otherSB.SG accepts pickedSB
						3) not stackable
				*/
				if(pickedSB.SG != otherSB.SG){
					if(otherSB.SG.AcceptsFilter(pickedSB)){
						if(!(pickedSB.ItemInst == otherSB.ItemInst && pickedSB.ItemInst.Item.IsStackable))
						 if(pickedSB.SG.AcceptsFilter(otherSB))
							return true;
					}
				}
				return false;
			}
			/*	SGM	*/
				public static string SGMStateName(SGMState state){
					string res = "";
					if(state is SGMDeactivatedState)
						res = Util.Red("Deactivated");
					else if(state is SGMDefocusedState)
						res = Util.Blue("Defocused");
					else if(state is SGMFocusedState)
						res = Util.Green("Focused");
					else if(state is SGMWaitForActionState)
						res = Util.Green("WaitForAction");
					else if(state is SGMProbingState)
						res = Util.Ciel("Probing");
					else if(state is SGMPerformingTransactionState)
						res = Util.Terra("PerformingTransaction");
					return res;
				}
				public static string TransactionName(SlotSystemTransaction ta){
					string res = "";
					if(ta is RevertTransaction)
						res = Util.Red("RevertTA");
					else if(ta is ReorderTransaction)
						res = Util.Blue("ReorderTA");
					else if(ta is ReorderInOtherSGTransaction)
						res = Util.Green("ReorderInOtherSGTA");
					else if(ta is StackTransaction)
						res = Util.Aqua("StackTA");
					else if(ta is SwapTransaction)
						res = Util.Terra("SwapTA");
					else if(ta is FillTransaction)
						res = Util.Forest("FillTA");
					else if(ta is FillEquipTransaction)
						res = Util.Berry("FillEquipTA");
					else if(ta is SortTransaction)
						res = Util.Khaki("SortTA");
					else if(ta is InsertTransaction)
						res = Util.Midnight("InsertTA");
					return res;
				}
				public static string SGMProcessName(SGMProcess proc){
					string res = "";
					if(proc is SGMGreyinProcess)
						res = Util.Red("Greyin");
					else if(proc is SGMGreyoutProcess)
						res = Util.Red("Greyout");
					else if(proc is SGMProbeProcess)
						res = Util.Red("Probe");
					else if(proc is SGMTransactionProcess)
						res = Util.Blue("Transaction");
					return res;
				}
				public static string SGMDebug(SlotGroupManager sgm){
					string res = "";
					string pSB = Util.SBofSG(sgm.PickedSB);
					string hSG = Util.SGName(sgm.HoveredSG);
					string hSB = Util.SBofSG(sgm.HoveredSB);
					string tSG = Util.SGName(sgm.TargetSG);
					string tSB = Util.SBofSG(sgm.TargetSB);
					string prevSel = Util.SGMStateName(sgm.PrevSelState);
					string curSel = Util.SGMStateName(sgm.CurSelState);
					string selProc = Util.SGMProcessName(sgm.SelectionProcess);
					string prevAct = Util.SGMStateName(sgm.PrevActState);
					string curAct = Util.SGMStateName(sgm.CurActState);
					string actProc = Util.SGMProcessName(sgm.ActionProcess);
					string ta = Util.TransactionName(sgm.Transaction);
					string pSBD = "pSBDone: " + (sgm.PickedSBDone?Util.Blue("true"):Util.Red("false"));
					string tSBD = "tSBDone: " + (sgm.TargetSBdone?Util.Blue("true"):Util.Red("false"));
					string oSGD = "oSGDone: " + (sgm.OrigSGDone?Util.Blue("true"):Util.Red("false"));
					string tSGD = "tSGDone: " + (sgm.TargetSGDone?Util.Blue("true"):Util.Red("false"));
					res = Util.Bold("SGM:") +
							" pSB " + pSB +
							", hSG " + hSG +
							", hSB " + hSB +
							", tSB " + tSB +
							" tSG " + tSG + ", " +
						Util.Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							"proc " + selProc + ", " +
						Util.Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							"proc " + actProc + ", " +
						Util.Bold("TA ") + ta + ", " + 
						Util.Bold("TAComp ") + pSBD + " " + tSBD + " " + oSGD + " " + tSGD;
					return res;
				}
			/*	SG	*/
				public static string SGName(SlotGroup sg){
					string result = "";
					if(sg != null){
						if(sg.IsPool){
							if(sg.Filter is SGNullFilter)
								result = "sgpAll";
							else if(sg.Filter is SGBowFilter)
								result = "sgpBow";
							else if(sg.Filter is SGWearFilter)
								result = "sgpWear";
							else if(sg.Filter is SGCGearsFilter)
								result = "sgpCGears";
							else if(sg.Filter is SGPartsFilter)
								result = "sgpParts";
							result = Red(result);
						}else if(sg.IsSGE){
							if(sg.Filter is SGBowFilter)
								result = "sgBow";
							else if(sg.Filter is SGWearFilter)
								result = "sgWear";
							else if(sg.Filter is SGCGearsFilter)
								result = "sgCGears";
							result = Blue(result);
						}
					}
					return result;
				}
				public static string SGStateName(SGState state){
					string res = "";
					if(state is SGDeactivatedState){
						res = Util.Sangria("SGDeactivated");
					}else if(state is SGDefocusedState){
						res = Util.Terra("SGDefocused");
					}else if(state is SGFocusedState){
						res = Util.Green("SGFocused");
					}else if(state is SGSelectedState){
						res = Util.Aqua("SGSelected");
					}else if(state is SGWaitForActionState){
						res = Util.Sangria("SGWFA");
					}else if(state is SGPerformingTransactionState){
						res = Util.Green("SGTransaction");
					}
					return res;
				}
				public static string SGProcessName(SGProcess proc){
					string res = "";
					if(proc is SGGreyinProcess)
						res = Util.Red("Greyin");
					else if(proc is SGGreyoutProcess)
						res = Util.Blue("Greyout");
					else if(proc is SGHighlightProcess)
						res = Util.Green("Highlight");
					else if(proc is SGDehighlightProcess)
						res = Util.Brown("Dehighlight");
					else if(proc is SGTransactionProcess)
						res = Util.Khaki("Transaction");
					return res;
				}
				public static string SGDebug(SlotGroup sg){
					string res = "";
					string sgName = SGName(sg);
					string prevSel = SGStateName(sg.PrevSelState);
					string curSel = SGStateName(sg.CurSelState);
					string selProc = SGProcessName(sg.SelectionProcess);
					string prevAct = SGStateName(sg.PrevActState);
					string curAct = SGStateName(sg.CurActState);
					string actProc = SGProcessName(sg.ActionProcess);
					res = sgName + " " +
						Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							" proc, " + selProc + ", " +
						Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							" proc, " + actProc;
					return res;
				}
			/*	SB	*/
				public static string SBName(Slottable sb){
					string result = "";
					if(sb != null){
						switch(sb.ItemInst.Item.ItemID){
							case 0:	result = "defBow"; break;
							case 1:	result = "crfBow"; break;
							case 2:	result = "frgBow"; break;
							case 3:	result = "mstBow"; break;
							case 100: result = "defWear"; break;
							case 101: result = "crfWear"; break;
							case 102: result = "frgWear"; break;
							case 103: result = "mstWear"; break;
							case 200: result = "defShield"; break;
							case 201: result = "crfShield"; break;
							case 202: result = "frgShield"; break;
							case 203: result = "mstShield"; break;
							case 300: result = "defMWeapon"; break;
							case 301: result = "crfMWeapon"; break;
							case 302: result = "frgMWeapon"; break;
							case 303: result = "mstMWeapon"; break;
							case 400: result = "defQuiver"; break;
							case 401: result = "crfQuiver"; break;
							case 402: result = "frgQuiver"; break;
							case 403: result = "mstQuiver"; break;
							case 500: result = "defPack"; break;
							case 501: result = "crfPack"; break;
							case 502: result = "frgPack"; break;
							case 503: result = "mstPack"; break;
							case 600: result = "defParts"; break;
							case 601: result = "crfParts"; break;
							case 602: result = "frgParts"; break;
							case 603: result = "mstParts"; break;
						}
						List<InventoryItemInstanceMock> sameItemInsts = new List<InventoryItemInstanceMock>();
						foreach(InventoryItemInstanceMock itemInst in SlotGroupManager.CurSGM.PoolInv.Items){
							if(itemInst.Item == sb.ItemInst.Item)
								sameItemInsts.Add(itemInst);
						}
						int index = sameItemInsts.IndexOf(sb.ItemInst);
						result += "_"+index.ToString();
					}
					return result;
				}
				public static string SBofSG(Slottable sb){
					string res = "";
					if(sb != null){
						res = Util.SBName(sb) + " of " + Util.SGName(sb.SG);
						if(sb.SG.IsPool)
							res = Util.Red(res);
						else
							res = Util.Blue(res);
						if(sb.IsEquipped && sb.SG.IsPool)
							res = Util.Bold(res);
					}
					return res;
				}
				public static string SBStateName(SBState state){
					string result = "";
					if(state is SBSelectionState){
						if(state is SBDeactivatedState)
							result = Red("Deactivated");
						else if(state is SBFocusedState)
							result = Blue("Focused");
						else if(state is SBDefocusedState)
							result = Green("Defocused");
						else if(state is SBSelectedState)
							result = Ciel("Selected");
					}else if(state is SBActionState){
						if(state is WaitForActionState)
							result = Aqua("WFAction");
						else if(state is WaitForPointerUpState)
							result = Forest("WFPointerUp");
						else if(state is WaitForPickUpState)
							result = Brown("WFPickUp");
						else if(state is WaitForNextTouchState)
							result = Terra("WFNextTouch");
						else if(state is PickedUpState)
							result = Berry("PickedUp");
						else if(state is SBRemovedState)
							result = Violet("Removed");
						else if(state is SBAddedState)
							result = Khaki("Added");
						else if(state is SBMovingInSGState)
							result = Midnight("MovingInSG");
						else if(state is SBRevertingState)
							result = Beni("Reverting");
						else if(state is SBMovingOutState)
							result = Sangria("MovingOut");
						else if(state is SBMovingInState)
							result = Yamabuki("MovingIn");
					}else if(state is SBEquipState){
						if(state is SBEquippedState)
							result = Red("Equipped");
						else if(state is SBUnequippedState)
							result = Blue("Unequipped");
					}
					return result;
				}
				public static string SBProcessName(SBProcess process){
					string res = "";
					if(process is SBGreyoutProcess)
						res = Red("Greyout");
					else if(process is SBGreyinProcess)
						res = Blue("Greyin");
					else if(process is SBHighlightProcess)
						res = Green("Highlight");
					else if(process is SBDehighlightProcess)
						res = Ciel("Dehighlight");
					else if(process is WaitForPointerUpProcess)
						res = Aqua("WFPointerUp");
					else if(process is WaitForPickUpProcess)
						res = Forest("WFPickUp");
					else if(process is PickedUpProcess)
						res = Brown("PickedUp");
					else if(process is WaitForNextTouchProcess)
						res = Terra("WFNextTouch");
					else if(process is SBUnpickProcess)
						res = Berry("Unpick");
					else if(process is SBRemovedProcess)
						res = Violet("Removed");
					else if(process is SBAddedProcess)
						res = Khaki("Added");
					else if(process is SBMoveInSGProcess)
						res = Midnight("MovingInSG");
					else if(process is SBRevertProcess)
						res = Beni("Reverting");
					else if(process is SBMoveOutProcess)
						res = Sangria("MovingOut");
					else if(process is SBMoveInProcess)
						res = Yamabuki("MovingIn");
					else if(process is SBUnequipProcess)
						res = Red("Unequip");
					else if(process is SBEquipProcess)
						res = Blue("Equipping");
					return res;
				}
				public static string SBDebug(Slottable sb){
					string res = "";
					string sbName = SBofSG(sb);
					string prevSel = SBStateName(sb.PrevSelState);
					string curSel = SBStateName(sb.CurSelState);
					string selProc = SBProcessName(sb.SelectionProcess);
					string prevAct = SBStateName(sb.PrevActState);
					string curAct = SBStateName(sb.CurActState);
					string actProc = SBProcessName(sb.ActionProcess);
					string prevEqp = SBStateName(sb.PrevEqpState);
					string curEqp = SBStateName(sb.CurEqpState);
					string eqpProc = SBProcessName(sb.EquipProcess);
					res = sbName + ": " +
						Bold("Sel ") + " from " + prevSel + " to " + curSel + " proc " + selProc + ", " + 
						Bold("Act ") + " from " + prevAct + " to " + curAct + " proc " + actProc + ", " + 
						Bold("Eqp ") + " from " + prevEqp + " to " + curEqp + " proc " + eqpProc;
					return res;
				}
			/*	Debug	*/
				public static string Red(string str){
					return "<color=#ff0000>" + str + "</color>";
				}
				public static string Blue(string str){
					return "<color=#0000ff>" + str + "</color>";

				}
				public static string Green(string str){
					return "<color=#02B902>" + str + "</color>";
				}
				public static string Ciel(string str){
					return "<color=#11A795>" + str + "</color>";
				}
				public static string Aqua(string str){
					return "<color=#128582>" + str + "</color>";
				}
				public static string Forest(string str){
					return "<color=#046C57>" + str + "</color>";
				}
				public static string Brown(string str){
					return "<color=#805A05>" + str + "</color>";
				}
				public static string Terra(string str){
					return "<color=#EA650F>" + str + "</color>";
				}
				public static string Berry(string str){
					return "<color=#A41565>" + str + "</color>";
				}
				public static string Violet(string str){
					return "<color=#793DBD>" + str + "</color>";
				}
				public static string Khaki(string str){
					return "<color=#747925>" + str + "</color>";
				}
				public static string Midnight(string str){
					return "<color=#1B2768>" + str + "</color>";
				}
				public static string Beni(string str){
					return "<color=#E32791>" + str + "</color>";
				}
				public static string Sangria(string str){
					return "<color=#640A16>" + str + "</color>";
				}
				public static string Yamabuki(string str){
					return "<color=#EAB500>" + str + "</color>";
				}
				public static string Bold(string str){
					return "<b>" + str + "</b>";
				}
				static string m_stacked;
				public static string Stacked{
					get{
						string result = m_stacked;
						m_stacked = "";
						return result;
					}
				}
				public static void Stack(string str){
					m_stacked += str + ", ";
				}
		}
}
