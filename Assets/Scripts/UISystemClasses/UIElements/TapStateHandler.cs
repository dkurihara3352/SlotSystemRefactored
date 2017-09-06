using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class TapStateHandler : ITapStateHandler{
		public TapStateHandler(IUIElement element){
			_uiElement = element;
			_tapStateEngine = new UIStateEngine<IUITapState>();
			InitializeStates();
		}
		IUIElement UIElement(){
			Debug.Assert(_uiElement != null);
			return _uiElement;
		}
			IUIElement _uiElement;
		IUIStateEngine<IUITapState> TapStateEngine(){
			Debug.Assert(_tapStateEngine != null);
			return _tapStateEngine;
		}
			IUIStateEngine<IUITapState> _tapStateEngine;
			void SetTapState(IUITapState state){
				TapStateEngine().SetState(state);
				if(state == null && TapStateProcess() != null)
					SetAndRunTapProcess(null);
			}
			IUITapState PrevState(){
				return TapStateEngine().PrevState();
			}
			IUITapState CurState(){
				return TapStateEngine().CurState();
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
		IUIProcessEngine<ITapStateProcess> TapProcessEngine(){
			Debug.Assert(_tapProcessEngine != null);
			return _tapProcessEngine;
		}
			IUIProcessEngine<ITapStateProcess> _tapProcessEngine;
		public void SetAndRunTapProcess(ITapStateProcess process){
			TapProcessEngine().SetAndRunProcess(process);
		}
		ITapStateProcess TapStateProcess(){
			return TapProcessEngine().Process();
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
	public interface ITapStateHandler: IUISystemInputHandler{
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
}
