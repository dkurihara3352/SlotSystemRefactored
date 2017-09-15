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
            SetDeactivatedState(new UIDeactivatedState(engine));
			SetActivatedState(new SGActivatedState(SG(), engine));
			SetHiddenState(new UIHiddenState(engine));
			SetShownState(new UIShownState(engine));
			SetSelectedState(new UISelectedState(engine));
			SetDeselectedState(new UIDeselectedState(engine));
			SetSelectableState(new UISelectableState(engine));
			SetUnselectableState(new UIUnselectableState(engine));
        }
    }
    public class SGActivatedState: UIActivatedState, IRelayState{
		ISlotGroup sg;

		public SGActivatedState(IUIElement element, IUISelStateEngine engine): base(element, engine){
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
        protected IResizableSGActStateEngine actStateHandler;
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
