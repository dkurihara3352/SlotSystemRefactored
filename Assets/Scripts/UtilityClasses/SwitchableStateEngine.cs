using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public abstract class SwitchableStateEngine<T>: ISwitchableStateEngine<T> where T: ISwitchableState{
		virtual public T GetPrevState(){
			return _prevState;
		}
			protected T _prevState;
		virtual public T GetCurState(){
			return _curState;
		}
			protected T _curState;
		public virtual void SetState(T newState){
			T curState = GetCurState();

			if((ISwitchableState)curState != (ISwitchableState)newState){
				_prevState = curState;
				if(curState != null)
					curState.ExitState();
				_curState = newState;
				if(newState != null){
					newState.EnterState();
				}
			}
		}
	}
	public interface ISwitchableStateEngine<T> where T: ISwitchableState{
		T GetPrevState();
		T GetCurState();
		void SetState(T state);

	}
	public interface ISwitchableState{
		void EnterState();
		void ExitState();
	}
}
