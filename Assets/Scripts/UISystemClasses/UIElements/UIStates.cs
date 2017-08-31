using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UISystem{
	/* StateEngine */
		public class UIStateEngine<T>: SwitchableStateEngine<T>, IUIStateEngine<T> where T: IUIState{
		}
		public interface IUIStateEngine<T>: ISwitchableStateEngine<T> where T: IUIState{}
	/* StateFacotory */
		public class UISelStateRepo: IUISelStateRepo{
			protected IUISelStateHandler handler;
			public UISelStateRepo(IUISelStateHandler handler){
				this.handler = handler;
			}
			public IUISelState DeactivatedState(){
				return deactivatedState;}
			IUISelState deactivatedState{
				get{
					if(_deactivatedState ==null)
						_deactivatedState = new UIDeactivatedState(handler);
					return _deactivatedState;}}
			IUISelState _deactivatedState;

			public IUISelState UnselectableState(){
				return unselectableState;}
			IUISelState unselectableState{
				get{
					if(_unselectableState ==null)
						_unselectableState = new UIUnselectableState(handler);
					return _unselectableState;}}
			IUISelState _unselectableState;
			
			public IUISelState SelectableState(){
				return selectableState;}
			IUISelState selectableState{
				get{
					if(_selectableState ==null)
						_selectableState = new UISelectableState(handler);
					return _selectableState;}}
			IUISelState _selectableState;
			
			public IUISelState SelectedState(){
				return selectedState;}
			IUISelState selectedState{
				get{
					if(_selectedState ==null)
						_selectedState = new UISelectedState(handler);
					return _selectedState;}}
			IUISelState _selectedState;
		}
		public interface IUISelStateRepo{
			IUISelState DeactivatedState();
			IUISelState ActivatedState();
			IUISelState HiddenState();
			IUISelState ShownState();
			IUISelState SelectedState();
			IUISelState DeselectedState();
			IUISelState SelectableState();
			IUISelState UnselectableState();
		}
	/* State */
		public abstract class UIState: IUIState{
			public virtual void EnterState(){
			}
			public virtual void ExitState(){
			}
		}
		public interface IUIState: ISwitchableState{}
		public abstract class UISelState: UIState, IUISelState{
			protected IUISelStateHandler handler;
			public UISelState(IUISelStateHandler handler){
				this.handler = handler;
			}
		}
		public interface IUISelState: IUIState{
		}
			public class UIDeactivatedState: UISelState{
				public UIDeactivatedState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(handler.WasActivated())
						handler.SetAndRunSelProcess(new UIDeactivateProcess(handler.DeactivateCoroutine()));
				}
			}
			public class UIActivatedState: UISelState{
				public UIActivatedState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(handler.WasDeactivated())
						handler.ShowInstantly();
				}
			}
			public class UIHiddenState: UISelState{
				public UIHiddenState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(handler.WasShown())
						handler.SetAndRunSelProcess(new UIHideProcess(handler.HideCoroutine()));
				}
			}
			public class UIShownState: UISelState{
				public UIShownState(IUISelStateHandler handler): base(handler){
				}
				public override void EnterState(){
					if(handler.WasHidden())
						handler.MakeSelectable();
				}
			}
			public class UISelectedState : UISelState{
				public UISelectedState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.WasSelStateNull())
						handler.SetAndRunSelProcess(new UISelectProcess(handler.SelectCoroutine()));
					else
						handler.SelectInstantly();
				}
			}
			public class UIUnselectedState: UISelState{
				public UIUnselectedState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(handler.WasSelected())
						handler.MakeSelectable();
				}
			}
			public class UISelectableState: UISelState{
				public UISelectableState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.WasSelStateNull()){
						handler.SetAndRunSelProcess(new UIFocusProcess(handler.MakeSelectableCoroutine()));
					}
					else
						handler.MakeSelectableInstantly();
				}
			}
			public class UIUnselectableState: UISelState{
				public UIUnselectableState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.WasSelStateNull())
						handler.SetAndRunSelProcess(new UIDefocusProcess(handler.MakeUnselectableCoroutine()));
					else
						handler.MakeUnselectableInstantly();
				}
			}
}