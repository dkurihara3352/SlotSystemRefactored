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
					if(state ==null && actProcess != null)
						SetAndRunActProcess(null);
				}
				ISGActState curActState{
					get{return actStateEngine.curState;}
				}
				ISGActState prevActState{
					get{return actStateEngine.prevState;}
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
					public bool isActStateNull{
						get{return curActState == null;}
					}
					public bool wasActStateNull{
						get{return prevActState == null;}
					}
				public void WaitForAction(){
					SetActState(waitForActionState);
				}
					public ISGActState waitForActionState{
						get{return statesRepo.waitForActionState;}
					}
					public bool isWaitingForAction{
						get{return curActState == waitForActionState;}
					}
					public bool wasWaitingForAction{
						get{return prevActState == waitForActionState;}
					}
				public void Revert(){
					SetActState(revertState);
				}
					public ISGActState revertState{
						get{return statesRepo.revertState;}
					}
					public bool isReverting{
						get{return curActState == revertState;}
					}
					public bool wasReverting{
						get{return prevActState == revertState;}
					}
				public void Reorder(){
					SetActState(reorderState);
				}
					public ISGActState reorderState{
						get{return statesRepo.reorderState;}
					}
					public bool isReordering{
						get{return curActState == reorderState;}
					}
					public bool wasReordering{
						get{return prevActState == reorderState;}
					}
				public void Add(){
					SetActState(addState);
				}
					public ISGActState addState{
						get{return statesRepo.addState;}
					}
					public bool isAdding{
						get{return curActState == addState;}
					}
					public bool wasAdding{
						get{return prevActState == addState;}
					}
				public void Remove(){
					SetActState(removeState);
				}
					public ISGActState removeState{
						get{return statesRepo.removeState;}
					}
					public bool isRemoving{
						get{return curActState == removeState;}
					}
					public bool wasRemoving{
						get{return prevActState == removeState;}
					}
				public void Swap(){
					SetActState(swapState);
				}
					public ISGActState swapState{
						get{return statesRepo.swapState;}
					}
					public bool isSwapping{
						get{return curActState == swapState;}
					}
					public bool wasSwapping{
						get{return prevActState == swapState;}
					}
				public void Fill(){
					SetActState(fillState);
				}
					public ISGActState fillState{
						get{return statesRepo.fillState;}
					}
					public bool isFilling{
						get{return curActState == fillState;}
					}
					public bool wasFilling{
						get{return prevActState == fillState;}
					}
				public void Sort(){
					SetActState(sortState);
				}
					public ISGActState sortState{
						get{return statesRepo.sortState;}
					}
					public bool isSorting{
						get{return curActState == sortState;}
					}
					public bool wasSorting{
						get{return prevActState == sortState;}
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
				if(actProcess != null) 
					actProcess.Expire();
			}
			public ISGActProcess actProcess{
				get{return actProcEngine.process;}
			}
			public IEnumeratorFake TransactionCoroutine(){
				return null;
			}
	}
	public interface ISGActStateHandler{
		void ClearCurActState();
			bool wasActStateNull{get;}
			bool isActStateNull{get;}
		ISGActState waitForActionState{get;}
			void WaitForAction();
			bool isWaitingForAction{get;}
			bool wasWaitingForAction{get;}
		ISGActState revertState{get;}
			void Revert();
			bool isReverting{get;}
			bool wasReverting{get;}
		ISGActState reorderState{get;}
			void Reorder();
			bool isReordering{get;}
			bool wasReordering{get;}
		ISGActState addState{get;}
			void Add();
			bool isAdding{get;}
			bool wasAdding{get;}
		ISGActState removeState{get;}
			void Remove();
			bool isRemoving{get;}
			bool wasRemoving{get;}
		ISGActState swapState{get;}
			void Swap();
			bool isSwapping{get;}
			bool wasSwapping{get;}
		ISGActState fillState{get;}
			void Fill();
			bool isFilling{get;}
			bool wasFilling{get;}
		ISGActState sortState{get;}
			void Sort();
			bool isSorting{get;}
			bool wasSorting{get;}
	/* Proc */
		void SetAndRunActProcess(ISGActProcess process);
		void ExpireActProcess();
		ISGActProcess actProcess{get;}
		IEnumeratorFake TransactionCoroutine();
	}
}
