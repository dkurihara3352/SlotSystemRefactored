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
            ISSESelStateHandler selStateHandler;
            ISBActStateHandler actStateHandler;
            public SBActStateRepo(ISSESelStateHandler selStateHandler, ISBActStateHandler actStateHandler, ITransactionManager tam, ITransactionCache taCache){
                this.tam = tam;
                this.taCache = taCache;
                this.selStateHandler = selStateHandler;
                this.actStateHandler = actStateHandler;
            }
            public ISBActState GetWaitForActionState(){
                if(_waitForActionState == null)
                    _waitForActionState = new WaitForActionState(selStateHandler, actStateHandler);
                return _waitForActionState;
            }
                ISBActState _waitForActionState;
            public ISBActState GetWaitForPickUpState(){
                if(_waitForPickUpState == null)
                    _waitForPickUpState = new WaitForPickUpState(selStateHandler, actStateHandler);
                return _waitForPickUpState;
            }
                ISBActState _waitForPickUpState;
            public ISBActState GetWaitForPointerUpState(){
                if(_waitForPointerUpState == null)
                    _waitForPointerUpState = new WaitForPointerUpState(selStateHandler, actStateHandler);
                return _waitForPointerUpState;
            }
                ISBActState _waitForPointerUpState;
            public ISBActState GetWaitForNextTouchState(){
                if(_waitForNextTouchState == null)
                    _waitForNextTouchState = new WaitForNextTouchState(selStateHandler, actStateHandler, tam);
                return _waitForNextTouchState;
            }
                ISBActState _waitForNextTouchState;
            public ISBActState GetPickingUpState(){
                if(_pickingUpState == null)
                    _pickingUpState = new PickingUpState(selStateHandler, actStateHandler, tam, taCache);
                return _pickingUpState;
            }
                ISBActState _pickingUpState;
            public ISBActState GetAddedState(){
                if(_addedState == null)
                    _addedState = new SBAddedState(actStateHandler);
                return _addedState;
            }
                ISBActState _addedState;
            public ISBActState GetRemovedState(){
                if(_removedState == null)
                    _removedState = new SBRemovedState(actStateHandler);
                return _removedState;
            }
                ISBActState _removedState;
            public ISBActState GetMoveWithinState(){
                if(_moveWithinState == null)
                    _moveWithinState = new MoveWithinState(actStateHandler);
                return _moveWithinState;
            }
                ISBActState _moveWithinState;
        }
        public interface ISBActStateRepo{
            ISBActState GetWaitForActionState();
            ISBActState GetWaitForPickUpState();
            ISBActState GetWaitForPointerUpState();
            ISBActState GetWaitForNextTouchState();
            ISBActState GetPickingUpState();
            ISBActState GetAddedState();
            ISBActState GetRemovedState();
            ISBActState GetMoveWithinState();

        }
        public class WaitForActionState: SBActState{//up state
            ISSESelStateHandler selStateHandler;
            public WaitForActionState(ISSESelStateHandler selStateHandler, ISBActStateHandler actStateHandler): base(actStateHandler){
                this.selStateHandler = selStateHandler;
            }
            public override void EnterState(){
                if(!sb.WasActStateNull())
                    sb.SetAndRunActProcess(null);
            }
            public override void OnPointerDown(){
                if(selStateHandler.IsFocused()){
                    selStateHandler.Select();
                    sb.WaitForPickUp();
                }
                else
                    sb.WaitForPointerUp();
            }
        }
        public class WaitForPickUpState: SBActState{//down state
            ISSESelStateHandler selStateHandler;
            public WaitForPickUpState(ISSESelStateHandler selStateHandler, ISBActStateHandler actStateHandler): base(actStateHandler){
                this.selStateHandler = selStateHandler;
            }
            public override void EnterState(){
                if(sb.WasWaitingForAction()){
                    ISBActProcess wfpuProcess = new WaitForPickUpProcess(sb, sb.GetWaitForPickUpCoroutine());
                    sb.SetAndRunActProcess(wfpuProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
            }
            public override void OnPointerUp(){
                sb.WaitForNextTouch();
            }
            public override void OnEndDrag(){
                sb.Refresh();                    
                selStateHandler.Focus();
            }
        }
        public class WaitForPointerUpState: SBActState{//down state
            ISSESelStateHandler selStateHandler;
            public WaitForPointerUpState(ISSESelStateHandler selStateHandler, ISBActStateHandler actStateHandler): base(actStateHandler){
                this.selStateHandler = selStateHandler;
            }
            public override void EnterState(){
                if(sb.WasWaitingForAction()){
                    ISBActProcess wfPtuProcess = new WaitForPointerUpProcess(selStateHandler, sb.GetWaitForPointerUpCoroutine());
                    sb.SetAndRunActProcess(wfPtuProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
                }
            public override void OnPointerUp(){
                sb.Tap();
                sb.Refresh();
                selStateHandler.Defocus();
                }
            public override void OnEndDrag(){
                sb.Refresh();
                selStateHandler.Defocus();
                }
        }
        public class WaitForNextTouchState: SBActState{//up state
            ITransactionManager tam;
            ISSESelStateHandler selStateHandler;
            public WaitForNextTouchState(ISSESelStateHandler selStateHandler, ISBActStateHandler actStateHandler, ITransactionManager tam): base(actStateHandler){
                this.tam = tam;
                this.selStateHandler = selStateHandler;
            }
            public override void EnterState(){
                if(sb.WasPickingUp() || sb.WasWaitingForPickUp()){
                    ISBActProcess wfntProcess = new WaitForNextTouchProcess(sb, selStateHandler, tam, sb.GetWaitForNextTouchCoroutine());
                    sb.SetAndRunActProcess(wfntProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than PickingUpState or WaitForPickUpState");
            }
            public override void OnPointerDown(){
                if(!sb.IsPickedUp())
                    sb.PickUp();
                else{
                    sb.Increment();
                }
            }
            public override void OnDeselected(){
                sb.Refresh();
                selStateHandler.Focus();
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
            ISSESelStateHandler selStateHandler;
            public PickingUpState(ISSESelStateHandler selStateHandler, ISBActStateHandler actStateHandler, ITransactionManager tam, ITransactionCache taCache): base(actStateHandler){
                this.tam = tam;
                this.iconHandler = tam.GetIconHandler();
                _taCache = taCache;
                this.tamStateHandler = tam.GetActStateHandler();
                this.selStateHandler = selStateHandler;
            }
            public override void EnterState(){
                if(sb.WasWaitingForPickUp() || sb.WasWaitingForNextTouch()){
                    sb.OnHoverEnter();
                    taCache.SetPickedSB(sb);
                    iconHandler.SetDIcon1(sb);
                    tamStateHandler.Probe();
                    taCache.CreateTransactionResults();
                    taCache.UpdateFields();
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForPickUpState or WaitForNextTouchState");
                ISBActProcess pickedUpProcess = new PickUpProcess(sb, sb.GetPickUpCoroutine());
                sb.SetAndRunActProcess(pickedUpProcess);
            }
            public override void OnDeselected(){
                sb.Refresh();
                selStateHandler.Focus();
            }
            public override void OnPointerUp(){
                if(sb.IsHovered() && sb.IsStackable())
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
                SBAddProcess process = new SBAddProcess(sb, sb.GetAddCoroutine());
                sb.SetAndRunActProcess(process);
            }
        }
        public class SBRemovedState: SBActState{
            public SBRemovedState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBRemoveProcess process = new SBRemoveProcess(sb, sb.GetRemoveCoroutine());
                sb.SetAndRunActProcess(process);
            }
        }
        public class MoveWithinState: SBActState{
            public MoveWithinState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.GetMoveWithinCoroutine());
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
            public ISBEqpState GetEquippedState(){
                if(_equippedState == null)
                    _equippedState = new SBEquippedState(sb);
                return _equippedState;
            }
                ISBEqpState _equippedState;
            public ISBEqpState GetUnequippedState(){
                if(_unequippedState == null)
                    _unequippedState = new SBUnequippedState(sb);
                return _unequippedState;
            }
                ISBEqpState _unequippedState;
        }
        public interface ISBEqpStateRepo{
            ISBEqpState GetEquippedState();
            ISBEqpState GetUnequippedState();
        }
        public class SBEquippedState: SBEqpState{
            public SBEquippedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.IsHierarchySetUp())
                    return;
                if(sb.IsPool()){
                    if(eqpStateHandler.IsUnequipped()){
                        ISBEqpProcess process = new SBEquipProcess(sb, eqpStateHandler.GetEquipCoroutine());
                        eqpStateHandler.SetAndRunEqpProcess(process);
                    }
                }
            }
        }
        public class SBUnequippedState: SBEqpState{
            public SBUnequippedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.IsHierarchySetUp()) 
                    return;
                if(sb.IsPool()){
                    if(eqpStateHandler.IsEquipped()){
                        ISBEqpProcess process = new SBUnequipProcess(sb, eqpStateHandler.GetUnequipCoroutine());
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
            public ISBMrkState GetMarkedState(){
                if(_markedState == null)
                    _markedState = new SBMarkedState(sb);
                return _markedState;
            }
                ISBMrkState _markedState;
            public ISBMrkState GetUnmarkedState(){
                if(_unmarkedState == null)
                    _unmarkedState = new SBUnmarkedState(sb);
                return _unmarkedState;
            }
                ISBMrkState _unmarkedState;
        }
        public interface ISBMrkStateRepo{
            ISBMrkState GetMarkedState();
            ISBMrkState GetUnmarkedState();
        }
        public class SBMarkedState: SBMrkState{
            public SBMarkedState(ISlottable sb): base(sb){}
            public override void EnterState(){
                if(!sb.IsHierarchySetUp())
                    return;
                if(sb.IsPool()){
                    if(mrkStateHandler.IsUnmarked()){
                        ISBMrkProcess process = new SBMarkProcess(sb, mrkStateHandler.GetMarkCoroutine());
                        mrkStateHandler.SetAndRunMrkProcess(process);
                    }
                }
            }
        }
        public class SBUnmarkedState: SBMrkState{
            public SBUnmarkedState(ISlottable sb): base (sb){}
            public override void EnterState(){
                if(!sb.IsHierarchySetUp())
                    return;
                if(sb.IsPool()){
                    if(mrkStateHandler.IsMarked()){
                        ISBMrkProcess process = new SBUnmarkProcess(sb, mrkStateHandler.GetUnmarkCoroutine());
                        mrkStateHandler.SetAndRunMrkProcess(process);
                    }
                }
            }
        }
}
