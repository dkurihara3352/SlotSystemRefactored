using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public abstract class SGState: SSEState{
        protected ISlotGroup sg;
        public SGState(ISlotGroup sg){
            this.sg = sg;
        }
	}
    public abstract class SGActState: SGState, ISGActState{
        protected ISGActStateHandler handler;
        public SGActState(ISGActStateHandler handler, ISlotGroup sg): base(sg){
            this.handler = handler;
        }
    }
    public interface ISGActState: ISSEState{
    }
    /* Factory */
    public class SGStatesFactory: ISGStatesFactory{
        ISGActStateHandler handler;
        ISlotGroup sg;
        public SGStatesFactory(ISGActStateHandler handler, ISlotGroup sg){
            this.handler = handler;
        }
        public ISGActState MakeWaitForActionState(){
            if(_WaitForActionState == null)
                _WaitForActionState = new SGWaitForActionState(handler, sg);
            return _WaitForActionState;
        }
            ISGActState _WaitForActionState;
        public ISGActState MakeRevertState(){
            if(_RevertState == null)
                _RevertState = new SGRevertState(handler, sg);
            return _RevertState;
        }
            ISGActState _RevertState;
        public ISGActState MakeReorderState(){
            if(_ReorderState == null)
                _ReorderState = new SGReorderState(handler, sg);
            return _ReorderState;
        }
            ISGActState _ReorderState;
        public ISGActState MakeSortState(){
            if(_SortState == null)
                _SortState = new SGSortState(handler, sg);
            return _SortState;
        }
            ISGActState _SortState;
        public ISGActState MakeFillState(){
            if(_FillState == null)
                _FillState = new SGFillState(handler, sg);
            return _FillState;
        }
            ISGActState _FillState;
        public ISGActState MakeSwapState(){
            if(_SwapState == null)
                _SwapState = new SGSwapState(handler, sg);
            return _SwapState;
        }
            ISGActState _SwapState;
        public ISGActState MakeAddState(){
            if(_AddState == null)
                _AddState = new SGAddState(handler, sg);
            return _AddState;
        }
            ISGActState _AddState;
        public ISGActState MakeRemoveState(){
            if(_RemoveState == null)
                _RemoveState = new SGRemoveState(handler, sg);
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
        public SGWaitForActionState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            handler.SetAndRunActProcess(null);
        }
    }
    public class SGRevertState: SGActState{
        public SGRevertState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            sg.RevertAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGReorderState: SGActState{
        public SGReorderState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            sg.ReorderAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSortState: SGActState{
        public SGSortState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            sg.SortAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGFillState: SGActState{
        public SGFillState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            sg.FillAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSwapState: SGActState{
        public SGSwapState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            sg.SwapAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGAddState: SGActState{
        public SGAddState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            sg.AddAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGRemoveState: SGActState{
        public SGRemoveState(ISGActStateHandler handler, ISlotGroup sg): base(handler, sg){}
        public override void EnterState(){
            sg.RemoveAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
}
