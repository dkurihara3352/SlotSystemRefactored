using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ITapStateEngine: IUISystemInputEngine{
		void WaitForTapPointerDown();
			bool WasWaitingForTapPointerDown();
			bool IsWaitingForTapPointerDown();
		void WaitForTapTimerUp();
			bool WasWaitingForTapTimerUp();
			bool IsWaitingForTapTimerUp();
		void WaitForTapPointerUp();
			bool WasWaitingForTapPointerUp();
			bool IsWaitingForTapPointerUp();
		void Tap();
			bool WasTapping();
			bool IsTapping();
		
		void ExecuteTapCommand();

		void SetAndRunTapProcess(ITapStateProcess process);

		IEnumeratorFake WaitForTapPointerDownCoroutine();
		IEnumeratorFake WaitForTapTimerUpCoroutine();
		IEnumeratorFake WaitForTapPointerUpCoroutine();
		IEnumeratorFake TapCoroutine();
	}
	public class TapStateEngine : ITapStateEngine{
		public TapStateEngine(IUIElement element){
			_uiElement = element;
			_tapStateSwitch = new UIStateSwitch<IUITapState>();
			InitializeStates();
		}
		IUIElement UIElement(){
			Debug.Assert(_uiElement != null);
			return _uiElement;
		}
			IUIElement _uiElement;
		IUIStateSwitch<IUITapState> TapStateSwitch(){
			Debug.Assert(_tapStateSwitch != null);
			return _tapStateSwitch;
		}
			IUIStateSwitch<IUITapState> _tapStateSwitch;
			void SetTapState(IUITapState state){
				TapStateSwitch().SwitchTo(state);
				if(state == null && TapStateProcess() != null)
					SetAndRunTapProcess(null);
			}
			IUITapState PrevState(){
				return TapStateSwitch().PrevState();
			}
			IUITapState CurState(){
				return TapStateSwitch().CurState();
			}
		void InitializeStates(){
			_waitingForTapPointerDownState = new UIWaitingForTapPointerDownState(this);
			_waitingForTapTimerUpState = new UIWaitingForTapTimerUpState(this);
			_waitingForTapPointerUpState = new UIWaitingForTapPointerUpState(this);
			_tappingState = new UITappingState(this);
		}


		public void WaitForTapPointerDown(){
			SetTapState( WaitingForPointerDownState());
		}
			public bool WasWaitingForTapPointerDown(){
				return PrevState() == WaitingForPointerDownState();
			}
			public bool IsWaitingForTapPointerDown(){
				return CurState() == WaitingForPointerDownState();
			}
			IUITapState WaitingForPointerDownState(){
				Debug.Assert(_waitingForTapPointerDownState != null);
				return _waitingForTapPointerDownState;
			}
			IUITapState _waitingForTapPointerDownState;
		public void WaitForTapTimerUp(){
			SetTapState( WaitingForTapTimerUpState());
		}
			public bool WasWaitingForTapTimerUp(){
				return PrevState() == WaitingForTapTimerUpState();
			}
			public bool IsWaitingForTapTimerUp(){
				return CurState() == WaitingForTapTimerUpState();
			}
			IUITapState WaitingForTapTimerUpState(){
				Debug.Assert(_waitingForTapTimerUpState != null);
				return _waitingForTapTimerUpState;
			}
			IUITapState _waitingForTapTimerUpState;
		public void WaitForTapPointerUp(){
			SetTapState( WaitingForTapPointerUpState());
		}
			public bool WasWaitingForTapPointerUp(){
				return PrevState() == WaitingForTapPointerUpState();
			}
			public bool IsWaitingForTapPointerUp(){
				return CurState() == WaitingForTapPointerUpState();
			}
			IUITapState WaitingForTapPointerUpState(){
				Debug.Assert(_waitingForTapPointerUpState != null);
				return _waitingForTapPointerUpState;
			}
			IUITapState _waitingForTapPointerUpState;
		public void Tap(){
			SetTapState( TappingState());
		}
			public bool WasTapping(){
				return PrevState() == TappingState();
			}
			public bool IsTapping(){
				return CurState() == TappingState();
			}
			IUITapState TappingState(){
				Debug.Assert(_tappingState != null);
				return _tappingState;
			}
			IUITapState _tappingState;
		
		public void ExecuteTapCommand(){
			UIElement().ExecuteTapCommand();
		}
		IUIProcessSwitch<ITapStateProcess> TapProcessSwitch(){
			Debug.Assert(_tapProcessSwitch != null);
			return _tapProcessSwitch;
		}
			IUIProcessSwitch<ITapStateProcess> _tapProcessSwitch;
		public void SetAndRunTapProcess(ITapStateProcess process){
			TapProcessSwitch().SetAndRunProcess(process);
		}
		ITapStateProcess TapStateProcess(){
			return TapProcessSwitch().Process();
		}


		public IEnumeratorFake WaitForTapPointerDownCoroutine(){
			return null;
		}
		public IEnumeratorFake WaitForTapTimerUpCoroutine(){
			return null;
		}
		public IEnumeratorFake WaitForTapPointerUpCoroutine(){
			return null;
		}
		public IEnumeratorFake TapCoroutine(){
			return null;
		}

		public void OnPointerDown(){
			if(CurState() is IUIPointerUpState)
				((IUIPointerUpState)CurState()).OnPointerDown();
		}
		public void OnPointerUp(){
			if(CurState() is IUIPointerDownState)
				((IUIPointerDownState)CurState()).OnPointerUp();
		}
		public void OnEndDrag(){
			if(CurState() is IUIPointerDownState)
				((IUIPointerDownState)CurState()).OnEndDrag();
		}
		public void OnDeselected(){
			if(CurState() is IUIPointerUpState)
				((IUIPointerUpState)CurState()).OnDeselected();
		}
	}
}
