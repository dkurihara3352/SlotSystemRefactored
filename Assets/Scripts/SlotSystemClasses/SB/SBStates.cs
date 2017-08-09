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
            public SBActState(ISBActStateHandler actStateHandler) : base((ISlottable)actStateHandler){}
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
        public abstract class SBStateRepo{
        }
        public class SBActStateRepo: SBStateRepo, ISBActStateRepo{
            ITransactionManager tam;
            ITransactionCache taCache;
            ISBActStateHandler actStateHandler;
            public SBActStateRepo(ISBActStateHandler actStateHandler, ITransactionManager tam, ITransactionCache taCache){
                this.tam = tam;
                this.taCache = taCache;
                this.actStateHandler = actStateHandler;
            }
            public ISBActState waitForActionState{
                get{
                    if(_waitForActionState == null)
                        _waitForActionState = new WaitForActionState(actStateHandler);
                    return _waitForActionState;
                }
            }
                ISBActState _waitForActionState;
            public ISBActState waitForPickUpState{
                get{
                    if(_waitForPickUpState == null)
                        _waitForPickUpState = new WaitForPickUpState(actStateHandler);
                    return _waitForPickUpState;
                }
            }
                ISBActState _waitForPickUpState;
            public ISBActState waitForPointerUpState{
                get{
                    if(_waitForPointerUpState == null)
                        _waitForPointerUpState = new WaitForPointerUpState(actStateHandler);
                    return _waitForPointerUpState;
                }
            }
                ISBActState _waitForPointerUpState;
            public ISBActState waitForNextTouchState{
                get{
                    if(_waitForNextTouchState == null)
                        _waitForNextTouchState = new WaitForNextTouchState(actStateHandler, tam);
                    return _waitForNextTouchState;
                }
            }
                ISBActState _waitForNextTouchState;
            public ISBActState pickingUpState{
                get{
                    if(_pickingUpState == null)
                        _pickingUpState = new PickingUpState(actStateHandler, tam, taCache);
                    return _pickingUpState;
                }
            }
                ISBActState _pickingUpState;
            public ISBActState addedState{
                get{
                    if(_addedState == null)
                        _addedState = new SBAddedState(actStateHandler);
                    return _addedState;
                }
            }
                ISBActState _addedState;
            public ISBActState removedState{
                get{
                    if(_removedState == null)
                        _removedState = new SBRemovedState(actStateHandler);
                    return _removedState;
                }
            }
                ISBActState _removedState;
            public ISBActState moveWithinState{
                get{
                    if(_moveWithinState == null)
                        _moveWithinState = new MoveWithinState(actStateHandler);
                    return _moveWithinState;
                }
            }
                ISBActState _moveWithinState;

            
        }
        public interface ISBActStateRepo{
            ISBActState waitForActionState{get;}
            ISBActState waitForPickUpState{get;}
            ISBActState waitForPointerUpState{get;}
            ISBActState waitForNextTouchState{get;}
            ISBActState pickingUpState{get;}
            ISBActState addedState{get;}
            ISBActState removedState{get;}
            ISBActState moveWithinState{get;}

        }
        public class WaitForActionState: SBActState{//up state
            public WaitForActionState(ISBActStateHandler actStateHandler): base(actStateHandler){}
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
            public WaitForPickUpState(ISBActStateHandler actStateHandler): base(actStateHandler){}
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
            public WaitForPointerUpState(ISBActStateHandler actStateHandler): base(actStateHandler){}
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
            ITransactionManager tam;
            public WaitForNextTouchState(ISBActStateHandler actStateHandler, ITransactionManager tam): base(actStateHandler){
                this.tam = tam;
            }
            public override void EnterState(){
                if(sb.wasPickingUp || sb.wasWaitingForPickUp){
                    ISBActProcess wfntProcess = new WaitForNextTouchProcess(sb, sb.waitForNextTouchCoroutine, tam);
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
            ITransactionManager tam;
            ITransactionIconHandler iconHandler;
            ITransactionCache taCache{
                get{
                    if(_taCache != null)
                        return _taCache;
                    else
                        throw new InvalidOperationException("taCache not set");
                }
            }
                ITransactionCache _taCache;
            ITAMActStateHandler tamStateHandler;
            public PickingUpState(ISBActStateHandler actStateHandler, ITransactionManager tam, ITransactionCache taCache): base(actStateHandler){
                this.tam = tam;
                this.iconHandler = tam.iconHandler;
                _taCache = taCache;
                this.tamStateHandler = tam.actStateHandler;
            }
            public override void EnterState(){
                if(sb.wasWaitingForPickUp || sb.wasWaitingForNextTouch){
                    sb.OnHoverEnter();
                    taCache.SetPickedSB(sb);
                    iconHandler.SetDIcon1(sb);
                    tamStateHandler.Probe();
                    taCache.CreateTransactionResults();
                    taCache.UpdateFields();
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
                    tam.ExecuteTransaction();
            }
            public override void OnEndDrag(){
                tam.ExecuteTransaction();
            }
        }
        public class SBAddedState: SBActState{
            public SBAddedState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBAddProcess process = new SBAddProcess(sb, sb.addCoroutine);
                sb.SetAndRunActProcess(process);
            }
        }
        public class SBRemovedState: SBActState{
            public SBRemovedState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBRemoveProcess process = new SBRemoveProcess(sb, sb.removeCoroutine);
                sb.SetAndRunActProcess(process);
            }
        }
        public class MoveWithinState: SBActState{
            public MoveWithinState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.moveWithinCoroutine);
                sb.SetAndRunActProcess(process);
            }
        }
    /* EqpState */
        public abstract class SBEqpState: SBState, ISBEqpState{
            protected ISBEqpStateHandler eqpStateHandler;
            public SBEqpState(ISlottable sb): base(sb){
                this.eqpStateHandler = sb;
            }
        }
        public interface ISBEqpState: ISSEState{}
        public class SBEqpStateRepo: SBStateRepo, ISBEqpStateRepo{
            ISlottable sb;
            protected ISBEqpStateHandler eqpStateHandler;
            public SBEqpStateRepo(ISlottable sb){
                this.sb = sb;
                this.eqpStateHandler = sb;
            }
            public ISBEqpState equippedState{
                get{
                    if(_equippedState == null)
                        _equippedState = new SBEquippedState(sb);
                    return _equippedState;
                }
            }
                ISBEqpState _equippedState;
            public ISBEqpState unequippedState{
                get{
                    if(_unequippedState == null)
                        _unequippedState = new SBUnequippedState(sb);
                    return _unequippedState;
                }
            }
                ISBEqpState _unequippedState;
        }
        public interface ISBEqpStateRepo{
            ISBEqpState equippedState{get;}
            ISBEqpState unequippedState{get;}
        }
        public class SBEquippedState: SBEqpState{
            public SBEquippedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.isHierarchySetUp)
                    return;
                if(sb.isPool){
                    if(eqpStateHandler.isUnequipped){
                        ISBEqpProcess process = new SBEquipProcess(sb, eqpStateHandler.equipCoroutine);
                        eqpStateHandler.SetAndRunEqpProcess(process);
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
                    if(eqpStateHandler.isEquipped){
                        ISBEqpProcess process = new SBUnequipProcess(sb, eqpStateHandler.unequipCoroutine);
                        eqpStateHandler.SetAndRunEqpProcess(process);
                    }
                }
            }
        }
    /* MrkState */
        public abstract class SBMrkState: SBState, ISBMrkState{
            protected ISBMrkStateHandler mrkStateHandler;
            public SBMrkState(ISlottable sb): base(sb){
                mrkStateHandler = sb;
            }
        }
        public interface ISBMrkState: ISSEState{
        }
        public class SBMrkStateRepo: SBStateRepo, ISBMrkStateRepo{
            ISlottable sb;
            public SBMrkStateRepo(ISlottable sb){
                this.sb = sb;
            }
            public ISBMrkState markedState{
                get{
                    if(_markedState == null)
                        _markedState = new SBMarkedState(sb);
                    return _markedState;
                }
            }
                ISBMrkState _markedState;
            public ISBMrkState unmarkedState{
                get{
                    if(_unmarkedState == null)
                        _unmarkedState = new SBUnmarkedState(sb);
                    return _unmarkedState;
                }
            }
                ISBMrkState _unmarkedState;
        }
        public interface ISBMrkStateRepo{
            ISBMrkState markedState{get;}
            ISBMrkState unmarkedState{get;}
        }
        public class SBMarkedState: SBMrkState{
            public SBMarkedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.isHierarchySetUp)
                    return;
                if(sb.isPool){
                    if(mrkStateHandler.isUnmarked){
                        ISBMrkProcess process = new SBMarkProcess(sb, mrkStateHandler.markCoroutine);
                        mrkStateHandler.SetAndRunMrkProcess(process);
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
                    if(mrkStateHandler.isMarked){
                        ISBMrkProcess process = new SBUnmarkProcess(sb, mrkStateHandler.unmarkCoroutine);
                        mrkStateHandler.SetAndRunMrkProcess(process);
                    }
                }
            }
        }
}
