using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SGWaitForActionState: SGActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			sg.SetAndRunActProcess(null);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
