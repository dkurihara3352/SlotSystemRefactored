using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public abstract class SwitchableStateEngine<T>: ISwitchableStateEngine<T> where T: ISwitchableState{
		virtual public T prevState{
			get{
				return m_prevState;
			}
			}protected T m_prevState;
		virtual public T curState{
			get{
				return m_curState;
			}
			}protected T m_curState;
		public virtual void SetState(T state){
			if((ISwitchableState)curState != (ISwitchableState)state){
				m_prevState = curState;
				if(prevState != null)
					prevState.ExitState();
				m_curState = state;
				if(curState != null){
					curState.EnterState();
				}
			}
		}
	}
	public interface ISwitchableStateEngine<T> where T: ISwitchableState{
		T prevState{get;}
		T curState{get;}
		void SetState(T state);

	}
	public interface ISwitchableState{
		void EnterState();
		void ExitState();
	}
}
