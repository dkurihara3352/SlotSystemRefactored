using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	/* StateEngine */
		public class SSEStateEngine<T>: SwitchableStateEngine<T>, ISSEStateEngine<T> where T: ISSEState{
			public SSEStateEngine(ISlotSystemElement sse){
				this.handler = sse;
			}
		}
		public interface ISSEStateEngine<T>: ISwitchableStateEngine<T> where T: ISSEState{}
	/* StateFacotory */
		public class SSESelStateFacotory: ISSESelStateFactory{
			public ISSESelState MakeDeactivatedState(){
				return deactivatedState;}
			ISSESelState deactivatedState{
				get{
					if(m_deactivatedState ==null)
						m_deactivatedState = new SSEDeactivatedState();
					return m_deactivatedState;}}
			ISSESelState m_deactivatedState;

			public ISSESelState MakeDefocusedState(){
				return defocusedState;}
			ISSESelState defocusedState{
				get{
					if(m_defocusedState ==null)
						m_defocusedState = new SSEDefocusedState();
					return m_defocusedState;}}
			ISSESelState m_defocusedState;
			
			public ISSESelState MakeFocusedState(){
				return focusedState;}
			ISSESelState focusedState{
				get{
					if(m_focusedState ==null)
						m_focusedState = new SSEFocusedState();
					return m_focusedState;}}
			ISSESelState m_focusedState;
			
			public ISSESelState MakeSelectedState(){
				return selectedState;}
			ISSESelState selectedState{
				get{
					if(m_selectedState ==null)
						m_selectedState = new SSESelectedState();
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
			protected ISlotSystemElement sse;
			public virtual void EnterState(IStateHandler handler){
				sse = (ISlotSystemElement)handler;
			}
			public virtual void ExitState(IStateHandler handler){}
		}
		public interface ISSEState: ISwitchableState{}
		public abstract class SSESelState: SSEState, ISSESelState{
		}
		public interface ISSESelState: ISSEState{
		}
			public class SSEDeactivatedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.wasSelStateNull)
						sse.SetAndRunSelProcess(new SSEDeactivateProcess(sse, sse.deactivateCoroutine));
				}
			}
			public class SSEFocusedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.wasSelStateNull){
						sse.SetAndRunSelProcess(new SSEFocusProcess(sse, sse.focusCoroutine));
					}
					else
						sse.InstantFocus();
				}
			}
			public class SSEDefocusedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.wasSelStateNull)
						sse.SetAndRunSelProcess(new SSEDefocusProcess(sse, sse.defocusCoroutine));
					else
						sse.InstantDefocus();
				}
			}
			public class SSESelectedState : SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.wasSelStateNull)
						sse.SetAndRunSelProcess(new SSESelectProcess(sse, sse.selectCoroutine));
					else
						sse.InstantSelect();
				}
			
			}
}