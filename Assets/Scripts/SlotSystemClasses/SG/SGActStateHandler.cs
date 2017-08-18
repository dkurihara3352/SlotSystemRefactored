using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGActStateHandler : ISGActStateHandler {
		/*	Engines	*/
			/*	Action State	*/
				public SGActStateHandler(ISlotGroup sg){
					SetActStateEngine(new SSEStateEngine<ISGActState>());
					SetStatesRepo(new SGStatesRepo(sg));
					SetActProcEngine(new SSEProcessEngine<ISGActProcess>());
				}
				ISSEStateEngine<ISGActState> actStateEngine{
					get{
						if(_actStateEngine != null)
							return _actStateEngine;
						else throw new InvalidOperationException("actStateEngine not set");
					}
				}
					ISSEStateEngine<ISGActState> _actStateEngine;
				void SetActStateEngine(ISSEStateEngine<ISGActState> engine){
					_actStateEngine = engine;
				}
				public void SetActState(ISGActState state){
					actStateEngine.SetState(state);
					if(state ==null && GetActProcess() != null)
						SetAndRunActProcess(null);
				}
				ISGActState curActState{
					get{return actStateEngine.GetCurState();}
				}
				ISGActState prevActState{
					get{return actStateEngine.GetPrevState();}
				}
				ISGStatesRepo statesRepo{
					get{
						if(_statesRepo != null)
							return _statesRepo;
						else throw new InvalidOperationException("stateRepo not set");
					}
				}
					ISGStatesRepo _statesRepo;
				public void SetStatesRepo(ISGStatesRepo repo){
					_statesRepo = repo;
				}
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
					SetActState(waitForActionState);
				}
					public ISGActState waitForActionState{
						get{return statesRepo.GetWaitForActionState();}
					}
					public bool IsWaitingForAction(){
						return curActState == waitForActionState;
					}
					public bool WasWaitingForAction(){
						return prevActState == waitForActionState;
					}
				public void Revert(){
					SetActState(revertState);
				}
					public ISGActState revertState{
						get{return statesRepo.GetRevertState();}
					}
					public bool IsReverting(){
						return curActState == revertState;
					}
					public bool WasReverting(){
						return prevActState == revertState;
					}
				public void Reorder(){
					SetActState(reorderState);
				}
					public ISGActState reorderState{
						get{return statesRepo.GetReorderState();}
					}
					public bool IsReordering(){
						return curActState == reorderState;
					}
					public bool WasReordering(){
						return prevActState == reorderState;
					}
				public void Add(){
					SetActState(addState);
				}
					public ISGActState addState{
						get{return statesRepo.GetAddState();}
					}
					public bool IsAdding(){
						return curActState == addState;
					}
					public bool WasAdding(){
						return prevActState == addState;
					}
				public void Remove(){
					SetActState(removeState);
				}
					public ISGActState removeState{
						get{return statesRepo.GetRemoveState();}
					}
					public bool IsRemoving(){
						return curActState == removeState;
					}
					public bool WasRemoving(){
						return prevActState == removeState;
					}
				public void Swap(){
					SetActState(swapState);
				}
					public ISGActState swapState{
						get{return statesRepo.GetSwapState();}
					}
					public bool IsSwapping(){
						return curActState == swapState;
					}
					public bool WasSwapping(){
						return prevActState == swapState;
					}
				public void Fill(){
					SetActState(fillState);
				}
					public ISGActState fillState{
						get{return statesRepo.GetFillState();}
					}
					public bool IsFilling(){
						return curActState == fillState;
					}
					public bool WasFilling(){
						return prevActState == fillState;
					}
				public void Sort(){
					SetActState(sortState);
				}
					public ISGActState sortState{
						get{return statesRepo.GetSortState();}
					}
					public bool IsSorting(){
						return curActState == sortState;
					}
					public bool WasSorting(){
						return prevActState == sortState;
					}
	/*	process	*/
		/*	Action Process	*/
			ISSEProcessEngine<ISGActProcess> actProcEngine{
				get{
					if(_actProcEngine != null)
						return _actProcEngine;
					else
						throw new InvalidOperationException("actProcEngine not set");
				}
			}
				ISSEProcessEngine<ISGActProcess> _actProcEngine;
			public void SetActProcEngine(ISSEProcessEngine<ISGActProcess> engine){
				_actProcEngine = engine;
			}
			public void SetAndRunActProcess(ISGActProcess process){
				actProcEngine.SetAndRunProcess(process);
			}
			public void ExpireActProcess(){
				ISGActProcess actProcess = GetActProcess();
				if(actProcess != null) 
					actProcess.Expire();
			}
			public ISGActProcess GetActProcess(){
				return actProcEngine.GetProcess();
			}
			public IEnumeratorFake TransactionCoroutine(){
				return null;
			}
	}
	public interface ISGActStateHandler{
		void ClearCurActState();
			bool WasActStateNull();
			bool IsActStateNull();
		void WaitForAction();
			bool IsWaitingForAction();
			bool WasWaitingForAction();
		void Revert();
			bool IsReverting();
			bool WasReverting();
		void Reorder();
			bool IsReordering();
			bool WasReordering();
		void Add();
			bool IsAdding();
			bool WasAdding();
		void Remove();
			bool IsRemoving();
			bool WasRemoving();
		void Swap();
			bool IsSwapping();
			bool WasSwapping();
		void Fill();
			bool IsFilling();
			bool WasFilling();
		void Sort();
			bool IsSorting();
			bool WasSorting();
	/* Proc */
		void SetAndRunActProcess(ISGActProcess process);
		void ExpireActProcess();
		ISGActProcess GetActProcess();
		IEnumeratorFake TransactionCoroutine();
	}
}
