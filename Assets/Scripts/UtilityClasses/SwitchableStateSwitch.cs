using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public abstract class SwitchableStateSwitch<T>: ISwitchableStateSwitch<T> where T: ISwitchableState{
		virtual public T PrevState(){
			return _prevState;
		}
			protected T _prevState;
		virtual public T CurState(){
			return _curState;
		}
			protected T _curState;
		public void SwitchTo(T newState){
			Debug.Assert( !(CurState() is IRelayState));
			if(newState.CanEnter()){
				UpdatePrevState();
				UpdateCurState(newState);
			}
		}
		void UpdatePrevState(){
			_prevState = CurState();
			if(CurState() != null)
				CurState().Exit();
		}
		void UpdateCurState(T toState){
			if(!(toState is IRelayState))
				_curState = toState;
			if(toState != null)
				toState.Enter();
		}
	}
	public interface ISwitchableStateSwitch<T> where T: ISwitchableState{
		T PrevState();
		T CurState();
		void SwitchTo(T state);
	}
	public interface ISwitchableState{
		void Enter();
		void Exit();
		bool CanEnter();
	}
	public interface IRelayState{
	}
}
