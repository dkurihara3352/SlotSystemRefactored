using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSESelStateHandler : ISSESelStateHandler {
		/*	state	*/
			public void ClearCurSelState(){
				SetSelState(null);
			}
				public bool isSelStateNull{
					get{return curSelState == null;}
				}
				public bool wasSelStateNull{
					get{return prevSelState == null;}
				}
			public virtual void Deactivate(){
				SetSelState(deactivatedState);
			}
				ISSESelState deactivatedState{
					get{return selStateFactory.MakeDeactivatedState();}
				}
				public bool isDeactivated{
					get{return curSelState == deactivatedState;}
				}
				public bool wasDeactivated{
					get{return prevSelState == deactivatedState;}
				}
			public virtual void Defocus(){
				SetSelState(defocusedState);
			}
				ISSESelState defocusedState{
					get{return selStateFactory.MakeDefocusedState();}
				}
				public bool isDefocused{
					get{return curSelState == defocusedState;}
				}
				public bool wasDefocused{
					get{return prevSelState == defocusedState;}
				}
			public virtual void Focus(){
				SetSelState(focusedState);
			}
				ISSESelState focusedState{
					get{return selStateFactory.MakeFocusedState();}
				}
				public bool isFocused{
					get{return curSelState == focusedState;}
				}
				public bool wasFocused{
					get{return prevSelState == focusedState;}
				}
			public virtual void Select(){
				SetSelState(selectedState);
			}
				ISSESelState selectedState{
					get{return selStateFactory.MakeSelectedState();}
				}
				public bool isSelected{
					get{return curSelState == selectedState;}
				}
				public bool wasSelected{
					get{return prevSelState == selectedState;}
				}
			public virtual void Activate(){
				Focus();
			}
			public virtual void Deselect(){
				Focus();
			}
			public virtual void InitializeStates(){
				Deactivate();
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
				if(state == null && selProcess != null)
					SetAndRunSelProcess(null);
			}
		/*	process	*/
			public void SetAndRunSelProcess(ISSESelProcess process){
				selProcEngine.SetAndRunProcess(process);
			}
			public ISSESelProcess selProcess{
				get{return selProcEngine.process;}
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
				public System.Func<IEnumeratorFake> deactivateCoroutine{
					get{return coroutineFactory.MakeDeactivateCoroutine();}
				}
				public System.Func<IEnumeratorFake> focusCoroutine{
					get{return coroutineFactory.MakeFocusCoroutine();}
				}
				public System.Func<IEnumeratorFake> defocusCoroutine{
					get{return coroutineFactory.MakeDefocusCoroutine();}
				}
				public System.Func<IEnumeratorFake> selectCoroutine{
					get{return coroutineFactory.MakeSelectCoroutine();}
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
		bool isSelStateNull{get;}
		bool wasSelStateNull{get;}
		void Deactivate();
			bool isDeactivated{get;}
			bool wasDeactivated{get;}
		void Focus();
			bool isFocused{get;}
			bool wasFocused{get;}
		void Defocus();
			bool isDefocused{get;}
			bool wasDefocused{get;}
		void Select();
			bool isSelected{get;}
			bool wasSelected{get;}
		void Activate();
		void Deselect();
		void InitializeStates();
		void SetAndRunSelProcess(ISSESelProcess process);
		System.Func<IEnumeratorFake> deactivateCoroutine{get;}
		System.Func<IEnumeratorFake> focusCoroutine{get;}
		System.Func<IEnumeratorFake> defocusCoroutine{get;}
		System.Func<IEnumeratorFake> selectCoroutine{get;}
		void InstantFocus();
		void InstantDefocus();
		void InstantSelect();
	}
}
