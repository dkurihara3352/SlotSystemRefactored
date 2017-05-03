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
					pickedSB.Move(sg, slot);
					SGMRevertTransactionProcess revertProcess = new SGMRevertTransactionProcess(sgm, sgm.RevertStateCoroutine);
					sgm.SetAndRun(revertProcess);
					sgm.SetState(SlotGroupManager.PerformingTransactionState);
					// pickedSB.SetState(Slottable.MovingState);
					// sgm.CompleteTransactionMock();
				}
				public void OnComplete(){
					if(pickedSB.IsEquipped)
						pickedSB.SetState(Slottable.EquippedAndDeselectedState);
					else
						pickedSB.SetState(Slottable.FocusedState);
					pickedSB.PickedAmount = 0;
					pickedSB.ClearDestination();
					sgm.CompleteAllTransactionMock();
				}
			}
			public class ReorderTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				public ReorderTransaction(Slottable picked, Slottable selected){
					this.pickedSB = picked;
					this.selectedSB = selected;
				}
				public void Indicate(){}
				public void Execute(){
					// pickedSB.SetState(Slottable.ReorderingState);
					pickedSB.SGM.CompleteAllTransactionMock();
				}
				public void OnComplete(){}
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
					pickedSB.SGM.CompleteAllTransactionMock();
				}
				public void OnComplete(){}
			}
			public class SwapTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				Slottable selectedSB;
				public SwapTransaction(Slottable picked, Slottable selected){
					this.pickedSB = picked;
					this.selectedSB = selected;
				}
				public void Indicate(){}
				public void Execute(){

					pickedSB.SGM.CompleteAllTransactionMock();
				}
				public void OnComplete(){}
			}
			public class FillTransaction: SlotSystemTransaction{
				Slottable pickedSB;
				SlotGroup selectedSG;
				public FillTransaction(Slottable picked, SlotGroup selSG){
					this.pickedSB = picked;
					this.selectedSG = selSG;
				}
				public void Indicate(){}
				public void Execute(){

					pickedSB.SGM.CompleteAllTransactionMock();
				}
				public void OnComplete(){}
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
							if(selectedSG == null || selectedSG == origSG){
								SlotSystemTransaction revertTs = new RevertTransaction(pickedSB);
								sgm.SetTransaction(revertTs);
							}else{
								/*	selectedSG != null && != origSG
									there's at least one vacant slot OR there's a sb of a same stackable item
								*/
								if(selectedSG.HasItem((InventoryItemInstanceMock)pickedSB.Item)){
									StackTransaction stackTs = new StackTransaction(pickedSB, selectedSB);
									sgm.SetTransaction(stackTs);
								}else{
									FillTransaction fillTs = new FillTransaction(pickedSB, selectedSG);
									sgm.SetTransaction(fillTs);
								}
							}

						}else{
							if(pickedSB == selectedSB){
								SlotSystemTransaction revertTs = new RevertTransaction(pickedSB);
								sgm.SetTransaction(revertTs);
							}else{
								if(sgm.GetSlotGroup(selectedSB) == sgm.GetSlotGroup(pickedSB)){
									if(!sgm.GetSlotGroup(pickedSB).AutoSort){
										SlotSystemTransaction reorderTs = new ReorderTransaction(pickedSB, selectedSB);
										sgm.SetTransaction(reorderTs);
									}
								}else{
									if(pickedSB.Item == selectedSB.Item){
										StackTransaction stackTs = new StackTransaction(pickedSB, selectedSB);
										sgm.SetTransaction(stackTs);
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
								if(sg.Filter is SGNullFilter)
									sg.UpdateSbState();
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
								if(origSG.AutoSort){
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
			// public class PrePickFilterCommand: SGMCommand{
			// 	public void Execute(SlotGroupManager sgm){
			// 		foreach(SlotGroup sg in sgm.SlotGroups){
			// 			if(sg.CurState == SlotGroup.DefocusedState || !sg.IsPool)
			// 				sg.SetState(SlotGroup.FocusedState);
			// 			else if(sg.CurState == SlotGroup.SelectedState)
			// 				sg.SetState(SlotGroup.FocusedState);
			// 			sg.UpdateSbState();
			// 		}
			// 	}
			// }
			// public class PrePickFilterCommandV2: SGMCommand{
			// 	public void Execute(SlotGroupManager sgm){
			// 		foreach(SlotGroup sg in sgm.SlotGroups){
			// 			sg.PrePickFilter();
			// 		}
			// 	}
			// }
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
					// Debug.Log("called");
					SGM.SetSBA(SGM.PickedSB);
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.Transaction.OnComplete();
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
				}
				public void ExitState(SlotGroup sg){
				}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void Focus(SlotGroup sg){
					sg.SetState(SlotGroup.FocusedState);
					// sg.UpdateSbState();
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					sg.SetState(SlotGroup.DefocusedState);
					// sg.UpdateSbState();
					sg.DefocusSBs();
				}
			}
			public class SGDefocusedState: SlotGroupState{
				public void EnterState(SlotGroup sg){}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void Focus(SlotGroup sg){
					sg.SetState(SlotGroup.FocusedState);
					// sg.UpdateSbState();
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					// sg.SetState(SlotGroup.DefocusedState);
					// sg.UpdateSbState();
					sg.DefocusSBs();
				}
			}
			public class SGFocusedState: SlotGroupState{
				public void EnterState(SlotGroup sg){}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){
					sg.SGM.SetSelectedSG(sg);
					sg.SetState(SlotGroup.SelectedState);
				}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void Focus(SlotGroup sg){
					// sg.UpdateSbState();
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					sg.SetState(SlotGroup.DefocusedState);
					// sg.UpdateSbState();
					sg.DefocusSBs();
				}
			}
			public class SGSelectedState: SlotGroupState{
				public void EnterState(SlotGroup sg){}
				public void ExitState(SlotGroup sg){}
				public void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventData){}
				public void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventData){
					if(sg.SGM.SelectedSG == sg){
						sg.SGM.SetSelectedSG(sg);
						sg.SetState(SlotGroup.FocusedState);
					}	
				}
				public void Focus(SlotGroup sg){
					// sg.UpdateSbState();
					sg.FocusSBs();
				}
				public void Defocus(SlotGroup sg){
					sg.SetState(SlotGroup.DefocusedState);
					// sg.UpdateSbState();
					sg.DefocusSBs();
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
					sg.UpdateSbState();
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
									if(sg.AutoSort){
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
					sg.SortItems();//sort Items
					sg.CreateSlots();
					sg.CreateSlottables();
					sg.UpdateEquipStatus();
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
						sb.Initialize(sg);
						sb.Delayed = true;
						sb.SetItem(sg.FilteredItems[i]);
						sg.Slots[i].Sb = sb;
					}
				}
			}
			public class UpdateEquipStatusForPoolCommmand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					return;
				}
			}
			public class UpdateEquipStatusForEquipSGCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					foreach(Slot slot in sg.Slots){
						if(slot.Sb != null){

							InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
							invItem.IsEquipped = true;
							// slot.Sb.SetState(Slottable.EquippedState);
							// sg.SGM.FindSbAndSetEquipped(sg, invItem);
						}
					}
				}
			}
			public class SGFocusCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					if(sg.Scroller != null)
						sg.SGM.ChangeFocus(sg);//-> and update all SBs states
					else{
						sg.SetState(SlotGroup.FocusedState);
						sg.UpdateSbState();
					}
				}
			}
			public class SGFocusCommandV2: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.CurState.Focus(sg);
				}
			}
			public class SGDefocusCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					if(sg.Scroller != null){
						return;
					}else{
						sg.SetState(SlotGroup.DefocusedState);
						sg.UpdateSbState();
					}
				}
			}
			public class SGDefocusCommandV2: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.CurState.Defocus(sg);
				}
			}
			// public class PoolPrePickFilter: SlotGroupCommand{
			// 	public void Execute(SlotGroup sg){
			// 		// if(sg.SGM.FocusedPoolSG == sg)
			// 		// 	sg.SetState(SlotGroup.FocusedState);
			// 		// else
			// 		// 	sg.SetState(SlotGroup.DefocusedState);
			// 		// sg.UpdateSbState();
			// 	}
			// }
			// public class EquipSGPrePickFilter: SlotGroupCommand{
			// 	public void Execute(SlotGroup sg){
			// 		// if(sg.SGM.FocusedEquipmentSetContains(sg))
			// 		// 	sg.SetState(SlotGroup.FocusedState);
			// 		// else
			// 		// 	sg.SetState(SlotGroup.DefocusedState);
			// 		// sg.UpdateSbState();
			// 	}
			// }
		/*	filters
		*/
			public interface SGFilter{
				void Execute(SlotGroup sg);
			}
			public class SGNullFilter: SGFilter{
				public void Execute(SlotGroup sg){

					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						filteredItems.Add(item);
					}
					sg.SetFilteredItems(filteredItems);
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
			}
			
		/*	sorters
		*/
			public interface SGSorter{
				void Execute(SlotGroup sg);
			}
			public class SGItemIndexSorter: SGSorter{
				public void Execute(SlotGroup sg){
					SlottableItem[] itemsArray = sg.FilteredItems.ToArray();
					Array.Sort(itemsArray);
					List<SlottableItem> newList = new List<SlottableItem>();
					foreach(SlottableItem item in itemsArray){
						newList.Add(item);
					}
					sg.SetFilteredItems(newList);
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
			public class GradualGrayoutProcess: AbsSBProcess{
				public GradualGrayoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}	
			}
			public class EquipGradualGrayoutProcess: AbsSBProcess{
				public EquipGradualGrayoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					SB = sb;
					CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}	
			}
			public class GradualGrayinProcess: AbsSBProcess{
				public GradualGrayinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class EquipGradualGrayinProcess: AbsSBProcess{
				public EquipGradualGrayinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class GradualDehighlightProcess: AbsSBProcess{
				public GradualDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class EquipGradualDehighlightProcess: AbsSBProcess{
				public EquipGradualDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
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
					SB.SetState(Slottable.PickedUpAndSelectedState);
				}
			}
			public class PickedUpAndSelectedProcess: AbsSBProcess{
				public PickedUpAndSelectedProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
			public class PickedUpAndDeselectedProcess: AbsSBProcess{
				public PickedUpAndDeselectedProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
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
					// SB.SGM.CompleteTransactionMock();
					SB.SGM.CompleteTransaction(SB);
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
					// sb.Deactivate();
					sb.SetAndRun(null);
				}
				public void ExitState(Slottable sb){
				}
				public void Focus(Slottable sb){
					if(sb.Item is PartsInstanceMock && !(sb.SGM.GetSlotGroup(sb).Filter is SGPartsFilter))
						sb.SetState(Slottable.DefocusedState);
					else{
						if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
							sb.SetState(Slottable.EquippedAndDeselectedState);
						else
							sb.SetState(Slottable.FocusedState);
					}
				}
				public void Defocus(Slottable sb){
					if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
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

					if(slottable.PrevState == Slottable.FocusedState || slottable.PrevState == Slottable.EquippedAndDeselectedState){

						SBProcess gradGOProcess = new GradualGrayoutProcess(slottable, slottable.GradualGrayoutCoroutine);
						slottable.SetAndRun(gradGOProcess);

					}else{

						slottable.InstantGrayout();
						slottable.SetAndRun(null);
					}
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.SetState(Slottable.WaitForPointerUpState);
				}
				public void Focus(Slottable sb){
					if(sb.Item is PartsInstanceMock && !(sb.SGM.GetSlotGroup(sb).Filter is SGPartsFilter))
						sb.SetState(Slottable.DefocusedState);
					else{
						if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
							sb.SetState(Slottable.EquippedAndDeselectedState);
						else
							sb.SetState(Slottable.FocusedState);
					}
				}
				public void Defocus(Slottable sb){
					if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
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
					
					if(slottable.PrevState == Slottable.DefocusedState || slottable.PrevState == Slottable.EquippedAndDefocusedState){

						SBProcess gradGIProcess = new GradualGrayinProcess(slottable, slottable.GradualGrayinCoroutine);
						slottable.SetAndRun(gradGIProcess);

					}else if(slottable.PrevState == Slottable.SelectedState || slottable.PrevState == Slottable.EquippedAndSelectedState){

						SBProcess gradDhProcess = new GradualDehighlightProcess(slottable, slottable.GradualDehighlightCoroutine);
						slottable.SetAndRun(gradDhProcess);

					}else{
						slottable.InstantGrayin();
						slottable.SetAndRun(null);
					}
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
					if(sb.Item is PartsInstanceMock && !(sb.SGM.GetSlotGroup(sb).Filter is SGPartsFilter))
						sb.SetState(Slottable.DefocusedState);
					else{
						if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
							sb.SetState(Slottable.EquippedAndDeselectedState);
					}
				}
				public void Defocus(Slottable sb){
					if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
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
						// sb.SetState(Slottable.FocusedState);
					}
					public void Defocus(Slottable sb){
						// sb.SetState(Slottable.DefocusedState);
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
					slottable.SetState(Slottable.FocusedState);
				}
				/*	undef
				*/
					public void Focus(Slottable sb){
						// sb.SetState(Slottable.FocusedState);
					}
					public void Defocus(Slottable sb){
						// sb.SetState(Slottable.DefocusedState);
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
						// sb.SetState(Slottable.FocusedState);
					}
					public void Defocus(Slottable sb){
						// sb.SetState(Slottable.DefocusedState);
					}
			}
			public class PickedUpAndSelectedState: SlottableState{
				/*	Execute transaction needs revision
					it should turn the state of sb not straight into RevertingState, but need to operate an alogorithm to decide whether it should turn sb state instead to WaitForNextTouch state before jumping into the conclusion
				*/
				public void EnterState(Slottable slottable){
					
					SBProcess puaSelProcess = new PickedUpAndSelectedProcess(slottable, slottable.PickedUpAndSelectedCoroutine);
					slottable.SetAndRun(puaSelProcess);

					slottable.SGM.SetState(SlotGroupManager.ProbingState);
					InitializeSGMFields(slottable);
					slottable.SGM.PostPickFilter();
					
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
					// if(sb.SGM.SelectedSB == sb){
					// 	sb.SGM.SelectedSB = null;
					// }
					//=> handled in SGM side
					if(sb.SGM.SelectedSB == sb){
						sb.SGM.SetSelectedSB(null);
					}
					sb.SetState(Slottable.PickedUpAndDeselectedState);

					
				}
				public void Focus(Slottable sb){
					// sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					// sb.SetState(Slottable.DefocusedState);
				}
				void InitializeSGMFields(Slottable slottable){
					slottable.SGM.SetSelectedSB(null);
					slottable.SGM.SetSelectedSG(null);
					slottable.SGM.SetPickedSB(slottable);//picked needs to be set prior to the other two in order to update transaction properly
					slottable.SGM.SetSelectedSB(slottable);
					SlotGroup sg = slottable.SGM.GetSlotGroup(slottable);
					slottable.SGM.SetSelectedSG(sg);
					sg.SetState(SlotGroup.SelectedState);
					// slottable.SGM.SetSelectedSG(slottable.SGM.GetSlotGroup(slottable));
					
				}
			}
			public class PickedUpAndDeselectedState: SlottableState{
				public void EnterState(Slottable sb){
					
					SBProcess puaDesProcess = new PickedUpAndDeselectedProcess(sb, sb.PickedUpAndDeselectedCoroutine);
					sb.SetAndRun(puaDesProcess);
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
					sb.SetState(Slottable.PickedUpAndSelectedState);
					sb.SGM.SetSelectedSB(sb);
				}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void Focus(Slottable sb){
					// sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					// sb.SetState(Slottable.DefocusedState);
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
					slottable.SetState(Slottable.PickedUpAndSelectedState);
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
					// slottable.Revert();
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void Focus(Slottable sb){
					// sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					// sb.SetState(Slottable.DefocusedState);
				}
			}
			public class MovingState: SlottableState{
				public void EnterState(Slottable slottable){
					MoveProcess moveProcess = new MoveProcess(slottable, slottable.MoveCoroutine);
					slottable.SetAndRun(moveProcess);
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
					// sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					// sb.SetState(Slottable.DefocusedState);
				}
			}
			public class EquippedAndDeselectedState: SlottableState{
				public void EnterState(Slottable slottable){
					if(slottable.PrevState == Slottable.DefocusedState || slottable.PrevState == Slottable.EquippedAndDefocusedState){

						SBProcess gradGIProcess = new EquipGradualGrayinProcess(slottable, slottable.EquipGradualGrayinCoroutine);
						slottable.SetAndRun(gradGIProcess);

					}else if(slottable.PrevState == Slottable.SelectedState || slottable.PrevState == Slottable.EquippedAndSelectedState){

						SBProcess gradDhProcess = new EquipGradualDehighlightProcess(slottable, slottable.EquipGradualDehighlightCoroutine);
						slottable.SetAndRun(gradDhProcess);

					}else{
						slottable.InstantEquipGrayin();
						slottable.SetAndRun(null);
					}
				}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					if(slottable.Delayed)
						slottable.SetState(Slottable.WaitForPickUpState);
					else
						slottable.SetState(Slottable.PickedUpAndSelectedState);
					// if(!slottable.SGM.GetSlotGroup(slottable).IsPool){
					// }
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
					if(sb.Item is PartsInstanceMock && !(sb.SGM.GetSlotGroup(sb).Filter is SGPartsFilter))
						sb.SetState(Slottable.DefocusedState);
					else{
						if(!((InventoryItemInstanceMock)sb.Item).IsEquipped)
							sb.SetState(Slottable.FocusedState);
					}
				}
				public void Defocus(Slottable sb){
					if(!((InventoryItemInstanceMock)sb.Item).IsEquipped)
						sb.SetState(Slottable.DefocusedState);
					else
						sb.SetState(Slottable.EquippedAndDefocusedState);
				}
			}
			public class EquippedAndSelectedState: SlottableState{
				public void EnterState(Slottable slottable){}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
				}
				public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(sb);
					sb.SetState(Slottable.EquippedAndDeselectedState);
				}
				public void Focus(Slottable sb){
					// sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					// sb.SetState(Slottable.DefocusedState);
				}
			}
			public class EquippedAndDefocusedState: SlottableState{
				public void EnterState(Slottable slottable){
					if(slottable.PrevState == Slottable.FocusedState || slottable.PrevState == Slottable.EquippedAndDeselectedState){

						SBProcess gradGOProcess = new EquipGradualGrayoutProcess(slottable, slottable.EquipGradualGrayoutCoroutine);
						slottable.SetAndRun(gradGOProcess);

					}else{

						slottable.InstantEquipGrayout();
						slottable.SetAndRun(null);
					}
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
					if(sb.Item is PartsInstanceMock && !(sb.SGM.GetSlotGroup(sb).Filter is SGPartsFilter))
						sb.SetState(Slottable.DefocusedState);
					else{
						if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
							sb.SetState(Slottable.EquippedAndDeselectedState);
						else
							sb.SetState(Slottable.FocusedState);
					}
				}
				public void Defocus(Slottable sb){
					if(!((InventoryItemInstanceMock)sb.Item).IsEquipped)
						sb.SetState(Slottable.DefocusedState);
				}

			}
			public class SBSelectedState: SlottableState{
				public void EnterState(Slottable sb){}
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
					// sb.SetState(Slottable.FocusedState);
				}
				public void Defocus(Slottable sb){
					// sb.SetState(Slottable.DefocusedState);
				}
			}
			// public class SBRevertingState: SlottableState{
				// 	public void EnterState(Slottable sb){
						
				// 		SBProcess revertProcess = new RevertProcess(sb, sb.RevertingStateCoroutine);
				// 		sb.SetAndRun(revertProcess);
				// 	}
				// 	public void ExitState(Slottable sb){
				// 	}
				// 	/*	undefined events
				// 	*/
				// 		public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 		public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 		public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 		public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 		public void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 		public void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){}
				// 		public void Focus(Slottable sb){
				// 			if(sb.Item is PartsInstanceMock && !(sb.SGM.GetSlotGroup(sb).Filter is SGPartsFilter))
				// 			sb.SetState(Slottable.DefocusedState);
				// 			else{
				// 				if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
				// 					sb.SetState(Slottable.EquippedAndDeselectedState);
				// 				else
				// 					sb.SetState(Slottable.FocusedState);
				// 			}
				// 		}
				// 		public void Defocus(Slottable sb){
				// 			if(((InventoryItemInstanceMock)sb.Item).IsEquipped)
				// 				sb.SetState(Slottable.EquippedAndDefocusedState);
				// 			else
				// 				sb.SetState(Slottable.DefocusedState);
				// 		}
				// }
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
					bool flag = m_item.Equals(otherInst.Item);
					flag &= m_item.IsStackable && otherInst.Item.IsStackable;
					return flag;		
				}
				public override int GetHashCode(){
					return m_item.ItemID.GetHashCode() + 31;
				}
				public static bool operator ==(InventoryItemInstanceMock a, InventoryItemInstanceMock b){
					if(object.ReferenceEquals(a, null)){
						return object.ReferenceEquals(b, null);
					}
					if(object.ReferenceEquals(b, null)){
						return object.ReferenceEquals(a, null);
					}
					bool flag = a.Item.ItemID == b.Item.ItemID;
					flag &= a.IsStackable && b.IsStackable;
					return flag;
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
					return m_item.ItemID.CompareTo(otherInst.Item.ItemID);
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
					void Add(SlottableItem item);
				}
				public class PoolInventory: Inventory{
					List<SlottableItem> m_items = new List<SlottableItem>();
					public List<SlottableItem> Items{
						get{return m_items;}
					}
					public void Add(SlottableItem item){
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
					public void Add(SlottableItem item){
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
					List<SlotSystemElement> m_pageElements;
					public EquipmentSet(SlotGroup bowSG, SlotGroup wearSG){
						m_bowSG = bowSG;
						m_wearSG = wearSG;
					}
					public override List<SlotSystemElement> Elements{
						get{
							m_pageElements = new List<SlotSystemElement>();
							m_pageElements.Add(m_bowSG);
							m_pageElements.Add(m_wearSG);
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
							if(m_focusedElement != element){
								if(element == null){
									// if(m_focusedElement != null) => always true
									m_focusedElement.Defocus();
									m_focusedElement = null;
								}else{
									if(m_focusedElement != null)
										m_focusedElement.Defocus();
									m_focusedElement = element;
								}
							}
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
				
				
}
