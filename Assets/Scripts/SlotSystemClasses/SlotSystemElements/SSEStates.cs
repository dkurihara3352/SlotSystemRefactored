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
			protected ISlotSystemElement sse;
			public SSESelStateFacotory(ISlotSystemElement sse){
				this.sse = sse;
			}
			public ISSESelState MakeDeactivatedState(){
				return deactivatedState;}
			ISSESelState deactivatedState{
				get{
					if(m_deactivatedState ==null)
						m_deactivatedState = new SSEDeactivatedState(sse);
					return m_deactivatedState;}}
			ISSESelState m_deactivatedState;

			public ISSESelState MakeDefocusedState(){
				return defocusedState;}
			ISSESelState defocusedState{
				get{
					if(m_defocusedState ==null)
						m_defocusedState = new SSEDefocusedState(sse);
					return m_defocusedState;}}
			ISSESelState m_defocusedState;
			
			public ISSESelState MakeFocusedState(){
				return focusedState;}
			ISSESelState focusedState{
				get{
					if(m_focusedState ==null)
						m_focusedState = new SSEFocusedState(sse);
					return m_focusedState;}}
			ISSESelState m_focusedState;
			
			public ISSESelState MakeSelectedState(){
				return selectedState;}
			ISSESelState selectedState{
				get{
					if(m_selectedState ==null)
						m_selectedState = new SSESelectedState(sse);
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
			public SSEState(ISlotSystemElement sse){
				this.sse = sse;
			}
			public virtual void EnterState(){
			}
			public virtual void ExitState(){
			}
		}
		public interface ISSEState: ISwitchableState{}
		public abstract class SSESelState: SSEState, ISSESelState{
			public SSESelState(ISlotSystemElement sse): base(sse){}
		}
		public interface ISSESelState: ISSEState{
		}
			public class SSEDeactivatedState: SSESelState{
				public SSEDeactivatedState(ISlotSystemElement sse): base(sse){}
				public override void EnterState(){
					if(!sse.wasSelStateNull)
						sse.SetAndRunSelProcess(new SSEDeactivateProcess(sse, sse.deactivateCoroutine));
				}
			}
			public class SSEFocusedState: SSESelState{
				public SSEFocusedState(ISlotSystemElement sse): base(sse){}
				public override void EnterState(){
					if(!sse.wasSelStateNull){
						sse.SetAndRunSelProcess(new SSEFocusProcess(sse, sse.focusCoroutine));
					}
					else
						sse.InstantFocus();
				}
			}
			public class SSEDefocusedState: SSESelState{
				public SSEDefocusedState(ISlotSystemElement sse): base(sse){}
				public override void EnterState(){
					if(!sse.wasSelStateNull)
						sse.SetAndRunSelProcess(new SSEDefocusProcess(sse, sse.defocusCoroutine));
					else
						sse.InstantDefocus();
				}
			}
			public class SSESelectedState : SSESelState{
				public SSESelectedState(ISlotSystemElement sse): base(sse){}
				public override void EnterState(){
					if(!sse.wasSelStateNull)
						sse.SetAndRunSelProcess(new SSESelectProcess(sse, sse.selectCoroutine));
					else
						sse.InstantSelect();
				}
			
			}
}