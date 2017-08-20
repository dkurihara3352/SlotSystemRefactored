using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace SlotSystem{
	public abstract class SBState: SSEState{
	}
    /* ActState */
        public abstract class SBActState: SBState, ISBActState{
            protected ISBActStateHandler actStateHandler;
            public SBActState(ISBActStateHandler actStateHandler){
                this.actStateHandler = actStateHandler;
            }
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
            ISlottable sb;
            public SBActStateRepo(ISlottable sb, ITransactionManager tam){
                this.tam = tam;
                this.taCache = sb.GetTAC();
                this.actStateHandler = sb.GetActStateHandler();
                this.sb = sb;
            }
            public ISBActState GetWaitForActionState(){
                if(_waitForActionState == null)
                    _waitForActionState = new WaitForActionState(sb);
                return _waitForActionState;
            }
                ISBActState _waitForActionState;
            public ISBActState GetWaitForPickUpState(){
                if(_waitForPickUpState == null)
                    _waitForPickUpState = new WaitForPickUpState(sb);
                return _waitForPickUpState;
            }
                ISBActState _waitForPickUpState;
            public ISBActState GetWaitForPointerUpState(){
                if(_waitForPointerUpState == null)
                    _waitForPointerUpState = new WaitForPointerUpState(sb);
                return _waitForPointerUpState;
            }
                ISBActState _waitForPointerUpState;
            public ISBActState GetWaitForNextTouchState(){
                if(_waitForNextTouchState == null)
                    _waitForNextTouchState = new WaitForNextTouchState(sb, tam);
                return _waitForNextTouchState;
            }
                ISBActState _waitForNextTouchState;
            public ISBActState GetPickingUpState(){
                if(_pickingUpState == null)
                    _pickingUpState = new PickingUpState(sb, tam);
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
            public WaitForActionState(ISlottable sb): base(sb.GetActStateHandler()){
                this.selStateHandler = sb.GetSelStateHandler();
            }
            public override void EnterState(){
                if(!actStateHandler.WasActStateNull())
                    actStateHandler.SetAndRunActProcess(null);
            }
            public override void OnPointerDown(){
                if(selStateHandler.IsFocused()){
                    selStateHandler.Select();
                    actStateHandler.WaitForPickUp();
                }
                else
                    actStateHandler.WaitForPointerUp();
            }
        }
        public class WaitForPickUpState: SBActState{//down state
            ISSESelStateHandler selStateHandler;
            ISlottable sb;
            public WaitForPickUpState(ISlottable sb): base(sb.GetActStateHandler()){
                this.selStateHandler = sb.GetSelStateHandler();
                this.sb = sb;
            }
            public override void EnterState(){
                if(actStateHandler.WasWaitingForAction()){
                    ISBActProcess wfpuProcess = new WaitForPickUpProcess(actStateHandler, actStateHandler.GetWaitForPickUpCoroutine());
                    actStateHandler.SetAndRunActProcess(wfpuProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
            }
            public override void OnPointerUp(){
                actStateHandler.WaitForNextTouch();
            }
            public override void OnEndDrag(){
                sb.Refresh();                    
                selStateHandler.Focus();
            }
        }
        public class WaitForPointerUpState: SBActState{//down state
            ISSESelStateHandler selStateHandler;
            ISlottable sb;
            public WaitForPointerUpState(ISlottable sb): base(sb.GetActStateHandler()){
                this.selStateHandler = sb.GetSelStateHandler();
                this.sb = sb;
            }
            public override void EnterState(){
                if(actStateHandler.WasWaitingForAction()){
                    ISBActProcess wfPtuProcess = new WaitForPointerUpProcess(selStateHandler, actStateHandler.GetWaitForPointerUpCoroutine());
                    actStateHandler.SetAndRunActProcess(wfPtuProcess);
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
            ISlottable sb;
            public WaitForNextTouchState(ISlottable sb, ITransactionManager tam): base(sb.GetActStateHandler()){
                this.tam = tam;
                this.selStateHandler = sb.GetSelStateHandler();
                this.sb = sb;
            }
            public override void EnterState(){
                if(actStateHandler.WasPickingUp() || actStateHandler.WasWaitingForPickUp()){
                    ISBActProcess wfntProcess = new WaitForNextTouchProcess(sb, tam, actStateHandler.GetWaitForNextTouchCoroutine());
                    actStateHandler.SetAndRunActProcess(wfntProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than PickingUpState or WaitForPickUpState");
            }
            public override void OnPointerDown(){
                if(!sb.IsPickedUp())
                    actStateHandler.PickUp();
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
            IHoverable sbHoverable;
            IItemHandler itemHandler;
            ISlottable sb;
            public PickingUpState(ISlottable sb, ITransactionManager tam): base(sb.GetActStateHandler()){
                this.tam = tam;
                iconHandler = tam.GetIconHandler();
                _taCache = sb.GetTAC();
                tamStateHandler = tam.GetActStateHandler();
                selStateHandler = sb.GetSelStateHandler();
                sbHoverable = sb.GetHoverable();
                itemHandler = sb.GetItemHandler();
                this.sb = sb;
            }
            public override void EnterState(){
                if(actStateHandler.WasWaitingForPickUp() || actStateHandler.WasWaitingForNextTouch()){
                    sbHoverable.OnHoverEnter();
                    taCache.SetPickedSB(sb);
                    iconHandler.SetDIcon1(sb);
                    tamStateHandler.Probe();
                    taCache.CreateTransactionResults();
                    taCache.UpdateFields();
                    ISBActProcess pickedUpProcess = new PickUpProcess(actStateHandler.GetPickUpCoroutine());
                    actStateHandler.SetAndRunActProcess(pickedUpProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForPickUpState or WaitForNextTouchState");
            }
            public override void OnDeselected(){
                sb.Refresh();
                selStateHandler.Focus();
            }
            public override void OnPointerUp(){
                if(sbHoverable.IsHovered() && itemHandler.IsStackable())
                    actStateHandler.WaitForNextTouch();
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
                SBAddProcess process = new SBAddProcess(actStateHandler.GetAddCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        public class SBRemovedState: SBActState{
            public SBRemovedState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBRemoveProcess process = new SBRemoveProcess(actStateHandler.GetRemoveCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        public class MoveWithinState: SBActState{
            public MoveWithinState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBMoveWithinProcess process = new SBMoveWithinProcess(actStateHandler.GetMoveWithinCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    /* EqpState */
        public abstract class SBEqpState: SBState, ISBEqpState{
            protected ISBEqpStateHandler eqpStateHandler;
            public SBEqpState(ISlottable sb){
                this.eqpStateHandler = sb.GetEqpStateHandler();
            }
        }
        public interface ISBEqpState: ISSEState{}
        public class SBEqpStateRepo: SBStateRepo, ISBEqpStateRepo{
            ISlottable sb;
            protected ISBEqpStateHandler eqpStateHandler;
            public SBEqpStateRepo(ISlottable sb){
                this.sb = sb;
                this.eqpStateHandler = sb.GetEqpStateHandler();
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
            ISlottable sb;
            public SBEquippedState(ISlottable sb): base(sb){
                this.sb = sb;
            }
            public override void EnterState(){
                if(!sb.IsHierarchySetUp())
                    return;
                if(sb.IsPool()){
                    if(eqpStateHandler.IsUnequipped()){
                        ISBEqpProcess process = new SBEquipProcess(eqpStateHandler.GetEquipCoroutine());
                        eqpStateHandler.SetAndRunEqpProcess(process);
                    }
                }
            }
        }
        public class SBUnequippedState: SBEqpState{
            ISlottable sb;
            public SBUnequippedState(ISlottable sb): base(sb){
                this.sb = sb;
            }
            public override void EnterState(){
                if(!sb.IsHierarchySetUp()) 
                    return;
                if(sb.IsPool()){
                    if(eqpStateHandler.IsEquipped()){
                        ISBEqpProcess process = new SBUnequipProcess(eqpStateHandler.GetUnequipCoroutine());
                        eqpStateHandler.SetAndRunEqpProcess(process);
                    }
                }
            }
        }
    /* MrkState */
        public abstract class SBMrkState: SBState, ISBMrkState{
            protected ISBMrkStateHandler mrkStateHandler;
            public SBMrkState(ISlottable sb){
                mrkStateHandler = sb.GetMrkStateHandler();
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
            ISlottable sb;
            public SBMarkedState(ISlottable sb): base(sb){
                this.sb = sb;
            }
            public override void EnterState(){
                if(!sb.IsHierarchySetUp())
                    return;
                if(sb.IsPool()){
                    if(mrkStateHandler.IsUnmarked()){
                        ISBMrkProcess process = new SBMarkProcess(mrkStateHandler.GetMarkCoroutine());
                        mrkStateHandler.SetAndRunMrkProcess(process);
                    }
                }
            }
        }
        public class SBUnmarkedState: SBMrkState{
            ISlottable sb;
            public SBUnmarkedState(ISlottable sb): base (sb){
                this.sb = sb;
            }
            public override void EnterState(){
                if(!sb.IsHierarchySetUp())
                    return;
                if(sb.IsPool()){
                    if(mrkStateHandler.IsMarked()){
                        ISBMrkProcess process = new SBUnmarkProcess(mrkStateHandler.GetUnmarkCoroutine());
                        mrkStateHandler.SetAndRunMrkProcess(process);
                    }
                }
            }
        }
}
