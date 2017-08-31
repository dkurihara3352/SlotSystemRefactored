using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UISystem{
    public abstract class SGActState: UIState, ISGActState{
        protected ISlotGroup sg;
        protected IResizableSGActStateHandler actStateHandler;
        public SGActState(ISlotGroup sg){
            this.sg = sg;
            this.actStateHandler = sg.ActStateHandler();
        }
    }
    public interface ISGActState: IUIState{
    }
    /* Repo */
    public class SGActStateRepo: ISGActStateRepo{
        ISlotGroup sg;
        public SGActStateRepo(ISlotGroup sg){
            this.sg = sg;
        }
        public ISGActState WaitingForActionState(){
            if(_waitForActionState == null)
                _waitForActionState = new SGWaitingForActionState(sg);
            return _waitForActionState;
        }
            ISGActState _waitForActionState;
        public ISGActState ResizingState(){
            if(_resizingState == null)
                _resizingState = new SGResizingState(sg);
            return _resizingState;
        }
            ISGActState _resizingState;
    }
    public interface ISGActStateRepo{
        ISGActState WaitingForActionState();
        ISGActState ResizingState();
    }
    /* ConcreteStates */
    public class SGWaitingForActionState: SGActState{
        public SGWaitingForActionState(ISlotGroup sg): base(sg){}
        public override void EnterState(){
            actStateHandler.SetAndRunActProcess(null);
        }
    }
    public class SGResizingState: SGActState{
        public SGResizingState(ISlotGroup sg): base(sg){
        }
        public override void EnterState(){
            actStateHandler.SetAndRunActProcess(new SGResizeProcess(sg, actStateHandler.ResizeCoroutine()));
        }
    }
}
