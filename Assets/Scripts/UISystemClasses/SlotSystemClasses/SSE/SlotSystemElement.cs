using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISlotSystemElement: IUIElement{
		ISlotSystemManager SSM();
		void SetSSM(ISlotSystemManager ssm);
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
		public void HoverEnter(){
			if( IsSelectable())
				SSM().SetHoveredSSE(this);
		}
		public bool IsHovered(){
			return SSM().HoveredSSE() == this;
		}
	}
}
