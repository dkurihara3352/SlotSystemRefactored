using System.Collections;
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
			}
			public class RevertTransaction: SlotSystemTransaction{
				Slottable slottable;
				public RevertTransaction(Slottable sb){
					this.slottable = sb;
				}
				public void Indicate(){
					//implement revert indication here
				}
				public void Execute(){
					//implement reverting here
					slottable.SetState(Slottable.RevertingState);
				}
			}
		/*	commands
		*/
			public interface SGMCommand{
				void Execute(SlotGroupManager sgm);
			}
			public class UpdateTransactionCommand: SGMCommand{
				public void Execute(SlotGroupManager sgm){
					if(sgm.PickedSB != null){
						if(sgm.PickedSB == sgm.SelectedSB){
							SlotSystemTransaction revertTs = new RevertTransaction(sgm.PickedSB);
							sgm.SetTransaction(revertTs);
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
			public class PrePickFilterCommand: SGMCommand{
				public void Execute(SlotGroupManager sgm){
					foreach(SlotGroup sg in sgm.SlotGroups){
						if(sg.CurState == SlotGroup.DefocusedState || !sg.IsPool)
							sg.SetState(SlotGroup.FocusedState);
						else if(sg.CurState == SlotGroup.SelectedState)
							sg.SetState(SlotGroup.FocusedState);
						sg.UpdateSbState();
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
			public class SGMProbingStateProcess: AbsSGMProcess{
				public SGMProbingStateProcess(SlotGroupManager sgm, System.Func<IEnumeratorMock> coroutineMock){
					this.SGM = sgm;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
				}
			}
		/*	state
		*/
			public interface SGMState{
				void EnterState(SlotGroupManager sgm);	
				void ExitState(SlotGroupManager sgm);	
			}
			public class SGMDeactivatedState: SGMState{
				public void EnterState(SlotGroupManager sgm){}
				public void ExitState(SlotGroupManager sgm){}
			}
			public class SGMDefocusedState: SGMState{
				public void EnterState(SlotGroupManager sgm){
					sgm.InitializeItems();
					sgm.InitializeProcesses();
				}
				public void ExitState(SlotGroupManager sgm){}
			}
			public class SGMFocusedState: SGMState{
				public void EnterState(SlotGroupManager sgm){
					for(int i =0; i<sgm.SlotGroups.Count; i++){
					sgm.SlotGroups[i].WakeUp();
				}	
				}
				public void ExitState(SlotGroupManager sgm){}

			}
			public class SGMProbingState: SGMState{
				public void EnterState(SlotGroupManager sgm){
					sgm.ProbingStateProcess.Start();
				}
				public void ExitState(SlotGroupManager sgm){
					if(sgm.ProbingStateProcess.IsRunning)
						sgm.ProbingStateProcess.Stop();
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
					SB.UTLog = "GradualGrayoutProcess done";
				}
				
			}
			public class GradualGrayinProcess: AbsSBProcess{
				public GradualGrayinProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.UTLog = "GradualGrayinProcess done";
				}
			}
			public class GradualDehighlightProcess: AbsSBProcess{
				public GradualDehighlightProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.UTLog = "GradualDehighlightProcess done";
				}
			}
			public class WaitAndSetBackToDefocusedStateProcess: AbsSBProcess{
				public WaitAndSetBackToDefocusedStateProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.SetState(Slottable.DefocusedState);
					SB.UTLog = "WaitAndSetBackToDefocusedStateProcess done";
				}
			}
			public class WaitAndPickUpProcess: AbsSBProcess{
				public WaitAndPickUpProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
					this.SB = sb;
					this.CoroutineMock = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					SB.UTLog = "WaitAndPickUpProcess done";
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
					SB.UTLog = "PickedUpAndSelectedProcess done";
				}
			}
			public class RevertingStateProcess: AbsSBProcess{
				public RevertingStateProcess(Slottable sb, System.Func<IEnumeratorMock> coroutineMock){
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
					SB.SGM.ClearFields();
					SB.SGM.PrePickFilter();
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
				void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock);
				void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock);
			}
			
			public class DeactivatedState: SlottableState{
				public void EnterState(Slottable sb){
					sb.Deactivate();
				}
				public void ExitState(Slottable sb){
				}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){
					
				}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class DefocusedState: SlottableState{
				public void EnterState(Slottable slottable){
					if(slottable.PrevState == Slottable.DeactivatedState){
						slottable.InstantGrayout();
					}else if(slottable.PrevState == Slottable.FocusedState){
						slottable.GradualGrayoutProcess.Start();
					}
				}
				public void ExitState(Slottable slottable){
					if(slottable.GradualGrayoutProcess.IsRunning)
						slottable.GradualGrayoutProcess.Stop();
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.SetState(Slottable.WaitForPointerUpState);
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class FocusedState: SlottableState{
				public void EnterState(Slottable slottable){
					if(slottable.PrevState == Slottable.DeactivatedState || slottable.PrevState == Slottable.MovingState || slottable.PrevState == Slottable.WaitForNextTouchState){
						slottable.InstantGrayin();
					}else if(slottable.PrevState == Slottable.DefocusedState){
						slottable.GradualGrayinProcess.Start();
					}else if(slottable.PrevState == Slottable.SelectedState){
						slottable.GradualDehighlightProcess.Start();
					}
				}
				public void ExitState(Slottable slottable){
					if(slottable.GradualGrayinProcess.IsRunning)
						slottable.GradualGrayinProcess.Stop();
					if(slottable.GradualDehighlightProcess.IsRunning)
						slottable.GradualDehighlightProcess.Stop();
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					if(slottable.Delayed)
						slottable.SetState(Slottable.WaitForPickUpState);
					else
						slottable.SetState(Slottable.PickedUpAndSelectedState);
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SGM.SetSelectedSB(sb);
					sb.SetState(Slottable.SelectedState);
					// if(eventDataMock.pointerDrag != null){
					// 	Slottable draggedSb = eventDataMock.pointerDrag.GetComponent<Slottable>();
					// 	if(draggedSb != null){
					// 		if(draggedSb.CurState == Slottable.PickedUpAndSelectedState){
					// 			sb.SetState(Slottable.SelectedState);
					// 			sb.SGM.SetSelectedSB(sb);
					// 		}
					// 	}
					// }
				}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class WaitForPointerUpState: SlottableState{
				public void EnterState(Slottable sb){
					sb.WaitAndSetBackToDefocusedStateProcess.Start();
				}
				public void ExitState(Slottable sb){
					if(sb.WaitAndSetBackToDefocusedStateProcess.IsRunning)
						sb.WaitAndSetBackToDefocusedStateProcess.Stop();
				}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.Tap();
					sb.SetState(Slottable.DefocusedState);
					
				}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SetState(Slottable.DefocusedState);
				}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class WaitForPickUpState: SlottableState{
				public void EnterState(Slottable slottable){
					// slottable.WaitAndPickupMock();
					slottable.WaitAndPickUpProcess.Start();
				}
				public void ExitState(Slottable slottable){
					if(slottable.WaitAndPickUpProcess.IsRunning)
						slottable.WaitAndPickUpProcess.Stop();
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.SetState(Slottable.WaitForNextTouchState);
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.SetState(Slottable.FocusedState);
				}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SetState(Slottable.FocusedState);
				}
			}
			public class WaitForNextTouchState: SlottableState{
				public void EnterState(Slottable slottable){
					slottable.WaitAndTapMock();
				}
				public void ExitState(Slottable slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.IsTapTimerOn = false;
					slottable.PickUp();
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.Cancel();
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class PickedUpAndSelectedState: SlottableState{
				public void EnterState(Slottable slottable){
					slottable.PickedUpAndSelectedProcess.Start();
					slottable.SGM.SetState(SlotGroupManager.ProbingState);
					InitializeSGMFields(slottable);
					slottable.SGM.PostPickFilter();
					
				}
				public void ExitState(Slottable slottable){
					if(slottable.PickedUpAndSelectedProcess.IsRunning)
						slottable.PickedUpAndSelectedProcess.Stop();

				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.ExecuteTransaction();
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.ExecuteTransaction();
				}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SGM.SetPickedSB(sb);
					sb.SGM.SetSelectedSB(sb);
				}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					// if(sb.SGM.SelectedSB == sb){
					// 	sb.SGM.SelectedSB = null;
					// }
					//=> handled in SGM side
					if(sb.SGM.SelectedSB == sb){
						sb.SGM.SetSelectedSB(null);
					}
					sb.SetState(Slottable.PickedUpAndDeselectedState);

					
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
				public void EnterState(Slottable sb){}
				public void ExitState(Slottable sb){}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SetState(Slottable.PickedUpAndSelectedState);
					sb.SGM.SetSelectedSB(sb);
				}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class WaitForNextTouchWhilePUState: SlottableState{
				public void EnterState(Slottable slottable){
					slottable.WaitAndRevertMock();
				}
				public void ExitState(Slottable Slottable){
				}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.Increment();
					slottable.SetState(Slottable.PickedUpAndSelectedState);
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){
					slottable.Revert();
				}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){
				}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class MovingState: SlottableState{
				public void EnterState(Slottable slottable){
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
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class EquippedAndDeselectedState: SlottableState{
				public void EnterState(Slottable slottable){}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){
					if(!slottable.SGM.GetSlotGroup(slottable).IsPool){
						if(slottable.Delayed)
							slottable.SetState(Slottable.WaitForPickUpState);
						else
							slottable.SetState(Slottable.PickedUpAndSelectedState);
					}
				}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					sb.SGM.SetSelectedSB(sb);
					sb.SetState(Slottable.EquippedAndSelectedState);
				}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
			}
			public class EquippedAndSelectedState: SlottableState{
				public void EnterState(Slottable slottable){}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
				}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(sb);
					sb.SetState(Slottable.EquippedAndDeselectedState);
				}

			}
			public class EquippedAndDefocusedState: SlottableState{
				public void EnterState(Slottable slottable){}
				public void ExitState(Slottable slottable){}
				public void OnPointerDownMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable slottable, PointerEventDataMock eventDataMock){}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
				}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
				}

			}
			public class SBSelectedState: SlottableState{
				public void EnterState(Slottable sb){}
				public void ExitState(Slottable sb){}
				public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
				public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					if(sb.SGM.SelectedSB == sb)
						sb.SGM.SetSelectedSB(null);
					sb.SetState(Slottable.FocusedState);
				}
			}
			public class SBRevertingState: SlottableState{
				public void EnterState(Slottable sb){
					sb.RevertingStateProcess.Start();
				}
				public void ExitState(Slottable sb){
					if(sb.RevertingStateProcess.IsRunning)
						sb.RevertingStateProcess.Expire();
				}
				/*	undefined events
				*/
					public void OnPointerDownMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnPointerUpMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnDeselectedMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnEndDragMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnHoveredMock(Slottable sb, PointerEventDataMock eventDataMock){}
					public void OnDehoveredMock(Slottable sb, PointerEventDataMock eventDataMock){
					
				}
			}
		/*	commands
		*/
			public interface SlottableCommand{
				void Execute(Slottable sb);
			}
			public class DefInstantDeactivateCommand: SlottableCommand{
				public void Execute(Slottable sb){
					sb.UTLog = "InstantDeactivate executed";
				}
			}
			public class SBTapCommand: SlottableCommand{
				public void Execute(Slottable sb){

				}
			}
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
				bool flag = a.Item.ItemID == b.Item.ItemID;
				flag &= a.IsStackable && b.IsStackable;
				return flag;
			}
			public static bool operator != (InventoryItemInstanceMock a, InventoryItemInstanceMock b){
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
	/*	SlotGroup Classes
	*/
		/*	states
		*/
			public interface SlotGroupState{
				void EnterState(SlotGroup sg);
				void ExitState(SlotGroup sg);
				void OnHoveredMock(SlotGroup sg, PointerEventDataMock eventData);
				void OnDehoveredMock(SlotGroup sg, PointerEventDataMock eventData);
			}
			public class SGDeactivatedState : SlotGroupState{
				public void EnterState(SlotGroup sg){
				}
				public void ExitState(SlotGroup sg){
				}
				public void OnHoveredMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void OnDehoveredMock(SlotGroup sg, PointerEventDataMock eventData){
				}
			}
			public class SGDefocusedState: SlotGroupState{
				public void EnterState(SlotGroup sg){}
				public void ExitState(SlotGroup sg){}
				public void OnHoveredMock(SlotGroup sg, PointerEventDataMock eventData){
				}
				public void OnDehoveredMock(SlotGroup sg, PointerEventDataMock eventData){
				}
			}
			public class SGFocusedState: SlotGroupState{
				public void EnterState(SlotGroup sg){}
				public void ExitState(SlotGroup sg){}
				public void OnHoveredMock(SlotGroup sg, PointerEventDataMock eventData){
					sg.SGM.SetSelectedSG(sg);
					sg.SetState(SlotGroup.SelectedState);
				}
				public void OnDehoveredMock(SlotGroup sg, PointerEventDataMock eventData){
				}
			}
			public class SGSelectedState: SlotGroupState{
				public void EnterState(SlotGroup sg){}
				public void ExitState(SlotGroup sg){}
				public void OnHoveredMock(SlotGroup sg, PointerEventDataMock eventData){}
				public void OnDehoveredMock(SlotGroup sg, PointerEventDataMock eventData){
					if(sg.SGM.SelectedSG == sg){
						sg.SGM.SetSelectedSG(sg);
						sg.SetState(SlotGroup.FocusedState);
					}
					
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
			public class SGUpdateSbStateCommand: SlotGroupCommand{
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
												}else{
													slot.Sb.SetState(Slottable.DefocusedState);
												}
											}
										}
									}
								}
							}
						}
					}
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
						foreach(SlottableItem item in sg.ItemInstances){
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
					for(int i = 0; i < sg.ItemInstances.Count; i++){
						GameObject go = new GameObject("SlottablePrefab");
						Slottable sb = go.AddComponent<Slottable>();
						sb.Initialize(sg);
						sb.Delayed = true;
						sb.SetSlottableItem(sg.ItemInstances[i]);
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
					sg.SetItems(filteredItems);
				}
			}
			public class SGBowFilter: SGFilter{
				public void Execute(SlotGroup sg){
					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						if(item is BowInstanceMock)
							filteredItems.Add(item);
					}
					sg.SetItems(filteredItems);
				}
			}
			public class SGWearFilter: SGFilter{
				public void Execute(SlotGroup sg){
					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						if(item is WearInstanceMock)
							filteredItems.Add(item);
					}
					sg.SetItems(filteredItems);
				}
			}
			public class SGPartsFilter: SGFilter{
				public void Execute(SlotGroup sg){
					List<SlottableItem> filteredItems = new List<SlottableItem>();
					foreach(SlottableItem item in sg.Inventory.Items){
						if(item is PartsInstanceMock)
							filteredItems.Add(item);
					}
					sg.SetItems(filteredItems);
				}
			}
			
		/*	sorters
		*/
			public interface SGSorter{
				void Execute(SlotGroup sg);
			}
			public class SGItemIndexSorter: SGSorter{
				public void Execute(SlotGroup sg){
					SlottableItem[] itemsArray = sg.ItemInstances.ToArray();
					Array.Sort(itemsArray);
					List<SlottableItem> newList = new List<SlottableItem>();
					foreach(SlottableItem item in itemsArray){
						newList.Add(item);
					}
					sg.SetItems(newList);
				}
			}
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
				m_items.Add(item);
			}
		}
		public class EquipmentSetInventory: Inventory{
			List<SlottableItem> m_items = new List<SlottableItem>();
			public List<SlottableItem> Items{
				get{return m_items;}
			}
			public void Add(SlottableItem item){
				m_items.Add(item);
			}
		}
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
		public class PartsMock: InventoryItemMock{
			public PartsMock(){
				IsStackable = true;
			}
		}
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
		public class PartsInstanceMock: InventoryItemInstanceMock{

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
}
