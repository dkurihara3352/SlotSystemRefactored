using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SGState: SSEState{
        protected ISlotGroup sg;

        public SGState(ISlotGroup sg): base(sg){
            this.sg = sg;
        }
	}
    public abstract class SGActState: SGState, ISGActState{
        public SGActState(ISlotGroup sg): base(sg){}
    }
    public interface ISGActState: ISSEState{
    }
    /* Factory */
    public class SGStatesFactory: ISGStatesFactory{
        ISlotGroup sg;
        public SGStatesFactory(ISlotGroup sg){
            this.sg = sg;
        }
        public ISGActState MakeWaitForActionState(){
            if(_WaitForActionState == null)
                _WaitForActionState = new SGWaitForActionState(sg);
            return _WaitForActionState;
        }
            ISGActState _WaitForActionState;
        public ISGActState MakeRevertState(){
            if(_RevertState == null)
                _RevertState = new SGRevertState(sg);
            return _RevertState;
        }
            ISGActState _RevertState;
        public ISGActState MakeReorderState(){
            if(_ReorderState == null)
                _ReorderState = new SGReorderState(sg);
            return _ReorderState;
        }
            ISGActState _ReorderState;
        public ISGActState MakeSortState(){
            if(_SortState == null)
                _SortState = new SGSortState(sg);
            return _SortState;
        }
            ISGActState _SortState;
        public ISGActState MakeFillState(){
            if(_FillState == null)
                _FillState = new SGFillState(sg);
            return _FillState;
        }
            ISGActState _FillState;
        public ISGActState MakeSwapState(){
            if(_SwapState == null)
                _SwapState = new SGSwapState(sg);
            return _SwapState;
        }
            ISGActState _SwapState;
        public ISGActState MakeAddState(){
            if(_AddState == null)
                _AddState = new SGAddState(sg);
            return _AddState;
        }
            ISGActState _AddState;
        public ISGActState MakeRemoveState(){
            if(_RemoveState == null)
                _RemoveState = new SGRemoveState(sg);
            return _RemoveState;
        }
            ISGActState _RemoveState;
    }
    public interface ISGStatesFactory{
        ISGActState MakeWaitForActionState();
        ISGActState MakeRevertState();
        ISGActState MakeReorderState();
        ISGActState MakeSortState();
        ISGActState MakeFillState();
        ISGActState MakeSwapState();
        ISGActState MakeAddState();
        ISGActState MakeRemoveState();
    }
    /* ConcreteStates */
    public class SGWaitForActionState: SGActState{
        public SGWaitForActionState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.SetAndRunActProcess(null);
        }
    }
    public class SGRevertState: SGActState{
        public SGRevertState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.UpdateToRevert();
            if(sg.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                sg.SetAndRunActProcess(process);
            }
        }
    }
    public class SGReorderState: SGActState{
        public SGReorderState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.ReorderAndUpdateSBs();
            if(sg.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                sg.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSortState: SGActState{
        public SGSortState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.SortAndUpdateSBs();
            if(sg.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                sg.SetAndRunActProcess(process);
            }
        }
    }
    public class SGFillState: SGActState{
        public SGFillState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.FillAndUpdateSBs();
            if(sg.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                sg.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSwapState: SGActState{
        public SGSwapState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.SwapAndUpdateSBs();
            if(sg.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                sg.SetAndRunActProcess(process);
            }
        }
    }
    public class SGAddState: SGActState{
        public SGAddState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.AddAndUpdateSBs();
            if(sg.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                sg.SetAndRunActProcess(process);
            }
        }
    }
    public class SGRemoveState: SGActState{
        public SGRemoveState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.RemoveAndUpdateSBs();
            if(sg.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, sg.TransactionCoroutine);
                sg.SetAndRunActProcess(process);
            }
        }
    }
}
