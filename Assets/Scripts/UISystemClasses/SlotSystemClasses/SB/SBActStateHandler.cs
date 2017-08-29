using System;
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
					m_actStateRepo = new SBActStateRepo(sb);
				return m_actStateRepo;
			}
		}
			ISBActStateRepo m_actStateRepo;
		public virtual void WaitForAction(){
			SetActState(waitingForActionState);
		}
			ISBActState waitingForActionState{
				get{return actStateRepo.WaitingForActionState();}
			}
			public virtual bool IsWaitingForAction(){
				return curActState == waitingForActionState;
			}
			public virtual bool WasWaitingForAction(){
				return prevActState == waitingForActionState;
			}
		public virtual void Travel(ISlotGroup slotGroup, ISlot slot){
			SetActState(travellingState);
		}
			ISBActState travellingState{
				get{return actStateRepo.TravellingState();}
			}
			public virtual bool IsTravelling(){
				return curActState == travellingState;
			}
			public virtual bool wasTravelling(){
				return prevActState == travellingState;
			}
		public virtual void Lift(){
			SetActState(liftingState);
		}
			ISBActState liftingState{
				get{return actStateRepo.LiftingState();}
			}
			public virtual bool IsLifting(){
				return curActState == liftingState;
			}
			public virtual bool WasLifting(){
				return prevActState == liftingState;
			}
		public virtual void Land(){
			SetActState(landingState);
		}
			ISBActState landingState{
				get{return actStateRepo.LandingState();}
			}
			public virtual bool IsLanding(){
				return curActState == landingState;
			}
			public virtual bool WasLanding(){
				return prevActState == landingState;
			}
		public virtual void Appear(){
			SetActState(appearingState);
		}
			ISBActState appearingState{
				get{return actStateRepo.AppearingState();}
			}
			public virtual bool IsAppearing(){
				return curActState == appearingState;
			}
			public virtual bool WasAppearing(){
				return prevActState == appearingState;
			}
		public virtual void Disappear(){
			SetActState(disappearingState);
		}
			ISBActState disappearingState{
				get{return actStateRepo.DisappearingState();}
			}
			public virtual bool IsMovingWithin(){
				return curActState == disappearingState;
			}
			public virtual bool WasMovingWithin(){
				return prevActState == disappearingState;
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
		public Func<IEnumeratorFake> WaitForActionCoroutine(){
			return coroutineRepo.WaitForActionCoroutine();
		}
		public Func<IEnumeratorFake> TravelCoroutine(){
			return coroutineRepo.TravelCoroutine();
		}
		public Func<IEnumeratorFake> LiftCoroutine(){
			return coroutineRepo.LiftCoroutine();
		}
		public Func<IEnumeratorFake> LandCoroutine(){
			return coroutineRepo.LandCoroutine();
		}
		public Func<IEnumeratorFake> AppearCoroutine(){
			return coroutineRepo.AppearCoroutine();
		}
		public Func<IEnumeratorFake> DisappearCoroutine(){
			return coroutineRepo.DisappearCoroutine();
		}
	}
	public interface ISBActStateHandler{
		void WaitForAction();
		void Travel(ISlotGroup sg, ISlot slot);
		void Lift();
		void Land();
		void Appear();
		void Disappear();
		bool IsActProcessRunning();
		void ExpireActProcess();
		void SetAndRunActProcess(ISBActProcess actProcess);
		Func<IEnumeratorFake> WaitForActionCoroutine();
		Func<IEnumeratorFake> TravelCoroutine();
		Func<IEnumeratorFake> LiftCoroutine();
		Func<IEnumeratorFake> LandCoroutine();
		Func<IEnumeratorFake> AppearCoroutine();
		Func<IEnumeratorFake> DisappearCoroutine();
	}
}
