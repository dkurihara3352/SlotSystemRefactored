using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public abstract class SwitchableStateEngine<T>: ISwitchableStateEngine<T> where T: ISwitchableState{
		public virtual IStateHandler handler{get{return m_handler;} set{m_handler = value;}}
			IStateHandler m_handler;
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
					prevState.ExitState(handler);
				m_curState = state;
				if(curState != null){
					curState.EnterState(handler);
				}
			}
		}
	}
	public interface ISwitchableStateEngine<T> where T: ISwitchableState{
		IStateHandler handler{get;set;}
		T prevState{get;}
		T curState{get;}
		void SetState(T state);

	}
	public interface ISwitchableState{
		void EnterState(IStateHandler handler);
		void ExitState(IStateHandler handler);
	}
}
