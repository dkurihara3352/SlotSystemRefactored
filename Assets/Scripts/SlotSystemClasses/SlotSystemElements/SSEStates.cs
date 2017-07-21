using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SSEState: SwitchableState{
		protected ISlotSystemElement sse;
		public virtual void EnterState(IStateHandler handler){
			sse = (ISlotSystemElement)handler;
		}
		public virtual void ExitState(IStateHandler handler){}
	}
	public class SSEStateEngine: SwitchableStateEngine, ISSEStateEngine{
		public SSEStateEngine(ISlotSystemElement sse){
			this.handler = sse;
		}
		public virtual void SetState(SSEState state){
			base.SetState(state);
		}
	}
		public interface ISSEStateEngine: ISwitchableStateEngine{}
		public abstract class SSESelState: SSEState, ISSESelState{
			public void OnHoverEnterMock(ISlotSystemElement element, PointerEventDataFake eventData){}
			public void OnHoverExitMock(ISlotSystemElement element, PointerEventDataFake eventData){}
		}
		public interface ISSESelState: SwitchableState{
			void OnHoverEnterMock(ISlotSystemElement element, PointerEventDataFake eventData);
			void OnHoverExitMock(ISlotSystemElement element, PointerEventDataFake eventData);
		}
			public class SSEDeactivatedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(sse.prevSelState != null)
						sse.SetAndRunSelProcess(new SSEDeactivateProcess(sse, sse.deactivateCoroutine));
				}
			}
			public class SSEFocusedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(sse.prevSelState != null){
						sse.SetAndRunSelProcess(new SSEFocusProcess(sse, sse.focusCoroutine));
					}
					else
						sse.InstantFocus();
				}
			}
			public class SSEDefocusedState: SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(sse.prevSelState != null)
						sse.SetAndRunSelProcess(new SSEDefocusProcess(sse, sse.defocusCoroutine));
					else
						sse.InstantDefocus();
				}
			}
			public class SSESelectedState : SSESelState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					if(sse.prevSelState != null)
						sse.SetAndRunSelProcess(new SSESelectProcess(sse, sse.selectCoroutine));
					else
						sse.InstantSelect();
				}
			
			}
		public abstract class SSEActState: SSEState{}
			public class SSEWaitForActionState: SSEActState{
				public override void EnterState(IStateHandler sh){
					base.EnterState(sh);
					sse.SetAndRunActProcess(null);
				}
			}
}