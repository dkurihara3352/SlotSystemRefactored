﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SSEState: SwitchableState{
		protected SlotSystemElement sse;
		public virtual void EnterState(StateHandler handler){
			sse = (SlotSystemElement)handler;
		}
		public virtual void ExitState(StateHandler handler){}
	}
	public class SSEStateEngine: SwitchableStateEngine{
		public SSEStateEngine(SlotSystemElement sse){
			this.handler = sse;
		}
		public virtual void SetState(SSEState state){
			base.SetState(state);
		}
	}
		public abstract class SSESelState: SSEState{}
			public class SSEDeactivatedState: SSESelState{
				public override void EnterState(StateHandler sh){
					base.EnterState(sh);
					sse.SetAndRunSelProcess(null);
				}
			}
			public class SSEFocusedState: SSESelState{
				public override void EnterState(StateHandler sh){
					base.EnterState(sh);
					SSEProcess process = null;
					if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
						process = null;
						sse.InstantGreyin();
					}
					else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
						process = new SSEGreyinProcess(sse, sse.greyinCoroutine);
					else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
						process = new SSEDehighlightProcess(sse, sse.dehighlightCoroutine);
					sse.SetAndRunSelProcess(process);
				}
			}
			public class SSEDefocusedState: SSESelState{
				public override void EnterState(StateHandler sh){
					base.EnterState(sh);
					SSEProcess process = null;
					if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
						process = null;
						sse.InstantGreyout();
					}else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
						process = new SSEGreyoutProcess(sse, sse.greyoutCoroutine);
					else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
						process = new SSEDehighlightProcess(sse, sse.dehighlightCoroutine);
					sse.SetAndRunSelProcess(process);
				}
			}
			public class SSESelectedState : SSESelState{
				public override void EnterState(StateHandler sh){
					base.EnterState(sh);
					SSEProcess process = null;
					if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
						process = null;
						sse.InstantHighlight();
					}
					else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
						process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
					else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
						process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
					sse.SetAndRunSelProcess(process);
				}
			
			}
		public abstract class SSEActState: SSEState{}
			public class SSEWaitForActionState: SSEActState{
				public override void EnterState(StateHandler sh){
					base.EnterState(sh);
					sse.SetAndRunActProcess(null);
				}
			}
}