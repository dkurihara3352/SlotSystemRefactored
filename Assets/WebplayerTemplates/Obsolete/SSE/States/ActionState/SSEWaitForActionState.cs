using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SSEWaitForActionState: SSEActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			sse.SetAndRunActProcess(null);
		}
	}
}