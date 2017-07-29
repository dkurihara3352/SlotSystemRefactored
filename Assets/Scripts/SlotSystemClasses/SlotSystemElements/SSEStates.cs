using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SSEState: ISSEState{
		protected ISlotSystemElement sse;
		public virtual void EnterState(IStateHandler handler){
			sse = (ISlotSystemElement)handler;
		}
		public virtual void ExitState(IStateHandler handler){}
	}
	public interface ISSEState: ISwitchableState{}
	public class SSEStateEngine<T>: SwitchableStateEngine<T>, ISSEStateEngine<T> where T: ISSEState{
		public SSEStateEngine(ISlotSystemElement sse){
			this.handler = sse;
		}
	}
		public interface ISSEStateEngine<T>: ISwitchableStateEngine<T> where T: ISSEState{}
		public abstract class SSESelState: SSEState, ISSESelState{
			public void OnHoverEnter(ISlotSystemElement element, PointerEventDataFake eventData){}
			public void OnHoverExit(ISlotSystemElement element, PointerEventDataFake eventData){}
		}
		public interface ISSESelState: ISSEState{
			void OnHoverEnter(ISlotSystemElement element, PointerEventDataFake eventData);
			void OnHoverExit(ISlotSystemElement element, PointerEventDataFake eventData);
		}
			public class SSEDeactivatedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.isSelStateInit)
						sse.SetAndRunSelProcess(new SSEDeactivateProcess(sse, sse.deactivateCoroutine));
				}
			}
			public class SSEFocusedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.isSelStateInit){
						sse.SetAndRunSelProcess(new SSEFocusProcess(sse, sse.focusCoroutine));
					}
					else
						sse.InstantFocus();
				}
			}
			public class SSEDefocusedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.isSelStateInit)
						sse.SetAndRunSelProcess(new SSEDefocusProcess(sse, sse.defocusCoroutine));
					else
						sse.InstantDefocus();
				}
			}
			public class SSESelectedState : SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(!sse.isSelStateInit)
						sse.SetAndRunSelProcess(new SSESelectProcess(sse, sse.selectCoroutine));
					else
						sse.InstantSelect();
				}
			
			}
}