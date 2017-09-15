using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISlotActStateEngine: IUISystemInputEngine{
		void WaitForAction();
			bool WasWaitingForAction();
			bool IsWaitingForAction();
		void WaitForPickUp();
			bool WasWaitingForPickUp();
			bool IsWaitingForPickUp();
		void WaitForPointerUp();
			bool WasWaitingForPointerUp();
			bool IsWaitingForPointerUp();
		void WaitForNextTouch();
			bool WasWaitingForNextTouch();
			bool IsWaitingForNextTouch();

		void PickUp();
		void SetAndRunActProcess(ISlotActProcess actProcess);
		IEnumeratorFake WaitForActionCoroutine();
		IEnumeratorFake WaitForPickUpCoroutine();
		IEnumeratorFake WaitForPointerUpCoroutine();
		IEnumeratorFake WaitForNextTouchCoroutine();
	}
	public class SlotActStateEngine : ISlotActStateEngine{
		public SlotActStateEngine(ISlot slot){
			SetSlot(slot);
			SetActStateSwitch(new UIStateSwitch<ISlotActState>());
			SetActProcessSwitch(new UIProcessSwitch<ISlotActProcess>());
			InitializeStates();
		}
		ISlot Slot(){
			Debug.Assert(_slot != null);
			return _slot;
		}
		void SetSlot(ISlot sb){
			_slot = sb;
		}
			ISlot _slot;
		IUIStateSwitch<ISlotActState> ActStateSwitch(){
			Debug.Assert(_actStateSwitch != null);
			return _actStateSwitch;
		}
		void SetActStateSwitch(IUIStateSwitch<ISlotActState> stateSwitch){
			_actStateSwitch = stateSwitch;
		}
			IUIStateSwitch<ISlotActState> _actStateSwitch;
		public void SetActState(ISlotActState state){
			ActStateSwitch().SwitchTo(state);
			if(state == null && ActProcess() != null)
				SetAndRunActProcess(null);
		}
		ISlotActState CurState(){
			return ActStateSwitch().CurState();
		}
		ISlotActState PrevState(){
			return ActStateSwitch().PrevState();
		}

		
		void InitializeStates(){
			_waitingForActionState = new SlotWaitingForActionState(this);
			_waitingForPickUpState = new SlotWaitingForPickUpState(this);
			_waitingForPointerUpState = new SlotWaitingForPointerUpState(this);
			_waitingForNextTouchState = new SlotWaitingForNextTouchState(this);
		}

		public void WaitForAction(){
			SetActState( WaitingForActionState());
		}
			ISlotActState WaitingForActionState(){
				Debug.Assert(_waitingForActionState != null);
				return _waitingForActionState;
			}
			ISlotActState _waitingForActionState;
			public bool WasWaitingForAction(){
				return PrevState() == WaitingForActionState();
			}
			public bool IsWaitingForAction(){
				return CurState() == WaitingForActionState();
			}
		public void WaitForPickUp(){
			SetActState( WaitingForPickUpState());
		}
			ISlotActState WaitingForPickUpState(){
				Debug.Assert(_waitingForPickUpState != null);
				return _waitingForPickUpState;
			}
			ISlotActState _waitingForPickUpState;
			public bool WasWaitingForPickUp(){
				return PrevState() == WaitingForPickUpState();
			}
			public bool IsWaitingForPickUp(){
				return CurState() == WaitingForPickUpState();
			}
		public void WaitForPointerUp(){
			SetActState( WaitingForPointerUpState());
		}
			ISlotActState WaitingForPointerUpState(){
				Debug.Assert(_waitingForPointerUpState != null);
				return _waitingForPointerUpState;
			}
			ISlotActState _waitingForPointerUpState;
			public bool WasWaitingForPointerUp(){
				return PrevState() == WaitingForPointerUpState();
			}
			public bool IsWaitingForPointerUp(){
				return CurState() == WaitingForPointerUpState();
			}
		public void WaitForNextTouch(){
			SetActState( WaitingForNextTouchState());
		}
			ISlotActState WaitingForNextTouchState(){
				Debug.Assert(_waitingForNextTouchState != null);
				return _waitingForNextTouchState;
			}
			ISlotActState _waitingForNextTouchState;
			public bool WasWaitingForNextTouch(){
				return PrevState() == WaitingForNextTouchState();
			}
			public bool IsWaitingForNextTouch(){
				return CurState() == WaitingForNextTouchState();
			}


		public void PickUp(){
			Slot().PickUp();
		}


		public ISlotActProcess ActProcess(){
			return ActProcSwitch().Process();
		}
		public bool IsActProcessRunning(){
			ISlotActProcess actProcess = ActProcess();
			if(actProcess != null)
				return actProcess.IsRunning();
			return false;
		}
		public void SetAndRunActProcess(ISlotActProcess process){
			ActProcSwitch().SetAndRunProcess(process);
		}
		IUIProcessSwitch<ISlotActProcess> ActProcSwitch(){
			Debug.Assert(_actProcessSwitch != null);
			return _actProcessSwitch;
		}
			IUIProcessSwitch<ISlotActProcess> _actProcessSwitch;
		public void SetActProcessSwitch(IUIProcessSwitch<ISlotActProcess> processSwitch){
			_actProcessSwitch = processSwitch;
		}
		public void ExpireActProcess(){
			ISlotActProcess actProcess = ActProcess();
			if(actProcess != null)
				actProcess.Expire();
		}


		public IEnumeratorFake WaitForActionCoroutine(){
			return null;
		}
		public IEnumeratorFake WaitForPickUpCoroutine(){
			return null;
		}
		public IEnumeratorFake WaitForPointerUpCoroutine(){
			return null;
		}
		public IEnumeratorFake WaitForNextTouchCoroutine(){
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
