﻿using System.Collections;
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
        protected ISGTransactionHandler sgTAHandler;
        public SGActState(ISlotGroup sg): base(sg){
            handler = sg;
            sgTAHandler = sg;
        }
    }
    public interface ISGActState: ISSEState{
    }
    /* Factory */
    public class SGStatesRepo: ISGStatesRepo{
        ISGActStateHandler handler;
        ISlotGroup sg;
        public SGStatesRepo(ISlotGroup sg){
            this.handler = sg;
            this.sg = sg;
        }
        public ISGActState waitForActionState{
            get{
                if(_waitForActionState == null)
                    _waitForActionState = new SGWaitForActionState(sg);
                return _waitForActionState;
            }
        }
            ISGActState _waitForActionState;
        public ISGActState revertState{
            get{
                if(_revertState == null)
                    _revertState = new SGRevertState(sg);
                return _revertState;
            }
        }
            ISGActState _revertState;
        public ISGActState reorderState{
            get{
                if(_reorderState == null)
                    _reorderState = new SGReorderState(sg);
                return _reorderState;
            }
        }
            ISGActState _reorderState;
        public ISGActState sortState{
            get{
                if(_sortState == null)
                    _sortState = new SGSortState(sg);
                return _sortState;
            }
        }
            ISGActState _sortState;
        public ISGActState fillState{
            get{
                if(_fillState == null)
                    _fillState = new SGFillState(sg);
                return _fillState;
            }
        }
            ISGActState _fillState;
        public ISGActState swapState{
            get{
                if(_swapState == null)
                    _swapState = new SGSwapState(sg);
                return _swapState;
            }
        }
            ISGActState _swapState;
        public ISGActState addState{
            get{
                if(_addState == null)
                    _addState = new SGAddState(sg);
                return _addState;
            }
        }
            ISGActState _addState;
        public ISGActState removeState{
            get{
                if(_removeState == null)
                    _removeState = new SGRemoveState(sg);
                return _removeState;
            }
        }
            ISGActState _removeState;
    }
    public interface ISGStatesRepo{
        ISGActState waitForActionState{get;}
        ISGActState revertState{get;}
        ISGActState reorderState{get;}
        ISGActState sortState{get;}
        ISGActState fillState{get;}
        ISGActState swapState{get;}
        ISGActState addState{get;}
        ISGActState removeState{get;}
    }
    /* ConcreteStates */
    public class SGWaitForActionState: SGActState{
        public SGWaitForActionState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            handler.SetAndRunActProcess(null);
        }
    }
    public class SGRevertState: SGActState{
        public SGRevertState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            sg.RevertAndUpdateSBs();
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGReorderState: SGActState{
        public SGReorderState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.ReorderedNewSBs();
            sg.UpdateSBs(newSBs);
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSortState: SGActState{
        public SGSortState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.SortedNewSBs();
            sg.UpdateSBs(newSBs);
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGFillState: SGActState{
        public SGFillState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.FilledNewSBs();
            sg.UpdateSBs(newSBs);
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGSwapState: SGActState{
        public SGSwapState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.SwappedNewSBs();
            sg.UpdateSBs(newSBs);
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGAddState: SGActState{
        public SGAddState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.AddedNewSBs();
            sg.UpdateSBs(newSBs);
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
    public class SGRemoveState: SGActState{
        public SGRemoveState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            List<ISlottable> newSBs = sgTAHandler.RemovedNewSBs();
            sg.UpdateSBs(newSBs);
            if(handler.wasWaitingForAction){
                SGTransactionProcess process = new SGTransactionProcess(sg, handler.TransactionCoroutine);
                handler.SetAndRunActProcess(process);
            }
        }
    }
}
