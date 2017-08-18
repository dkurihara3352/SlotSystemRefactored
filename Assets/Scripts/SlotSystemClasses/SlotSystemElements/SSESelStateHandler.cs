using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSESelStateHandler : ISSESelStateHandler {
		/*	state	*/
			public void ClearCurSelState(){
				SetSelState(null);
			}
				public bool IsSelStateNull(){
					return curSelState == null;
				}
				public bool WasSelStateNull(){
					return prevSelState == null;
				}
			public virtual void Deactivate(){
				SetSelState(deactivatedState);
			}
				ISSESelState deactivatedState{
					get{return selStateFactory.MakeDeactivatedState();}
				}
				public bool IsDeactivated(){
					return curSelState == deactivatedState;
				}
				public bool WasDeactivated(){
					return prevSelState == deactivatedState;
				}
			public virtual void Defocus(){
				SetSelState(defocusedState);
			}
				ISSESelState defocusedState{
					get{return selStateFactory.MakeDefocusedState();}
				}
				public bool IsDefocused(){
					return curSelState == defocusedState;
				}
				public bool WasDefocused(){
					return prevSelState == defocusedState;
				}
			public virtual void Focus(){
				SetSelState(focusedState);
			}
				ISSESelState focusedState{
					get{return selStateFactory.MakeFocusedState();}
				}
				public bool IsFocused(){
					return curSelState == focusedState;
				}
				public bool WasFocused(){
					return prevSelState == focusedState;
				}
			public virtual void Select(){
				SetSelState(selectedState);
			}
				ISSESelState selectedState{
					get{return selStateFactory.MakeSelectedState();}
				}
				public bool IsSelected(){
					return curSelState == selectedState;
				}
				public bool WasSelected(){
					return prevSelState == selectedState;
				}
			public virtual void Activate(){
				Focus();
			}
			public virtual void Deselect(){
				Focus();
			}
			ISSESelStateFactory selStateFactory{
				get{
					if(m_selStateFactory == null)
						m_selStateFactory = new SSESelStateFacotory(this);
					return m_selStateFactory;
				}
			}
				ISSESelStateFactory m_selStateFactory;
			ISSEStateEngine<ISSESelState> selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new SSEStateEngine<ISSESelState>();
					return m_selStateEngine;
				}
			}
				ISSEStateEngine<ISSESelState> m_selStateEngine;
			ISSESelState prevSelState{
				get{return selStateEngine.prevState;}
			}
			ISSESelState curSelState{
				get{return selStateEngine.curState;}
			}
			void SetSelState(ISSESelState state){
				selStateEngine.SetState(state);
				if(state == null && GetSelProcess() != null)
					SetAndRunSelProcess(null);
			}
		/*	process	*/
			public void SetAndRunSelProcess(ISSESelProcess process){
				selProcEngine.SetAndRunProcess(process);
			}
			public ISSESelProcess GetSelProcess(){
				return selProcEngine.GetProcess();
			}
			ISSEProcessEngine<ISSESelProcess> selProcEngine{
				get{
					if(m_selProcEngine == null)
						m_selProcEngine = new SSEProcessEngine<ISSESelProcess>();
					return m_selProcEngine;
				}
			}
				ISSEProcessEngine<ISSESelProcess> m_selProcEngine;
			public void SetSelProcEngine(ISSEProcessEngine<ISSESelProcess> engine){
				m_selProcEngine = engine;
			}
			/* Coroutines */
				public System.Func<IEnumeratorFake> GetDeactivateCoroutine(){
					return coroutineFactory.MakeDeactivateCoroutine();
				}
				public System.Func<IEnumeratorFake> GetFocusCoroutine(){
					return coroutineFactory.MakeFocusCoroutine();
				}
				public System.Func<IEnumeratorFake> GetDefocusCoroutine(){
					return coroutineFactory.MakeDefocusCoroutine();
				}
				public System.Func<IEnumeratorFake> GetSelectCoroutine(){
					return coroutineFactory.MakeSelectCoroutine();
				}
				ISSECoroutineFactory coroutineFactory{
					get{
						if(m_coroutineFactory == null)
							m_coroutineFactory = new SSECoroutineFactory();
						return m_coroutineFactory;
					}
				}
					ISSECoroutineFactory m_coroutineFactory;
					public void SetCoroutineFactory(ISSECoroutineFactory factory){m_coroutineFactory = factory;}

		/* Instant Methods */
			IInstantCommands instantCommands{
				get{
					if(m_instantCommands == null)
						m_instantCommands = new InstantCommands();
					return m_instantCommands;
				}
			}
				IInstantCommands m_instantCommands;
				public void SetInstantCommands(IInstantCommands comms){
					m_instantCommands = comms;
				}
			public virtual void InstantDefocus(){
				instantCommands.ExecuteInstantDefocus();
			}
			public virtual void InstantFocus(){
				instantCommands.ExecuteInstantFocus();
			}
			public virtual void InstantSelect(){
				instantCommands.ExecuteInstantSelect();
			}
	}
	public interface ISSESelStateHandler{
			bool IsSelStateNull();
			bool WasSelStateNull();
		void Deactivate();
			bool IsDeactivated();
			bool WasDeactivated();
		void Focus();
			bool IsFocused();
			bool WasFocused();
		void Defocus();
			bool IsDefocused();
			bool WasDefocused();
		void Select();
			bool IsSelected();
			bool WasSelected();
		void Activate();
		void Deselect();
		void SetAndRunSelProcess(ISSESelProcess process);
		ISSESelProcess GetSelProcess();
		System.Func<IEnumeratorFake> GetDeactivateCoroutine();
		System.Func<IEnumeratorFake> GetFocusCoroutine();
		System.Func<IEnumeratorFake> GetDefocusCoroutine();
		System.Func<IEnumeratorFake> GetSelectCoroutine();
		void InstantFocus();
		void InstantDefocus();
		void InstantSelect();
	}
}
