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
    /* SelState */
    /* ActState */
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
        public class SBActStateFactory:ISBActStateFactory{
            public ISBActState MakeWaitForActionState(){
                if(waitForActionState == null)
                    waitForActionState = new WaitForActionState();
                return waitForActionState;
            }
                ISBActState waitForActionState;
            public ISBActState MakeWaitForPickUpState(){
                if(waitForPickUpState == null)
                    waitForPickUpState = new WaitForPickUpState();
                return waitForPickUpState;
            }
                ISBActState waitForPickUpState;
            public ISBActState MakeWaitForPointerUpState(){
                if(waitForPointerUpState == null)
                    waitForPointerUpState = new WaitForPointerUpState();
                return waitForPointerUpState;
            }
                ISBActState waitForPointerUpState;
            public ISBActState MakeWaitForNextTouchState(){
                if(waitForNextTouchState == null)
                    waitForNextTouchState = new WaitForNextTouchState();
                return waitForNextTouchState;
            }
                ISBActState waitForNextTouchState;
            public ISBActState MakePickingUpState(){
                if(pickingUpState == null)
                    pickingUpState = new PickingUpState();
                return pickingUpState;
            }
                ISBActState pickingUpState;
            public ISBActState MakeAddedState(){
                if(addedState == null)
                    addedState = new SBAddedState();
                return addedState;
            }
                ISBActState addedState;
            public ISBActState MakeRemovedState(){
                if(removedState == null)
                    removedState = new SBRemovedState();
                return removedState;
            }
                ISBActState removedState;
            public ISBActState MakeMoveWithinState(){
                if(moveWithinState == null)
                    moveWithinState = new MoveWithinState();
                return moveWithinState;
            }
                ISBActState moveWithinState;

            
        }
        public interface ISBActStateFactory{
            ISBActState MakeWaitForActionState();
            ISBActState MakeWaitForPickUpState();
            ISBActState MakeWaitForPointerUpState();
            ISBActState MakeWaitForNextTouchState();
            ISBActState MakePickingUpState();
            ISBActState MakeAddedState();
            ISBActState MakeRemovedState();
            ISBActState MakeMoveWithinState();

        }
        public class WaitForActionState: SBActState{//up state
            public override void EnterState(IStateHandler sh){
                base.EnterState(sh);
                if(!sb.wasActStateNull)
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
                    if(!sb.isSelStateNull)
                        sb.OnHoverEnter();
                    if(sb.tam != null){
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
        public class MoveWithinState: SBActState{
            public override void EnterState(IStateHandler sh){
                base.EnterState(sh);
                SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.moveWithinCoroutine);
                sb.SetAndRunActProcess(process);
            }
        }
    /* EqpState */
        public abstract class SBEqpState: SBState, ISBEqpState{}
        public interface ISBEqpState: ISSEState{}
        public class SBEqpStateFactory:ISBEqpStateFactory{
            public ISBEqpState MakeEquippedState(){
                if(equippedState == null)
                    equippedState = new SBEquippedState();
                return equippedState;
            }
                ISBEqpState equippedState;
            public ISBEqpState MakeUnequippedState(){
                if(unequippedState == null)
                    unequippedState = new SBUnequippedState();
                return unequippedState;
            }
                ISBEqpState unequippedState;
        }
        public interface ISBEqpStateFactory{
            ISBEqpState MakeEquippedState();
            ISBEqpState MakeUnequippedState();
        }
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
    /* MrkState */
        public abstract class SBMrkState: SBState, ISBMrkState{
        }
        public interface ISBMrkState: ISSEState{
        }
        public class SBMrkStateFactory: ISBMrkStateFactory{
            public ISBMrkState MakeMarkedState(){
                if(markedState == null)
                    markedState = new SBMarkedState();
                return markedState;
            }
                ISBMrkState markedState;
            public ISBMrkState MakeUnmarkedState(){
                if(unmarkedState == null)
                    unmarkedState = new SBUnmarkedState();
                return unmarkedState;
            }
                ISBMrkState unmarkedState;
        }
        public interface ISBMrkStateFactory{
            ISBMrkState MakeMarkedState();
            ISBMrkState MakeUnmarkedState();
        }
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
