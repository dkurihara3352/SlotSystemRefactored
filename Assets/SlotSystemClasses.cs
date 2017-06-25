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
	
	/*	SSM	Classes	*/
		/*	states	*/
			/*	Selection State	*/
				public abstract class SSMState: SSEState{
					protected SlotSystemManager ssm{
						get{
							return (SlotSystemManager)base.sse;
						}
					}
				}
				public class SSMSelState: SSMState{}
				public class SSMDeactivatedState: SSMSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						ssm.SetAndRunSelProcess(null);
						ssm.SetActState(SlotSystemManager.ssmWaitForActionState);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SSMDefocusedState: SSMSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(ssm.prevSelState == SlotSystemManager.ssmFocusedState)
							ssm.SetAndRunSelProcess(new SSMGreyoutProcess(ssm, ssm.greyoutCoroutine));
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SSMFocusedState: SSMSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(ssm.prevSelState == SlotSystemManager.ssmDefocusedState)
							ssm.SetAndRunSelProcess(new SSMGreyinProcess(ssm, ssm.greyinCoroutine));
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	Action State	*/
				public class SSMActState: SSMState{}
				public class SSMWaitForActionState: SSMActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						ssm.SetAndRunActProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SSMProbingState: SSMActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(ssm.prevActState == SlotSystemManager.ssmWaitForActionState)
							ssm.SetAndRunActProcess(new SSMProbeProcess(ssm, ssm.probeCoroutine));
						else
							throw new System.InvalidOperationException("SGMProbingState: Entering from an invalid state");
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SSMTransactionState: SSMActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						ssm.SetAndRunActProcess(new SSMTransactionProcess(ssm));
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
		/*	process	*/
			/*	superclasses	*/
				public abstract class SSMProcess: AbsSSEProcess{
					protected SlotSystemManager ssm{
						get{return (SlotSystemManager)sse;}
					}
				}
			/*	Selection Process	*/
				public class SSMSelProcess: SSMProcess{}
				public class SSMGreyinProcess: SSMSelProcess{
					public SSMGreyinProcess(SlotSystemManager ssm, System.Func<IEnumeratorMock> coroutineMock){
						this.sse = ssm;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SSMGreyoutProcess: SSMSelProcess{
					public SSMGreyoutProcess(SlotSystemManager ssm, System.Func<IEnumeratorMock> coroutineMock){
						this.sse = ssm;
						this.coroutineMock = coroutineMock;
					}
				}
			/*	Action Process	*/
				public class SSMActProcess: SSMProcess{}
				public class SSMProbeProcess: SSMActProcess{
					public SSMProbeProcess(SlotSystemManager ssm, System.Func<IEnumeratorMock> coroutineMock){
						this.sse = ssm;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SSMTransactionProcess: SSMActProcess{
					public SSMTransactionProcess(SlotSystemManager ssm){
						this.sse = ssm;
						this.coroutineMock = ssm.transactionCoroutine;
					}
					public override void Expire(){
						base.Expire();
						ssm.transaction.OnComplete();
					}
				}
	/*	Transaction classes	*/
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
						if(targetSG.isPool && targetSG != SlotSystemManager.curSSM.focusedSGP)
							throw new System.InvalidOperationException("GetTransaction: targetSG is poolSG but not focused");
						else if(targetSG.isSGE && !SlotSystemManager.curSSM.focusedSGEs.Contains(targetSG))
							throw new System.InvalidOperationException("GetTransaction: targetSG is SGE but does not belong to the focused EquipmentSet");
					}
					if(targetSG == null){// meaning selectedSB is also null
						return new RevertTransaction(pickedSB);
					}else{// hoveredSB could be null
						if(targetSB == null){// on SG
							if(targetSG.AcceptsFilter(pickedSB)){
								if(targetSG != origSG && origSG.isShrinkable){
									if(targetSG.HasItem(pickedSB.itemInst) && pickedSB.itemInst.Item.IsStackable)
										return new StackTransaction(pickedSB, targetSG.GetSB(pickedSB.itemInst));
										
									if(targetSG.hasEmptySlot){
										if(!targetSG.HasItem(pickedSB.itemInst))
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
										if(targetSG.HasItem(pickedSB.itemInst)){
											if(!origSG.HasItem(targetSB.itemInst)){
												if(targetSG.isPool){
													if(origSG.AcceptsFilter(targetSB))
														return new SwapTransaction(pickedSB, targetSB);
													if(origSG.isShrinkable)
														return new FillTransaction(pickedSB, targetSG);
												}
											}
										}else{
											if(origSG.AcceptsFilter(targetSB))
												return new SwapTransaction(pickedSB, targetSB);
											if(targetSG.hasEmptySlot || targetSG.isExpandable)
												return new FillTransaction(pickedSB, targetSG);
										}
									}
								}
							}
							return new RevertTransaction(pickedSB);
						}
					}
				}
				protected SlotSystemManager ssm = SlotSystemManager.curSSM;
				protected List<InventoryItemInstanceMock> removed = new List<InventoryItemInstanceMock>();
				protected List<InventoryItemInstanceMock> added = new List<InventoryItemInstanceMock>();
				public virtual Slottable targetSB{get{return null;}}
				public virtual SlotGroup sg1{get{return null;}}
				public virtual SlotGroup sg2{get{return null;}}
				public virtual List<InventoryItemInstanceMock> moved{get{return null;}}
				public virtual void Indicate(){}
				public virtual void Execute(){
					ssm.SetActState(SlotSystemManager.ssmTransactionState);
				}
				public virtual void OnComplete(){
					ssm.ResetAndFocus();
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
					m_origSG.SetActState(SlotGroup.revertState);
					ssm.dIcon1.SetDestination(m_origSG, m_origSG.GetNewSlot(m_pickedSB.itemInst));
					m_origSG.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					m_origSG.OnCompleteSlotMovements();
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
					sg1.SetActState(SlotGroup.reorderState);
					ssm.dIcon1.SetDestination(sg1, sg1.GetNewSlot(m_pickedSB.itemInst));
					sg1.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovements();
					base.OnComplete();
				}
			}
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
					sg1.SetActState(SlotGroup.removeState);
					sg2.SetActState(SlotGroup.addState);
					ssm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovements();
					sg2.OnCompleteSlotMovements();
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
					sg1.SetActState(SlotGroup.swapState);
					sg2.SetActState(SlotGroup.swapState);
					ssm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
					DraggedIcon di2 = new DraggedIcon(targetSB);
					ssm.SetDIcon2(di2);
					ssm.dIcon2.SetDestination(sg1, sg1.GetNewSlot(targetSB.itemInst));
					sg1.OnActionExecute();
					sg2.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovements();
					sg2.OnCompleteSlotMovements();
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
					sg1.SetActState(SlotGroup.fillState);
					sg2.SetActState(SlotGroup.fillState);
					ssm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
					sg1.OnActionExecute();
					sg2.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovements();
					sg2.OnCompleteSlotMovements();
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
					sg1.SetActState(SlotGroup.sortState);
					sg1.OnActionExecute();
					base.Execute();
				}
				public override void OnComplete(){
					sg1.OnCompleteSlotMovements();
					base.OnComplete();
				}
			}
	/*	SlotGroup Classes	*/
		/*	states	*/
			/*	superclasses	*/
				public abstract class SGState: SSEState{
					protected SlotGroup sg{
						get{
							return (SlotGroup)sse;
						}
					}
				}
				public abstract class SGSelState: SGState{
					public virtual void OnHoverEnterMock(SlotGroup sg, PointerEventDataMock eventDataMock){
						sg.ssm.SetHoveredSG(sg);
					}
					public virtual void OnHoverExitMock(SlotGroup sg, PointerEventDataMock eventDataMock){

					}
				}
				public abstract class SGActState: SGState{}
			/*	Selection States	*/
				public class SGDeactivatedState : SGSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sg.SetAndRunSelProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGDefocusedState: SGSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SGSelProcess process = null;
						if(sg.prevSelState == SlotGroup.sgDeactivatedState){
							process = null;
							sg.InstantGreyout();
						}else if(sg.prevSelState == SlotGroup.sgFocusedState)
							process = new SGGreyoutProcess(sg, sg.greyoutCoroutine);
						else if(sg.prevSelState == SlotGroup.sgSelectedState)
							process = new SGDehighlightProcess(sg, sg.greyoutCoroutine);
						sg.SetAndRunSelProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGFocusedState: SGSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SGSelProcess process = null;
						if(sg.prevSelState == SlotGroup.sgDeactivatedState){
							process = null;
							sg.InstantGreyin();
						}
						else if(sg.prevSelState == SlotGroup.sgDefocusedState)
							process = new SGGreyinProcess(sg, sg.greyinCoroutine);
						else if(sg.prevSelState == SlotGroup.sgSelectedState)
							process = new SGDehighlightProcess(sg, sg.dehighlightCoroutine);
						sg.SetAndRunSelProcess(process);
					}
					
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGSelectedState: SGSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SGSelProcess process = null;
						if(sg.prevSelState == SlotGroup.sgDeactivatedState){
							sg.InstantHighlight();
						}else if(sg.prevSelState == SlotGroup.sgDefocusedState)
							process = new SGHighlightProcess(sg, sg.highlightCoroutine);
						else if(sg.prevSelState == SlotGroup.sgFocusedState)
							process = new SGHighlightProcess(sg, sg.highlightCoroutine);
						sg.SetAndRunSelProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	Action State	*/
				public class SGWaitForActionState: SGActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sg.SetAndRunActProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGRevertState: SGActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sg.UpdateSBs(new List<Slottable>(sg.toList));
						if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGReorderState: SGActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						Slottable sb1 = sg.ssm.pickedSB;
						Slottable sb2 = sg.ssm.targetSB;
						List<Slottable> newSBs = new List<Slottable>(sg.toList);
						newSBs.Reorder(sb1, sb2);
						sg.UpdateSBs(newSBs);
						if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGAddState: SGActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						List<InventoryItemInstanceMock> cache = sg.ssm.transaction.moved;
						List<Slottable> newSBs = sg.toList;
						int origCount = newSBs.Count;
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
								newSB.Initialize(itemInst);
								newSB.SetSSM(sg.ssm);
								// newSB.rootElement = sg.rootElement;
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
						if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGRemoveState: SGActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						List<InventoryItemInstanceMock> cache = sg.ssm.transaction.moved;
						List<Slottable> newSBs = sg.toList;
						int origCount = newSBs.Count;
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
						if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGSwapState: SGActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						Slottable added;
							if(sg.ssm.transaction.sg1 == sg)
								added = sg.ssm.transaction.targetSB;
							else
								added = sg.ssm.pickedSB;
						Slottable removed;
							if(sg.ssm.transaction.sg1 == sg)
								removed = sg.ssm.pickedSB;
							else
								removed = sg.ssm.transaction.targetSB;
						List<Slottable> newSBs = new List<Slottable>(sg.toList);
						int origCount = newSBs.Count;
						if(!sg.isPool){
							GameObject newSBGO = new GameObject("newSBGO");
							Slottable newSB = newSBGO.AddComponent<Slottable>();
							newSB.Initialize(added.itemInst);
							newSB.SetSSM(sg.ssm);
							newSB.SetEqpState(Slottable.unequippedState);
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
						if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGFillState: SGActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						Slottable added;
							if(sg.ssm.transaction.sg1 == sg)
								added = null;
							else
								added = sg.ssm.pickedSB;
						Slottable removed;
							if(sg.ssm.transaction.sg1 == sg)
								removed = sg.ssm.pickedSB;
							else
								removed = null;

						List<Slottable> newSBs = new List<Slottable>(sg.toList);
						int origCount = newSBs.Count;
						if(!sg.isPool){
							if(added != null){
								GameObject newSBGO = new GameObject("newSBGO");
								Slottable newSB = newSBGO.AddComponent<Slottable>();
								newSB.Initialize(added.itemInst);
								newSB.SetSSM(sg.ssm);
								newSB.Defocus();
								newSB.SetEqpState(Slottable.unequippedState);
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
						}
						if(!sg.isExpandable){
							while(newSBs.Count <origCount){
								newSBs.Add(null);
							}
						}
						sg.UpdateSBs(newSBs);
						if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SGSortState: SGActState{
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
						if(sg.prevActState != null && sg.prevActState == SlotGroup.sgWaitForActionState){
							SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
							sg.SetAndRunActProcess(process);
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
		/*	process	*/
			public abstract class SGProcess: AbsSSEProcess{
				public SlotGroup sg{
					get{return (SlotGroup)sse;}
				}
			}
			/*	Selecttion Process	*/
				public abstract class SGSelProcess: SGProcess{}
				public class SGGreyinProcess: SGSelProcess{
					public SGGreyinProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
						sse = sg;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SGGreyoutProcess: SGSelProcess{
					public SGGreyoutProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
						sse = sg;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SGHighlightProcess: SGSelProcess{
					public SGHighlightProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
						sse = sg;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SGDehighlightProcess: SGSelProcess{
					public SGDehighlightProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
						sse = sg;
						this.coroutineMock = coroutineMock;
					}
				}
			/*	Action Process*/
				public abstract class SGActProcess: SGProcess{}
				public class SGTransactionProcess: SGActProcess{
					public SGTransactionProcess(SlotGroup sg, System.Func<IEnumeratorMock> coroutineMock){
						sse = sg;
						this.coroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						sg.ssm.AcceptSGTAComp(sg);
					}
				}
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
							newSB.SetSSM(sg.ssm);
							sg.slots[items.IndexOf(item)].sb = newSB;
						}
						sg.SyncSBsToSlots();
					if(sg.isAutoSort)
						sg.InstantSort();
				}
			}
			public class SGUpdateEquipStatusCommand: SlotGroupCommand{
				public void Execute(SlotGroup sg){
					sg.ssm.UpdateEquipStatesOnAll();
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
								sg.ssm.MarkEquippedInPool(item, false);
								sg.ssm.SetEquippedOnAllSBs(item, false);
								/*	Set unequipped with transition
										all sbp in FocusedSGP
									Set unequipped without transition
										sll sbp in Defocused SGPs
								*/
							}else if(sb.slotID == -1){/*	added	*/
								sg.inventory.Add(item);
								sg.ssm.MarkEquippedInPool(item, true);
								sg.ssm.SetEquippedOnAllSBs(item, true);
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
				public abstract class SBProcess: AbsSSEProcess{
					public Slottable sb{
						get{return (Slottable)sse;}
					}
				}
			/*	Selecttion process	*/
				public abstract class SBSelProcess: SBProcess{}
				public class SBGreyoutProcess: SBSelProcess{
					public SBGreyoutProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SBGreyinProcess: SBSelProcess{
					public SBGreyinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SBHighlightProcess: SBSelProcess{
					public SBHighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SBDehighlightProcess: SBSelProcess{
					public SBDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
			/*	Action process	*/
				public abstract class SBActProcess: SBProcess{}
				public class WaitForPointerUpProcess: SBActProcess{
					public WaitForPointerUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						sb.SetSelState(Slottable.sbDefocusedState);
					}
				}
				public class WaitForPickUpProcess: SBActProcess{
					public WaitForPickUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						sb.PickUp();
					}
				}
				public class SBPickedUpProcess: SBActProcess{
					public SBPickedUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
				public class WaitForNextTouchProcess: SBActProcess{
					public WaitForNextTouchProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
					public override void Expire(){
						base.Expire();
						if(!sb.isPickedUp){
							sb.Tap();
							sb.Reset();
							sb.Focus();
						}else{
							sb.ExecuteTransaction();
						}
					}
				}
				public class SBRemoveProcess: SBActProcess{
					public SBRemoveProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SBAddProcess: SBActProcess{
					public SBAddProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SBMoveWithinProcess: SBActProcess{
					public SBMoveWithinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
			/*	Equip process*/
				public abstract class SBEqpProcess: SBProcess{}
				public class SBUnequipProcess: SBEqpProcess{
					public SBUnequipProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
				public class SBEquipProcess: SBEqpProcess{
					public SBEquipProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
						sse = sb;
						this.coroutineMock = coroutineMock;
					}
				}
		/*	states	*/
			/*	superclasses	*/
				public abstract class SBState: SSEState{
					protected Slottable sb{
						get{
							return (Slottable)sse;
						}
					}
				}
				public abstract class SBSelState: SBState{
					public virtual void OnHoverEnterMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.ssm.SetHoveredSB(sb);
					}
					public virtual void OnHoverExitMock(Slottable sb, PointerEventDataMock eventDataMock){
					}
				}
				public abstract class SBActState: SBState{
					public abstract void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock);
					public abstract void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock);
					public abstract void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock);
					public abstract void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock);
				}
				public abstract class SBEqpState: SBState{}
			/*	SB Selection States	*/
				public class SBDeactivatedState: SBSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sb.SetAndRunSelProcess(null);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBDefocusedState: SBSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBSelProcess process = null;
						if(sb.curSelState == Slottable.sbDeactivatedState){
							sb.InstantGreyout();
							process = null;
						}else if(sb.prevSelState == Slottable.sbFocusedState){
							process = new SBGreyoutProcess(sb, sb.greyoutCoroutine);
						}else if(sb.prevSelState == Slottable.sbSelectedState){
							process = new SBGreyoutProcess(sb, sb.greyoutCoroutine);
						}
						sb.SetAndRunSelProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBFocusedState: SBSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBSelProcess process = null;
						if(sb.prevSelState == Slottable.sbDeactivatedState){
							sb.InstantGreyin();
						}else if(sb.prevSelState == Slottable.sbDefocusedState){
							process = new SBGreyinProcess(sb, sb.greyinCoroutine);
						}else if(sb.prevSelState == Slottable.sbSelectedState){
							process = new SBDehighlightProcess(sb, sb.dehighlightCoroutine);
						}
						sb.SetAndRunSelProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBSelectedState: SBSelState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBSelProcess process = null;
						if(sb.prevSelState == Slottable.sbDeactivatedState){
							sb.InstantHighlight();
						}else if(sb.prevSelState == Slottable.sbDefocusedState){
							process = new SBHighlightProcess(sb, sb.highlightCoroutine);
						}else if(sb.prevSelState == Slottable.sbFocusedState){
							process = new SBHighlightProcess(sb, sb.highlightCoroutine);
						}
						sb.SetAndRunSelProcess(process);
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	SB Action States	*/
				public class WaitForActionState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBActProcess process = null;
						sb.SetAndRunActProcess(process);
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
							sb.SetSelState(Slottable.sbSelectedState);
							sb.SetActState(Slottable.waitForPickUpState);
						}
						else
							sb.SetActState(Slottable.waitForPointerUpState);
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class WaitForPointerUpState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBActProcess wfPtuProcess = new WaitForPointerUpProcess(sb, sb.WaitForPointerUpCoroutine);
						sb.SetAndRunActProcess(wfPtuProcess);
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
				public class WaitForPickUpState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBActProcess wfpuProcess = new WaitForPickUpProcess(sb, sb.WaitForPickUpCoroutine);
						sb.SetAndRunActProcess(wfpuProcess);
					}
					public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.SetActState(Slottable.waitForNextTouchState);
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
				public class WaitForNextTouchState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBActProcess wfntProcess = new WaitForNextTouchProcess(sb, sb.WaitForNextTouchCoroutine);
						sb.SetAndRunActProcess(wfntProcess);
					}
					public override void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){
						if(!sb.isPickedUp)
							sb.PickUp();
						else{
							sb.SetActState(Slottable.pickedUpState);
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
				public class PickedUpState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						sb.ssm.SetPickedSB(sb);
						sb.ssm.SetActState(SlotSystemManager.ssmProbingState);
						DraggedIcon di = new DraggedIcon(sb);
						sb.ssm.SetDIcon1(di);
						sb.ssm.CreateTransactionResults();
						sb.OnHoverEnterMock();
						sb.ssm.UpdateTransaction();
						SBActProcess pickedUpProcess = new SBPickedUpProcess(sb, sb.PickUpCoroutine);
						sb.SetAndRunActProcess(pickedUpProcess);
					}
					public override void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){
						sb.Reset();
						sb.Focus();
					}
					public override void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
						if(sb.ssm.hoveredSB == sb && sb.isStackable)
							sb.SetActState(Slottable.waitForNextTouchState);
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
				public class SBRemovedState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBRemoveProcess process = new SBRemoveProcess(sb, sb.RemoveCoroutine);
						sb.SetAndRunActProcess(process);
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
				public class SBAddedState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBAddProcess process = new SBAddProcess(sb, sb.AddCorouine);
						sb.SetAndRunActProcess(process);
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
				public class SBMoveWithinState: SBActState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.MoveWithinCoroutine);
						sb.SetAndRunActProcess(process);
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
				public class SBEquippedState: SBEqpState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sb.sg.isPool){
							if(sb.PrevEqpState != null && sb.PrevEqpState == Slottable.unequippedState){
								SBEqpProcess process = new SBEquipProcess(sb, sb.EquipCoroutine);
								sb.SetAndRunEquipProcess(process);
							}
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBUnequippedState: SBEqpState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sb.PrevEqpState == null || sb.PrevEqpState == Slottable.unequippedState){
							/*	when initialized	*/
							return;
						}
						if(sb.sg.isPool){
							if(sb.PrevEqpState != null && sb.PrevEqpState == Slottable.equippedState){
								SBEqpProcess process = new SBUnequipProcess(sb, sb.UnequipCoroutine);
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
			SlotSystemManager m_ssm;
			public Slottable sb{
				get{return m_sb;}
				}Slottable m_sb;
			public DraggedIcon(Slottable sb){
				m_sb = sb;
				m_item = this.sb.itemInst;
				m_ssm = SlotSystemManager.curSSM;
			}
			public void CompleteMovement(){
				m_ssm.AcceptDITAComp(this);
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
			/*	states	*/
				/*	superstates	*/
					public class SSEStateEngine: SwitchableStateEngine{
						public SSEStateEngine(SlotSystemElement sse){
							this.handler = sse;
						}
						public void SetState(SSEState state){
							base.SetState(state);
						}
					}
					public abstract class SSEState: SwitchableState{
						protected SlotSystemElement sse;
						public virtual void EnterState(StateHandler handler){
							sse = (SlotSystemElement)handler;
						}
						public virtual void ExitState(StateHandler handler){}
					}					
				/*	Sel States	*/
					public abstract class SSESelState: SSEState{}
					public class SSEDeactivatedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							sse.SetAndRunSelProcess(null);
						}
					}
					public class SSEDefocusedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							SSEProcess process = null;
							if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
							process = null;
							sse.InstantGreyout();
							}else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
								process = new SSEGreyoutProcess(sse, sse.greyoutCoroutine);
							else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
								process = new SSEHighlightProcess(sse, sse.greyoutCoroutine);
							sse.SetAndRunSelProcess(process);
						}
					}
					public class SSEFocusedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							SSEProcess process = null;
							if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
								process = null;
								sse.InstantGreyin();
							}
							else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
								process = new SSEGreyinProcess(sse, sse.greyinCoroutine);
							else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
								process = new SSEDehighlightProcess(sse, sse.dehighlightCoroutine);
							sse.SetAndRunSelProcess(process);
						}
					}
					public class SSESelectedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							SSEProcess process = null;
							if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
								sse.InstantHighlight();
							}else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
								process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
							else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
								process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
							sse.SetAndRunSelProcess(process);
						}
					}
				/*	act state	*/
					public abstract class SSEActState: SSEState{}
					public class SSEWaitForActionState: SSEActState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							sse.SetAndRunActProcess(null);
						}
					}
			/*	process	*/
				/*	superclasses	*/
					public class SSEProcessEngine{
						public SSEProcess process{
							get{return m_process;}
							}SSEProcess m_process;
						public void SetAndRunProcess(SSEProcess process){
							if(process != null)
								process.Stop();
							m_process = process;
							if(process != null)
								process.Start();
						}
					}
					public interface SSEProcess{
						bool isRunning{get;}
						System.Func<IEnumeratorMock> coroutineMock{set;}
						void Start();
						void Stop();
						void Expire();
					}
					public class AbsSSEProcess: SSEProcess{
						public bool isRunning{
							get{return m_isRunning;}
							} bool m_isRunning;
						public System.Func<IEnumeratorMock> coroutineMock{
							set{m_coroutineMock = value;}
							}System.Func<IEnumeratorMock> m_coroutineMock;
						protected SlotSystemElement sse{
							get{return m_sse;}
							set{m_sse = value;}
							} SlotSystemElement m_sse;
						public virtual void Start(){
							m_isRunning = true;
							m_coroutineMock();
						}
						public virtual void Stop(){
							if(isRunning)
								m_isRunning = false;
						}
						public virtual void Expire(){
							if(isRunning)
								m_isRunning = false;
						}
					}
				/*	sel process	*/
					public abstract class SSESelProcess: AbsSSEProcess{}
						public class SSEGreyoutProcess: SSESelProcess{
							public SSEGreyoutProcess(SlotSystemElement sse, System.Func<IEnumeratorMock> coroutineMock){
								this.sse = sse;
								this.coroutineMock = coroutineMock;
							}
						}
						public class SSEGreyinProcess: SSESelProcess{
							public SSEGreyinProcess(SlotSystemElement sse, System.Func<IEnumeratorMock> coroutineMock){
								this.sse = sse;
								this.coroutineMock = coroutineMock;
							}
						}
						public class SSEHighlightProcess: SSESelProcess{
							public SSEHighlightProcess(SlotSystemElement sse, System.Func<IEnumeratorMock> coroutineMock){
								this.sse = sse;
								this.coroutineMock = coroutineMock;
							}
						}
						public class SSEDehighlightProcess: SSESelProcess{
							public SSEDehighlightProcess(SlotSystemElement sse, System.Func<IEnumeratorMock> coroutineMock){
								this.sse = sse;
								this.coroutineMock = coroutineMock;
							}
						}
				/*	act process	*/
					public abstract class SSEActProcess: AbsSSEProcess{}
			public interface SlotSystemElement: IEnumerable<SlotSystemElement>, StateHandler{
				SSEState curSelState{get;}
				SSEState prevSelState{get;}
				SSEState curActState{get;}
				SSEState prevActState{get;}
				void SetAndRunSelProcess(SSEProcess process);
				void SetAndRunActProcess(SSEProcess process);
				SSEProcess selProcess{get;}
				SSEProcess actProcess{get;}
				IEnumeratorMock greyoutCoroutine();
				IEnumeratorMock greyinCoroutine();
				IEnumeratorMock highlightCoroutine();
				IEnumeratorMock dehighlightCoroutine();
				void InstantGreyin();
				void InstantGreyout();
				void InstantHighlight();
				string eName{get;}
				bool isBundleElement{get;}
				bool isPageElement{get;}
				bool isToggledOn{get;}
				bool isFocused{get;}
				bool isDefocused{get;}
				bool isDeactivated{get;}
				bool isFocusedInHierarchy{get;}
				void Activate();
				void Deactivate();
				void Focus();
				void Defocus();
				SlotSystemBundle immediateBundle{get;}
				SlotSystemElement parent{get;set;}
				// SlotGroupManager sgm{get;set;}
				SlotSystemManager ssm{get;set;}
				bool ContainsInHierarchy(SlotSystemElement ele);
				void PerformInHierarchy(System.Action<SlotSystemElement> act);
				void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj);
				void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list);
				// SlotSystemElement rootElement{get;set;}
				int level{get;}
				bool Contains(SlotSystemElement element);
				SlotSystemElement this[int i]{get;}
				void ToggleOnPageElement();
			}
			public class SlotSystemPageElement{
				public SlotSystemElement element{
					get{return m_element;}
					}SlotSystemElement m_element;
				public bool isFocusedOnActivate{
					get{return m_isFocusedOnActivate;}
					}bool m_isFocusedOnActivate;
				public bool isFocusToggleOn{
					get{return m_isFocusToggleOn;}
					set{m_isFocusToggleOn = value;}
					}bool m_isFocusToggleOn;
				public SlotSystemPageElement(SlotSystemElement element, bool isFocusToggleOn){
					m_element = element;
					m_isFocusToggleOn = isFocusToggleOn;
					m_isFocusedOnActivate = isFocusToggleOn;				
				}
			}
			public interface TransactionManager{
				SlotSystemTransaction transaction{get;}
				void AcceptSGTAComp(SlotGroup sg);
				void AcceptDITAComp(DraggedIcon di);
				Slottable pickedSB{get;}
				Slottable targetSB{get;}
				SlotGroup sg1{get;}
				SlotGroup sg2{get;}
				DraggedIcon dIcon1{get;}
				DraggedIcon dIcon2{get;}
				Slottable hoveredSB{get;}
				SlotGroup hoveredSG{get;}
				TransactionResults transactionResults{get;}
				void CreateTransactionResults();
				void UpdateTransaction();
				SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotGroup hovSG, Slottable hovSB);
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
				public void Add(SlottableItem item){
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
			/*	SSE	*/
				public static string SSEDebug(SlotSystemElement sse){
					string res = "";
					string prevSel = SSEStateNamePlain(sse.prevSelState);
					string curSel = SSEStateName(sse.curSelState);
					string selProc;
						if(sse.selProcess == null)
							selProc = "";
						else
							selProc = SSEProcessName(sse.selProcess) + " running? " + (sse.selProcess.isRunning?Blue("true"):Red("false"));
					string prevAct = SSEStateNamePlain(sse.prevActState);
					string curAct = SSEStateName(sse.curActState);
					string actProc;
						if(sse.actProcess == null)
							actProc = "";
						else
							actProc = SSEProcessName(sse.actProcess) + " running " + (sse.actProcess.isRunning?Blue("true"):Red("false"));
					res =
						sse.eName + " " +
						Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							" proc, " + selProc + ", " +
						Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							" proc, " + actProc;
					return res;
				}
				public static string SSEStateName(SSEState state){
					string res = "";

					if(state is SSEDeactivatedState){
						res = Util.Red("SSEDeactivated");
					}else if(state is SSEDefocusedState){
						res = Util.Green("SSEDefocused");
					}else if(state is SSEFocusedState){
						res = Util.Blue("SSEFocused");
					}else if(state is SSESelectedState){
						res = Util.Aqua("SSESelected");
					}else if(state is SSEWaitForActionState){
						res = Util.Sangria("SSEWFA");
					}
					return res;
				}
				public static string SSEStateNamePlain(SSEState state){
					string res = "";

					if(state is SSEDeactivatedState){
						res = "SSEDeactivated";
					}else if(state is SSEDefocusedState){
						res = "SSEDefocused";
					}else if(state is SSEFocusedState){
						res = "SSEFocused";
					}else if(state is SSESelectedState){
						res = "SSESelected";
					}else if(state is SSEWaitForActionState){
						res = "SSEWFA";
					}
					return res;
				}
				public static string SSEProcessName(SSEProcess process){
					string res = "";
					if(process is SSEGreyinProcess)
						res = Util.Blue("Greyin");
					else if(process is SSEGreyoutProcess)
						res = Util.Green("Greyout");
					else if(process is SSEHighlightProcess)
						res = Util.Red("Highlight");
					else if(process is SSEDehighlightProcess)
						res = Util.Brown("Dehighlight");
					return res;
				}
			/*	SSM	*/
				public static string SSMStateName(SSMState state){
					string res = "";
					if(state is SSMDeactivatedState)
						res = Util.Red("Deactivated");
					else if(state is SSMDefocusedState)
						res = Util.Blue("Defocused");
					else if(state is SSMFocusedState)
						res = Util.Green("Focused");
					else if(state is SSMWaitForActionState)
						res = Util.Green("WaitForAction");
					else if(state is SSMProbingState)
						res = Util.Ciel("Probing");
					else if(state is SSMTransactionState)
						res = Util.Terra("Transaction");
					return res;
				}
				public static string SSMStateNamePlain(SSMState state){
					string res = "";
					if(state is SSMDeactivatedState)
						res = "Deactivated";
					else if(state is SSMDefocusedState)
						res = "Defocused";
					else if(state is SSMFocusedState)
						res = "Focused";
					else if(state is SSMWaitForActionState)
						res = "WaitForAction";
					else if(state is SSMProbingState)
						res = "Probing";
					else if(state is SSMTransactionState)
						res = "Transaction";
					return res;
				}
				public static string TransactionName(SlotSystemTransaction ta){
					string res = "";
					if(ta is RevertTransaction)
						res = Util.Red("RevertTA");
					else if(ta is ReorderTransaction)
						res = Util.Blue("ReorderTA");
					else if(ta is StackTransaction)
						res = Util.Aqua("StackTA");
					else if(ta is SwapTransaction)
						res = Util.Terra("SwapTA");
					else if(ta is FillTransaction)
						res = Util.Forest("FillTA");
					else if(ta is SortTransaction)
						res = Util.Khaki("SortTA");
					else if(ta is EmptyTransaction)
						res = Util.Beni("Empty");
					return res;
				}
				public static string SSMProcessName(SSMProcess proc){
					string res = "";
					if(proc is SSMGreyinProcess)
						res = Util.Red("Greyin");
					else if(proc is SSMGreyoutProcess)
						res = Util.Red("Greyout");
					else if(proc is SSMProbeProcess)
						res = Util.Red("Probe");
					else if(proc is SSMTransactionProcess)
						res = Util.Blue("Transaction");
					return res;
				}
				public static string SSMDebug(SlotSystemManager ssm){
					string res = "";
					string pSB = Util.SBofSG(ssm.pickedSB);
					string tSB = Util.SBofSG(ssm.targetSB);
					string hSG = "";
						if(ssm.hoveredSG == null) hSG = "null";
						else hSG = ssm.hoveredSG.eName;
					string hSB = Util.SBofSG(ssm.hoveredSB);
					string di1;
						if(ssm.dIcon1 == null)
							di1 = "null";
						else
							di1 = Util.SBofSG(ssm.dIcon1.sb);
					string di2;
						if(ssm.dIcon2 == null)
							di2 = "null";
						else
							di2 = Util.SBofSG(ssm.dIcon2.sb);
					
					string sg1 = ssm.sg1 == null?"null":ssm.sg1.eName;
					string sg2 = ssm.sg2 == null?"null":ssm.sg2.eName;
					string prevSel = Util.SSMStateNamePlain((SSMSelState)ssm.prevSelState);
					string curSel = Util.SSMStateName((SSMSelState)ssm.curSelState);
					string selProc;
						if(ssm.selProcess == null)
							selProc = "";
						else
							selProc = Util.SSMProcessName((SSMSelProcess)ssm.selProcess) + " running? " + (ssm.selProcess.isRunning?Blue("true"):Red("false"));
					string prevAct = Util.SSMStateNamePlain((SSMActState)ssm.prevActState);
					string curAct = Util.SSMStateName((SSMActState)ssm.curActState);
					string actProc;
						if((SSMActProcess)ssm.actProcess == null)
							actProc = "";
						else
							actProc = Util.SSMProcessName((SSMActProcess)ssm.actProcess) + " running? " + (ssm.actProcess.isRunning?Blue("true"):Red("false"));
					string ta = Util.TransactionName(ssm.transaction);
					string d1Done = "d1Done: " + (ssm.dIcon1Done?Util.Blue("true"):Util.Red("false"));
					string d2Done = "d2Done: " + (ssm.dIcon2Done?Util.Blue("true"):Util.Red("false"));
					string sg1Done = "sg1Done: " + (ssm.sg1Done?Util.Blue("true"):Util.Red("false"));
					string sg2Done = "sg2Done: " + (ssm.sg2Done?Util.Blue("true"):Util.Red("false"));
					res = Util.Bold("SSM:") +
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
				public static string SGStateNamePlain(SGState state){
					string res = "";
					if(state is SGDeactivatedState){
						res = "SGDeactivated";
					}else if(state is SGDefocusedState){
						res = "SGDefocused";
					}else if(state is SGFocusedState){
						res = "SGFocused";
					}else if(state is SGSelectedState){
						res = "SGSelected";
					}else if(state is SGWaitForActionState){
						res = "SGWFA";
					}else if(state is SGRevertState){
						res = "SGRevert";
					}else if(state is SGReorderState){
						res = "SGReorder";
					}else if(state is SGFillState){
						res = "SGFill";
					}else if(state is SGSwapState){
						res = "SGSwap";
					}else if(state is SGAddState){
						res = "SGAdd";
					}else if(state is SGRemoveState){
						res = "SGRemove";
					}else if(state is SGSortState){
						res = "SGSort";
					}
					return res;
				}
				public static string SGProcessName(SGProcess proc){
					string res = "";
					if(proc is SGGreyinProcess)
						res = Util.Blue("Greyin");
					else if(proc is SGGreyoutProcess)
						res = Util.Green("Greyout");
					else if(proc is SGHighlightProcess)
						res = Util.Red("Highlight");
					else if(proc is SGDehighlightProcess)
						res = Util.Brown("Dehighlight");
					else if(proc is SGTransactionProcess)
						res = Util.Khaki("Transaction");
					return res;
				}
				public static string SGDebug(SlotGroup sg){
					string res = "";
					string prevSel = SGStateNamePlain((SGSelState)sg.prevSelState);
					string curSel = SGStateName((SGSelState)sg.curSelState);
					string selProc;
						if(sg.selProcess == null)
							selProc = "";
						else
							selProc = SGProcessName((SGSelProcess)sg.selProcess) + " running? " + (sg.selProcess.isRunning?Blue("true"):Red("false"));
					string prevAct = SGStateNamePlain((SGActState)sg.prevActState);
					string curAct = SGStateName((SGActState)sg.curActState);
					string actProc;
						if(sg.actProcess == null)
							actProc = "";
						else
							actProc = SGProcessName((SGActProcess)sg.actProcess) + " running? " + (sg.actProcess.isRunning?Blue("true"):Red("false"));
					res =  
						sg.eName + " " +
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
					}
					return result;
				}
				public static string SBName(Slottable sb){
					string result = "null";
					if(sb != null){
						result = ItemInstName(sb.itemInst);
						if(SlotSystemManager.curSSM != null){
							List<InventoryItemInstanceMock> sameItemInsts = new List<InventoryItemInstanceMock>();
							foreach(InventoryItemInstanceMock itemInst in SlotSystemManager.curSSM.poolInv){
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
					}
					return result;
				}
				public static string SBofSG(Slottable sb){
					string res = "";
					if(sb != null){
						res = Util.SBName(sb) + " of " + sb.sg.eName;
						if(sb.isEquipped && sb.sg.isPool)
							res = Util.Bold(res);
					}
					return res;
				}
				public static string SBStateName(SBState state){
					string result = "";
					if(state is SBSelState){
						if(state is SBDeactivatedState)
							result = Red("Deactivated");
						else if(state is SBFocusedState)
							result = Blue("Focused");
						else if(state is SBDefocusedState)
							result = Green("Defocused");
						else if(state is SBSelectedState)
							result = Ciel("Selected");
					}else if(state is SBActState){
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
					}else if(state is SBEqpState){
						if(state is SBEquippedState)
							result = Red("Equipped");
						else if(state is SBUnequippedState)
							result = Blue("Unequipped");
					}
					return result;
				}
				public static string SBStateNamePlain(SBState state){
					string result = "";
					if(state is SBSelState){
						if(state is SBDeactivatedState)
							result = "Deactivated";
						else if(state is SBFocusedState)
							result = "Focused";
						else if(state is SBDefocusedState)
							result = "Defocused";
						else if(state is SBSelectedState)
							result = "Selected";
					}else if(state is SBActState){
						if(state is WaitForActionState)
							result = "WFAction";
						else if(state is WaitForPointerUpState)
							result = "WFPointerUp";
						else if(state is WaitForPickUpState)
							result = "WFPickUp";
						else if(state is WaitForNextTouchState)
							result = "WFNextTouch";
						else if(state is PickedUpState)
							result = "PickedUp";
						else if(state is SBRemovedState)
							result = "Removed";
						else if(state is SBAddedState)
							result = "Added";
						else if(state is SBMoveWithinState)
							result = "MoveWithin";
					}else if(state is SBEqpState){
						if(state is SBEquippedState)
							result = "Equipped";
						else if(state is SBUnequippedState)
							result = "Unequipped";
					}
					return result;
				}
				public static string SBProcessName(SBProcess process){
					string res = "";
					if(process is SBGreyoutProcess)
						res = Green("Greyout");
					else if(process is SBGreyinProcess)
						res = Blue("Greyin");
					else if(process is SBHighlightProcess)
						res = Red("Highlight");
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
						string prevSel = SBStateNamePlain((SBSelState)sb.prevSelState);
						string curSel = SBStateName((SBSelState)sb.curSelState);
						string selProc;
							if(sb.selProcess == null)
								selProc = "";
							else
								selProc = SBProcessName((SBSelProcess)sb.selProcess) + " running? " + (sb.selProcess.isRunning?Blue("true"):Red("false"));
						string prevAct = SBStateNamePlain((SBActState)sb.prevActState);
						string curAct = SBStateName((SBActState)sb.curActState);
						string actProc;
							if(sb.actProcess == null)
								actProc = "";
							else
								actProc = SBProcessName((SBActProcess)sb.actProcess) + " running? " + (sb.actProcess.isRunning?Blue("true"):Red("false"));
						string prevEqp = SBStateNamePlain((SBEqpState)sb.PrevEqpState);
						string curEqp = SBStateName((SBEqpState)sb.CurEqpState);
						string eqpProc;
							if(sb.eqpProcess == null)
								eqpProc = "";
							else
								eqpProc = SBProcessName((SBEqpProcess)sb.eqpProcess) + " running? " + (sb.eqpProcess.isRunning?Blue("true"):Red("false"));
						res = sbName + ": " +
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
					SlotSystemTransaction ta = testSB.ssm.GetTransaction(testSB, tarSG, tarSB);
					string taStr = TransactionName(ta);
					string taTargetSB = Util.SBofSG(ta.targetSB);
					string taSG1 = ta.sg1.eName;
					string taSG2 = ta.sg2.eName;
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
