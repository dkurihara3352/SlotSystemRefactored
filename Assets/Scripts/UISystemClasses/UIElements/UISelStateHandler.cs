using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class UISelStateHandler : IUISelStateHandler {
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
				IUISelState deactivatedState{
					get{return selStateFactory.MakeDeactivatedState();}
				}
				public bool IsDeactivated(){
					return curSelState == deactivatedState;
				}
				public bool WasDeactivated(){
					return prevSelState == deactivatedState;
				}
			public virtual void MakeUnselectable(){
				SetSelState(defocusedState);
			}
				IUISelState defocusedState{
					get{return selStateFactory.MakeDefocusedState();}
				}
				public bool IsUnselectable(){
					return curSelState == defocusedState;
				}
				public bool WasDefocused(){
					return prevSelState == defocusedState;
				}
			public virtual void MakeSelectable(){
				SetSelState(focusedState);
			}
				IUISelState focusedState{
					get{return selStateFactory.MakeFocusedState();}
				}
				public bool IsSelectable(){
					return curSelState == focusedState;
				}
				public bool WasFocused(){
					return prevSelState == focusedState;
				}
			public virtual void Select(){
				SetSelState(selectedState);
			}
				IUISelState selectedState{
					get{return selStateFactory.MakeSelectedState();}
				}
				public bool IsSelected(){
					return curSelState == selectedState;
				}
				public bool WasSelected(){
					return prevSelState == selectedState;
				}
			public virtual void Activate(){
				MakeSelectable();
			}
			public virtual void Deselect(){
				MakeSelectable();
			}
			IUISelStateFactory selStateFactory{
				get{
					if(m_selStateFactory == null)
						m_selStateFactory = new UISelStateFacotory(this);
					return m_selStateFactory;
				}
			}
				IUISelStateFactory m_selStateFactory;
			IUIStateEngine<IUISelState> selStateEngine{
				get{
					if(m_selStateEngine == null)
						m_selStateEngine = new UIStateEngine<IUISelState>();
					return m_selStateEngine;
				}
			}
				IUIStateEngine<IUISelState> m_selStateEngine;
			IUISelState prevSelState{
				get{return selStateEngine.GetPrevState();}
			}
			IUISelState curSelState{
				get{return selStateEngine.GetCurState();}
			}
			void SetSelState(IUISelState state){
				selStateEngine.SetState(state);
				if(state == null && GetSelProcess() != null)
					SetAndRunSelProcess(null);
			}
		/*	process	*/
			public void SetAndRunSelProcess(IUISelProcess process){
				selProcEngine.SetAndRunProcess(process);
			}
			public IUISelProcess GetSelProcess(){
				return selProcEngine.GetProcess();
			}
			IUIProcessEngine<IUISelProcess> selProcEngine{
				get{
					if(m_selProcEngine == null)
						m_selProcEngine = new UIProcessEngine<IUISelProcess>();
					return m_selProcEngine;
				}
			}
				IUIProcessEngine<IUISelProcess> m_selProcEngine;
			public void SetSelProcEngine(IUIProcessEngine<IUISelProcess> engine){
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
				IUICoroutineFactory coroutineFactory{
					get{
						if(m_coroutineFactory == null)
							m_coroutineFactory = new UICoroutineFactory();
						return m_coroutineFactory;
					}
				}
					IUICoroutineFactory m_coroutineFactory;
					public void SetCoroutineFactory(IUICoroutineFactory factory){m_coroutineFactory = factory;}

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
	public interface IUISelStateHandler{
			bool IsSelStateNull();
			bool WasSelStateNull();
		void Deactivate();
			bool IsDeactivated();
			bool WasDeactivated();
		void MakeSelectable();
			bool IsSelectable();
			bool WasFocused();
		void MakeUnselectable();
			bool IsUnselectable();
			bool WasDefocused();
		void Select();
			bool IsSelected();
			bool WasSelected();
		void Activate();
		void Deselect();
		void SetAndRunSelProcess(IUISelProcess process);
		IUISelProcess GetSelProcess();
		System.Func<IEnumeratorFake> GetDeactivateCoroutine();
		System.Func<IEnumeratorFake> GetFocusCoroutine();
		System.Func<IEnumeratorFake> GetDefocusCoroutine();
		System.Func<IEnumeratorFake> GetSelectCoroutine();
		void InstantFocus();
		void InstantDefocus();
		void InstantSelect();
	}
}
