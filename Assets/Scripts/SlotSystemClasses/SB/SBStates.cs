using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace SlotSystem{
	public abstract class SBState: SSEState{
        protected ISlottable sb;
        public SBState(ISlottable sb){
            this.sb = sb;
        }
	}
    /* ActState */
        public abstract class SBActState: SBState, ISBActState{
            public SBActState(ISlottable sb) : base(sb){}
            public virtual void OnPointerDown(){}
            public virtual void OnPointerUp(){}
            public virtual void OnDeselected(){}
            public virtual void OnEndDrag(){}
        }
        public interface ISBActState: ISSEState{
            void OnPointerDown();
            void OnPointerUp();
            void OnDeselected();
            void OnEndDrag();
        }
        public abstract class SBStateFactory{
            protected ISlottable sb;
            protected IItemHandler itemHandler;
            public SBStateFactory(ISlottable sb){
                this.sb = sb;
            }
        }
        public class SBActStateFactory: SBStateFactory, ISBActStateFactory{
            public SBActStateFactory(ISlottable sb): base(sb){
            }
            public ISBActState MakeWaitForActionState(){
                if(waitForActionState == null)
                    waitForActionState = new WaitForActionState(sb);
                return waitForActionState;
            }
                ISBActState waitForActionState;
            public ISBActState MakeWaitForPickUpState(){
                if(waitForPickUpState == null)
                    waitForPickUpState = new WaitForPickUpState(sb);
                return waitForPickUpState;
            }
                ISBActState waitForPickUpState;
            public ISBActState MakeWaitForPointerUpState(){
                if(waitForPointerUpState == null)
                    waitForPointerUpState = new WaitForPointerUpState(sb);
                return waitForPointerUpState;
            }
                ISBActState waitForPointerUpState;
            public ISBActState MakeWaitForNextTouchState(){
                if(waitForNextTouchState == null)
                    waitForNextTouchState = new WaitForNextTouchState(sb);
                return waitForNextTouchState;
            }
                ISBActState waitForNextTouchState;
            public ISBActState MakePickingUpState(){
                if(pickingUpState == null)
                    pickingUpState = new PickingUpState(sb);
                return pickingUpState;
            }
                ISBActState pickingUpState;
            public ISBActState MakeAddedState(){
                if(addedState == null)
                    addedState = new SBAddedState(sb);
                return addedState;
            }
                ISBActState addedState;
            public ISBActState MakeRemovedState(){
                if(removedState == null)
                    removedState = new SBRemovedState(sb);
                return removedState;
            }
                ISBActState removedState;
            public ISBActState MakeMoveWithinState(){
                if(moveWithinState == null)
                    moveWithinState = new MoveWithinState(sb);
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
            public WaitForActionState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.wasActStateNull)
                    sb.SetAndRunActProcess(null);
            }
            public override void OnPointerDown(){
                if(sb.isFocused){
                    sb.Select();
                    sb.WaitForPickUp();
                }
                else
                    sb.WaitForPointerUp();
            }
        }
        public class WaitForPickUpState: SBActState{//down state
            public WaitForPickUpState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(sb.wasWaitingForAction){
                    ISBActProcess wfpuProcess = new WaitForPickUpProcess(sb, sb.waitForPickUpCoroutine);
                    sb.SetAndRunActProcess(wfpuProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
            }
            public override void OnPointerUp(){
                sb.WaitForNextTouch();
            }
            public override void OnEndDrag(){
                sb.Refresh();                    
                sb.Focus();
            }
        }
        public class WaitForPointerUpState: SBActState{//down state
            public WaitForPointerUpState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(sb.wasWaitingForAction){
                    ISBActProcess wfPtuProcess = new WaitForPointerUpProcess(sb, sb.waitForPointerUpCoroutine);
                    sb.SetAndRunActProcess(wfPtuProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
                }
            public override void OnPointerUp(){
                sb.Tap();
                sb.Refresh();
                sb.Defocus();
                }
            public override void OnEndDrag(){
                sb.Refresh();
                sb.Defocus();
                }
        }
        public class WaitForNextTouchState: SBActState{//up state
            public WaitForNextTouchState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(sb.wasPickingUp || sb.wasWaitingForPickUp){
                    ISBActProcess wfntProcess = new WaitForNextTouchProcess(sb, sb.waitForNextTouchCoroutine);
                    sb.SetAndRunActProcess(wfntProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than PickingUpState or WaitForPickUpState");
            }
            public override void OnPointerDown(){
                if(!sb.isPickedUp)
                    sb.PickUp();
                else{
                    sb.Increment();
                }
            }
            public override void OnDeselected(){
                sb.Refresh();
                sb.Focus();
            }
        }
        public class PickingUpState: SBActState{//down state
            public PickingUpState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(sb.wasWaitingForPickUp || sb.wasWaitingForNextTouch){
                    sb.OnHoverEnter();
                    sb.SetPickedSB();
                    sb.Probe();
                    sb.SetDIcon1();
                    sb.CreateTAResult();
                    sb.UpdateTA();
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForPickUpState or WaitForNextTouchState");
                ISBActProcess pickedUpProcess = new PickUpProcess(sb, sb.pickUpCoroutine);
                sb.SetAndRunActProcess(pickedUpProcess);
            }
            public override void OnDeselected(){
                sb.Refresh();
                sb.Focus();
            }
            public override void OnPointerUp(){
                if(sb.isHovered && sb.isStackable)
                    sb.WaitForNextTouch();
                else
                    sb.ExecuteTransaction();
            }
            public override void OnEndDrag(){
                sb.ExecuteTransaction();
            }
        }
        public class SBAddedState: SBActState{
            public SBAddedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                SBAddProcess process = new SBAddProcess(sb, sb.addCoroutine);
                sb.SetAndRunActProcess(process);
            }
        }
        public class SBRemovedState: SBActState{
            public SBRemovedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                SBRemoveProcess process = new SBRemoveProcess(sb, sb.removeCoroutine);
                sb.SetAndRunActProcess(process);
            }
        }
        public class MoveWithinState: SBActState{
            public MoveWithinState(ISlottable sb): base(sb){}
            public override void EnterState(){
                SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.moveWithinCoroutine);
                sb.SetAndRunActProcess(process);
            }
        }
    /* EqpState */
        public abstract class SBEqpState: SBState, ISBEqpState{
            public SBEqpState(ISlottable sb): base(sb){}
        }
        public interface ISBEqpState: ISSEState{}
        public class SBEqpStateFactory: SBStateFactory, ISBEqpStateFactory{
            public SBEqpStateFactory(ISlottable sb): base(sb){
            }
            public ISBEqpState MakeEquippedState(){
                if(equippedState == null)
                    equippedState = new SBEquippedState(sb);
                return equippedState;
            }
                ISBEqpState equippedState;
            public ISBEqpState MakeUnequippedState(){
                if(unequippedState == null)
                    unequippedState = new SBUnequippedState(sb);
                return unequippedState;
            }
                ISBEqpState unequippedState;
        }
        public interface ISBEqpStateFactory{
            ISBEqpState MakeEquippedState();
            ISBEqpState MakeUnequippedState();
        }
        public class SBEquippedState: SBEqpState{
            public SBEquippedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.isHierarchySetUp)
                    return;
                if(sb.isPool){
                    if(sb.isUnequipped){
                        ISBEqpProcess process = new SBEquipProcess(sb, sb.equipCoroutine);
                        sb.SetAndRunEqpProcess(process);
                    }
                }
            }
        }
        public class SBUnequippedState: SBEqpState{
            public SBUnequippedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.isHierarchySetUp) 
                    return;
                if(sb.isPool){
                    if(sb.isEquipped){
                        ISBEqpProcess process = new SBUnequipProcess(sb, sb.unequipCoroutine);
                        sb.SetAndRunEqpProcess(process);
                    }
                }
            }
        }
    /* MrkState */
        public abstract class SBMrkState: SBState, ISBMrkState{
            public SBMrkState(ISlottable sb): base(sb){}
        }
        public interface ISBMrkState: ISSEState{
        }
        public class SBMrkStateFactory: SBStateFactory, ISBMrkStateFactory{
            public SBMrkStateFactory(ISlottable sb): base(sb){
            }
            public ISBMrkState MakeMarkedState(){
                if(markedState == null)
                    markedState = new SBMarkedState(sb);
                return markedState;
            }
                ISBMrkState markedState;
            public ISBMrkState MakeUnmarkedState(){
                if(unmarkedState == null)
                    unmarkedState = new SBUnmarkedState(sb);
                return unmarkedState;
            }
                ISBMrkState unmarkedState;
        }
        public interface ISBMrkStateFactory{
            ISBMrkState MakeMarkedState();
            ISBMrkState MakeUnmarkedState();
        }
        public class SBMarkedState: SBMrkState{
            public SBMarkedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.isHierarchySetUp)
                    return;
                if(sb.isPool){
                    if(sb.isUnmarked){
                        ISBMrkProcess process = new SBMarkProcess(sb, sb.markCoroutine);
                        sb.SetAndRunMrkProcess(process);
                    }
                }
            }
        }
        public class SBUnmarkedState: SBMrkState{
            public SBUnmarkedState(ISlottable sb): base (sb){}
            public override void EnterState(){
                if(!sb.isHierarchySetUp)
                    return;
                if(sb.isPool){
                    if(sb.isMarked){
                        ISBMrkProcess process = new SBUnmarkProcess(sb, sb.unmarkCoroutine);
                        sb.SetAndRunMrkProcess(process);
                    }
                }
            }
        }
}
