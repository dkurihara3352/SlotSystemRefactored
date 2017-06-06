using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public class UtilityClasses{
	}
	public interface StateHandler{}
	public interface SwitchableState{
		void EnterState(StateHandler handler);
		void ExitState(StateHandler handler);
	}
	public abstract class SwitchableStateEngine{
		protected StateHandler handler;
		public SwitchableState prevState;
		public SwitchableState curState;
		protected void SetState(SwitchableState state){
			if(curState != null){
				if(curState != state){
					curState.ExitState(handler);
					prevState = curState;
				}
				if(state != null){
					state.EnterState(handler);
				}
				curState = state;
			}else{// used as initialization
				prevState = state;
				curState = state;
				if(state != null)
					state.EnterState(handler);
			}
		}
	} 
}
