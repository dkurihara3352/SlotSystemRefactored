using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility{
	public abstract class SwitchableStateEngine<T>: ISwitchableStateEngine<T> where T: ISwitchableState{
		virtual public T PrevState(){
			return _prevState;
		}
			protected T _prevState;
		virtual public T CurState(){
			return _curState;
		}
			protected T _curState;
		public void SetState(T newState){
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
	public interface ISwitchableStateEngine<T> where T: ISwitchableState{
		T PrevState();
		T CurState();
		void SetState(T state);

	}
	public interface ISwitchableState{
		void Enter();
		void Exit();
		bool CanEnter();
	}
	public interface IRelayState{
	}
}
