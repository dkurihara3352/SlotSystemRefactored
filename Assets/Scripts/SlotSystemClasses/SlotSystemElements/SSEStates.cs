using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	/* StateEngine */
		public class SSEStateEngine<T>: SwitchableStateEngine<T>, ISSEStateEngine<T> where T: ISSEState{
			public SSEStateEngine(){
			}
		}
		public interface ISSEStateEngine<T>: ISwitchableStateEngine<T> where T: ISSEState{}
	/* StateFacotory */
		public class SSESelStateFacotory: ISSESelStateFactory{
			protected ISSESelStateHandler handler;
			public SSESelStateFacotory(ISSESelStateHandler handler){
				this.handler = handler;
			}
			public ISSESelState MakeDeactivatedState(){
				return deactivatedState;}
			ISSESelState deactivatedState{
				get{
					if(m_deactivatedState ==null)
						m_deactivatedState = new SSEDeactivatedState(handler);
					return m_deactivatedState;}}
			ISSESelState m_deactivatedState;

			public ISSESelState MakeDefocusedState(){
				return defocusedState;}
			ISSESelState defocusedState{
				get{
					if(m_defocusedState ==null)
						m_defocusedState = new SSEDefocusedState(handler);
					return m_defocusedState;}}
			ISSESelState m_defocusedState;
			
			public ISSESelState MakeFocusedState(){
				return focusedState;}
			ISSESelState focusedState{
				get{
					if(m_focusedState ==null)
						m_focusedState = new SSEFocusedState(handler);
					return m_focusedState;}}
			ISSESelState m_focusedState;
			
			public ISSESelState MakeSelectedState(){
				return selectedState;}
			ISSESelState selectedState{
				get{
					if(m_selectedState ==null)
						m_selectedState = new SSESelectedState(handler);
					return m_selectedState;}}
			ISSESelState m_selectedState;
		}
		public interface ISSESelStateFactory{
			ISSESelState MakeDeactivatedState();
			ISSESelState MakeDefocusedState();
			ISSESelState MakeFocusedState();
			ISSESelState MakeSelectedState();
		}
	/* State */
		public abstract class SSEState: ISSEState{
			public virtual void EnterState(){
			}
			public virtual void ExitState(){
			}
		}
		public interface ISSEState: ISwitchableState{}
		public abstract class SSESelState: SSEState, ISSESelState{
			protected ISSESelStateHandler handler;
			public SSESelState(ISSESelStateHandler handler){
				this.handler = handler;
			}
		}
		public interface ISSESelState: ISSEState{
		}
			public class SSEDeactivatedState: SSESelState{
				public SSEDeactivatedState(ISSESelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.wasSelStateNull)
						handler.SetAndRunSelProcess(new SSEDeactivateProcess(handler, handler.deactivateCoroutine));
				}
			}
			public class SSEFocusedState: SSESelState{
				public SSEFocusedState(ISSESelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.wasSelStateNull){
						handler.SetAndRunSelProcess(new SSEFocusProcess(handler, handler.focusCoroutine));
					}
					else
						handler.InstantFocus();
				}
			}
			public class SSEDefocusedState: SSESelState{
				public SSEDefocusedState(ISSESelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.wasSelStateNull)
						handler.SetAndRunSelProcess(new SSEDefocusProcess(handler, handler.defocusCoroutine));
					else
						handler.InstantDefocus();
				}
			}
			public class SSESelectedState : SSESelState{
				public SSESelectedState(ISSESelStateHandler handler): base(handler){}
				public override void EnterState(){
					if(!handler.wasSelStateNull)
						handler.SetAndRunSelProcess(new SSESelectProcess(handler, handler.selectCoroutine));
					else
						handler.InstantSelect();
				}
			
			}
}