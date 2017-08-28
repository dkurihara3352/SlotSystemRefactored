using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
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
		IUIStateEngine<ISBActState> actStateEngine{
			get{
				if(m_actStateEngine == null)
					m_actStateEngine = new UIStateEngine<ISBActState>();
				return m_actStateEngine;
			}
		}
			IUIStateEngine<ISBActState> m_actStateEngine;
		public void SetActState(ISBActState state){
			actStateEngine.SetState(state);
			if(state == null && GetActProcess() != null)
				SetAndRunActProcess(null);
		}
		ISBActState curActState{
			get{return actStateEngine.GetCurState();}
		}
		ISBActState prevActState{
			get{return actStateEngine.GetPrevState();}
		}
		ISBActStateRepo actStateRepo{
			get{
				if(m_actStateRepo == null)
					m_actStateRepo = new SBActStateRepo(sb, tam);
				return m_actStateRepo;
			}
		}
			ISBActStateRepo m_actStateRepo;
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
		public bool IsActProcessRunning(){
			ISBActProcess actProcess = GetActProcess();
			if(actProcess != null)
				return actProcess.IsRunning();
			return false;
		}
		public void SetAndRunActProcess(ISBActProcess process){
			actProcEngine.SetAndRunProcess(process);
		}
		IUIProcessEngine<ISBActProcess> actProcEngine{
			get{
				if(m_actProcEngine == null)
					m_actProcEngine = new UIProcessEngine<ISBActProcess>();
				return m_actProcEngine;
			}
		}
			IUIProcessEngine<ISBActProcess> m_actProcEngine;
		public void SetActProcessEngine(IUIProcessEngine<ISBActProcess> engine){
			m_actProcEngine = engine;
		}
		public void ExpireActProcess(){
			ISBActProcess actProcess = GetActProcess();
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
		void WaitForAction();
		ISBActProcess GetActProcess();
		bool IsActProcessRunning();
		void SetAndRunActProcess(ISBActProcess process);
		void ExpireActProcess();
			System.Func<IEnumeratorFake> GetRemoveCoroutine();
			System.Func<IEnumeratorFake> GetAddCoroutine();
			System.Func<IEnumeratorFake> GetMoveWithinCoroutine();
		void OnPointerDown(PointerEventDataFake eventDataMock);
		void OnPointerUp(PointerEventDataFake eventDataMock);
		void OnDeselected(PointerEventDataFake eventDataMock);
		void OnEndDrag(PointerEventDataFake eventDataMock);
	}
}
