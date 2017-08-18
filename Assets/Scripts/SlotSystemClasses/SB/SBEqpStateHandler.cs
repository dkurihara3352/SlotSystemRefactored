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
				if(state == null && GetEqpProcess() != null)
					SetAndRunEqpProcess(null);
			}
			ISBEqpState curEqpState{
				get{return eqpStateEngine.GetCurState();}
			}
			ISBEqpState prevEqpState{
				get{return eqpStateEngine.GetPrevState();}
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
				public virtual bool IsEqpStateNull(){
					return curEqpState == null;
				}
				public virtual bool WasEqpStateNull(){
					return prevEqpState == null;
				}				
			public virtual void Equip(){
				SetEqpState(equippedState);
			}
				ISBEqpState equippedState{
					get{return eqpStateRepo.GetEquippedState();}
				}
				public virtual bool IsEquipped(){
					return curEqpState == equippedState;
				}
				public virtual bool WasEquipped(){
					return prevEqpState == equippedState;
				}
			public virtual void Unequip(){
				SetEqpState(unequippedState);
			}
				ISBEqpState unequippedState{
					get{return eqpStateRepo.GetUnequippedState();}
				}
				public virtual bool IsUnequipped(){
					return curEqpState == unequippedState;
				}
				public virtual bool WasUnequipped(){
					return prevEqpState == unequippedState;
				}
		/*	Equip Process	*/
			public ISBEqpProcess GetEqpProcess(){
				return eqpProcEngine.GetProcess();
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
			public System.Func<IEnumeratorFake> GetEquipCoroutine(){
				return eqpCoroutineRepo.GetEquipCoroutine();
			}
			public System.Func<IEnumeratorFake> GetUnequipCoroutine(){
				return eqpCoroutineRepo.GetUnequipCoroutine();
			}
	}
	public interface ISBEqpStateHandler{
		/* Eqp States */
			void ClearCurEqpState();
			bool IsEqpStateNull();
			bool WasEqpStateNull();
			void Equip();
			bool IsEquipped();
			bool WasEquipped();
			void Unequip();
			bool IsUnequipped();
			bool WasUnequipped();
			ISBEqpProcess GetEqpProcess();
			void SetAndRunEqpProcess(ISBEqpProcess process);
			System.Func<IEnumeratorFake> GetUnequipCoroutine();
			System.Func<IEnumeratorFake> GetEquipCoroutine();
	}
}
