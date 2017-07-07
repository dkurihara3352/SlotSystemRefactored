using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSEStateEngine: SwitchableStateEngine{
		public SSEStateEngine(SlotSystemElement sse){
			this.handler = sse;
		}
		public void SetState(SSEState state){
			base.SetState(state);
		}
	}
}