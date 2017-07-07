using System.Collections;
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
}