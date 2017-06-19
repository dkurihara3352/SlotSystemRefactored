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
				Slottable targetSB{get;}
				SlotGroup sg1{get;}
				SlotGroup sg2{get;}
				List<InventoryItemInstanceMock> moved{get;}
				void Indicate();
				void Execute();
				void OnComplete();
			}
			public abstract class AbsSlotSystemTransaction: SlotSystemTransaction{
				public static SlotSystemTransaction GetTransaction(Slottable pickedSB, Slottable targetSB, SlotGroup targetSG){
					/*	notes	*/
						/*	On Second Thought ...	*/
							/*	no more Insert and ReorderInOther transactions, as they unnecessarily complicates behaviout withou adding anything of value
							*/
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

					if(!pickedSB.isPickable){
						throw new System.InvalidOperationException("GetTransaction: pickedSB is NOT in a pickable state");
					}
					SlotGroup origSG = pickedSB.sg;
					if(targetSB != null){
						targetSG = targetSB.sg;
					}
					if(targetSG != null){
						if(targetSG.isPool && targetSG != SlotGroupManager.CurSGM.focusedSGP)
							throw new System.InvalidOperationException("GetTransaction: targetSG is poolSG but not focused");
						else if(targetSG.isSGE && !SlotGroupManager.CurSGM.focusedSGEs.Contains(targetSG))
							throw new System.InvalidOperationException("GetTransaction: targetSG is SGE but does not belong to the focused EquipmentSet");
					}
					if(targetSG == null){// meaning selectedSB is also null
						return new RevertTransaction(pickedSB);
					}else{// hoveredSB could be null
						if(targetSB == null){// on SG
							if(targetSG.AcceptsFilter(pickedSB)){
								if(targetSG != origSG && origSG.isShrinkable){
									if(targetSG.HasItemCurrently(pickedSB.itemInst) && pickedSB.itemInst.Item.IsStackable)
										return new StackTransaction(pickedSB, targetSG.GetSB(pickedSB.itemInst));
										
									if(targetSG.hasEmptySlot){
										if(!targetSG.HasItemCurrently(pickedSB.itemInst))
											return new FillTransaction(pickedSB, targetSG);
									}else{
										if(targetSG.SwappableSBs(pickedSB).Count == 1){
											Slottable calcedSB = targetSG.SwappableSBs(pickedSB)[0];
											if(calcedSB.itemInst != pickedSB.itemInst)
												return new SwapTransaction(pickedSB, calcedSB);
										}else{
											if(targetSG.isExpandable)
												return new FillTransaction(pickedSB, targetSG);
										}
									}
								}
							}
							return new RevertTransaction(pickedSB);
						}else{// targetSB specified, targetSG == targetSB.SG
							if(targetSG == origSG){//
								if(targetSB != pickedSB){
									if(!targetSG.isAutoSort)
										return new ReorderTransaction(pickedSB, targetSB);
								}
							}else{
								if(targetSG.AcceptsFilter(pickedSB)){
									//swap or stack, else insert
									if(pickedSB.itemInst == targetSB.itemInst){
										if(targetSG.isPool && origSG.isShrinkable)
											return new FillTransaction(pickedSB, targetSG);
										if(pickedSB.itemInst.Item.IsStackable)
											return new StackTransaction(pickedSB, targetSB);
									}else{
										if(targetSG.HasItemCurrently(pickedSB.itemInst)){
											if(!origSG.HasItemCurrently(targetSB.itemInst)){
												if(targetSG.isPool){
													if(origSG.AcceptsFilter(targetSB))
														return new SwapTransaction(pickedSB, targetSB);
													if(origSG.isShrinkable)
														return new FillTransaction(pickedSB, targetSG);
												}
												// if(!targetSG.isAutoSort)
												// 	return new ReorderInOtherSGTransaction(targetSB);
											}
										}else{
											if(origSG.AcceptsFilter(targetSB))
												return new SwapTransaction(pickedSB, targetSB);
											if(targetSG.hasEmptySlot || targetSG.isExpandable)
												return new FillTransaction(pickedSB, targetSG);
											// if(!targetSG.isAutoSort)
											// 	return new InsertTransaction(targetSB);
										}
									}
								}
							}
							return new RevertTransaction(pickedSB);
						}
					}
				}
				protected SlotGroupManager sgm = SlotGroupManager.CurSGM;
				protected List<InventoryItemInstanceMock> removed = new List<InventoryItemInstanceMock>();
				protected List<InventoryItemInstanceMock> added = new List<InventoryItemInstanceMock>();
				public virtual Slottable targetSB{get{return null;}}
				public virtual SlotGroup sg1{get{return null;}}
				public virtual SlotGroup sg2{get{return null;}}
				public virtual List<InventoryItemInstanceMock> moved{get{return null;}}
				public virtual void Indicate(){}
				public virtual void Execute(){
					sgm.SetActState(SlotGroupManager.PerformingTransactionState);
				}
				public virtual void OnComplete(){
					sgm.ResetAndFocus();
				}
			}
			public class EmptyTransaction: AbsSlotSystemTransaction{}
			public class RevertTransaction: AbsSlotSystemTransaction{
				Slottable m_pickedSB;
				SlotGroup m_origSG;
				public RevertTransaction(Slottable pickedSB){
					m_pickedSB = pickedSB;
					m_origSG = m_pickedSB.sg;
				}
				public override void Indicate(){}
				public override void Execute(){
					m_origSG.SetActState(SlotGroup.RevertState);
					sgm.dIcon1.SetDestination(m_origSG, m_origSG.GetNewSlot(m_pickedSB.itemInst));
					m_origSG.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					m_origSG.OnCompleteSlotMovementsV3();
					base.OnComplete();
				}
			}
			public class ReorderTransaction: AbsSlotSystemTransaction{
				Slottable m_pickedSB;
				Slottable m_selectedSB;
				SlotGroup m_origSG;
				public ReorderTransaction(Slottable pickedSB, Slottable selected){
					m_pickedSB = pickedSB;
					m_selectedSB = selected;
					m_origSG = m_pickedSB.sg;
				}
				public override Slottable targetSB{get{return m_selectedSB;}}
				public override SlotGroup sg1{get{return m_origSG;}}
				public override void Indicate(){}
				public override void Execute(){
					sg1.SetActState(SlotGroup.ReorderState);
					sgm.dIcon1.SetDestination(sg1, sg1.GetNewSlot(m_pickedSB.itemInst));
					sg1.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovementsV3();
					base.OnComplete();
				}
			}
			// public class ReorderInOtherSGTransaction: AbsSlotSystemTransaction{
				// 	Slottable m_pickedSB;
				// 	Slottable m_selectedSB;
				// 	SlotGroup m_origSG;
				// 	SlotGroup m_selectedSG;
				// 	public ReorderInOtherSGTransaction(Slottable pickedSB, Slottable selected){
				// 		m_pickedSB = pickedSB;
				// 		m_selectedSB = selected;
				// 		m_origSG = pickedSB.sg;
				// 		m_selectedSG = selected.sg;
				// 	}
				// 	public override Slottable targetSB{get{return m_selectedSB;}}
				// 	public override SlotGroup sg1{get{return m_origSG;}}
				// 	public override SlotGroup sg2{get{return m_selectedSG;}}
				// 	public override void Indicate(){}
				// 	public override void Execute(){
				// 		sg1.SetActState(SlotGroup.RevertState);
				// 		sg2.SetActState(SlotGroup.ReorderState);
				// 		sgm.dIcon1.SetDestination(sg1, sg1.GetNewSlot(m_pickedSB.itemInst));
				// 		base.Execute();
				// 	}
				// 	public override void OnComplete(){
				// 		sg1.OnCompleteSlotMovementsV2();
				// 		sg2.OnCompleteSlotMovementsV2();
				// 		base.OnComplete();
				// 	}
				// }
			public class StackTransaction: AbsSlotSystemTransaction{
				Slottable m_pickedSB;
				SlotGroup m_origSG;
				Slottable m_selectedSB;
				SlotGroup m_selectedSG;
				List<InventoryItemInstanceMock> itemCache = new List<InventoryItemInstanceMock>();
				public StackTransaction(Slottable pickedSB ,Slottable selected){
					m_pickedSB = pickedSB;
					m_origSG = pickedSB.sg;
					m_selectedSB = selected;
					m_selectedSG = m_selectedSB.sg;
					InventoryItemInstanceMock cache = pickedSB.itemInst;
					cache.Quantity = pickedSB.pickedAmount;
					itemCache.Add(cache);
				}
				public override Slottable targetSB{get{return m_selectedSB;}}
				public override SlotGroup sg1{get{return m_origSG;}}
				public override SlotGroup sg2{get{return m_selectedSG;}}
				public override List<InventoryItemInstanceMock> moved{get{return itemCache;}}
				public override void Indicate(){}
				public override void Execute(){
					sg1.SetActState(SlotGroup.RemoveState);
					sg2.SetActState(SlotGroup.AddState);
					sgm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovementsV2();
					sg2.OnCompleteSlotMovementsV2();
					base.OnComplete();
				}
			}
			public class SwapTransaction: AbsSlotSystemTransaction{
				Slottable m_pickedSB;
				SlotGroup m_origSG;
				Slottable m_selectedSB;
				SlotGroup m_selectedSG;
				public SwapTransaction(Slottable pickedSB, Slottable selected){
					m_pickedSB = pickedSB;
					m_selectedSB = selected;
					m_origSG = m_pickedSB.sg;
					m_selectedSG = m_selectedSB.sg;
				}
				public override Slottable targetSB{get{return m_selectedSB;}}
				public override SlotGroup sg1{get{return m_origSG;}}
				public override SlotGroup sg2{get{return m_selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					sg1.SetActState(SlotGroup.SwapState);
					sg2.SetActState(SlotGroup.SwapState);
					sgm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
					DraggedIcon di2 = new DraggedIcon(targetSB);
					sgm.SetDIcon2(di2);
					sgm.dIcon2.SetDestination(sg1, sg1.GetNewSlot(targetSB.itemInst));
					sg1.OnActionExecute();
					sg2.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovementsV3();
					sg2.OnCompleteSlotMovementsV3();
					base.OnComplete();
				}
			}
			public class FillTransaction: AbsSlotSystemTransaction{
				Slottable m_pickedSB;
				SlotGroup m_selectedSG;
				SlotGroup m_origSG;
				public FillTransaction(Slottable pickedSB, SlotGroup selected){
					m_pickedSB = pickedSB;
					m_selectedSG = selected;
					m_origSG = m_pickedSB.sg;
				}
				public override SlotGroup sg1{get{return m_origSG;}}
				public override SlotGroup sg2{get{return m_selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					sg1.SetActState(SlotGroup.FillState);
					sg2.SetActState(SlotGroup.FillState);
					sgm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
					sg1.OnActionExecute();
					sg2.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovementsV3();
					sg2.OnCompleteSlotMovementsV3();
					base.OnComplete();
				}
			}
			public class SortTransaction: AbsSlotSystemTransaction{
				SlotGroup m_selectedSG;
				SGSorter m_sorter;
				public SortTransaction(SlotGroup sg, SGSorter sorter){
					m_selectedSG = sg;
					m_sorter = sorter;
				}
				public override SlotGroup sg1{get{return m_selectedSG;}}
				public override void Indicate(){}
				public override void Execute(){
					sg1.SetSorter(m_sorter);
					sg1.SetActState(SlotGroup.SortState);
					sg1.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovementsV3();
					base.OnComplete();
				}
			}
			// public class InsertTransaction: AbsSlotSystemTransaction{
				// 	Slottable m_pickedSB;
				// 	SlotGroup m_origSG;
				// 	Slottable m_selectedSB;
				// 	SlotGroup m_selectedSG;
				// 	public InsertTransaction(Slottable pickedSB, Slottable sb){
				// 		m_pickedSB = pickedSB;
				// 		m_origSG = m_pickedSB.sg;
				// 		m_selectedSB = sb;
				// 		m_selectedSG = sb.sg;
				// 	}
				// 	public override Slottable targetSB{get{return m_selectedSB;}}
				// 	public override SlotGroup sg1{get{return m_origSG;}}
				// 	public override SlotGroup sg2{get{return m_origSG;}}
				// 	public override void Indicate(){}
				// 	public override void Execute(){
				// 		sg1.SetActState(SlotGroup.FillState);
				// 		sg2.SetActState(SlotGroup.FillState);
				// 		sgm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
				// 		base.Execute();
				// 	}
				// 	public override void OnComplete(){
				// 		sg1.OnCompleteSlotMovementsV2();
				// 		sg2.OnCompleteSlotMovementsV2();
				// 		sgm.UpdateEquipStatesOnAll();
				// 		base.OnComplete();
				// 	}
				// }
		/*	commands	*/
			public interface SGMCommand{
				void Execute(SlotGroupManager sgm);
			}
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
						sgm.rootPage.Defocus();
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
						sgm.rootPage.Focus();
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
						if(sgm.PrevActState == SlotGroupManager.WaitForActionState)
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
				public override void Start(){
					base.Start();
				}
				public override void Expire(){
					base.Expire();
					SGM.OnAllTransactionComplete();
				}
			}
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
						sg.sgm.SetHoveredSG(sg);
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
				public class SGRevertState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sg.UpdateSBs(new List<Slottable>(sg.toList));
						// sg.SetNewSBs(sg.toList);
						// sg.CreateNewSlots();
						// sg.SetSBsActStates();
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGReorderState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						Slottable sb1 = sg.sgm.pickedSB;
						Slottable sb2 = sg.sgm.targetSB;
						List<Slottable> newSBs = new List<Slottable>(sg.toList);
						newSBs.Reorder(sb1, sb2);
						// newSBs.AddRange(sg.toList);
						// sg.SetNewSBs(newSBs);
						// sg.CreateNewSlots();
						// sg.SetSBsActStates();
						sg.UpdateSBs(newSBs);
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGAddState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						List<InventoryItemInstanceMock> cache = sg.sgm.Transaction.moved;
						List<Slottable> newSBs = sg.toList;
						int origCount = newSBs.Count;
						// Util.Trim(ref newSBs);
						foreach(InventoryItemInstanceMock itemInst in cache){
							bool found = false;
							foreach(Slottable sb in newSBs){
								if(sb!= null){
									if(sb.itemInst == itemInst){
										if(itemInst.Item.IsStackable){
											sb.itemInst.Quantity += itemInst.Quantity;
											found = true;
										}
									}
								}
							}
							if(!found){
								GameObject newSBSG = new GameObject("newSBSG");
								Slottable newSB = newSBSG.AddComponent<Slottable>();
								// newSB.Initialize(sg, true, itemInst);
								newSB.Initialize(itemInst);
								newSB.SetSGM(sg.sgm);
								newSB.Defocus();
								Util.AddInEmptyOrConcat(ref newSBs, newSB);
							}
						}
						if(sg.isAutoSort)
							sg.Sorter.TrimAndOrderSBs(ref newSBs);
						if(!sg.isExpandable){
							while(newSBs.Count <origCount){
								newSBs.Add(null);
							}
						}
						sg.SetNewSBs(newSBs);
						sg.CreateNewSlots();
						sg.SetSBsActStates();
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGRemoveState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						List<InventoryItemInstanceMock> cache = sg.sgm.Transaction.moved;
						List<Slottable> newSBs = sg.toList;
						int origCount = newSBs.Count;
						// Util.Trim(ref newSBs);
						List<Slottable> removedList = new List<Slottable>();
						List<Slottable> nonremoved = new List<Slottable>();
						foreach(InventoryItemInstanceMock itemInst in cache){
							foreach(Slottable sb in newSBs){
								if(sb!= null){
									if(sb.itemInst == itemInst){
										if(itemInst.Item.IsStackable){
											sb.itemInst.Quantity -= itemInst.Quantity;
											if(sb.itemInst.Quantity <= 0)
												removedList.Add(sb);
										}else{
											removedList.Add(sb);
										}
									}
								}
							}
						}
						foreach(Slottable sb in removedList){
							newSBs[newSBs.IndexOf(sb)] = null;
						}
						if(sg.isAutoSort){
							sg.Sorter.TrimAndOrderSBs(ref newSBs);
							if(!sg.isExpandable){
								while(newSBs.Count <origCount){
									newSBs.Add(null);
								}
							}
						}else{
							if(sg.isExpandable)
								Util.Trim(ref newSBs);
						}
						sg.SetNewSBs(nonremoved);
						sg.CreateNewSlots();
						sg.SetSBsActStates();
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGSwapState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						Slottable added;
							if(sg.sgm.Transaction.sg1 == sg)
								added = sg.sgm.Transaction.targetSB;
							else
								added = sg.sgm.pickedSB;
						Slottable removed;
							if(sg.sgm.Transaction.sg1 == sg)
								removed = sg.sgm.pickedSB;
							else
								removed = sg.sgm.Transaction.targetSB;
						List<Slottable> newSBs = new List<Slottable>(sg.toList);
						int origCount = newSBs.Count;
						if(!sg.isPool){
							GameObject newSBGO = new GameObject("newSBGO");
							Slottable newSB = newSBGO.AddComponent<Slottable>();
							newSB.Initialize(added.itemInst);
							newSB.SetSGM(sg.sgm);
							newSB.SetEqpState(Slottable.UnequippedState);
							newSB.Defocus();
							newSBs[newSBs.IndexOf(removed)] = newSB;
						}
						if(sg.isAutoSort){
							sg.Sorter.TrimAndOrderSBs(ref newSBs);
							if(!sg.isExpandable){
								while(newSBs.Count <origCount){
									newSBs.Add(null);
								}
							}
						}
						sg.UpdateSBs(newSBs);
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGFillState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						Slottable added;
							if(sg.sgm.Transaction.sg1 == sg)
								added = null;
							else
								added = sg.sgm.pickedSB;
						Slottable removed;
							if(sg.sgm.Transaction.sg1 == sg)
								removed = sg.sgm.pickedSB;
							else
								removed = null;

						List<Slottable> newSBs = new List<Slottable>(sg.toList);
						int origCount = newSBs.Count;
						if(!sg.isPool){
							if(added != null){
								GameObject newSBGO = new GameObject("newSBGO");
								Slottable newSB = newSBGO.AddComponent<Slottable>();
								newSB.Initialize(added.itemInst);
								newSB.SetSGM(sg.sgm);
								newSB.Defocus();
								newSB.SetEqpState(Slottable.UnequippedState);
								Util.AddInEmptyOrConcat(ref newSBs, newSB);
							}
							if(removed != null){
								Slottable rem = null;
								foreach(Slottable sb in newSBs){
									if(sb != null){
										if(sb.itemInst == removed.itemInst)
											rem = sb;
									}
								}
								newSBs[newSBs.IndexOf(rem)] = null;
							}
						}
						if(sg.isAutoSort){
							sg.Sorter.TrimAndOrderSBs(ref newSBs);
						// }else if(sg.sgm.Transaction is InsertTransaction){
						// 	Util.Trim(ref newSBs);
						// 	Slottable newAdded = null;
						// 	foreach(Slottable sb in newSBs){
						// 		if(sb.itemInst == added.itemInst)
						// 			newAdded = sb;
						// 	}
						// 	Slottable targetSB = sg.sgm.targetSB;
						// 	newSBs.Reorder(newAdded, targetSB);
						// 	// Util.ReorderSBs(newAdded, targetSB, ref newSBs);
						}
						if(!sg.isExpandable){
							while(newSBs.Count <origCount){
								newSBs.Add(null);
							}
						}
						// sg.SetNewSBs(newSBs);
						// sg.CreateNewSlots();
						// sg.SetSBsActStates();
						sg.UpdateSBs(newSBs);
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGSortState: SGActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						List<Slottable> newSBs = new List<Slottable>(sg.toList);
						int origCount = newSBs.Count;
						sg.Sorter.TrimAndOrderSBs(ref newSBs);
						if(!sg.isExpandable){
							while(newSBs.Count <origCount){
								newSBs.Add(null);
							}
						}
						sg.UpdateSBs(newSBs);
						// sg.SetNewSBs(newSBs);
						// sg.CreateNewSlots();
						// sg.SetSBsActStates();
						if(sg.PrevActState != null && sg.PrevActState == SlotGroup.WaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
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
					SG.sgm.AcceptSGTAComp(SG);
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
					List<SlottableItem> items = new List<SlottableItem>(sg.inventory);
					sg.Filter.Filter(ref items);
					/*	Slots	*/
						List<Slot> newSlots = new List<Slot>();
						int slotCountToCreate = sg.initSlotsCount == 0? items.Count: sg.initSlotsCount;
						for(int i = 0; i <slotCountToCreate; i++){
							Slot newSlot = new Slot();
							newSlots.Add(newSlot);
						}
						sg.SetSlots(newSlots);
					/*	SBs	*/
						/*	if the number of filtered items exceeds the slot count, remove unfittable items from the inventory	*/
						while(sg.slots.Count < items.Count){
							items.RemoveAt(sg.slots.Count);
						}
						foreach(SlottableItem item in items){
							GameObject newSBGO = new GameObject("newSBGO");
							Slottable newSB = newSBGO.AddComponent<Slottable>();
							newSB.Initialize((InventoryItemInstanceMock)item);
							newSB.SetSGM(sg.sgm);
							sg.slots[items.IndexOf(item)].sb = newSB;
						}
						sg.SyncSBsToSlots();
					if(sg.isAutoSort)
						sg.InstantSort();
				}
			}
			public class SGUpdateEquipStatusCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.sgm.UpdateEquipStatesOnAll();
				}
			}
			public class SGEmptyCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
				}
			}
			public class SGUpdateEquipAtExecutionCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					/*	update inventory
						update item's equip status
						update sb's equip status and state
					*/
					foreach(Slottable sb in sg){
						if(sb != null){
							InventoryItemInstanceMock item = sb.itemInst;
							if(sb.newSlotID == -1){/* removed	*/
								sg.inventory.Remove(item);
								sg.sgm.MarkEquippedInPool(item, false);
								sg.sgm.SetEquippedOnAllSBs(item, false);
								/*	Set unequipped with transition
										all sbp in FocusedSGP
									Set unequipped without transition
										sll sbp in Defocused SGPs
								*/
							}else if(sb.slotID == -1){/*	added	*/
								sg.inventory.Add(item);
								sg.sgm.MarkEquippedInPool(item, true);
								sg.sgm.SetEquippedOnAllSBs(item, true);
								/*	Set equipped with transition
										all the sbp in Focused SGP
										all sbe in FocusedSGEs (NOT those defocused)
									Set equipped without transition
										all sbp in defocused SGPs
								*/
							}else{/*	merely moved	*/
								/*	do nothing	*/
							}
						}
					}
				}
			}
		/*	filters	*/
			public interface SGFilter{
				void Filter(ref List<SlottableItem> items);
			}
			public class SGNullFilter: SGFilter{
				public void Filter(ref List<SlottableItem> items){}
			}
			public class SGBowFilter: SGFilter{
				public void Filter(ref List<SlottableItem> items){
					List<SlottableItem> res = new List<SlottableItem>();
					foreach(SlottableItem item in items){
						if(item is BowInstanceMock)
							res.Add(item);
					}
					items = res;
				}
			}
			public class SGWearFilter: SGFilter{
				public void Filter(ref List<SlottableItem> items){
					List<SlottableItem> res = new List<SlottableItem>();
					foreach(SlottableItem item in items){
						if(item is WearInstanceMock)
							res.Add(item);
					}
					items = res;
				}
			}
			public class SGCGearsFilter: SGFilter{
				public void Filter(ref List<SlottableItem> items){
					List<SlottableItem> res = new List<SlottableItem>();
					foreach(SlottableItem item in items){
						if(item is CarriedGearInstanceMock)
							res.Add(item);
					}
					items = res;
				}
			}
			public class SGPartsFilter: SGFilter{
				public void Filter(ref List<SlottableItem> items){
					List<SlottableItem> res = new List<SlottableItem>();
					foreach(SlottableItem item in items){
						if(item is PartsInstanceMock)
							res.Add(item);
					}
					items = res;
				}
			}
		/*	sorters	*/
			public interface SGSorter{
				void OrderSBsWithRetainedSize(ref List<Slottable> sbs);
				void TrimAndOrderSBs(ref List<Slottable> sbs);
			}	
			public class　SGItemIDSorter: SGSorter{
				public void OrderSBsWithRetainedSize(ref List<Slottable> sbs){
					int origCount = sbs.Count;
					List<Slottable> trimmed = sbs;
					this.TrimAndOrderSBs(ref trimmed);
					while(trimmed.Count < origCount){
						trimmed.Add(null);
					}
					sbs = trimmed;
				}
				public void TrimAndOrderSBs(ref List<Slottable> sbs){
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
				public void OrderSBsWithRetainedSize(ref List<Slottable> sbs){
					int origCount = sbs.Count;
					List<Slottable> trimmed = sbs;
					this.TrimAndOrderSBs(ref trimmed);
					while(trimmed.Count < origCount){
						trimmed.Add(null);
					}
					sbs = trimmed;
				}
				public void TrimAndOrderSBs(ref List<Slottable> sbs){
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
				public void OrderSBsWithRetainedSize(ref List<Slottable> sbs){
					int origCount = sbs.Count;
					List<Slottable> trimmed = sbs;
					this.TrimAndOrderSBs(ref trimmed);
					while(trimmed.Count < origCount){
						trimmed.Add(null);
					}
					sbs = trimmed;
				}
				public void TrimAndOrderSBs(ref List<Slottable> sbs){
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
						else addedAO = ((InventoryItemInstanceMock)addedMax.item).AcquisitionOrder;

						for(int i = 0; i < trimmed.Count; i++){
							InventoryItemInstanceMock inst = (InventoryItemInstanceMock)trimmed[i].item;
							if(inst.AcquisitionOrder > addedAO){
								if(indexAtMin == -1 || inst.AcquisitionOrder < ((InventoryItemInstanceMock)trimmed[indexAtMin].item).AcquisitionOrder){
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
					}
				}
				public class SBPickedUpProcess: AbsSBProcess{
					public SBPickedUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
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
						if(!SB.isPickedUp){
							SB.Tap();
							SB.Reset();
							SB.Focus();
						}else{
							SB.ExecuteTransaction();
						}
					}
				}
				public class SBRemoveProcess: AbsSBProcess{
					public SBRemoveProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						// SB.ClearDraggedIconDestination();
					}
				}
				public class SBAddProcess: AbsSBProcess{
					public SBAddProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						// SB.ClearDraggedIconDestination();
					}
				}
				public class SBMoveWithinProcess: AbsSBProcess{
					public SBMoveWithinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
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
					}
				}
				public class SBEquipProcess: AbsSBProcess{
					public SBEquipProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						SB = sb;
						CoroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
					}
				}
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
						sb.sgm.SetHoveredSB(sb);
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
						sb.Reset();
						sb.Defocus();
					}
					public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Reset();
						sb.Defocus();
					}
					public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){
						if(sb.isFocused){
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
						sb.Reset();
						sb.Defocus();
					}
					public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Reset();
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
					}
					public override void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Reset();
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
						if(!sb.isPickedUp)
							sb.PickUp();
						else{
							sb.SetActState(Slottable.PickedUpState);
							sb.Increment();
						}
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Reset();
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
						sb.sgm.SetPickedSB(sb);
						sb.sgm.SetActState(SlotGroupManager.ProbingState);
						DraggedIcon di = new DraggedIcon(sb);
						sb.sgm.SetDIcon1(di);
						sb.sgm.CreateTransactionResults();
						sb.OnHoverEnterMock();
						sb.sgm.UpdateTransaction();
						SBProcess pickedUpProcess = new SBPickedUpProcess(sb, sb.PickUpCoroutine);
						sb.SetAndRunActionProcess(pickedUpProcess);
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Reset();
						sb.Focus();
					}
					public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						if(sb.sgm.hoveredSB == sb && sb.isStackable)
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
						SBRemoveProcess process = new SBRemoveProcess(sb, sb.RemoveCoroutine);
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
						SBAddProcess process = new SBAddProcess(sb, sb.AddCorouine);
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
				public class SBMoveWithinState: SBActionState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.MoveWithinCoroutine);
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
						if(sb.sg.isPool){
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
						if(sb.PrevEqpState == null || sb.PrevEqpState == Slottable.UnequippedState){
							/*	when initialized	*/
							return;
						}
						if(sb.sg.isPool){
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
		/*	commands	*/
			public interface SlottableCommand{
				void Execute(Slottable sb);
			}
			public class SBTapCommand: SlottableCommand{
				public void Execute(Slottable sb){

				}
			}
	/*	Other Classes	*/
		public class Slot{
			Slottable m_sb;
			public Slottable sb{
				get{return m_sb;}
				set{m_sb = value;}
			}
			Vector2 m_position;
			public Vector2 Position{
				get{return m_position;}
				set{m_position = value;}
			}
		}
		public class DraggedIcon{
			public InventoryItemInstanceMock item{
				get{return m_item;}
				}InventoryItemInstanceMock m_item;
			public IconDestination dest{
				get{return m_dest;}
				}IconDestination m_dest;
				public void SetDestination(SlotGroup sg, Slot slot){
					IconDestination newDest = new IconDestination(sg, slot);
					m_dest = newDest;
				}
			SlotGroupManager m_sgm;
			public Slottable sb{
				get{return m_sb;}
				}Slottable m_sb;
			public DraggedIcon(Slottable sb){
				m_sb = sb;
				m_item = this.sb.itemInst;
				m_sgm = SlotGroupManager.CurSGM;
			}
			public void CompleteMovement(){
				m_sgm.AcceptDITAComp(this);
			}
		}
		public class IconDestination{
			public SlotGroup sg{
				get{return m_sg;}
				}SlotGroup m_sg;
				public void SetSG(SlotGroup sg){
					m_sg = sg;
				}
			public Slot slot{
				get{return m_slot;}
				}Slot m_slot;
				public void SetSlot(Slot slot){
					m_slot = slot;
				}
			public IconDestination(SlotGroup sg, Slot slot){
				SetSG(sg); SetSlot(slot);
			}
		}
		/*	SlotSystemElements	*/
			public interface SlotSystemElement: IEnumerable<SlotSystemElement>{
				void Activate();
				void Deactivate();
				void Focus();
				void Defocus();
				SlotGroupManager sgm{get;set;}
				SlotSystemElement DirectParent(SlotSystemElement element);
				bool ContainsInHierarchy(SlotSystemElement ele);
				void PerformInHierarchy(System.Action<SlotSystemElement> act);
				void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj);
				SlotSystemElement rootElement{get;set;}
				int level{get;}
				bool Contains(SlotSystemElement element);
				SlotSystemElement this[int i]{get;}
			}
			public abstract class AbsSlotSysElement: SlotSystemElement{
				// public abstract List<SlotSystemElement> Elements{get;}
				protected abstract List<SlotSystemElement> elements{get;}
				public IEnumerator<SlotSystemElement> GetEnumerator(){
					foreach(SlotSystemElement ele in elements)
						yield return ele;
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}
				public bool Contains(SlotSystemElement element){
					return elements.Contains(element);
				}
				public SlotSystemElement this[int i]{
					get{return elements[i];}
				}
				public virtual bool ContainsInHierarchy(SlotSystemElement ele){
					return DirectParent(ele) != null;
				}
				public virtual SlotSystemElement DirectParent(SlotSystemElement element){
					foreach(SlotSystemElement ele in this){
						if(ele == element)
							return this;
						else{
							SlotSystemElement dirPar = ele.DirectParent(element);
							if(dirPar != null)
								return dirPar;
						}
					}
					return null;
				}
				public virtual void Activate(){
					foreach(SlotSystemElement ele in this){
						ele.Activate();
					}
				}
				public virtual void Deactivate(){
					foreach(SlotSystemElement ele in this){
						ele.Deactivate();
					}
				}
				public virtual void Focus(){
					foreach(SlotSystemElement ele in this){
						ele.Focus();
					}
				}
				public virtual void Defocus(){
					foreach(SlotSystemElement ele in this){
						ele.Defocus();
					}
				}
				public SlotGroupManager sgm{
					get{return m_sgm;}
					set{m_sgm = value;}
					}SlotGroupManager m_sgm;
				public void PerformInHierarchy(System.Action<SlotSystemElement> act){
					act(this);
					foreach(SlotSystemElement ele in this){
						ele.PerformInHierarchy(act);
					}
				}
				public void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){
					act(this, obj);
					foreach(SlotSystemElement ele in this){
						ele.PerformInHierarchy(act, obj);
					}
				}
				public int level{
					get{
						if(rootElement == this)
							return 0;
						else
							return rootElement.DirectParent(this).level + 1;
					}
				}
				public virtual SlotSystemElement rootElement{
					get{return m_rootElement;}
					set{m_rootElement = value;}
					}
					SlotSystemElement m_rootElement;
			}
			public class InventoryManagerPage: AbsSlotSysElement{
				public SlotSystemBundle PoolBundle{
					get{return m_poolBundle;}
					}SlotSystemBundle m_poolBundle;
				public SlotSystemBundle EquipBundle{
					get{return m_equipBundle;}
					}SlotSystemBundle m_equipBundle;
				public List<SlotSystemBundle> otherBundles{
					get{
						if(m_otherBundles == null)
							m_otherBundles = new List<SlotSystemBundle>();
						return m_otherBundles;
						}
					}List<SlotSystemBundle> m_otherBundles;

				public InventoryManagerPage(SlotSystemBundle poolBundle, SlotSystemBundle equipBundle){
					this.m_poolBundle = poolBundle;
					this.m_equipBundle = equipBundle;
					PerformInHierarchy(SetRoot);
				}
				protected override List<SlotSystemElement> elements{
					get{
						List<SlotSystemElement> pageElements = new List<SlotSystemElement>();
						pageElements.Add(m_poolBundle);
						pageElements.Add(m_equipBundle);
						return pageElements;
					}
				}
				void SetRoot(SlotSystemElement ele){
					ele.rootElement = this;
				}
				public override SlotSystemElement rootElement{
					get{return this;}
					set{}
				}
				public void SetSGMRecursively(SlotGroupManager sgm){
					this.sgm = sgm;
					PerformInHierarchy(SetSGM);
				}
				public void SetSGM(SlotSystemElement ele){
					ele.sgm = this.sgm;
				}
			}
			public class GenericPage: AbsSlotSysElement{
				protected override List<SlotSystemElement> elements{
					get{return m_elements;}
				}List<SlotSystemElement> m_elements;
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
				protected override List<SlotSystemElement> elements{
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
				protected override List<SlotSystemElement> elements{
					get{return m_elements;}
				}
				public void Add(SlotSystemElement element){
					elements.Add(element);
				}
				public void Remove(SlotSystemElement element){
					elements.Remove(element);
				}
				public SlotSystemElement focusedElement{
					get{return m_focusedElement;}
					}SlotSystemElement m_focusedElement;
					public void SetFocusedBundleElement(SlotSystemElement element){
						if(DirectParent(element) == this)
							m_focusedElement = element;
						else
							throw new InvalidOperationException("trying to set focsed element that is not one of its members");
					}
				public override void Focus(){
					if(m_focusedElement != null)
						m_focusedElement.Focus();
					foreach(SlotSystemElement ele in this){
						if(ele != m_focusedElement)
						ele.Defocus();
					}
				}
				public override void Defocus(){
					foreach(SlotSystemElement ele in this){
						ele.Defocus();
					}
				}	
			}
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
					if((sb != null && this.selectedSB == sb) ||
						(sg != null && this.selectedSG == sg))
							return ta;
					else return null;
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
				public bool isEquipped{
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
			public interface Inventory: IEnumerable<SlottableItem>{
				// List<SlottableItem> items{get;}
				void Add(SlottableItem item);
				void Remove(SlottableItem item);
				SlotGroup sg{get;}
				void SetSG(SlotGroup sg);
				SlottableItem this[int i]{get;}
				int count{get;}
				bool Contains(SlottableItem item);
			}
			public class GenericInventory: Inventory{
				public IEnumerator<SlottableItem> GetEnumerator(){
					foreach(SlottableItem item in items){
						yield return item;
					}
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}

				public int count{
					get{return items.Count;}
				}
				public bool Contains(SlottableItem item){
					return items.Contains(item);
				}
				public SlottableItem this[int i]{
					get{return items[i];}
				}
				List<SlottableItem> items = new List<SlottableItem>();
				public void Add(SlottableItem item){
					items.Add(item);
				}
				public void Remove(SlottableItem item){
					items.Remove(item);
				}
				public SlotGroup sg{
					get{return m_sg;}
					}SlotGroup m_sg;
					public void SetSG(SlotGroup sg){
						m_sg = sg;
					}
			}
			public class PoolInventory: Inventory{
				public IEnumerator<SlottableItem> GetEnumerator(){
					foreach(SlottableItem item in m_items){
						yield return item;
					}
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}
				public bool Contains(SlottableItem item){
					return m_items.Contains(item);
				}
				public int count{
					get{return m_items.Count;}
				}
				public SlottableItem this[int i]{
					get{return m_items[i];}
				}
				List<SlottableItem> m_items = new List<SlottableItem>();
				public SlotGroup sg{get{return m_sg;}}
					SlotGroup m_sg;
					public void SetSG(SlotGroup sg){
						m_sg = sg;
					}
				// public List<SlottableItem> items{
				// 	get{return m_items;}
				// }
				public void Add(SlottableItem item){
					// m_items.Add(item);
					foreach(SlottableItem it in m_items){
						InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)it;
						InventoryItemInstanceMock addedInst = (InventoryItemInstanceMock)item;
						if(invInst == addedInst && invInst.IsStackable){
							invInst.Quantity += addedInst.Quantity;
							return;
						}
					}
					m_items.Add(item);
					IndexItems();
				}
				public void Remove(SlottableItem item){
					SlottableItem itemToRemove = null;
					foreach(SlottableItem it in m_items){
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
						m_items.Remove(itemToRemove);
					IndexItems();
				}
				void IndexItems(){
					for(int i = 0; i < m_items.Count; i ++){
						((InventoryItemInstanceMock)m_items[i]).SetAcquisitionOrder(i);
					}
				}
			}
			public class EquipmentSetInventory: Inventory{
				public IEnumerator<SlottableItem> GetEnumerator(){
					foreach(SlottableItem item in m_items){
						yield return item;
					}
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}
				public EquipmentSetInventory(BowInstanceMock initBow, WearInstanceMock initWear, List<CarriedGearInstanceMock> initCGears ,int initCGCount){
					m_equippedBow = initBow;
					m_equippedWear = initWear;
					m_equippedCGears = initCGears;
					SetEquippableCGearsCount(initCGCount);
				}
				public bool Contains(SlottableItem item){
					foreach(SlottableItem it in this){
						if(it == item)
							return true;
					}
					return false;
				}
				public int count{
					get{return m_items.Count;}
				}
				public SlottableItem this[int i]{
					get{return m_items[i];}
				}
				public SlotGroup sg{get{return m_sg;}}
					SlotGroup m_sg;
					public void SetSG(SlotGroup sg){
						m_sg = sg;
					}
				BowInstanceMock m_equippedBow;
				WearInstanceMock m_equippedWear;
				List<CarriedGearInstanceMock> m_equippedCGears = new List<CarriedGearInstanceMock>();
				public int equippableCGearsCount{
					get{return m_equippableCGearsCount;}
					}int m_equippableCGearsCount;
				public void SetEquippableCGearsCount(int num){
					m_equippableCGearsCount = num;
					if(sg != null && sg.Filter is SGCGearsFilter && !sg.isExpandable)
					sg.SetInitSlotsCount(num);
				}
				
				List<SlottableItem> m_items{
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
				public void Remove(SlottableItem removedItem){
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
			public class CarriedGearInstanceMock: InventoryItemInstanceMock{}
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
			public class PartsInstanceMock: InventoryItemInstanceMock{}
	/*	utility	*/
		public static class Util{
			public static void Trim(ref List<Slottable> sbs){
				List<Slottable> trimmed = new List<Slottable>();
				foreach(Slottable sb in sbs){
					if(sb != null)
						trimmed.Add(sb);
				}
				sbs = trimmed;
			}
			public static void AddInEmptyOrConcat(ref List<Slottable> sbs, Slottable added){
				foreach(Slottable sb in sbs){
					if(sb == null){
						sbs[sbs.IndexOf(sb)] = added;
						return;
					}
				}
				sbs.Add(added);
			}
			public static bool HaveCommonItemFamily(Slottable sb, Slottable other){
				if(sb.item is BowInstanceMock)
					return (other.item is BowInstanceMock);
				else if(sb.item is WearInstanceMock)
					return (other.item is WearInstanceMock);
				else if(sb.item is CarriedGearInstanceMock)
					return (other.item is CarriedGearInstanceMock);
				else if(sb.item is PartsInstanceMock)
					return (other.item is PartsInstanceMock);
				else
					return false;
			}
			public static bool IsSwappable(Slottable pickedSB, Slottable otherSB){
				/*	precondition
						1) they do not share same SG
						2) otherSB.SG accepts pickedSB
						3) not stackable
				*/
				if(pickedSB.sg != otherSB.sg){
					if(otherSB.sg.AcceptsFilter(pickedSB)){
						if(!(pickedSB.itemInst == otherSB.itemInst && pickedSB.itemInst.Item.IsStackable))
						 if(pickedSB.sg.AcceptsFilter(otherSB))
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
					// else if(ta is ReorderInOtherSGTransaction)
					// 	res = Util.Green("ReorderInOtherSGTA");
					else if(ta is StackTransaction)
						res = Util.Aqua("StackTA");
					else if(ta is SwapTransaction)
						res = Util.Terra("SwapTA");
					else if(ta is FillTransaction)
						res = Util.Forest("FillTA");
					// else if(ta is FillTransaction)
					// 	res = Util.Berry("FillEquipTA");
					else if(ta is SortTransaction)
						res = Util.Khaki("SortTA");
					// else if(ta is InsertTransaction)
					// 	res = Util.Midnight("InsertTA");
					else if(ta is EmptyTransaction)
						res = Util.Beni("Empty");
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
					string pSB = Util.SBofSG(sgm.pickedSB);
					string tSB = Util.SBofSG(sgm.targetSB);
					string hSG = Util.SGName(sgm.hoveredSG);
					string hSB = Util.SBofSG(sgm.hoveredSB);
					string di1;
						if(sgm.dIcon1 == null)
							di1 = "null";
						else
							di1 = Util.SBofSG(sgm.dIcon1.sb);
					string di2;
						if(sgm.dIcon2 == null)
							di2 = "null";
						else
							di2 = Util.SBofSG(sgm.dIcon2.sb);
					
					string sg1 = Util.SGName(sgm.sg1);
					string sg2 = Util.SGName(sgm.sg2);
					string prevSel = Util.SGMStateName(sgm.PrevSelState);
					string curSel = Util.SGMStateName(sgm.CurSelState);
					string selProc;
						if(sgm.SelectionProcess == null)
							selProc = "";
						else
							selProc = Util.SGMProcessName(sgm.SelectionProcess) + " exp? " + (sgm.SelectionProcess.IsExpired?Blue("true"):Red("false"));
					string prevAct = Util.SGMStateName(sgm.PrevActState);
					string curAct = Util.SGMStateName(sgm.CurActState);
					string actProc;
						if(sgm.ActionProcess == null)
							actProc = "";
						else
							actProc = Util.SGMProcessName(sgm.ActionProcess) + " exp? " + (sgm.ActionProcess.IsExpired?Blue("true"):Red("false"));
					string ta = Util.TransactionName(sgm.Transaction);
					string d1Done = "d1Done: " + (sgm.dIcon1Done?Util.Blue("true"):Util.Red("false"));
					string d2Done = "d2Done: " + (sgm.dIcon2Done?Util.Blue("true"):Util.Red("false"));
					string sg1Done = "sg1Done: " + (sgm.sg1Done?Util.Blue("true"):Util.Red("false"));
					string sg2Done = "sg2Done: " + (sgm.sg2Done?Util.Blue("true"):Util.Red("false"));
					res = Bold("DebugTarget: ") + Util.Bold("SGM:") +
							" pSB " + pSB +
							", tSB " + tSB +
							", hSG " + hSG +
							", hSB " + hSB +
							", di1 " + di1 +
							", di2 " + di2 +
							", sg1 " + sg1 +
							", sg2 " + sg2 + ", " +
						Util.Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							"proc " + selProc + ", " +
						Util.Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							"proc " + actProc + ", " +
						Util.Bold("TA ") + ta + ", " + 
						Util.Bold("TAComp ") + d1Done + " " + d2Done + " " + sg1Done + " " + sg2Done;
					return res;
				}
			/*	SG	*/
				public static string SGName(SlotGroup sg){
					string result = "";
					if(sg != null){
						if(sg.isPool){
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
						}else if(sg.isSGE){
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
						res = Util.Red("SGDeactivated");
					}else if(state is SGDefocusedState){
						res = Util.Green("SGDefocused");
					}else if(state is SGFocusedState){
						res = Util.Blue("SGFocused");
					}else if(state is SGSelectedState){
						res = Util.Aqua("SGSelected");
					}else if(state is SGWaitForActionState){
						res = Util.Sangria("SGWFA");
					// }else if(state is SGPerformingTransactionState){
					// 	res = Util.Green("SGTransaction");
					}else if(state is SGRevertState){
						res = Util.Sangria("SGRevert");
					}else if(state is SGReorderState){
						res = Util.Aqua("SGReorder");
					}else if(state is SGFillState){
						res = Util.Forest("SGFill");
					}else if(state is SGSwapState){
						res = Util.Berry("SGSwap");
					}else if(state is SGAddState){
						res = Util.Violet("SGAdd");
					}else if(state is SGRemoveState){
						res = Util.Khaki("SGRemove");
					}else if(state is SGSortState){
						res = Util.Midnight("SGSort");
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
					string selProc;
						if(sg.SelectionProcess == null)
							selProc = "";
						else
							selProc = SGProcessName(sg.SelectionProcess) + " exp? " + (sg.SelectionProcess.IsExpired?Blue("true"):Red("false"));
					string prevAct = SGStateName(sg.PrevActState);
					string curAct = SGStateName(sg.CurActState);
					string actProc;
						if(sg.ActionProcess == null)
							actProc = "";
						else
							actProc = SGProcessName(sg.ActionProcess) + " exp? " + (sg.ActionProcess.IsExpired?Blue("true"):Red("false"));
					res = Bold("DebugTarget: ") + 
						sgName + " " +
						Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							" proc, " + selProc + ", " +
						Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							" proc, " + actProc;
					return res;
				}
			/*	SB	*/
				public static string ItemInstName(InventoryItemInstanceMock itemInst){
					string result = "";
					if(itemInst != null){
						switch(itemInst.Item.ItemID){
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
						foreach(InventoryItemInstanceMock iInst in SlotGroupManager.CurSGM.poolInv){
							if(iInst.Item == itemInst.Item)
								sameItemInsts.Add(iInst);
						}
						int index = sameItemInsts.IndexOf(itemInst);
						result += "_"+index.ToString();
						if(itemInst is BowInstanceMock)
							result = Forest(result);
						if(itemInst is WearInstanceMock)
							result = Sangria(result);
						if(itemInst is CarriedGearInstanceMock)
							result = Terra(result);
						if(itemInst is PartsInstanceMock)
							result = Midnight(result);
					}
					return result;
				}
				public static string SBName(Slottable sb){
					string result = "";
					if(sb != null){
						switch(sb.itemInst.Item.ItemID){
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
						foreach(InventoryItemInstanceMock itemInst in SlotGroupManager.CurSGM.poolInv){
							if(itemInst.Item == sb.itemInst.Item)
								sameItemInsts.Add(itemInst);
						}
						int index = sameItemInsts.IndexOf(sb.itemInst);
						result += "_"+index.ToString();
						if(sb.itemInst is BowInstanceMock)
							result = Forest(result);
						if(sb.itemInst is WearInstanceMock)
							result = Sangria(result);
						if(sb.itemInst is CarriedGearInstanceMock)
							result = Terra(result);
						if(sb.itemInst is PartsInstanceMock)
							result = Midnight(result);
					}
					return result;
				}
				public static string SBofSG(Slottable sb){
					string res = "";
					if(sb != null){
						res = Util.SBName(sb) + " of " + Util.SGName(sb.sg);
						if(sb.isEquipped && sb.sg.isPool)
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
						else if(state is SBMoveWithinState)
							result = Midnight("MoveWithin");
						// else if(state is SBMovingInSGState)
						// 	result = Midnight("MovingInSG");
						// else if(state is SBRevertingState)
						// 	result = Beni("Reverting");
						// else if(state is SBMovingOutState)
						// 	result = Sangria("MovingOut");
						// else if(state is SBMovingInState)
						// 	result = Yamabuki("MovingIn");
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
					else if(process is SBPickedUpProcess)
						res = Brown("PickedUp");
					else if(process is WaitForNextTouchProcess)
						res = Terra("WFNextTouch");
					else if(process is SBRemoveProcess)
						res = Violet("Removed");
					else if(process is SBAddProcess)
						res = Khaki("Added");
					else if(process is SBMoveWithinProcess)
						res = Midnight("MoveWithin");
					// else if(process is SBUnpickProcess)
					// 	res = Berry("Unpick");
					// else if(process is SBMoveInSGProcess)
					// 	res = Midnight("MovingInSG");
					// else if(process is SBRevertProcess)
					// 	res = Beni("Reverting");
					// else if(process is SBMoveOutProcess)
					// 	res = Sangria("MovingOut");
					// else if(process is SBMoveInProcess)
					// 	res = Yamabuki("MovingIn");
					else if(process is SBUnequipProcess)
						res = Red("Unequip");
					else if(process is SBEquipProcess)
						res = Blue("Equipping");
					return res;
				}
				public static string SBDebug(Slottable sb){
					string res = "";
					if(sb == null)
						res = "null";
					else{	
						string sbName = SBofSG(sb);
						string prevSel = SBStateName(sb.PrevSelState);
						string curSel = SBStateName(sb.CurSelState);
						string selProc;
							if(sb.SelectionProcess == null)
								selProc = "";
							else
								selProc = SBProcessName(sb.SelectionProcess) + " exp? " + (sb.SelectionProcess.IsExpired?Blue("true"):Red("false"));
						string prevAct = SBStateName(sb.PrevActState);
						string curAct = SBStateName(sb.CurActState);
						string actProc;
							if(sb.ActionProcess == null)
								actProc = "";
							else
								actProc = SBProcessName(sb.ActionProcess) + " exp? " + (sb.ActionProcess.IsExpired?Blue("true"):Red("false"));
						string prevEqp = SBStateName(sb.PrevEqpState);
						string curEqp = SBStateName(sb.CurEqpState);
						string eqpProc;
							if(sb.EquipProcess == null)
								eqpProc = "";
							else
								eqpProc = SBProcessName(sb.EquipProcess) + " exp? " + (sb.EquipProcess.IsExpired?Blue("true"):Red("false"));
						res = Bold("DebugTarget: ") + sbName + ": " +
							Bold("Sel ") + " from " + prevSel + " to " + curSel + " proc " + selProc + ", " + 
							Bold("Act ") + " from " + prevAct + " to " + curAct + " proc " + actProc + ", " + 
							Bold("Eqp ") + " from " + prevEqp + " to " + curEqp + " proc " + eqpProc + ", " +
							Bold("SlotID: ") + " from " + sb.slotID.ToString() + " to " + sb.newSlotID.ToString() 
							;
					}
					return res;
				}
			/*	Debug	*/
				public static string TADebug(Slottable testSB, SlotGroup tarSG, Slottable tarSB){
					SlotSystemTransaction ta = testSB.sgm.GetTransaction(testSB, tarSG, tarSB);
					string taStr = TransactionName(ta);
					string taTargetSB = Util.SBofSG(ta.targetSB);
					string taSG1 = Util.SGName(ta.sg1);
					string taSG2 = Util.SGName(ta.sg2);
					return "DebugTarget: " + taStr + " " +
						"targetSB: " + taTargetSB + ", " + 
						"sg1: " + taSG1 + ", " +
						"sg2: " + taSG2
						;
				}
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
