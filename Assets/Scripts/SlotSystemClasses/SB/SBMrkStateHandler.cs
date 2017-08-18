using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SBMrkStateHandler : ISBMrkStateHandler {
		ISlottable sb;
		public SBMrkStateHandler(ISlottable sb){
			this.sb = sb;
		}
		/* States */
			ISSEStateEngine<ISBMrkState> mrkStateEngine{
				get{
					if(m_markStateEngine == null)
						m_markStateEngine = new SSEStateEngine<ISBMrkState>();
					return m_markStateEngine;
				}
			}
				ISSEStateEngine<ISBMrkState> m_markStateEngine;
			public void SetMrkState(ISBMrkState state){
				mrkStateEngine.SetState(state);
				if(state == null && GetMrkProcess() != null)
					SetAndRunMrkProcess(null);
			}
			ISBMrkState curMrkState{
				get{return mrkStateEngine.GetCurState();}
			}
			ISBMrkState prevMrkState{
				get{return mrkStateEngine.GetPrevState();}
			}
			ISBMrkStateRepo mrkStateRepo{
				get{
					if(m_mrkStateRepo == null)
						m_mrkStateRepo = new SBMrkStateRepo(sb);
					return m_mrkStateRepo;
				}
			}
				ISBMrkStateRepo m_mrkStateRepo;
			public virtual void ClearCurMrkState(){
				SetMrkState(null);
			}
				public virtual bool IsMrkStateNull(){
					return curMrkState == null;
				}
				public virtual bool WasMrkStateNull(){
					return prevMrkState == null;
				}
			public virtual void Mark(){
				SetMrkState(markedState);
			}
				ISBMrkState markedState{
					get{return mrkStateRepo.GetMarkedState();}
				}
				public virtual bool IsMarked(){
					return curMrkState == markedState;
				}
				public virtual bool WasMarked(){
					return prevMrkState == markedState;
				}
			public virtual void Unmark(){
				SetMrkState(unmarkedState);
			}
				ISBMrkState unmarkedState{
					get{return mrkStateRepo.GetUnmarkedState();}
				}
				public virtual bool IsUnmarked(){
					return curMrkState == unmarkedState;
				}
				public virtual bool WasUnmarked(){
					return prevMrkState == markedState;
				}
		/* Process */
			public ISBMrkProcess GetMrkProcess(){
				return mrkProcEngine.GetProcess();
			}
			public void SetAndRunMrkProcess(ISBMrkProcess process){
				mrkProcEngine.SetAndRunProcess(process);
			}
			ISSEProcessEngine<ISBMrkProcess> mrkProcEngine{
				get{
					if(m_mrkProcEngine == null)
						m_mrkProcEngine = new SSEProcessEngine<ISBMrkProcess>();
					return m_mrkProcEngine;
				}
			}
				ISSEProcessEngine<ISBMrkProcess> m_mrkProcEngine;
			public void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine){
				m_mrkProcEngine = engine;
			}
			ISBMrkCoroutineRepo mrkCoroutineRepo{
				get{
					if(_mrkCoroutineRepo == null)
						_mrkCoroutineRepo = new SBMrkCoroutineRepo();
					return _mrkCoroutineRepo;
				}
			}
				ISBMrkCoroutineRepo _mrkCoroutineRepo;
			public void SetMrkCoroutineRepo(ISBMrkCoroutineRepo repo){
				_mrkCoroutineRepo = repo;
			}
			public System.Func<IEnumeratorFake> GetMarkCoroutine(){
				return mrkCoroutineRepo.GetMarkCoroutine();
			}
			public System.Func<IEnumeratorFake> GetUnmarkCoroutine(){
				return mrkCoroutineRepo.GetUnmarkCoroutine();
			}
	}
	public interface ISBMrkStateHandler{
		void ClearCurMrkState();
		bool IsMrkStateNull();
		bool WasMrkStateNull();
		void Mark();
		bool IsMarked();
		bool WasMarked();
		void Unmark();
		bool IsUnmarked();
		bool WasUnmarked();
		ISBMrkProcess GetMrkProcess();
		void SetAndRunMrkProcess(ISBMrkProcess process);
		System.Func<IEnumeratorFake> GetUnmarkCoroutine();
		System.Func<IEnumeratorFake> GetMarkCoroutine();
	}
}
