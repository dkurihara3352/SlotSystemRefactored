using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public class SSMSelStateRepo: UISelStateRepo{
		ISlotSystemManager SSM(){
			Debug.Assert((element is ISlotSystemManager));
			return element as ISlotSystemManager;
		}
		public override void InitializeStates(){
			SetDeactivatedState(new UIDeactivatedState(handler));
			SetActivatedState(new SSMActivatedState(SSM(), handler));
			SetHiddenState(new UIHiddenState(handler));
			SetShownState(new UIShownState(handler));
			SetSelectedState(new UISelectedState(handler));
			SetDeselectedState(new UIDeselectedState(handler));
			SetSelectableState(new UISelectableState(handler));
			SetUnselectableState(new UIUnselectableState(handler));
		}
	}
	public class SSMActivatedState: UIActivatedState, IRelayState{
		ISlotSystemManager ssm;

		public SSMActivatedState(IUIElement element, IUISelStateHandler handler): base(element, handler){
			Debug.Assert((element is ISlotSystemManager));
			ssm = (ISlotSystemManager)element;
		}
		public override void Enter(){
			base.Enter();
			ssm.InitializeSlotSystemOnActivate();
		}
	}
}
