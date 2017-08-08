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
			void SetMrkState(ISBMrkState state){
				mrkStateEngine.SetState(state);
				if(state == null && mrkProcess != null)
					SetAndRunMrkProcess(null);
			}
			ISBMrkState curMrkState{
				get{return mrkStateEngine.curState;}
			}
			ISBMrkState prevMrkState{
				get{return mrkStateEngine.prevState;}
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
				public virtual bool isMrkStateNull{
					get{return curMrkState == null;}
				}
				public virtual bool wasMrkStateNull{
					get{return prevMrkState == null;}
				}
			public virtual void Mark(){
				SetMrkState(markedState);
			}
				public ISBMrkState markedState{
					get{return mrkStateRepo.markedState;}
				}
				public virtual bool isMarked{
					get{return curMrkState == markedState;}
				}
				public virtual bool wasMarked{
					get{return prevMrkState == markedState;}
				}
			public virtual void Unmark(){
				SetMrkState(unmarkedState);
			}
				public ISBMrkState unmarkedState{
					get{return mrkStateRepo.unmarkedState;}
				}
				public virtual bool isUnmarked{
					get{return curMrkState == unmarkedState;}
				}
				public virtual bool wasUnmarked{
					get{return prevMrkState == markedState;}
				}
			public ISBMrkProcess mrkProcess{
				get{return mrkProcEngine.process;}
			}
		/* Process */
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
			public System.Func<IEnumeratorFake> markCoroutine{
				get{return mrkCoroutineRepo.markCoroutine;}
			}
			public System.Func<IEnumeratorFake> unmarkCoroutine{
				get{return mrkCoroutineRepo.unmarkCoroutine;}
			}
	}
	public interface ISBMrkStateHandler{
		void ClearCurMrkState();
		bool isMrkStateNull{get;}
		bool wasMrkStateNull{get;}
		void Mark();
		ISBMrkState	markedState{get;}
		bool isMarked{get;}
		bool wasMarked{get;}
		void Unmark();
		ISBMrkState unmarkedState{get;}
		bool isUnmarked{get;}
		bool wasUnmarked{get;}
		ISBMrkProcess mrkProcess{get;}
		void SetAndRunMrkProcess(ISBMrkProcess process);
		System.Func<IEnumeratorFake> unmarkCoroutine{get;}
		System.Func<IEnumeratorFake> markCoroutine{get;}
	}
}
