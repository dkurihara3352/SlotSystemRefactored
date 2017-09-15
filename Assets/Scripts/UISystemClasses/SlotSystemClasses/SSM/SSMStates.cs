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
			SetDeactivatedState(new UIDeactivatedState(engine));
			SetActivatedState(new SSMActivatedState(SSM(), engine));
			SetHiddenState(new UIHiddenState(engine));
			SetShownState(new UIShownState(engine));
			SetSelectedState(new UISelectedState(engine));
			SetDeselectedState(new UIDeselectedState(engine));
			SetSelectableState(new UISelectableState(engine));
			SetUnselectableState(new UIUnselectableState(engine));
		}
	}
	public class SSMActivatedState: UIActivatedState, IRelayState{
		ISlotSystemManager ssm;

		public SSMActivatedState(IUIElement element, IUISelStateEngine engine): base(element, engine){
			Debug.Assert((element is ISlotSystemManager));
			ssm = (ISlotSystemManager)element;
		}
		public override void Enter(){
			base.Enter();
			ssm.InitializeSlotSystemOnActivate();
		}
	}
}
