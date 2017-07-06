using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public interface SwitchableState{

		void EnterState(StateHandler handler);
		void ExitState(StateHandler handler);
	}
}
