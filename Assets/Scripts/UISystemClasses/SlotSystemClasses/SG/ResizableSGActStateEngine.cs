using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IResizableSGActStateEngine{
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
	public class ResizableSGActStateEngine : IResizableSGActStateEngine {
		/*	Engines	*/
			/*	Action State	*/
				public ResizableSGActStateEngine(IResizableSG sg){
					SetActStateSwitch(new UIStateSwitch<ISGActState>());
					SetStatesRepo(new ResizableSGActStateRepo(sg));
					SetActProcSwitch(new UIProcessSwitch<ISGActProcess>());
				}
				IUIStateSwitch<ISGActState> ActStateSwitch(){
					Debug.Assert(_actStateSwitch != null);
					return _actStateSwitch;
				}
				void SetActStateSwitch(IUIStateSwitch<ISGActState> stateSwitch){
					_actStateSwitch = stateSwitch;
				}
					IUIStateSwitch<ISGActState> _actStateSwitch;
				public void SetActState(ISGActState state){
					ActStateSwitch().SwitchTo(state);
					if(state ==null && ActProcess() != null)
						SetAndRunActProcess(null);
				}
				ISGActState curActState{
					get{return ActStateSwitch().CurState();}
				}
				ISGActState prevActState{
					get{return ActStateSwitch().PrevState();}
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
			IUIProcessSwitch<ISGActProcess> ActProcSwitch(){
				Debug.Assert(_actProcSwitch != null);
				return _actProcSwitch;
			}
			public void SetActProcSwitch(IUIProcessSwitch<ISGActProcess> processSwitch){
				_actProcSwitch = processSwitch;
			}
				IUIProcessSwitch<ISGActProcess> _actProcSwitch;
			public void SetAndRunActProcess(ISGActProcess process){
				ActProcSwitch().SetAndRunProcess(process);
			}
			public void ExpireActProcess(){
				ISGActProcess actProcess = ActProcess();
				if(actProcess != null) 
					actProcess.Expire();
			}
			public ISGActProcess ActProcess(){
				return ActProcSwitch().Process();
			}
			public Func<IEnumeratorFake> ResizeCoroutine(){
				return null;
			}
	}
}
