using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UISystem{
    public class SGSelStateRepo: UISelStateRepo{
        ISlotGroup SG(){
            Debug.Assert((element is ISlotGroup));
            return element as ISlotGroup;
        }
        public override void InitializeStates(){
            SetDeactivatedState(new UIDeactivatedState(handler));
			SetActivatedState(new SGActivatedState(SG(), handler));
			SetHiddenState(new UIHiddenState(handler));
			SetShownState(new UIShownState(handler));
			SetSelectedState(new UISelectedState(handler));
			SetDeselectedState(new UIDeselectedState(handler));
			SetSelectableState(new UISelectableState(handler));
			SetUnselectableState(new UIUnselectableState(handler));
        }
    }
    public class SGActivatedState: UIActivatedState, IRelayState{
		ISlotGroup sg;

		public SGActivatedState(IUIElement element, IUISelStateHandler handler): base(element, handler){
			Debug.Assert((element is ISlotGroup));
			sg = (ISlotGroup)element;
		}
		public override void Enter(){
            base.Enter();
			sg.InitializeOnSlotSystemActivate();
		}
	}
    public abstract class ResizableSGActState: ISGActState{
        protected IResizableSG sg;
        protected IResizableSGActStateHandler actStateHandler;
        public ResizableSGActState(IResizableSG sg){
            this.sg = sg;
            this.actStateHandler = sg.ActStateHandler();
        }
        public virtual void Enter(){}
        public virtual void Exit(){}
        public virtual bool CanEnter(){return false;}
    }
    public interface ISGActState: IUIState{
    }
    /* Repo */
    public class ResizableSGActStateRepo: IResizableSGActStateRepo{
        IResizableSG sg;
        public ResizableSGActStateRepo(IResizableSG sg){
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
    public interface IResizableSGActStateRepo{
        ISGActState WaitingForActionState();
        ISGActState ResizingState();
    }
    /* ConcreteStates */
    public class SGWaitingForActionState: ResizableSGActState{
        public SGWaitingForActionState(IResizableSG sg): base(sg){}
        public override void Enter(){
            actStateHandler.SetAndRunActProcess(null);
        }
        public override void Exit(){}
        public override bool CanEnter(){
            if(sg.IsWaitingForResize())
                return false;
            else
                return true;
        }
    }
    public class SGResizingState: ResizableSGActState{
        public SGResizingState(IResizableSG sg): base(sg){
        }
        public override void Enter(){
            actStateHandler.SetAndRunActProcess(new SGResizeProcess(sg, actStateHandler.ResizeCoroutine()));
        }
        public override void Exit(){}
        public override bool CanEnter(){
            if(sg.IsResizing())
                return false;
            else
                return true;
        }
    }
}
