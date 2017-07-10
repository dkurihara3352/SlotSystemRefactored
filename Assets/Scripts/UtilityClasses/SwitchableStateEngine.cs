using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public abstract class SwitchableStateEngine{
		public virtual StateHandler handler{get{return m_handler;} set{m_handler = value;}}
			StateHandler m_handler;
		virtual public SwitchableState prevState{
			get{
				return m_prevState;
			}
			}protected SwitchableState m_prevState;
		virtual public SwitchableState curState{
			get{
				return m_curState;
			}
			}protected SwitchableState m_curState;
		public virtual void SetState(SwitchableState state){
			if(curState != null){
				if(curState != state){
					curState.ExitState(handler);
					m_prevState = curState;
					m_curState = state;
					if(curState != null){
						curState.EnterState(handler);
					}
				}
			}else{// used as initialization
				m_prevState = state;
				m_curState = state;
				if(state != null)
					state.EnterState(handler);
			}
		}
	}
}
