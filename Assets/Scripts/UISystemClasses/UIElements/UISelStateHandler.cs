using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class UISelStateHandler : IUISelStateHandler {
		public UISelStateHandler(){
			_selStateRepo = new UISelStateRepo(this);
			_selStateEngine = new UIStateEngine<IUISelState>();
			_selProcEngine = new UIProcessEngine<IUISelProcess>();
			_coroutineRepo = new UISelCoroutineRepo();
		}
		/*	state	*/
			IUIStateEngine<IUISelState> SelStateEngine(){
				Debug.Assert(_selStateEngine != null);
				return _selStateEngine;
			}
				IUIStateEngine<IUISelState> _selStateEngine;
			IUISelStateRepo SelStateRepo(){
				Debug.Assert(_selStateRepo != null);
				return _selStateRepo;
			}
				IUISelStateRepo _selStateRepo;
			IUISelState prevSelState{
				get{return SelStateEngine().GetPrevState();}
			}
			IUISelState curSelState{
				get{return SelStateEngine().GetCurState();}
			}
			void SetSelState(IUISelState state){
				SelStateEngine().SetState(state);
				if(state == null && GetSelProcess() != null)
					SetAndRunSelProcess(null);
			}
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
					get{return SelStateRepo().DeactivatedState();}
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
					get{return SelStateRepo().UnselectableState();}
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
					get{return SelStateRepo().SelectableState();}
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
					get{return SelStateRepo().SelectedState();}
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
		/*	process	*/
			IUIProcessEngine<IUISelProcess> SelProcEngine(){
				Debug.Assert(_selProcEngine != null);
				return _selProcEngine;
			}
				IUIProcessEngine<IUISelProcess> _selProcEngine;
			public void SetSelProcEngine(IUIProcessEngine<IUISelProcess> engine){
				_selProcEngine = engine;
			}
			public void SetAndRunSelProcess(IUISelProcess process){
				SelProcEngine().SetAndRunProcess(process);
			}
			public IUISelProcess GetSelProcess(){
				return SelProcEngine().GetProcess();
			}
			/* Coroutines */
				IUISelCoroutineRepo CoroutineRepo(){
					Debug.Assert(_coroutineRepo != null);
					return _coroutineRepo;
				}
					IUISelCoroutineRepo _coroutineRepo;
				public void SetSelCoroutineRepo(IUISelCoroutineRepo repo){_coroutineRepo = repo;}
				public System.Func<IEnumeratorFake> DeactivateCoroutine(){
					return CoroutineRepo().DeactivateCoroutine();
				}
				public System.Func<IEnumeratorFake> MakeSelectableCoroutine(){
					return CoroutineRepo().MakeSelectableCoroutine();
				}
				public System.Func<IEnumeratorFake> MakeUnselectableCoroutine(){
					return CoroutineRepo().MakeUnselectableCoroutine();
				}
				public System.Func<IEnumeratorFake> SelectCoroutine(){
					return CoroutineRepo().SelectCoroutine();
				}

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
			public virtual void MakeUnselectableInstantly(){
				instantCommands.ExecuteInstantDefocus();
			}
			public virtual void MakeSelectableInstantly(){
				instantCommands.ExecuteInstantFocus();
			}
			public virtual void SelectInstantly(){
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
		void SetSelCoroutineRepo(IUISelCoroutineRepo repo);
		System.Func<IEnumeratorFake> DeactivateCoroutine();
		System.Func<IEnumeratorFake> MakeSelectableCoroutine();
		System.Func<IEnumeratorFake> MakeUnselectableCoroutine();
		System.Func<IEnumeratorFake> SelectCoroutine();
		void MakeSelectableInstantly();
		void MakeUnselectableInstantly();
		void SelectInstantly();
	}
}
