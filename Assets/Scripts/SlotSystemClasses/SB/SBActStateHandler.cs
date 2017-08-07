using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SBActStateHandler : ISBActStateHandler{
		ISlottable sb{
			get{
				if(_sb != null)
					return _sb;
				else
					throw new System.InvalidOperationException("sb not set");
			}
		}
			ISlottable _sb;
		public SBActStateHandler(ISlottable sb){
			_sb = sb;
		}
		ISSEStateEngine<ISBActState> actStateEngine{
			get{
				if(m_actStateEngine == null)
					m_actStateEngine = new SSEStateEngine<ISBActState>();
				return m_actStateEngine;
			}
		}
			ISSEStateEngine<ISBActState> m_actStateEngine;
		void SetActState(ISBActState state){
			actStateEngine.SetState(state);
			if(state == null && actProcess != null)
				SetAndRunActProcess(null);
		}
		ISBActState curActState{
			get{return actStateEngine.curState;}
		}
		ISBActState prevActState{
			get{return actStateEngine.prevState;}
		}
		ISBActStateRepo actStateRepo{
			get{
				if(m_actStateRepo == null)
					m_actStateRepo = new SBActStateRepo(sb);
				return m_actStateRepo;
			}
		}
			ISBActStateRepo m_actStateRepo;
		public void ClearCurActState(){
			SetActState(null);
		}
			public bool wasActStateNull{
				get{return prevActState == null;}
			}
			public bool isActStateNull{
				get{return curActState == null;}
			}
		public virtual void WaitForAction(){
			SetActState(waitForActionState);
		}
			public ISBActState waitForActionState{
				get{return actStateRepo.waitForActionState;}
			}
			public virtual bool isWaitingForAction{
				get{return curActState == waitForActionState;}
			}
			public virtual bool wasWaitingForAction{
				get{return prevActState == waitForActionState;}
			}
		public virtual void WaitForPointerUp(){
			SetActState(waitForPointerUpState);
		}
			public ISBActState waitForPointerUpState{
				get{return actStateRepo.waitForPointerUpState;}
			}
			public virtual bool isWaitingForPointerUp{
				get{return curActState == waitForPointerUpState;}
			}
			public virtual bool wasWaitingForPointerUp{
				get{return prevActState == waitForPointerUpState;}
			}
		public virtual void WaitForPickUp(){
			SetActState(waitForPickUpState);
		}
			public ISBActState waitForPickUpState{
				get{return actStateRepo.waitForPickUpState;}
			}
			public virtual bool isWaitingForPickUp{
				get{return curActState == waitForPickUpState;}
			}
			public virtual bool wasWaitingForPickUp{
				get{return prevActState == waitForPickUpState;}
			}
		public virtual void WaitForNextTouch(){
			SetActState(waitForNextTouchState);
		}
			public ISBActState waitForNextTouchState{
				get{return actStateRepo.waitForNextTouchState;}
			}
			public virtual bool isWaitingForNextTouch{
				get{return curActState == waitForNextTouchState;}
			}
			public virtual bool wasWaitingForNextTouch{
				get{return prevActState == waitForNextTouchState;}
			}
		public virtual void PickUp(){
			SetActState(pickedUpState);
			sb.pickedAmount = 1;
		}
		public void SetPickedUpState(){
			SetActState(pickedUpState);
		}
			public ISBActState pickedUpState{
				get{return actStateRepo.pickingUpState;}
			}
			public virtual bool isPickingUp{
				get{return curActState == pickedUpState;}
			}
			public virtual bool wasPickingUp{
				get{return prevActState == pickedUpState;}
			}
		public virtual void Remove(){
			SetActState(removedState);
		}
			public ISBActState removedState{
				get{return actStateRepo.removedState;}
			}
			public virtual bool isRemoving{
				get{return curActState == removedState;}
			}
			public virtual bool wasRemoving{
				get{return prevActState == removedState;}
			}
		public virtual void Add(){
			SetActState(addedState);
		}
			public ISBActState addedState{
				get{return actStateRepo.addedState;}
			}
			public virtual bool isAdding{
				get{return curActState == addedState;}
			}
			public virtual bool wasAdding{
				get{return prevActState == addedState;}
			}
		public virtual void MoveWithin(){
			SetActState(moveWithinState);
		}
			public ISBActState moveWithinState{
				get{return actStateRepo.moveWithinState;}
			}
			public virtual bool isMovingWithin{
				get{return curActState == moveWithinState;}
			}
			public virtual bool wasMovingWithin{
				get{return prevActState == moveWithinState;}
			}
		public ISBActProcess actProcess{
			get{return actProcEngine.process;}
		}
		public void SetAndRunActProcess(ISBActProcess process){
			actProcEngine.SetAndRunProcess(process);
		}
		ISSEProcessEngine<ISBActProcess> actProcEngine{
			get{
				if(m_actProcEngine == null)
					m_actProcEngine = new SSEProcessEngine<ISBActProcess>();
				return m_actProcEngine;
			}
		}
			ISSEProcessEngine<ISBActProcess> m_actProcEngine;
		public void SetActProcessEngine(ISSEProcessEngine<ISBActProcess> engine){
			m_actProcEngine = engine;
		}
		public void ExpireActProcess(){
			if(actProcess != null)
				actProcess.Expire();
		}
		ISBActCoroutineRepo coroutineRepo{
			get{
				if(_coroutineRepo == null)
					_coroutineRepo = new SBActCoroutineRepo();
				return _coroutineRepo;
			}
		}
			ISBActCoroutineRepo _coroutineRepo;
		public void SetCoroutineRepo(ISBActCoroutineRepo coroutineRepo){
			_coroutineRepo = coroutineRepo;
		}
		public System.Func<IEnumeratorFake> waitForPointerUpCoroutine{
			get{return coroutineRepo.waitForPointerUpCoroutine;}
		}
		public System.Func<IEnumeratorFake> waitForPickUpCoroutine{
			get{return coroutineRepo.waitForPickUpCoroutine;}
		}
		public System.Func<IEnumeratorFake> pickUpCoroutine{
			get{return coroutineRepo.pickUpCoroutine;}
		}
		public System.Func<IEnumeratorFake> waitForNextTouchCoroutine{
			get{return coroutineRepo.waitForNextTouchCoroutine;}
		}
		public System.Func<IEnumeratorFake> removeCoroutine{
			get{return coroutineRepo.removeCoroutine;}
		}
		public System.Func<IEnumeratorFake> addCoroutine{
			get{return coroutineRepo.addCoroutine;}
		}
		public System.Func<IEnumeratorFake> moveWithinCoroutine{
			get{return coroutineRepo.moveWithinCoroutine;}
		}
		public void OnPointerDown(PointerEventDataFake eventDataMock){
			curActState.OnPointerDown();
		}
		public void OnPointerUp(PointerEventDataFake eventDataMock){
			curActState.OnPointerUp();
		}
		public void OnDeselected(PointerEventDataFake eventDataMock){
			curActState.OnDeselected();
		}
		public void OnEndDrag(PointerEventDataFake eventDataMock){
			curActState.OnEndDrag();
		}
	}
	public interface ISBActStateHandler{
		void ClearCurActState();
			bool wasActStateNull{get;}
			bool isActStateNull{get;}
		void WaitForAction();
			ISBActState waitForActionState{get;}
			bool isWaitingForAction{get;}
			bool wasWaitingForAction{get;}
		void WaitForPointerUp();
			ISBActState waitForPointerUpState{get;}
			bool isWaitingForPointerUp{get;}
			bool wasWaitingForPointerUp{get;}
		void WaitForPickUp();
			ISBActState waitForPickUpState{get;}
			bool isWaitingForPickUp{get;}
			bool wasWaitingForPickUp{get;}
		void WaitForNextTouch();
			ISBActState waitForNextTouchState{get;}
			bool isWaitingForNextTouch{get;}
			bool wasWaitingForNextTouch{get;}
		void PickUp();
		void SetPickedUpState();
			ISBActState pickedUpState{get;}
			bool isPickingUp{get;}
			bool wasPickingUp{get;}
		void Remove();
			ISBActState removedState{get;}
			bool isRemoving{get;}
			bool wasRemoving{get;}
		void Add();
			ISBActState addedState{get;}
			bool isAdding{get;}
			bool wasAdding{get;}
		void MoveWithin();
			ISBActState moveWithinState{get;}
			bool isMovingWithin{get;}
			bool wasMovingWithin{get;}
		ISBActProcess actProcess{get;}
		void SetAndRunActProcess(ISBActProcess process);
		void ExpireActProcess();
			System.Func<IEnumeratorFake> waitForPointerUpCoroutine{get;}
			System.Func<IEnumeratorFake> waitForPickUpCoroutine{get;}
			System.Func<IEnumeratorFake> pickUpCoroutine{get;}
			System.Func<IEnumeratorFake> waitForNextTouchCoroutine{get;}
			System.Func<IEnumeratorFake> removeCoroutine{get;}
			System.Func<IEnumeratorFake> addCoroutine{get;}
			System.Func<IEnumeratorFake> moveWithinCoroutine{get;}
		void OnPointerDown(PointerEventDataFake eventDataMock);
		void OnPointerUp(PointerEventDataFake eventDataMock);
		void OnDeselected(PointerEventDataFake eventDataMock);
		void OnEndDrag(PointerEventDataFake eventDataMock);
	}
}
