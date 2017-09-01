using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class SBSelStateHandler : ISBSelStateHandler{
		public SBSelStateHandler(){
			_selStateRepo = new SBSelStateRepo(this);
			_stateEngine = new UIStateEngine<ISBSelState>();
			_procEngine = new UIProcessEngine<ISBSelProcess>();
			_coroutineRepo = new SBSelCoroutineRepo();
		}

		
		ISBSelStateRepo SelStateRepo(){
			Debug.Assert(_selStateRepo != null);
			return _selStateRepo;
		}
			ISBSelStateRepo _selStateRepo;
		IUIStateEngine<ISBSelState> StateEngine(){
			Debug.Assert(_stateEngine != null);
			return _stateEngine;
		}
			IUIStateEngine<ISBSelState> _stateEngine;
		IUIProcessEngine<ISBSelProcess> ProcessEngine(){
			Debug.Assert(_procEngine != null);
			return _procEngine;
		}
			IUIProcessEngine<ISBSelProcess> _procEngine;
		ISBSelCoroutineRepo CoroutineRepo(){
			Debug.Assert(_coroutineRepo != null);
			return _coroutineRepo;
		}
			ISBSelCoroutineRepo _coroutineRepo;


		public void MakeSelectable(){
			StateEngine().SetState( SelStateRepo().SelectableState() );
		}
		bool IsSelectable(){
			return StateEngine().CurState() == SelStateRepo().SelectableState();
		}
		bool WasSelectable(){
			return StateEngine().PrevState() == SelStateRepo().SelectableState();
		}
		public void MakeUnselectable(){
			StateEngine().SetState( SelStateRepo().UnselectableState() );
		}
		public void Select(){
			StateEngine().SetState( SelStateRepo().SelectedState() );
		}
		public void SetAndRunSelProcess(ISBSelProcess proc){
			ProcessEngine().SetAndRunProcess(proc);
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
	}
	public interface ISBSelStateHandler{
		void MakeSelectable();
		void MakeUnselectable();
		void Select();
		void SetAndRunSelProcess(ISBSelProcess process);
		Func<IEnumeratorFake> MakeSelectableCoroutine();
		Func<IEnumeratorFake> MakeUnselectableCoroutine();
		Func<IEnumeratorFake> SelectCoroutine();
	}
}
