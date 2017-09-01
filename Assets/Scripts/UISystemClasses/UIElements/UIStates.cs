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
		public interface IUIState: ISwitchableState{}
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
					handler.SetAndRunSelProcess(new UIDeactivateProcess(handler.DeactivateCoroutine()));
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
					handler.SetAndRunSelProcess(new UIHideProcess(handler.HideCoroutine()));
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
					handler.SetAndRunSelProcess(new UISelectProcess(handler.SelectCoroutine()));
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
					handler.SetAndRunSelProcess(new UIMakeSelectableProcess(handler.MakeSelectableCoroutine()));
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
					handler.SetAndRunSelProcess(new UIMakeUnselectableProcess(handler.MakeUnselectableCoroutine()));
					if(handler.WasDeactivated())
						handler.ExpireProcess();
				}
			}
}