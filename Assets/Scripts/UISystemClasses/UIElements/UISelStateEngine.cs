using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IUISelStateEngine{
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

		/* Relay States */
		void Activate();
			bool WasActivated();
			bool IsActivated();
		void Deselect();
			bool WasDeselected();
			bool IsDeselected();
		void Show();
			bool WasShown();
			bool IsShown();
		void SetAndRunSelProcess(IUISelProcess process);
		IUISelProcess SelProcess();
		void SetSelCoroutineRepo(IUISelCoroutineRepo repo);
		void ExpireProcess();
		IEnumeratorFake DeactivateCoroutine();
		IEnumeratorFake HideCoroutine();
		IEnumeratorFake MakeSelectableCoroutine();
		IEnumeratorFake MakeUnselectableCoroutine();
		IEnumeratorFake SelectCoroutine();
	}
	public class UISelStateEngine : IUISelStateEngine {
		public UISelStateEngine(IUIElement element, IUISelStateRepo selStateRepo){
			SetSelStateRepo(selStateRepo);
			SelStateRepo().InitializeFields(element);
			SelStateRepo().InitializeStates();
			SetSelCoroutineRepo(new UISelCoroutineRepo(element, this));
			_selStateSwitch = new UIStateSwitch<IUISelectionState>();
			_selProcSwitch = new UIProcessSwitch<IUISelProcess>();
		}
		/*	state	*/
			IUIStateSwitch<IUISelectionState> SelStateSwitch(){
				Debug.Assert(_selStateSwitch != null);
				return _selStateSwitch;
			}
				IUIStateSwitch<IUISelectionState> _selStateSwitch;
			IUISelStateRepo SelStateRepo(){
				Debug.Assert(_selStateRepo != null);
				return _selStateRepo;
			}
				IUISelStateRepo _selStateRepo;
			public void SetSelStateRepo(IUISelStateRepo repo){
				_selStateRepo = repo;
			}
			IUISelectionState prevSelState{
				get{return SelStateSwitch().PrevState();}
			}
			IUISelectionState curSelState{
				get{return SelStateSwitch().CurState();}
			}
			void SetSelState(IUISelectionState state){
				SelStateSwitch().SwitchTo(state);
				if(state == null && SelProcess() != null)
					SetAndRunSelProcess(null);
			}
			public void Deactivate(){
				SetSelState(deactivatedState);
			}
				IUISelectionState deactivatedState{
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
				IUISelectionState defocusedState{
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
				IUISelectionState focusedState{
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
				IUISelectionState selectedState{
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
				public bool IsActivated(){
					return IsHidden() || IsShown();
				}
			public void Deselect(){
				SetSelState(SelStateRepo().DeselectedState());
			}
				public bool WasDeselected(){
					return WasSelectable() || WasUnselectable();
				}
				public bool IsDeselected(){
					return IsSelectable() || IsUnselectable();
				}
			public void Show(){
				SetSelState(SelStateRepo().ShownState());
			}
				public bool WasShown(){
					return WasSelected() || WasDeselected();
				}
				public bool IsShown(){
					return IsSelected() || IsDeselected();
				}
		/*	process	*/
			IUIProcessSwitch<IUISelProcess> SelProcSwitch(){
				Debug.Assert(_selProcSwitch != null);
				return _selProcSwitch;
			}
				IUIProcessSwitch<IUISelProcess> _selProcSwitch;
			public void SetSelProcSwitch(IUIProcessSwitch<IUISelProcess> engine){
				_selProcSwitch = engine;
			}
			public void SetAndRunSelProcess(IUISelProcess process){
				SelProcSwitch().SetAndRunProcess(process);
			}
			public IUISelProcess SelProcess(){
				return SelProcSwitch().Process();
			}
			public void ExpireProcess(){
				IUISelProcess selProc = SelProcess();
				if(selProc != null)
					selProc.Expire();
			}
		/* Coroutines */
			IUISelCoroutineRepo CoroutineRepo(){
				Debug.Assert(_coroutineRepo != null);
				return _coroutineRepo;
			}
				IUISelCoroutineRepo _coroutineRepo;
			public void SetSelCoroutineRepo(IUISelCoroutineRepo repo){_coroutineRepo = repo;}
			public IEnumeratorFake DeactivateCoroutine(){
				return CoroutineRepo().DeactivateCoroutine();
			}
			public IEnumeratorFake HideCoroutine(){
				return CoroutineRepo().HideCoroutine();
			}
			public IEnumeratorFake MakeSelectableCoroutine(){
				return CoroutineRepo().MakeSelectableCoroutine();
			}
			public IEnumeratorFake MakeUnselectableCoroutine(){
				return CoroutineRepo().MakeUnselectableCoroutine();
			}
			public IEnumeratorFake SelectCoroutine(){
				return CoroutineRepo().SelectCoroutine();
			}
	}
}
