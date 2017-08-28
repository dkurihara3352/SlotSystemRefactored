using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UISystem{
	public abstract class SGState: UIState{
	}
    public abstract class SGActState: SGState, ISGActState{
        protected ISlotGroup sg;
        protected ISGActStateHandler actStateHandler;
        protected ISGTransactionHandler sgTAHandler;
        public SGActState(ISlotGroup sg){
            this.sg = sg;
            this.actStateHandler = sg.GetSGActStateHandler();
            sgTAHandler = sg.GetSGTAHandler();
        }
    }
    public interface ISGActState: IUIState{
    }
    /* Factory */
    public class SGStatesRepo: ISGStatesRepo{
        ISlotGroup sg;
        public SGStatesRepo(ISlotGroup sg){
            this.sg = sg;
        }
        public ISGActState GetWaitForActionState(){
            if(_waitForActionState == null)
                _waitForActionState = new SGWaitForActionState(sg);
            return _waitForActionState;
        }
            ISGActState _waitForActionState;
        public ISGActState GetRevertState(){
            if(_revertState == null)
                _revertState = new SGRevertState(sg);
            return _revertState;
        }
            ISGActState _revertState;
        public ISGActState GetReorderState(){
            if(_reorderState == null)
                _reorderState = new SGReorderState(sg);
            return _reorderState;
        }
            ISGActState _reorderState;
        public ISGActState GetSortState(){
            if(_sortState == null)
                _sortState = new SGSortState(sg);
            return _sortState;
        }
            ISGActState _sortState;
        public ISGActState GetFillState(){
            if(_fillState == null)
                _fillState = new SGFillState(sg);
            return _fillState;
        }
            ISGActState _fillState;
        public ISGActState GetSwapState(){
            if(_swapState == null)
                _swapState = new SGSwapState(sg);
            return _swapState;
        }
            ISGActState _swapState;
        public ISGActState GetAddState(){
            if(_addState == null)
                _addState = new SGAddState(sg);
            return _addState;
        }
            ISGActState _addState;
        public ISGActState GetRemoveState(){
            if(_removeState == null)
                _removeState = new SGRemoveState(sg);
            return _removeState;
        }
            ISGActState _removeState;
    }
    public interface ISGStatesRepo{
        ISGActState GetWaitForActionState();
        ISGActState GetRevertState();
        ISGActState GetReorderState();
        ISGActState GetSortState();
        ISGActState GetFillState();
        ISGActState GetSwapState();
        ISGActState GetAddState();
        ISGActState GetRemoveState();
    }
    /* ConcreteStates */
    public class SGWaitForActionState: SGActState{
        public SGWaitForActionState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            actStateHandler.SetAndRunActProcess(null);
        }
    }
    public class SGRevertState: SGActState{
        public SGRevertState(ISlotGroup sg): base(sg){
        }
        public override void EnterState(){
            sg.RevertAndUpdateSBs();
            if(actStateHandler.WasWaitingForAction()){
                SGTransactionProcess process = new SGTransactionProcess(sg, actStateHandler.TransactionCoroutine);
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGReorderState: SGActState{
        public SGReorderState(ISlotGroup sg): base(sg){
        }
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.ReorderedNewSBs();
            sg.ReadySBsForTransaction(newSBs);
            if(actStateHandler.WasWaitingForAction()){
                SGTransactionProcess process = new SGTransactionProcess(sg, actStateHandler.TransactionCoroutine);
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSortState: SGActState{
        public SGSortState(ISlotGroup sg): base(sg){
        }
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.SortedNewSBs();
            sg.ReadySBsForTransaction(newSBs);
            if(actStateHandler.WasWaitingForAction()){
                SGTransactionProcess process = new SGTransactionProcess(sg, actStateHandler.TransactionCoroutine);
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGFillState: SGActState{
        public SGFillState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.FilledNewSBs();
            sg.ReadySBsForTransaction(newSBs);
            if(actStateHandler.WasWaitingForAction()){
                SGTransactionProcess process = new SGTransactionProcess(sg, actStateHandler.TransactionCoroutine);
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSwapState: SGActState{
        public SGSwapState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.SwappedNewSBs();
            sg.ReadySBsForTransaction(newSBs);
            if(actStateHandler.WasWaitingForAction()){
                SGTransactionProcess process = new SGTransactionProcess(sg, actStateHandler.TransactionCoroutine);
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGAddState: SGActState{
        public SGAddState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.AddedNewSBs();
            sg.ReadySBsForTransaction(newSBs);
            if(actStateHandler.WasWaitingForAction()){
                SGTransactionProcess process = new SGTransactionProcess(sg, actStateHandler.TransactionCoroutine);
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGRemoveState: SGActState{
        public SGRemoveState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.RemovedNewSBs();
            sg.ReadySBsForTransaction(newSBs);
            if(actStateHandler.WasWaitingForAction()){
                SGTransactionProcess process = new SGTransactionProcess(sg, actStateHandler.TransactionCoroutine);
                actStateHandler.SetAndRunActProcess(process);
            }
        }
    }
}
