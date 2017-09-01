using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SlotActStateHandler : ISlotActStateHandler {
		ISlot slot{
			get{
				Debug.Assert(_slot != null);
				return _slot;
			}
		}
		ISlot _slot;
		public SlotActStateHandler(ISlot slot){
			_slot = slot;
		}
		IUIStateEngine<ISlotActState> actStateEngine{
			get{
				if(_actStateEngine == null)
					_actStateEngine = new UIStateEngine<ISlotActState>();
				return _actStateEngine;
			}
		}
			IUIStateEngine<ISlotActState> _actStateEngine;
		void SetActState(ISlotActState state){
			actStateEngine.SetState(state);
			if(state == null && GetActProcess() != null)
				SetAndRunActProcess(null);
		}
		ISlotActState curActState{
			get{return actStateEngine.CurState();}
		}
		ISlotActState prevActState{
			get{return actStateEngine.PrevState();}
		}
		ISlotActStateRepo actStateRepo{
			get{
				if(_actStateRepo == null)
					_actStateRepo = new SlotActStateRepo(slot);
				return _actStateRepo;
			}
		}
			ISlotActStateRepo _actStateRepo;
		public void ClearCurActState(){
			SetActState(null);
		}
			public bool WasActStateNull(){
				return prevActState == null;
			}
			public bool IsActStateNull(){
				return curActState == null;
			}
		public virtual void WaitForAction(){
			SetActState(waitForActionState);
		}
			ISlotActState waitForActionState{
				get{return actStateRepo.GetWaitForActionState();}
			}
			public virtual bool IsWaitingForAction(){
				return curActState == waitForActionState;
			}
			public virtual bool WasWaitingForAction(){
				return prevActState == waitForActionState;
			}
		public virtual void WaitForPointerUp(){
			SetActState(waitForPointerUpState);
		}
			ISlotActState waitForPointerUpState{
				get{return actStateRepo.GetWaitForPointerUpState();}
			}
			public virtual bool IsWaitingForPointerUp(){
				return curActState == waitForPointerUpState;
			}
			public virtual bool WasWaitingForPointerUp(){
				return prevActState == waitForPointerUpState;
			}
		public virtual void WaitForPickUp(){
			SetActState(waitForPickUpState);
		}
			ISlotActState waitForPickUpState{
				get{return actStateRepo.GetWaitForPickUpState();}
			}
			public virtual bool IsWaitingForPickUp(){
				return curActState == waitForPickUpState;
			}
			public virtual bool WasWaitingForPickUp(){
				return prevActState == waitForPickUpState;
			}
		public virtual void WaitForNextTouch(){
			SetActState(waitForNextTouchState);
		}
			ISlotActState waitForNextTouchState{
				get{return actStateRepo.GetWaitForNextTouchState();}
			}
			public virtual bool IsWaitingForNextTouch(){
				return curActState == waitForNextTouchState;
			}
			public virtual bool WasWaitingForNextTouch(){
				return prevActState == waitForNextTouchState;
			}
		public virtual void PickUp(){
			SetActState(pickedUpState);
		}
		public void SetPickedUpState(){
			SetActState(pickedUpState);
		}
			ISlotActState pickedUpState{
				get{return actStateRepo.GetPickingUpState();}
			}
			public virtual bool IsPickingUp(){
				return curActState == pickedUpState;
			}
			public virtual bool WasPickingUp(){
				return prevActState == pickedUpState;
			}
		public ISlotActProcess GetActProcess(){
			return actProcEngine.GetProcess();
		}
		public bool IsActProcessRunning(){
			ISlotActProcess actProcess = GetActProcess();
			if(actProcess != null)
				return actProcess.IsRunning();
			return false;
		}
		public void SetAndRunActProcess(ISlotActProcess process){
			actProcEngine.SetAndRunProcess(process);
		}
		IUIProcessEngine<ISlotActProcess> actProcEngine{
			get{
				if(m_actProcEngine == null)
					m_actProcEngine = new UIProcessEngine<ISlotActProcess>();
				return m_actProcEngine;
			}
		}
			IUIProcessEngine<ISlotActProcess> m_actProcEngine;
		public void SetActProcessEngine(IUIProcessEngine<ISlotActProcess> engine){
			m_actProcEngine = engine;
		}
		public void ExpireActProcess(){
			ISlotActProcess actProcess = GetActProcess();
			if(actProcess != null)
				actProcess.Expire();
		}
		ISlotActCoroutineRepo coroutineRepo{
			get{
				if(_coroutineRepo == null)
					_coroutineRepo = new SlotActCoroutineRepo();
				return _coroutineRepo;
			}
		}
			ISlotActCoroutineRepo _coroutineRepo;
		public void SetCoroutineRepo(ISlotActCoroutineRepo coroutineRepo){
			_coroutineRepo = coroutineRepo;
		}
		public System.Func<IEnumeratorFake> WaitForPointerUpCoroutine(){
			return coroutineRepo.GetWaitForPointerUpCoroutine();
		}
		public System.Func<IEnumeratorFake> WaitForPickUpCoroutine(){
			return coroutineRepo.GetWaitForPickUpCoroutine();
		}
		public System.Func<IEnumeratorFake> PickUpCoroutine(){
			return coroutineRepo.GetPickUpCoroutine();
		}
		public System.Func<IEnumeratorFake> WaitForNextTouchCoroutine(){
			return coroutineRepo.GetWaitForNextTouchCoroutine();
		}
		public void OnPointerDown(){
			curActState.OnPointerDown();
		}
		public void OnPointerUp(){
			curActState.OnPointerUp();
		}
		public void OnEndDrag(){
			curActState.OnEndDrag();
		}
		public void OnDeselected(){
			curActState.OnDeselected();
		}
	}
	public interface ISlotActStateHandler{
		void ClearCurActState();
			bool WasActStateNull();
			bool IsActStateNull();
		void WaitForAction();
			bool IsWaitingForAction();
			bool WasWaitingForAction();
		void WaitForPointerUp();
			bool IsWaitingForPointerUp();
			bool WasWaitingForPointerUp();
		void WaitForPickUp();
			bool IsWaitingForPickUp();
			bool WasWaitingForPickUp();
		void WaitForNextTouch();
			bool IsWaitingForNextTouch();
			bool WasWaitingForNextTouch();
		void PickUp();
		void SetPickedUpState();
			bool IsPickingUp();
			bool WasPickingUp();
		ISlotActProcess GetActProcess();
		void SetAndRunActProcess(ISlotActProcess process);
		bool IsActProcessRunning();
		void ExpireActProcess();
		Func<IEnumeratorFake> WaitForPointerUpCoroutine();
		Func<IEnumeratorFake> WaitForPickUpCoroutine();
		Func<IEnumeratorFake> WaitForNextTouchCoroutine();
		Func<IEnumeratorFake> PickUpCoroutine();
		void OnPointerDown();
		void OnPointerUp();
		void OnEndDrag();
		void OnDeselected();
	}
}
