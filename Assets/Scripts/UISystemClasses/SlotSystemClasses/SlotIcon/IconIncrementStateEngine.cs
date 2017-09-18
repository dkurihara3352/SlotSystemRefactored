using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public interface IIconIncrementStateEngine{
		void WaitForIncrement();
		bool WasWaitingForIncrement();
		bool IsWaitingForIncrement();

		void GetReadyForIncrement();
		bool WasReadyForIncrement();
		bool IsReadyForIncrement();

		void SetAndRunIconProcess( IconProcess process);
		IEnumeratorFake WaitForIncrementCoroutine();
		IEnumeratorFake GetReadyForIncrementCoroutine();
	}
	public class IconIncrementStateEngine : IIconIncrementStateEngine {
		public IconIncrementStateEngine( IHoverIcon slotIcon){
			SetSlotIcon( slotIcon);
			SetStateSwitch( new IconIncrementStateSwitch());
			InitializeStates();
		}


		IHoverIcon SlotIcon(){
			return _slotIcon;
		}
		void SetSlotIcon( IHoverIcon slotIcon){
			_slotIcon = slotIcon;
		}
			IHoverIcon _slotIcon;


		ISwitchableStateSwitch<IIconIncrementState> StateSwitch(){
			return _stateSwitch;
		}
		void SetStateSwitch( ISwitchableStateSwitch<IIconIncrementState> stateSwitch){
			_stateSwitch = stateSwitch;
		}
		ISwitchableStateSwitch<IIconIncrementState> _stateSwitch;


		public void InitializeStates(){
			_waitingForIncrementState = new IconWaitingForIncrementState( this);
			_readyForIncrementState = new IconReadyForIncrementState( this);
		}


		public void WaitForIncrement(){
			StateSwitch().SwitchTo( WaitingForIncrementState());
		}
		IIconIncrementState WaitingForIncrementState(){
			return _waitingForIncrementState;
		}
			IIconIncrementState _waitingForIncrementState;
		public bool WasWaitingForIncrement(){
			return StateSwitch().PrevState() == WaitingForIncrementState();
		}
		public bool IsWaitingForIncrement(){
			return StateSwitch().CurState() == WaitingForIncrementState();
		}


		public void GetReadyForIncrement(){
			StateSwitch().SwitchTo( ReadyForIncrementState());
		}
		IIconIncrementState ReadyForIncrementState(){
			return _readyForIncrementState;
		}
			IIconIncrementState _readyForIncrementState;
		public bool WasReadyForIncrement(){
			return StateSwitch().PrevState() == ReadyForIncrementState();
		}
		public bool IsReadyForIncrement(){
			return StateSwitch().CurState() == ReadyForIncrementState();
		}
		

		IUIProcessSwitch<IconProcess> IconProcessSwitch(){
			return _iconProcessSwitch;
		}
		void SetProcessSwitch( IUIProcessSwitch<IconProcess> procSwitch){
			_iconProcessSwitch = procSwitch;
		}
		IUIProcessSwitch<IconProcess> _iconProcessSwitch;
		public void SetAndRunIconProcess( IconProcess process){
			IconProcessSwitch().SetAndRunProcess( process);
		}


		public IEnumeratorFake WaitForIncrementCoroutine(){
			return null;
		}
		public IEnumeratorFake GetReadyForIncrementCoroutine(){
			return null;
		}
	}
}
