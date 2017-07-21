using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public interface SwitchableState{
		void EnterState(IStateHandler handler);
		void ExitState(IStateHandler handler);
	}
}
