using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UISystem{
	/* StateEngine */
		public class UIStateEngine<T>: SwitchableStateEngine<T>, IUIStateEngine<T> where T: IUIState{
		}
		public interface IUIStateEngine<T>: ISwitchableStateEngine<T> where T: IUIState{}
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
			protected IUISelStateHandler handler;
			public void InitializeFields(IUIElement element){
				this.element = element;
				this.handler = element.SelStateHandler();
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
				SetDeactivatedState(new UIDeactivatedState(handler));
				SetActivatedState(new UIActivatedState(element, handler));
				SetHiddenState(new UIHiddenState(handler));
				SetShownState(new UIShownState(handler));
				SetSelectedState(new UISelectedState(handler));
				SetDeselectedState(new UIDeselectedState(handler));
				SetSelectableState(new UISelectableState(handler));
				SetUnselectableState(new UIUnselectableState(handler));
			}
		}
	/* State */
		public interface IUIState: ISwitchableState{}// for engine use
	/* Selection State */
		public interface IUISelectionState: IUIState{
		}
		public abstract class UISelectionState: IUISelectionState{
			protected IUISelStateHandler handler;
			public UISelectionState(IUISelStateHandler handler){
				this.handler = handler;
			}
			public virtual void Enter(){}
			public virtual void Exit(){}
			public virtual bool CanEnter(){
				return false;
			}
			protected void RunDeactivateProcess(){
				handler.SetAndRunSelProcess(new UIDeactivateProcess(handler.DeactivateCoroutine()));
			}
			protected void RunHideProcess(){
				handler.SetAndRunSelProcess(new UIHideProcess(handler.HideCoroutine()));
			}
			protected void RunSelectProcess(){
				handler.SetAndRunSelProcess(new UISelectProcess(handler.SelectCoroutine()));
			}
			protected void RunMakeSelectableProcess(){
				handler.SetAndRunSelProcess(new UIMakeSelectableProcess(handler.MakeSelectableCoroutine()));
			}
			protected void RunMakeUnselectableProcess(){
				handler.SetAndRunSelProcess(new UIMakeUnselectableProcess(handler.MakeUnselectableCoroutine()));
			}
		}
			public class UIDeactivatedState: UISelectionState{
				public UIDeactivatedState(IUISelStateHandler handler): base(handler){}
				public override bool CanEnter(){
					if(handler.IsDeactivated())
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
				public UIActivatedState(IUIElement element, IUISelStateHandler handler): base(handler){
					this.element = element;
				}
				public override bool CanEnter(){
					if(handler.IsActivated())
						return false;
					else
						return true;
				}
				public override void Enter(){
					if(element.IsShownOnActivation())
						handler.Show();
				}
			}
			public class UIHiddenState: UISelectionState{
				public UIHiddenState(IUISelStateHandler handler): base(handler){}
				public override bool CanEnter(){
					if(handler.IsHidden())
						return false;
					else
						return true;
				}
				public override void Enter(){
					RunHideProcess();
					if(handler.WasDeactivated())
						handler.ExpireProcess();
				}
			}
			public class UIShownState: UISelectionState, IRelayState{
				public UIShownState(IUISelStateHandler handler): base(handler){
				}
				public override bool CanEnter(){
					if(handler.IsShown())
						return false;
					else
						return true;
				}
				public override void Enter(){
					handler.Deselect();
				}
			}
			public class UISelectedState : UISelectionState{
				public UISelectedState(IUISelStateHandler handler): base(handler){}
				public override bool CanEnter(){
					if(handler.IsSelected())
						return false;
					else
						return true;
				}
				public override void Enter(){
					RunSelectProcess();
					if(handler.WasDeactivated())
						handler.ExpireProcess();
				}
			}
			public class UIDeselectedState: UISelectionState, IRelayState{
				public UIDeselectedState(IUISelStateHandler handler): base(handler){}
				public override bool CanEnter(){
					if(handler.IsDeselected())
						return false;
					else
						return true;
				}
				public override void Enter(){
					handler.MakeSelectable();
				}
			}
			public class UISelectableState: UISelectionState{
				public UISelectableState(IUISelStateHandler handler): base(handler){}
				public override bool CanEnter(){
					if(handler.IsSelectable())
						return false;
					else
						return true;
				}
				public override void Enter(){
					RunMakeSelectableProcess();
					if(handler.WasDeactivated())
						handler.ExpireProcess();
				}
			}
			public class UIUnselectableState: UISelectionState{
				public UIUnselectableState(IUISelStateHandler handler): base(handler){}
				public override bool CanEnter(){
					if(handler.IsUnselectable())
						return false;
					else 
						return true;
				}
				public override void Enter(){
					RunMakeUnselectableProcess();
					if(handler.WasDeactivated())
						handler.ExpireProcess();
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
			protected ITapStateHandler handler;
			public UITapState(ITapStateHandler handler){
				this.handler = handler;
			}
			public virtual void Enter(){}
			public virtual void Exit(){}
			public virtual bool CanEnter(){return false;}

			protected void RunWaitForPointerDownProcess(){
				handler.SetAndRunTapProcess(new UIWaitForTapPointerDownProcess(handler.WaitForTapPointerDownCoroutine(), handler));
			}
			protected void RunWaitForTapTimerUpProcess(){
				handler.SetAndRunTapProcess(new UIWaitForTapTimerUpProcess(handler.WaitForTapTimerUpCoroutine(), handler));
			}
			protected void RunWaitForTapPointerUpProcess(){
				handler.SetAndRunTapProcess(new UIWaitForTapPointerUpProcess(handler.WaitForTapPointerUpCoroutine(), handler));
			}
			protected void RunTapProcess(){
				handler.SetAndRunTapProcess(new UITapProcess(handler.TapCoroutine(), handler));
			}
		}
		public class UIWaitingForTapPointerDownState: UITapState, IUIPointerUpState{
			public UIWaitingForTapPointerDownState(ITapStateHandler handler): base(handler){
			}
			public override bool CanEnter(){
				if(handler.IsWaitingForTapPointerDown())
					return false;
				else
					return true;
			}
			public override void Enter(){
				RunWaitForPointerDownProcess();
			}
			public void OnPointerDown(){
				handler.WaitForTapTimerUp();
			}
			public void OnDeselected(){}
		}
		public class UIWaitingForTapTimerUpState: UITapState, IUIPointerDownState{
			public UIWaitingForTapTimerUpState(ITapStateHandler handler): base(handler){
			}
			public override bool CanEnter(){
				if(handler.IsWaitingForTapTimerUp())
					return false;
				else if(handler.IsWaitingForTapPointerDown())
					return true;
				else
					return false;
			}
			public override void Enter(){
				RunWaitForTapTimerUpProcess();
			}
			public void OnPointerUp(){
				handler.Tap();
			}
			public void OnEndDrag(){
				handler.WaitForTapPointerDown();
			}
		}
		public class UIWaitingForTapPointerUpState: UITapState, IUIPointerDownState{
			public UIWaitingForTapPointerUpState(ITapStateHandler handler): base(handler){
			}
			public override bool CanEnter(){
				if(handler.IsWaitingForTapPointerUp())
					return false;
				else if(handler.IsWaitingForTapTimerUp())
					return true;
				else
					return false;
			}
			public override void Enter(){
				RunWaitForTapPointerUpProcess();
			}
			public void OnPointerUp(){
				handler.WaitForTapPointerDown();
			}
			public void OnEndDrag(){
				handler.WaitForTapPointerDown();
			}
		}
		public class UITappingState: UITapState, IUIPointerUpState{
			public UITappingState(ITapStateHandler handler): base(handler){
			}
			public override bool CanEnter(){
				if(handler.IsTapping())
					return false;
				else if(handler.WasWaitingForTapTimerUp())
					return true;
				else
					return false;
			}
			public override void Enter(){
				handler.ExecuteTapCommand();
				RunTapProcess();
			}
			public void OnPointerDown(){
				handler.WaitForTapTimerUp();
			}
			public void OnDeselected(){}
		}
}