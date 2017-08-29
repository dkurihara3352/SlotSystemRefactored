using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace UISystem{
	public abstract class SBState: UIState{
	}
    /* SelState */
        public abstract class SBSelState: SBState, ISBSelState{
            protected ISBSelStateHandler sbSelStateHandler;
            public SBSelState(ISBSelStateHandler selStateHandler){
                this.sbSelStateHandler = selStateHandler;
            }
        }
        public interface ISBSelState: IUIState{
        }
        public class SBSelStateRepo: ISBSelStateRepo{
            ISBSelStateHandler sbSelStateHandler;
            public SBSelStateRepo(ISBSelStateHandler handler){
                sbSelStateHandler = handler;
            }
            public ISBSelState SelectableState(){
                if(_selectableState == null)
                    _selectableState = new SBSelectableState(sbSelStateHandler);
                return _selectableState;
            }
                ISBSelState _selectableState;
            public ISBSelState UnselectableState(){
                if(_unselectableState == null) 
                    _unselectableState = new SBUnselectableState(sbSelStateHandler);
                return _unselectableState;
            }
                ISBSelState _unselectableState;
            public ISBSelState SelectedState(){
                if(_selectedState == null) 
                    _selectedState = new SBSelectedState(sbSelStateHandler);
                return _selectedState;
            }
                ISBSelState _selectedState;
        }
        public interface ISBSelStateRepo{
            ISBSelState SelectableState();
            ISBSelState UnselectableState();
            ISBSelState SelectedState();
        }
        public class SBSelectableState: SBSelState{
            public SBSelectableState(ISBSelStateHandler handler): base(handler){}
            public override void EnterState(){
                SBMakeSelectableProcess process = new SBMakeSelectableProcess(sbSelStateHandler.MakeSelectableCoroutine());
                sbSelStateHandler.SetAndRunSelProcess(process);
            }
        }
        public class SBUnselectableState: SBSelState{
            public SBUnselectableState(ISBSelStateHandler handler): base(handler){}
            public override void EnterState(){
                SBMakeUnselectableProcess process = new SBMakeUnselectableProcess(sbSelStateHandler.MakeUnselectableCoroutine());
                sbSelStateHandler.SetAndRunSelProcess(process);
            }
        }
        public class SBSelectedState: SBSelState{
            public SBSelectedState(ISBSelStateHandler handler): base(handler){}
            public override void EnterState(){
                SBSelectProcess process = new SBSelectProcess(sbSelStateHandler.SelectCoroutine());
                sbSelStateHandler.SetAndRunSelProcess(process);
            }
        }

    /* ActState */
        public abstract class SBActState: SBState, ISBActState{
            protected ISBActStateHandler actStateHandler;
            public SBActState(ISBActStateHandler actStateHandler){
                this.actStateHandler = actStateHandler;
            }
        }
        public interface ISBActState: IUIState{
        }
        public class SBActStateRepo: ISBActStateRepo{
            ISBActStateHandler actStateHandler;
            ISlottable sb;
            public SBActStateRepo(ISlottable sb){
                this.actStateHandler = sb.ActStateHandler();
                this.sb = sb;
            }
            public ISBActState WaitingForActionState(){
                if(_waitingForActionState == null)
                    _waitingForActionState = new SBWaitingForActionState(actStateHandler);
                return _waitingForActionState;
            }
                ISBActState _waitingForActionState;
            public ISBActState TravellingState(){
                if(_travellingState == null)
                    _travellingState = new SBTravellingState(actStateHandler);
                return _travellingState;
            }
                ISBActState _travellingState;
            public ISBActState LiftingState(){
                if(_liftingState == null)
                    _liftingState = new SBLiftingState(actStateHandler);
                return _liftingState;
            }
                ISBActState _liftingState;
            public ISBActState LandingState(){
                if(_landingState == null)
                    _landingState = new SBLandingState(actStateHandler);
                return _landingState;
            }
                ISBActState _landingState;
            public ISBActState AppearingState(){
                if(_appearingState == null)
                    _appearingState = new SBAppearingState(actStateHandler);
                return _appearingState;
            }
                ISBActState _appearingState;
            public ISBActState DisappearingState(){
                if(_disappearingState == null)
                    _disappearingState = new SBDisappearingState(actStateHandler);
                return _disappearingState;
            }
                ISBActState _disappearingState;
        }
        public interface ISBActStateRepo{
            ISBActState WaitingForActionState();
            ISBActState TravellingState();
            ISBActState LiftingState();
            ISBActState LandingState();
            ISBActState AppearingState();
            ISBActState DisappearingState();
        }
        public class SBWaitingForActionState: SBActState{
            public SBWaitingForActionState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBWaitForActionProcess process = new SBWaitForActionProcess(actStateHandler.WaitForActionCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        public class SBTravellingState: SBActState{
            public SBTravellingState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBTravelProcess process = new SBTravelProcess(actStateHandler.TravelCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        public class SBLiftingState: SBActState{
            public SBLiftingState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBLiftProcess process = new SBLiftProcess(actStateHandler.LiftCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        public class SBLandingState: SBActState{
            public SBLandingState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBLandProcess process = new SBLandProcess(actStateHandler.LandCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        public class SBAppearingState: SBActState{
            public SBAppearingState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBAppearProcess process = new SBAppearProcess(actStateHandler.AppearCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        public class SBDisappearingState: SBActState{
            public SBDisappearingState(ISBActStateHandler actStateHandler): base(actStateHandler){}
            public override void EnterState(){
                SBDisappearProcess process = new SBDisappearProcess(actStateHandler.DisappearCoroutine());
                actStateHandler.SetAndRunActProcess(process);
            }
        }
        
    /* EqpState */
        public abstract class SBEqpState: SBState, ISBEqpState{
            protected ISBEqpStateHandler eqpStateHandler;
            protected ISBEquipToolHandler equipToolHandler;
            public SBEqpState(ISlottable sb){
                ISBToolHandler sbToolHandler = sb.GetToolHandler();
                Debug.Assert((sbToolHandler is ISBEquipToolHandler));
                equipToolHandler = (ISBEquipToolHandler)sb.GetToolHandler();
                this.eqpStateHandler = equipToolHandler.GetEqpStateHandler();
            }
        }
        public interface ISBEqpState: IUIState{}
        public class SBEqpStateRepo: ISBEqpStateRepo{
            ISlottable sb;
            public SBEqpStateRepo(ISlottable sb){
                this.sb = sb;
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
                if(equipToolHandler.IsPool()){
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
                if(equipToolHandler.IsPool()){
                    if(eqpStateHandler.IsEquipped()){
                        ISBEqpProcess process = new SBUnequipProcess(eqpStateHandler.GetUnequipCoroutine());
                        eqpStateHandler.SetAndRunEqpProcess(process);
                    }
                }
            }
        }
}
