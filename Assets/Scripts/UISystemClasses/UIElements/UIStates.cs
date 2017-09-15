using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UISystem{
	/* StateEngine */
		public class UIStateSwitch<T>: SwitchableStateSwitch<T>, IUIStateSwitch<T> where T: IUIState{
		}
		public interface IUIStateSwitch<T>: ISwitchableStateSwitch<T> where T: IUIState{}
	/* StateRepo */
		public interface IUISelStateRepo{
			void InitializeFields(IUIElement element);
			void InitializeStates();
			IUISelectionState DeactivatedState();
			IUISelectionState ActivatedState();
			IUISelectionState HiddenState();
			IUISelectionState ShownState();
			IUISelectionState SelectedState();
			IUISelectionState DeselectedState();
			IUISelectionState SelectableState();
			IUISelectionState UnselectableState();
		}
		public abstract class UISelStateRepo: IUISelStateRepo{
			protected IUIElement element;
			protected IUISelStateEngine engine;
			public void InitializeFields(IUIElement element){
				this.element = element;
				this.engine = element.SelStateHandler();
			}
			public abstract void InitializeStates();
			public IUISelectionState DeactivatedState(){
				return _deactivatedState;
			}
			protected void SetDeactivatedState(IUISelectionState state){
				_deactivatedState = state;
			}
				IUISelectionState _deactivatedState;
			
			public IUISelectionState ActivatedState(){
				return _activatedState;
			}
			protected void SetActivatedState(IUISelectionState state){
				_activatedState = state;
			}
				IUISelectionState _activatedState;
			
			public IUISelectionState HiddenState(){
				return _hiddenState;
			}
			protected void SetHiddenState(IUISelectionState state){
				_hiddenState = state;
			}
				IUISelectionState _hiddenState;
			
			public IUISelectionState ShownState(){
				return _shownState;
			}
			protected void SetShownState(IUISelectionState state){
				_shownState = state;
			}
				IUISelectionState _shownState;
			
			public IUISelectionState SelectedState(){
				return _selectedState;
			}
			protected void SetSelectedState(IUISelectionState state){
				_selectedState = state;
			}
				IUISelectionState _selectedState;
			
			public IUISelectionState DeselectedState(){
				return _deselectedState;
			}
			protected void SetDeselectedState(IUISelectionState state){
				_deselectedState = state;
			}
				IUISelectionState _deselectedState;
			
			public IUISelectionState SelectableState(){
				return _selectableState;
			}
			protected void SetSelectableState(IUISelectionState state){
				_selectableState = state;
			}
				IUISelectionState _selectableState;
			
			public IUISelectionState UnselectableState(){
				return _unselectableState;
			}
			protected void SetUnselectableState(IUISelectionState state){
				_unselectableState = state;
			}
				IUISelectionState _unselectableState;
			
		}
		public class UIDefaultSelStateRepo: UISelStateRepo{
			public override void InitializeStates(){
				SetDeactivatedState(new UIDeactivatedState(engine));
				SetActivatedState(new UIActivatedState(element, engine));
				SetHiddenState(new UIHiddenState(engine));
				SetShownState(new UIShownState(engine));
				SetSelectedState(new UISelectedState(engine));
				SetDeselectedState(new UIDeselectedState(engine));
				SetSelectableState(new UISelectableState(engine));
				SetUnselectableState(new UIUnselectableState(engine));
			}
		}
	/* State */
		public interface IUIState: ISwitchableState{}// for engine use
	/* Selection State */
		public interface IUISelectionState: IUIState{
		}
		public abstract class UISelectionState: IUISelectionState{
			protected IUISelStateEngine engine;
			public UISelectionState(IUISelStateEngine engine){
				this.engine = engine;
			}
			public virtual void Enter(){}
			public virtual void Exit(){}
			public virtual bool CanEnter(){
				return false;
			}
			protected void RunDeactivateProcess(){
				engine.SetAndRunSelProcess(new UIDeactivateProcess(engine.DeactivateCoroutine()));
			}
			protected void RunHideProcess(){
				engine.SetAndRunSelProcess(new UIHideProcess(engine.HideCoroutine()));
			}
			protected void RunSelectProcess(){
				engine.SetAndRunSelProcess(new UISelectProcess(engine.SelectCoroutine()));
			}
			protected void RunMakeSelectableProcess(){
				engine.SetAndRunSelProcess(new UIMakeSelectableProcess(engine.MakeSelectableCoroutine()));
			}
			protected void RunMakeUnselectableProcess(){
				engine.SetAndRunSelProcess(new UIMakeUnselectableProcess(engine.MakeUnselectableCoroutine()));
			}
		}
			public class UIDeactivatedState: UISelectionState{
				public UIDeactivatedState(IUISelStateEngine engine): base(engine){}
				public override bool CanEnter(){
					if(engine.IsDeactivated())
						return false;
					else
						return true;
				}
				public override void Enter(){
					RunDeactivateProcess();
				}
			}
			public class UIActivatedState: UISelectionState, IRelayState{
				IUIElement element;
				public UIActivatedState(IUIElement element, IUISelStateEngine engine): base(engine){
					this.element = element;
				}
				public override bool CanEnter(){
					if(engine.IsActivated())
						return false;
					else
						return true;
				}
				public override void Enter(){
					if(element.IsShownOnActivation())
						engine.Show();
				}
			}
			public class UIHiddenState: UISelectionState{
				public UIHiddenState(IUISelStateEngine engine): base(engine){}
				public override bool CanEnter(){
					if(engine.IsHidden())
						return false;
					else
						return true;
				}
				public override void Enter(){
					RunHideProcess();
					if(engine.WasDeactivated())
						engine.ExpireProcess();
				}
			}
			public class UIShownState: UISelectionState, IRelayState{
				public UIShownState(IUISelStateEngine engine): base(engine){
				}
				public override bool CanEnter(){
					if(engine.IsShown())
						return false;
					else
						return true;
				}
				public override void Enter(){
					engine.Deselect();
				}
			}
			public class UISelectedState : UISelectionState{
				public UISelectedState(IUISelStateEngine engine): base(engine){}
				public override bool CanEnter(){
					if(engine.IsSelected())
						return false;
					else
						return true;
				}
				public override void Enter(){
					RunSelectProcess();
					if(engine.WasDeactivated())
						engine.ExpireProcess();
				}
			}
			public class UIDeselectedState: UISelectionState, IRelayState{
				public UIDeselectedState(IUISelStateEngine engine): base(engine){}
				public override bool CanEnter(){
					if(engine.IsDeselected())
						return false;
					else
						return true;
				}
				public override void Enter(){
					engine.MakeSelectable();
				}
			}
			public class UISelectableState: UISelectionState{
				public UISelectableState(IUISelStateEngine engine): base(engine){}
				public override bool CanEnter(){
					if(engine.IsSelectable())
						return false;
					else
						return true;
				}
				public override void Enter(){
					RunMakeSelectableProcess();
					if(engine.WasDeactivated())
						engine.ExpireProcess();
				}
			}
			public class UIUnselectableState: UISelectionState{
				public UIUnselectableState(IUISelStateEngine engine): base(engine){}
				public override bool CanEnter(){
					if(engine.IsUnselectable())
						return false;
					else 
						return true;
				}
				public override void Enter(){
					RunMakeUnselectableProcess();
					if(engine.WasDeactivated())
						engine.ExpireProcess();
				}
			}
	/* TapState */
		// public interface IUIInputState: IUIState{
		// 	void OnPointerDown();
		// 	void OnPointerUp();
		// 	void OnEndDrag();
		// 	void OnDeselected();
		// }
		public interface IUIPointerDownState: IUIState{
			void OnPointerUp();
			void OnEndDrag();
		}
		public interface IUIPointerUpState: IUIState{
			void OnPointerDown();
			void OnDeselected();
		}
		public interface IUITapState: IUIState{
		}
		public abstract class UITapState: IUITapState{
			protected ITapStateEngine engine;
			public UITapState(ITapStateEngine engine){
				this.engine = engine;
			}
			public virtual void Enter(){}
			public virtual void Exit(){}
			public virtual bool CanEnter(){return false;}

			protected void RunWaitForPointerDownProcess(){
				engine.SetAndRunTapProcess(new UIWaitForTapPointerDownProcess(engine.WaitForTapPointerDownCoroutine(), engine));
			}
			protected void RunWaitForTapTimerUpProcess(){
				engine.SetAndRunTapProcess(new UIWaitForTapTimerUpProcess(engine.WaitForTapTimerUpCoroutine(), engine));
			}
			protected void RunWaitForTapPointerUpProcess(){
				engine.SetAndRunTapProcess(new UIWaitForTapPointerUpProcess(engine.WaitForTapPointerUpCoroutine(), engine));
			}
			protected void RunTapProcess(){
				engine.SetAndRunTapProcess(new UITapProcess(engine.TapCoroutine(), engine));
			}
		}
		public class UIWaitingForTapPointerDownState: UITapState, IUIPointerUpState{
			public UIWaitingForTapPointerDownState(ITapStateEngine engine): base(engine){
			}
			public override bool CanEnter(){
				if(engine.IsWaitingForTapPointerDown())
					return false;
				else
					return true;
			}
			public override void Enter(){
				RunWaitForPointerDownProcess();
			}
			public void OnPointerDown(){
				engine.WaitForTapTimerUp();
			}
			public void OnDeselected(){}
		}
		public class UIWaitingForTapTimerUpState: UITapState, IUIPointerDownState{
			public UIWaitingForTapTimerUpState(ITapStateEngine engine): base(engine){
			}
			public override bool CanEnter(){
				if(engine.IsWaitingForTapTimerUp())
					return false;
				else if(engine.IsWaitingForTapPointerDown())
					return true;
				else
					return false;
			}
			public override void Enter(){
				RunWaitForTapTimerUpProcess();
			}
			public void OnPointerUp(){
				engine.Tap();
			}
			public void OnEndDrag(){
				engine.WaitForTapPointerDown();
			}
		}
		public class UIWaitingForTapPointerUpState: UITapState, IUIPointerDownState{
			public UIWaitingForTapPointerUpState(ITapStateEngine engine): base(engine){
			}
			public override bool CanEnter(){
				if(engine.IsWaitingForTapPointerUp())
					return false;
				else if(engine.IsWaitingForTapTimerUp())
					return true;
				else
					return false;
			}
			public override void Enter(){
				RunWaitForTapPointerUpProcess();
			}
			public void OnPointerUp(){
				engine.WaitForTapPointerDown();
			}
			public void OnEndDrag(){
				engine.WaitForTapPointerDown();
			}
		}
		public class UITappingState: UITapState, IUIPointerUpState{
			public UITappingState(ITapStateEngine engine): base(engine){
			}
			public override bool CanEnter(){
				if(engine.IsTapping())
					return false;
				else if(engine.WasWaitingForTapTimerUp())
					return true;
				else
					return false;
			}
			public override void Enter(){
				engine.ExecuteTapCommand();
				RunTapProcess();
			}
			public void OnPointerDown(){
				engine.WaitForTapTimerUp();
			}
			public void OnDeselected(){}
		}
}