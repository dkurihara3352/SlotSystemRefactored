using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBEqpStateHandler : ISBEqpStateHandler {
		ISlottable sb{
			get{
				if(_sb != null)
					return _sb;
				else
					throw new System.InvalidOperationException("sb not set");
			}
		}
			ISlottable _sb;
		public SBEqpStateHandler(ISlottable sb){
			_sb = sb;
		}
		/* States */
			ISSEStateEngine<ISBEqpState> eqpStateEngine{
				get{
					if(m_eqpStateEngine == null)
						m_eqpStateEngine = new SSEStateEngine<ISBEqpState>();
					return m_eqpStateEngine;
				}
			}
				ISSEStateEngine<ISBEqpState> m_eqpStateEngine;
			public void SetEqpState(ISBEqpState state){
				eqpStateEngine.SetState(state);
				if(state == null && eqpProcess != null)
					SetAndRunEqpProcess(null);
			}
			ISBEqpState curEqpState{
				get{return eqpStateEngine.curState;}
			}
			ISBEqpState prevEqpState{
				get{return eqpStateEngine.prevState;}
			}
			ISBEqpStateRepo eqpStateRepo{
				get{
					if(m_eqpStateRepo == null)
						m_eqpStateRepo = new SBEqpStateRepo(sb);
					return m_eqpStateRepo;
				}
			}
				ISBEqpStateRepo m_eqpStateRepo;
			public virtual void ClearCurEqpState(){
				SetEqpState(null);
			}
				public virtual bool isEqpStateNull{
					get{return curEqpState == null;}
				}
				public virtual bool wasEqpStateNull{
					get{return prevEqpState == null;}
				}				
			public virtual void Equip(){
				SetEqpState(equippedState);
			}
				public ISBEqpState equippedState{
					get{return eqpStateRepo.equippedState;}
				}
				public virtual bool isEquipped{
					get{ return curEqpState == equippedState;}
				}
				public virtual bool wasEquipped{
					get{return prevEqpState == equippedState;}
				}
			public virtual void Unequip(){
				SetEqpState(unequippedState);
			}
				public ISBEqpState unequippedState{
					get{return eqpStateRepo.unequippedState;}
				}
				public virtual bool isUnequipped{
					get{ return curEqpState == unequippedState;}
				}
				public virtual bool wasUnequipped{
					get{ return prevEqpState == unequippedState;}
				}
		/*	Equip Process	*/
			public ISBEqpProcess eqpProcess{
				get{return eqpProcEngine.process;}
			}
			public void SetAndRunEqpProcess(ISBEqpProcess process){
				eqpProcEngine.SetAndRunProcess(process);
			}
			ISSEProcessEngine<ISBEqpProcess> eqpProcEngine{
				get{
					if(m_eqpProcEngine == null)
						m_eqpProcEngine = new SSEProcessEngine<ISBEqpProcess>();
					return m_eqpProcEngine;
				}
			}
				ISSEProcessEngine<ISBEqpProcess> m_eqpProcEngine;
			public void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine){
				m_eqpProcEngine = engine;
			}
			ISBEqpCoroutineRepo eqpCoroutineRepo{
				get{
					if(_eqpCoroutineRepo == null)
						_eqpCoroutineRepo = new SBEqpCoroutineRepo();
					return _eqpCoroutineRepo;
				}
			}
				ISBEqpCoroutineRepo _eqpCoroutineRepo;
			public void SetEqpCoroutineRepo(ISBEqpCoroutineRepo repo){
				_eqpCoroutineRepo = repo;
			}
			public System.Func<IEnumeratorFake> equipCoroutine{
				get{return eqpCoroutineRepo.equipCoroutine;}
			}
			public System.Func<IEnumeratorFake> unequipCoroutine{
				get{return eqpCoroutineRepo.unequipCoroutine;}
			}
	}
	public interface ISBEqpStateHandler{
		/* Eqp States */
			void ClearCurEqpState();
			bool isEqpStateNull{get;}
			bool wasEqpStateNull{get;}
			void Equip();
			ISBEqpState equippedState{get;}
			bool isEquipped{get;}
			bool wasEquipped{get;}
			void Unequip();
			ISBEqpState unequippedState{get;}
			bool isUnequipped{get;}
			bool wasUnequipped{get;}
			ISBEqpProcess eqpProcess{get;}
			void SetAndRunEqpProcess(ISBEqpProcess process);
			System.Func<IEnumeratorFake> unequipCoroutine{get;}
			System.Func<IEnumeratorFake> equipCoroutine{get;}
	}
}
