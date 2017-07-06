using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBDeactivatedState: SBSelState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			sb.SetAndRunSelProcess(null);
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}
