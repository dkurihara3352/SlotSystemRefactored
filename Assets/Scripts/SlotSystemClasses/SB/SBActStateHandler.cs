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
		ITransactionManager tam{
			get{
				if(_tam != null)
					return _tam;
				else
					throw new System.InvalidOperationException("tam not set");
			}
		}
			ITransactionManager _tam;
		public SBActStateHandler(ISlottable sb, ITransactionManager tam){
			_sb = sb;
			_tam = tam;
		}
		ISSEStateEngine<ISBActState> actStateEngine{
			get{
				if(m_actStateEngine == null)
					m_actStateEngine = new SSEStateEngine<ISBActState>();
				return m_actStateEngine;
			}
		}
			ISSEStateEngine<ISBActState> m_actStateEngine;
		public void SetActState(ISBActState state){
			actStateEngine.SetState(state);
			if(state == null && GetActProcess() != null)
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
					m_actStateRepo = new SBActStateRepo(sb.GetSelStateHandler(), sb, tam, sb.taCache);
				return m_actStateRepo;
			}
		}
			ISBActStateRepo m_actStateRepo;
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
			ISBActState waitForActionState{
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
			ISBActState waitForPointerUpState{
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
			ISBActState waitForPickUpState{
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
			ISBActState waitForNextTouchState{
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
			sb.SetPickedAmount(1);
		}
		public void SetPickedUpState(){
			SetActState(pickedUpState);
		}
			ISBActState pickedUpState{
				get{return actStateRepo.GetPickingUpState();}
			}
			public virtual bool IsPickingUp(){
				return curActState == pickedUpState;
			}
			public virtual bool WasPickingUp(){
				return prevActState == pickedUpState;
			}
		public virtual void Remove(){
			SetActState(removedState);
		}
			ISBActState removedState{
				get{return actStateRepo.GetRemovedState();}
			}
			public virtual bool IsRemoving(){
				return curActState == removedState;
			}
			public virtual bool WasRemoving(){
				return prevActState == removedState;
			}
		public virtual void Add(){
			SetActState(addedState);
		}
			ISBActState addedState{
				get{return actStateRepo.GetAddedState();}
			}
			public virtual bool IsAdding(){
				return curActState == addedState;
			}
			public virtual bool WasAdding(){
				return prevActState == addedState;
			}
		public virtual void MoveWithin(){
			SetActState(moveWithinState);
		}
			ISBActState moveWithinState{
				get{return actStateRepo.GetMoveWithinState();}
			}
			public virtual bool IsMovingWithin(){
				return curActState == moveWithinState;
			}
			public virtual bool WasMovingWithin(){
				return prevActState == moveWithinState;
			}
		public ISBActProcess GetActProcess(){
			return actProcEngine.GetProcess();
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
			ISBActProcess actProcess = GetActProcess();
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
		public System.Func<IEnumeratorFake> GetWaitForPointerUpCoroutine(){
			return coroutineRepo.GetWaitForPointerUpCoroutine();
		}
		public System.Func<IEnumeratorFake> GetWaitForPickUpCoroutine(){
			return coroutineRepo.GetWaitForPickUpCoroutine();
		}
		public System.Func<IEnumeratorFake> GetPickUpCoroutine(){
			return coroutineRepo.GetPickUpCoroutine();
		}
		public System.Func<IEnumeratorFake> GetWaitForNextTouchCoroutine(){
			return coroutineRepo.GetWaitForNextTouchCoroutine();
		}
		public System.Func<IEnumeratorFake> GetRemoveCoroutine(){
			return coroutineRepo.GetRemoveCoroutine();
		}
		public System.Func<IEnumeratorFake> GetAddCoroutine(){
			return coroutineRepo.GetAddCoroutine();
		}
		public System.Func<IEnumeratorFake> GetMoveWithinCoroutine(){
			return coroutineRepo.GetMoveWithinCoroutine();
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
		void Remove();
			bool IsRemoving();
			bool WasRemoving();
		void Add();
			bool IsAdding();
			bool WasAdding();
		void MoveWithin();
			bool IsMovingWithin();
			bool WasMovingWithin();
		ISBActProcess GetActProcess();
		void SetAndRunActProcess(ISBActProcess process);
		void ExpireActProcess();
			System.Func<IEnumeratorFake> GetWaitForPointerUpCoroutine();
			System.Func<IEnumeratorFake> GetWaitForPickUpCoroutine();
			System.Func<IEnumeratorFake> GetPickUpCoroutine();
			System.Func<IEnumeratorFake> GetWaitForNextTouchCoroutine();
			System.Func<IEnumeratorFake> GetRemoveCoroutine();
			System.Func<IEnumeratorFake> GetAddCoroutine();
			System.Func<IEnumeratorFake> GetMoveWithinCoroutine();
		void OnPointerDown(PointerEventDataFake eventDataMock);
		void OnPointerUp(PointerEventDataFake eventDataMock);
		void OnDeselected(PointerEventDataFake eventDataMock);
		void OnEndDrag(PointerEventDataFake eventDataMock);
	}
}
