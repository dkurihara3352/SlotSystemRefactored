using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace SlotSystem{
	public abstract class SBState: SSEState{
		protected ISlottable sb{
			get{
				return (ISlottable)sse;
			}
		}
	}
        public abstract class SBActState: SBState, ISBActState{
            public virtual void OnPointerDown(ISlottable sb, PointerEventDataFake eventDataMock){}
            public virtual void OnPointerUp(ISlottable sb, PointerEventDataFake eventDataMock){}
            public virtual void OnDeselected(ISlottable sb, PointerEventDataFake eventDataMock){}
            public virtual void OnEndDrag(ISlottable sb, PointerEventDataFake eventDataMock){}
        }
        public interface ISBActState: ISSEState{
            void OnPointerDown(ISlottable sb, PointerEventDataFake eventDataMock);
            void OnPointerUp(ISlottable sb, PointerEventDataFake eventDataMock);
            void OnDeselected(ISlottable sb, PointerEventDataFake eventDataMock);
            void OnEndDrag(ISlottable sb, PointerEventDataFake eventDataMock);
        }
            public class WaitForActionState: SBActState{//up state
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(!sb.isPrevActStateNull)
                        sb.SetAndRunActProcess(null);
                }
                public override void OnPointerDown(ISlottable sb, PointerEventDataFake eventDataMock){
                    if(sb.isFocused){
                        sb.Select();
                        sb.WaitForPickUp();
                    }
                    else
                        sb.WaitForPointerUp();
                }
              }
            public class WaitForPickUpState: SBActState{//down state
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.wasWaitingForAction){
                        ISBActProcess wfpuProcess = new WaitForPickUpProcess(sb, sb.waitForPickUpCoroutine);
                        sb.SetAndRunActProcess(wfpuProcess);
                    }else
                        throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
                }
                public override void OnPointerUp(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.WaitForNextTouch();
                }
                public override void OnEndDrag(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Refresh();                    
                    sb.Focus();
                }
              }
            public class WaitForPointerUpState: SBActState{//down state
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.wasWaitingForAction){
                        ISBActProcess wfPtuProcess = new WaitForPointerUpProcess(sb, sb.waitForPointerUpCoroutine);
                        sb.SetAndRunActProcess(wfPtuProcess);
                    }else
                        throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
                 }
                public override void OnPointerUp(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Tap();
                    sb.Refresh();
                    sb.Defocus();
                 }
                public override void OnEndDrag(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Refresh();
                    sb.Defocus();
                    }
             }
            public class WaitForNextTouchState: SBActState{//up state
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.wasPickingUp || sb.wasWaitingForPickUp){
                        ISBActProcess wfntProcess = new WaitForNextTouchProcess(sb, sb.waitForNextTouchCoroutine);
                        sb.SetAndRunActProcess(wfntProcess);
                    }else
                        throw new InvalidOperationException("cannot enter this state from anything other than PickingUpState or WaitForPickUpState");
                }
                public override void OnPointerDown(ISlottable sb, PointerEventDataFake eventDataMock){
                    if(!sb.isPickedUp)
                        sb.PickUp();
                    else{
                        sb.Increment();
                    }
                }
                public override void OnDeselected(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Refresh();
                    sb.Focus();
                }
                }
            public class PickingUpState: SBActState{//down state
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(sb.wasWaitingForPickUp || sb.wasWaitingForNextTouch){
                        if(!sb.isCurSelStateNull)
                            sb.OnHoverEnter();
                        if(sb.ssm != null){
                            sb.SetPickedSB();
                            sb.Probe();
                            sb.SetDIcon1();
                            sb.CreateTAResult();
                            sb.UpdateTA();
                        }
                    }else
                        throw new InvalidOperationException("cannot enter this state from anything other than WaitForPickUpState or WaitForNextTouchState");
                    ISBActProcess pickedUpProcess = new PickUpProcess(sb, sb.pickUpCoroutine);
                    sb.SetAndRunActProcess(pickedUpProcess);
                }
                public override void OnDeselected(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.Refresh();
                    sb.Focus();
                }
                public override void OnPointerUp(ISlottable sb, PointerEventDataFake eventDataMock){
                    if(sb.isHovered && sb.isStackable)
                        sb.WaitForNextTouch();
                    else
                        sb.ExecuteTransaction();
                }
                public override void OnEndDrag(ISlottable sb, PointerEventDataFake eventDataMock){
                    sb.ExecuteTransaction();
                }
            }
            public class MoveWithinState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.moveWithinCoroutine);
                    sb.SetAndRunActProcess(process);
                }
            }
            public class SBAddedState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SBAddProcess process = new SBAddProcess(sb, sb.addCoroutine);
                    sb.SetAndRunActProcess(process);
                }
            }
            public class SBRemovedState: SBActState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    SBRemoveProcess process = new SBRemoveProcess(sb, sb.removeCoroutine);
                    sb.SetAndRunActProcess(process);
                }
            }
        public abstract class SBEqpState: SBState, ISBEqpState{}
        public interface ISBEqpState: ISSEState{}
            public class SBEquippedState: SBEqpState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(!sb.isHierarchySetUp)
                        return;
                    if(sb.isPool){
                        if(sb.isUnequipped){
                            ISBEqpProcess process = new SBEquipProcess(sb, sb.equipCoroutine);
                            sb.SetAndRunEqpProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SBUnequippedState: SBEqpState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(!sb.isHierarchySetUp) 
                        return;
                    if(sb.isPool){
                        if(sb.isEquipped){
                            ISBEqpProcess process = new SBUnequipProcess(sb, sb.unequipCoroutine);
                            sb.SetAndRunEqpProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
        public abstract class SBMrkState: SBState, ISBMrkState{}
        public interface ISBMrkState: ISSEState{}
            public class SBMarkedState: SBMrkState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(!sb.isHierarchySetUp)
                        return;
                    if(sb.isPool){
                        if(sb.isUnmarked){
                            ISBMrkProcess process = new SBMarkProcess(sb, sb.markCoroutine);
                            sb.SetAndRunMrkProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
            public class SBUnmarkedState: SBMrkState{
                public override void EnterState(IStateHandler sh){
                    base.EnterState(sh);
                    if(!sb.isHierarchySetUp)
                        return;
                    if(sb.isPool){
                        if(sb.isMarked){
                            ISBMrkProcess process = new SBUnmarkProcess(sb, sb.unmarkCoroutine);
                            sb.SetAndRunMrkProcess(process);
                        }
                    }
                }
                public override void ExitState(IStateHandler sh){
                    base.ExitState(sh);
                }
            }
}
