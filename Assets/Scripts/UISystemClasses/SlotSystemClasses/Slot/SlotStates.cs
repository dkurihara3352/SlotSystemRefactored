using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace UISystem{
    public class SlotSelStateRepo: UISelStateRepo{
        public override void InitializeStates(){
            SetHiddenState( new SlotHiddenState( handler, element));
        }
    }
        public class SlotHiddenState: UISelectionState{
            ISlot slot;
            public SlotHiddenState( IUISelStateHandler handler, IUIElement element): base( handler){
                this.slot = (ISlot)element;
            }
            public override void Enter(){
                RunSlotHideProcess();
                if(handler.WasDeactivated())
                    handler.ExpireProcess();
            }
            void RunSlotHideProcess(){
                handler.SetAndRunSelProcess( new SlotHideProcess(handler.HideCoroutine(), slot));
            }
        }
        
    /* Slot Act State */
        public interface ISlotActState: IUIState{
        }
        public abstract class SlotActState: ISlotActState{
            protected ISlotActStateHandler handler;
            public SlotActState(ISlotActStateHandler handler){
                this.handler = handler;
            }
            public virtual bool CanEnter(){return false;}
            public virtual void Enter(){}
            public virtual void Exit(){}

            protected void RunWaitForActionProcess(){
                handler.SetAndRunActProcess(new SlotWaitForActionProcess(handler.WaitForActionCoroutine(), handler));
            }
            protected void RunWaitForPickUpProcess(){
                handler.SetAndRunActProcess(new SlotWaitForPickUpProcess(handler.WaitForPickUpCoroutine(), handler));
            }
            protected void RunWaitForPointerUpProcess(){
                handler.SetAndRunActProcess(new SlotWaitForPointerUpProcess(handler.WaitForPointerUpCoroutine(), handler));
            }
            protected void RunWaitForNextTouchProcess(){
                handler.SetAndRunActProcess(new SlotWaitForNextTouchProcess(handler.WaitForNextTouchCoroutine(), handler));
            }
        }
        public class SlotWaitingForActionState: SlotActState, IUIPointerUpState{
            public SlotWaitingForActionState(ISlotActStateHandler handler): base(handler){}
            public override bool CanEnter(){
                if(handler.IsWaitingForAction())
                    return false;
                else
                    return true;
            }
            public override void Enter(){
                RunWaitForActionProcess();
            }
            public void OnPointerDown(){
                handler.WaitForPickUp();
            }
            public void OnDeselected(){
            }
        }
        public class SlotWaitingForPickUpState: SlotActState, IUIPointerDownState{
            public SlotWaitingForPickUpState(ISlotActStateHandler handler): base(handler){}
            public override bool CanEnter(){
                if(handler.IsWaitingForPickUp())
                    return false;
                else if(handler.IsWaitingForAction())
                    return true;
                else
                    return false;
            }
            public override void Enter(){
                RunWaitForPickUpProcess();
            }
            public void OnPointerUp(){
                handler.WaitForNextTouch();
            }
            public void OnEndDrag(){
                handler.WaitForAction();
            }
        }
        public class SlotWaitingForPointerUpState: SlotActState, IUIPointerDownState{
            public SlotWaitingForPointerUpState(ISlotActStateHandler handler): base(handler){}
            public override bool CanEnter(){
                if(handler.IsWaitingForPointerUp())
                    return false;
                else if(handler.IsWaitingForPickUp())
                    return true;
                else
                    return false;
            }
            public override void Enter(){
                RunWaitForPointerUpProcess();
            }
            public void OnPointerUp(){
                handler.WaitForAction();
            }
            public void OnEndDrag(){
                handler.WaitForAction();
            }
        }
        public class SlotWaitingForNextTouchState: SlotActState, IUIPointerUpState{
            public SlotWaitingForNextTouchState(ISlotActStateHandler handler): base(handler){}
            public override bool CanEnter(){
                if(handler.IsWaitingForNextTouch())
                    return false;
                else if(handler.IsWaitingForPickUp())
                    return true;
                else
                    return false;
            }
            public override void Enter(){
                RunWaitForNextTouchProcess();
            }
            public void OnPointerDown(){
                handler.PickUp();
                handler.WaitForAction();
            }
            public void OnDeselected(){
                handler.WaitForAction();
            }
        }
    /* Slot Swap State */
        public interface ISlotSwapState: IUIState{
        }
}
