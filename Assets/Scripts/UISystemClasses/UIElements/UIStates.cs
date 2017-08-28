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
		public class UISelStateFacotory: IUISelStateFactory{
			protected IUISelStateHandler handler;
			public UISelStateFacotory(IUISelStateHandler handler){
				this.handler = handler;
			}
			public IUISelState MakeDeactivatedState(){
				return deactivatedState;}
			IUISelState deactivatedState{
				get{
					if(m_deactivatedState ==null)
						m_deactivatedState = new UIDeactivatedState(handler);
					return m_deactivatedState;}}
			IUISelState m_deactivatedState;

			public IUISelState MakeDefocusedState(){
				return defocusedState;}
			IUISelState defocusedState{
				get{
					if(m_defocusedState ==null)
						m_defocusedState = new UIDefocusedState(handler);
					return m_defocusedState;}}
			IUISelState m_defocusedState;
			
			public IUISelState MakeFocusedState(){
				return focusedState;}
			IUISelState focusedState{
				get{
					if(m_focusedState ==null)
						m_focusedState = new UIFocusedState(handler);
					return m_focusedState;}}
			IUISelState m_focusedState;
			
			public IUISelState MakeSelectedState(){
				return selectedState;}
			IUISelState selectedState{
				get{
					if(m_selectedState ==null)
						m_selectedState = new UISelectedState(handler);
					return m_selectedState;}}
			IUISelState m_selectedState;
		}
		public interface IUISelStateFactory{
			IUISelState MakeDeactivatedState();
			IUISelState MakeDefocusedState();
			IUISelState MakeFocusedState();
			IUISelState MakeSelectedState();
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
					if(!handler.WasSelStateNull())
						handler.SetAndRunSelProcess(new UIDeactivateProcess(handler.GetDeactivateCoroutine()));
				}
			}
			public class UIFocusedState: UISelState{
				public UIFocusedState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.WasSelStateNull()){
						handler.SetAndRunSelProcess(new UIFocusProcess(handler.GetFocusCoroutine()));
					}
					else
						handler.InstantFocus();
				}
			}
			public class UIDefocusedState: UISelState{
				public UIDefocusedState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.WasSelStateNull())
						handler.SetAndRunSelProcess(new UIDefocusProcess(handler.GetDefocusCoroutine()));
					else
						handler.InstantDefocus();
				}
			}
			public class UISelectedState : UISelState{
				public UISelectedState(IUISelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.WasSelStateNull())
						handler.SetAndRunSelProcess(new UISelectProcess(handler.GetSelectCoroutine()));
					else
						handler.InstantSelect();
				}
			
			}
}