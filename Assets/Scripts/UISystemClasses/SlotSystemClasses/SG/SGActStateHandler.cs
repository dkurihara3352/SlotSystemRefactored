using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class SGActStateHandler : IResizableSGActStateHandler {
		/*	Engines	*/
			/*	Action State	*/
				public SGActStateHandler(IResizableSG sg){
					SetActStateEngine(new UIStateEngine<ISGActState>());
					SetStatesRepo(new ResizableSGActStateRepo(sg));
					SetActProcEngine(new UIProcessEngine<ISGActProcess>());
				}
				IUIStateEngine<ISGActState> ActStateEngine(){
					Debug.Assert(_actStateEngine != null);
					return _actStateEngine;
				}
				void SetActStateEngine(IUIStateEngine<ISGActState> engine){
					_actStateEngine = engine;
				}
					IUIStateEngine<ISGActState> _actStateEngine;
				public void SetActState(ISGActState state){
					ActStateEngine().SetState(state);
					if(state ==null && ActProcess() != null)
						SetAndRunActProcess(null);
				}
				ISGActState curActState{
					get{return ActStateEngine().CurState();}
				}
				ISGActState prevActState{
					get{return ActStateEngine().PrevState();}
				}
				IResizableSGActStateRepo StatesRepo(){
					Debug.Assert(_statesRepo != null);
					return _statesRepo;
				}
				public void SetStatesRepo(IResizableSGActStateRepo repo){
					_statesRepo = repo;
				}
					IResizableSGActStateRepo _statesRepo;
				public void ClearCurActState(){
					SetActState(null);
				}
					public bool IsActStateNull(){
						return curActState == null;
					}
					public bool WasActStateNull(){
						return prevActState == null;
					}
				public void WaitForAction(){
					SetActState(waitingForActionState);
				}
					public ISGActState waitingForActionState{
						get{return StatesRepo().WaitingForActionState();}
					}
					public bool IsWaitingForResize(){
						return curActState == waitingForActionState;
					}
					public bool WasWaitingForAction(){
						return prevActState == waitingForActionState;
					}
				public void Resize(){
					SetActState(resizingState);
				}
					public ISGActState resizingState{
						get{return StatesRepo().ResizingState();}
					}
					public bool IsResizing(){
						return curActState == resizingState;
					}
					public bool WasResizing(){
						return prevActState == resizingState;
					}
	/*	process	*/
		/*	Action Process	*/
			IUIProcessEngine<ISGActProcess> ActProcEngine(){
				Debug.Assert(_actProcEngine != null);
				return _actProcEngine;
			}
			public void SetActProcEngine(IUIProcessEngine<ISGActProcess> engine){
				_actProcEngine = engine;
			}
				IUIProcessEngine<ISGActProcess> _actProcEngine;
			public void SetAndRunActProcess(ISGActProcess process){
				ActProcEngine().SetAndRunProcess(process);
			}
			public void ExpireActProcess(){
				ISGActProcess actProcess = ActProcess();
				if(actProcess != null) 
					actProcess.Expire();
			}
			public ISGActProcess ActProcess(){
				return ActProcEngine().GetProcess();
			}
			public Func<IEnumeratorFake> ResizeCoroutine(){
				return null;
			}
	}
	public interface IResizableSGActStateHandler{
		void ClearCurActState();
			bool WasActStateNull();
			bool IsActStateNull();
		void WaitForAction();
			bool IsWaitingForResize();
			bool WasWaitingForAction();
		void Resize();
			bool IsResizing();
			bool WasResizing();
	/* Proc */
		void SetAndRunActProcess(ISGActProcess process);
		void ExpireActProcess();
		ISGActProcess ActProcess();
		Func<IEnumeratorFake> ResizeCoroutine();
	}
}
