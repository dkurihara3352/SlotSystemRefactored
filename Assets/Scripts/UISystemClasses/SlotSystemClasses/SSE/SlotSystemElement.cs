using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISlotSystemElement: IUIElement{
		ISlotSystemManager SSM();
		void SetSSM(ISlotSystemManager ssm);
		void PerformHoverEnterAction();
		void PerformHoverExitAction();
		void HoverEnter();
		bool IsHovered();
	}
	public abstract class SlotSystemElement : UIElement, ISlotSystemElement{
		public ISlotSystemManager SSM(){
			return _ssm;
		}
		public void SetSSM(ISlotSystemManager ssm){
			_ssm = ssm;
		}
			ISlotSystemManager _ssm;
		public SlotSystemElement(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ITapCommand tapCommand): base(rectTrans, selStateRepo, tapCommand){
			ISlotSystemManager ssm = SSM();
		}
		public abstract void PerformHoverEnterAction();
		public abstract void PerformHoverExitAction();
		public void HoverEnter(){
			SSM().SetHoveredSSE(this);
		}
		public abstract bool IsHovered();
	}
}
