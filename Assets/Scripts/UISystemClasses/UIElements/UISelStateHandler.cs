using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class UISelStateHandler : IUISelStateHandler {
		public UISelStateHandler(IUISelCoroutineRepo selCoroutineRepo){
			SetSelStateRepo(new UISelStateRepo(this));
			SetSelCoroutineRepo(selCoroutineRepo);
			_selStateEngine = new UIStateEngine<IUISelState>();
			_selProcEngine = new UIProcessEngine<IUISelProcess>();
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
			public void SetSelStateRepo(IUISelStateRepo repo){
				_selStateRepo = repo;
			}
			IUISelState prevSelState{
				get{return SelStateEngine().GetPrevState();}
			}
			IUISelState curSelState{
				get{return SelStateEngine().GetCurState();}
			}
			void SetSelState(IUISelState state){
				SelStateEngine().SetState(state);
				if(state == null && SelProcess() != null)
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
			public void Deactivate(){
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
			public void Hide(){
				SetSelState(SelStateRepo().HiddenState());
			}
				public bool WasHidden(){
					return prevSelState == SelStateRepo().HiddenState();
				}
				public bool IsHidden(){
					return curSelState == SelStateRepo().HiddenState();
				}
			public void MakeUnselectable(){
				SetSelState(defocusedState);
			}
				IUISelState defocusedState{
					get{return SelStateRepo().UnselectableState();}
				}
				public bool IsUnselectable(){
					return curSelState == defocusedState;
				}
				public bool WasUnselectable(){
					return prevSelState == defocusedState;
				}
			public void MakeSelectable(){
				SetSelState(focusedState);
			}
				IUISelState focusedState{
					get{return SelStateRepo().SelectableState();}
				}
				public bool IsSelectable(){
					return curSelState == focusedState;
				}
				public bool WasSelectable(){
					return prevSelState == focusedState;
				}
			public void Select(){
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
			public void Activate(){
				SetSelState(SelStateRepo().ActivatedState());
			}
				public bool WasActivated(){
					return WasHidden() || WasShown();
				}
			public void Deselect(){
				SetSelState(SelStateRepo().DeselectedState());
			}
				public bool WasDeselected(){
					return WasSelectable() || WasUnselectable();
				}
			public void Show(){
				SetSelState(SelStateRepo().ShownState());
			}
				public bool WasShown(){
					return WasSelected() || WasDeselected();
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
			public IUISelProcess SelProcess(){
				return SelProcEngine().GetProcess();
			}
			void ExpireSelProc(){
				SelProcess().Expire();
			}
			/* Coroutines */
				IUISelCoroutineRepo CoroutineRepo(){
					Debug.Assert(_coroutineRepo != null);
					return _coroutineRepo;
				}
					IUISelCoroutineRepo _coroutineRepo;
				public void SetSelCoroutineRepo(IUISelCoroutineRepo repo){_coroutineRepo = repo;}
				public Func<IEnumeratorFake> DeactivateCoroutine(){
					return CoroutineRepo().DeactivateCoroutine();
				}
				public Func<IEnumeratorFake> HideCoroutine(){
					return CoroutineRepo().HideCoroutine();
				}
				public Func<IEnumeratorFake> MakeSelectableCoroutine(){
					return CoroutineRepo().MakeSelectableCoroutine();
				}
				public Func<IEnumeratorFake> MakeUnselectableCoroutine(){
					return CoroutineRepo().MakeUnselectableCoroutine();
				}
				public Func<IEnumeratorFake> SelectCoroutine(){
					return CoroutineRepo().SelectCoroutine();
				}

		/* Instant Methods */
			public virtual void MakeUnselectableInstantly(){
				MakeUnselectable();
				ExpireSelProc();
			}
			public virtual void MakeSelectableInstantly(){
				MakeSelectable();
				ExpireSelProc();
			}
			public virtual void SelectInstantly(){
				Select();
				ExpireSelProc();
			}
			public virtual void ShowInstantly(){
				MakeSelectableInstantly();
			}
	}
	public interface IUISelStateHandler{
			bool IsSelStateNull();
			bool WasSelStateNull();
		/* Process States */
		void Deactivate();
			bool IsDeactivated();
			bool WasDeactivated();
		void Hide();
			bool IsHidden();
			bool WasHidden();
		void MakeSelectable();
			bool IsSelectable();
			bool WasSelectable();
		void MakeUnselectable();
			bool IsUnselectable();
			bool WasUnselectable();
		void Select();
			bool IsSelected();
			bool WasSelected();

		/* SwitchStates */
		void Activate();
			bool WasActivated();
		void Deselect();
			bool WasDeselected();
		void Show();
			bool WasShown();
		void SetAndRunSelProcess(IUISelProcess process);
		IUISelProcess SelProcess();
		void SetSelCoroutineRepo(IUISelCoroutineRepo repo);
		System.Func<IEnumeratorFake> DeactivateCoroutine();
		System.Func<IEnumeratorFake> HideCoroutine();
		System.Func<IEnumeratorFake> MakeSelectableCoroutine();
		System.Func<IEnumeratorFake> MakeUnselectableCoroutine();
		System.Func<IEnumeratorFake> SelectCoroutine();
		void MakeSelectableInstantly();
		void MakeUnselectableInstantly();
		void SelectInstantly();
		void ShowInstantly();
	}
}
