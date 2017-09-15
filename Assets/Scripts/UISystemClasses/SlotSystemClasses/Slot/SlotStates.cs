﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace UISystem{
    public class SlotSelStateRepo: UISelStateRepo{
        public override void InitializeStates(){
            SetHiddenState( new SlotHiddenState( engine, element));
        }
    }
        public class SlotHiddenState: UISelectionState{
            ISlot slot;
            public SlotHiddenState( IUISelStateEngine engine, IUIElement element): base( engine){
                this.slot = (ISlot)element;
            }
            public override void Enter(){
                RunSlotHideProcess();
                if(engine.WasDeactivated())
                    engine.ExpireProcess();
            }
            void RunSlotHideProcess(){
                engine.SetAndRunSelProcess( new SlotHideProcess(engine.HideCoroutine(), slot));
            }
        }
        
    /* Slot Act State */
        public interface ISlotActState: IUIState{
        }
        public abstract class SlotActState: ISlotActState{
            protected ISlotActStateEngine engine;
            public SlotActState(ISlotActStateEngine engine){
                this.engine = engine;
            }
            public virtual bool CanEnter(){return false;}
            public virtual void Enter(){}
            public virtual void Exit(){}

            protected void RunWaitForActionProcess(){
                engine.SetAndRunActProcess(new SlotWaitForActionProcess(engine.WaitForActionCoroutine(), engine));
            }
            protected void RunWaitForPickUpProcess(){
                engine.SetAndRunActProcess(new SlotWaitForPickUpProcess(engine.WaitForPickUpCoroutine(), engine));
            }
            protected void RunWaitForPointerUpProcess(){
                engine.SetAndRunActProcess(new SlotWaitForPointerUpProcess(engine.WaitForPointerUpCoroutine(), engine));
            }
            protected void RunWaitForNextTouchProcess(){
                engine.SetAndRunActProcess(new SlotWaitForNextTouchProcess(engine.WaitForNextTouchCoroutine(), engine));
            }
        }
        public class SlotWaitingForActionState: SlotActState, IUIPointerUpState{
            public SlotWaitingForActionState(ISlotActStateEngine engine): base(engine){}
            public override bool CanEnter(){
                if(engine.IsWaitingForAction())
                    return false;
                else
                    return true;
            }
            public override void Enter(){
                RunWaitForActionProcess();
            }
            public void OnPointerDown(){
                engine.WaitForPickUp();
            }
            public void OnDeselected(){
            }
        }
        public class SlotWaitingForPickUpState: SlotActState, IUIPointerDownState{
            public SlotWaitingForPickUpState(ISlotActStateEngine engine): base(engine){}
            public override bool CanEnter(){
                if(engine.IsWaitingForPickUp())
                    return false;
                else if(engine.IsWaitingForAction())
                    return true;
                else
                    return false;
            }
            public override void Enter(){
                RunWaitForPickUpProcess();
            }
            public void OnPointerUp(){
                engine.WaitForNextTouch();
            }
            public void OnEndDrag(){
                engine.WaitForAction();
            }
        }
        public class SlotWaitingForPointerUpState: SlotActState, IUIPointerDownState{
            public SlotWaitingForPointerUpState(ISlotActStateEngine engine): base(engine){}
            public override bool CanEnter(){
                if(engine.IsWaitingForPointerUp())
                    return false;
                else if(engine.IsWaitingForPickUp())
                    return true;
                else
                    return false;
            }
            public override void Enter(){
                RunWaitForPointerUpProcess();
            }
            public void OnPointerUp(){
                engine.WaitForAction();
            }
            public void OnEndDrag(){
                engine.WaitForAction();
            }
        }
        public class SlotWaitingForNextTouchState: SlotActState, IUIPointerUpState{
            public SlotWaitingForNextTouchState(ISlotActStateEngine engine): base(engine){}
            public override bool CanEnter(){
                if(engine.IsWaitingForNextTouch())
                    return false;
                else if(engine.IsWaitingForPickUp())
                    return true;
                else
                    return false;
            }
            public override void Enter(){
                RunWaitForNextTouchProcess();
            }
            public void OnPointerDown(){
                engine.PickUp();
                engine.WaitForAction();
            }
            public void OnDeselected(){
                engine.WaitForAction();
            }
        }
    /* Slot Fade State */
        public interface ISlotFadeState: IUIState{
        }
        public abstract class SlotFadeState: ISlotFadeState{
            protected ISlotFadeStateEngine engine;
        }
}
