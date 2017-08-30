﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
    /* ActState */
	public abstract class SlotActState: UIState, ISlotActState{
		protected ISlotActStateHandler actStateHandler;
		public SlotActState(ISlotActStateHandler handler){
			actStateHandler = handler;
		}
		public virtual void OnPointerDown(){}
		public virtual void OnPointerUp(){}
		public virtual void OnEndDrag(){}
		public virtual void OnDeselected(){}
	}
	public interface ISlotActState: IUIState{
		void OnPointerDown();
		void OnPointerUp();
		void OnEndDrag();
		void OnDeselected();
	}
	public class WaitForActionState: SlotActState{//up state
		IUISelStateHandler selStateHandler;
		public WaitForActionState(ISlot slot): base(slot.ActStateHandler()){
			this.selStateHandler = slot.SelStateHandler();
		}
		public override void EnterState(){
			if(!actStateHandler.WasActStateNull())
				actStateHandler.SetAndRunActProcess(null);
		}
		public override void OnPointerDown(){
			if(selStateHandler.IsSelectable()){
				selStateHandler.Select();
				actStateHandler.WaitForPickUp();
			}
			else
				actStateHandler.WaitForPointerUp();
		}
	}
    public class WaitForPickUpState: SlotActState{//down state
        IUISelStateHandler selStateHandler;
        ISlot slot;
        public WaitForPickUpState(ISlot slot): base(slot.ActStateHandler()){
            this.selStateHandler = slot.SelStateHandler();
            this.slot = slot;
        }
        public override void EnterState(){
            if(actStateHandler.WasWaitingForAction()){
                ISlotActProcess wfpuProcess = new WaitForPickUpProcess(actStateHandler, actStateHandler.WaitForPickUpCoroutine());
                actStateHandler.SetAndRunActProcess(wfpuProcess);
            }else
                throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
        }
        public override void OnPointerUp(){
            actStateHandler.WaitForNextTouch();
        }
        public override void OnEndDrag(){
            slot.Refresh();                    
            selStateHandler.MakeSelectable();
        }
    }
    public class WaitForPointerUpState: SlotActState{//down state
        IUISelStateHandler selStateHandler;
        ISlot slot;
        public WaitForPointerUpState(ISlot slot): base(slot.ActStateHandler()){
            this.selStateHandler = slot.SelStateHandler();
            this.slot = slot;
        }
        public override void EnterState(){
            if(actStateHandler.WasWaitingForAction()){
                ISlotActProcess wfPtuProcess = new WaitForPointerUpProcess(selStateHandler, actStateHandler.WaitForPointerUpCoroutine());
                actStateHandler.SetAndRunActProcess(wfPtuProcess);
            }else
                throw new InvalidOperationException("cannot enter this state from anything other than WaitForActionState");
        }
        public override void OnPointerUp(){
            slot.Tap();
            slot.Refresh();
            selStateHandler.MakeUnselectable();
        }
        public override void OnEndDrag(){
            slot.Refresh();
            selStateHandler.MakeUnselectable();
        }
    }
        public class WaitForNextTouchState: SlotActState{//up state
            IUISelStateHandler selStateHandler;
            ISlot slot;
            public WaitForNextTouchState(ISlot slot): base(slot.ActStateHandler()){
                this.selStateHandler = slot.SelStateHandler();
                this.slot = slot;
            }
            public override void EnterState(){
                if(actStateHandler.WasPickingUp() || actStateHandler.WasWaitingForPickUp()){
                    ISlotActProcess wfntProcess = new WaitForNextTouchProcess(slot, actStateHandler.WaitForNextTouchCoroutine());
                    actStateHandler.SetAndRunActProcess(wfntProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than PickingUpState or WaitForPickUpState");
            }
            public override void OnPointerDown(){
                if(!slot.IsPickedUp())
                    actStateHandler.PickUp();
                else{
                    slot.Increment();
                }
            }
            public override void OnDeselected(){
                slot.Refresh();
                selStateHandler.MakeSelectable();
            }
        }
        public class PickingUpState: SlotActState{//down state
            IUISelStateHandler selStateHandler;
            ISlot slot;
            public PickingUpState(ISlot slot): base(slot.ActStateHandler()){
                selStateHandler = slot.SelStateHandler();
                this.slot = slot;
            }
            public override void EnterState(){
                if(actStateHandler.WasWaitingForPickUp() || actStateHandler.WasWaitingForNextTouch()){
                    slot.HoverEnter();

                    ISlotActProcess pickedUpProcess = new PickUpProcess(actStateHandler.PickUpCoroutine());
                    actStateHandler.SetAndRunActProcess(pickedUpProcess);
                }else
                    throw new InvalidOperationException("cannot enter this state from anything other than WaitForPickUpState or WaitForNextTouchState");
            }
            public override void OnDeselected(){
                slot.Refresh();
                selStateHandler.MakeSelectable();
            }
            public override void OnPointerUp(){
                if(slot.IsHovered() && slot.HasItemAndIsStackable())
                    actStateHandler.WaitForNextTouch();
            }
            public override void OnEndDrag(){

            }
        }
        public class SlotActStateRepo: ISlotActStateRepo{
            ISlotActStateHandler actStateHandler;
            ISlot slot;
            public SlotActStateRepo(ISlot slot){
                this.actStateHandler = slot.ActStateHandler();
                this.slot = slot;
            }
            public ISlotActState GetWaitForActionState(){
                if(_waitForActionState == null)
                    _waitForActionState = new WaitForActionState(slot);
                return _waitForActionState;
            }
                ISlotActState _waitForActionState;
            public ISlotActState GetWaitForPickUpState(){
                if(_waitForPickUpState == null)
                    _waitForPickUpState = new WaitForPickUpState(slot);
                return _waitForPickUpState;
            }
                ISlotActState _waitForPickUpState;
            public ISlotActState GetWaitForPointerUpState(){
                if(_waitForPointerUpState == null)
                    _waitForPointerUpState = new WaitForPointerUpState(slot);
                return _waitForPointerUpState;
            }
                ISlotActState _waitForPointerUpState;
            public ISlotActState GetWaitForNextTouchState(){
                if(_waitForNextTouchState == null)
                    _waitForNextTouchState = new WaitForNextTouchState(slot);
                return _waitForNextTouchState;
            }
                ISlotActState _waitForNextTouchState;
            public ISlotActState GetPickingUpState(){
                if(_pickingUpState == null)
                    _pickingUpState = new PickingUpState(slot);
                return _pickingUpState;
            }
                ISlotActState _pickingUpState;
        }
        public interface ISlotActStateRepo{
            ISlotActState GetWaitForActionState();
            ISlotActState GetWaitForPickUpState();
            ISlotActState GetWaitForPointerUpState();
            ISlotActState GetWaitForNextTouchState();
            ISlotActState GetPickingUpState();
        }
}
