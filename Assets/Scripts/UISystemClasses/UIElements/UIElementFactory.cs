using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class UIElementFactory : IUIElementFactory {
		public IUIElement CreateUIElement(RectTransformFake rectTrans){
			IUIElement result = new UIElement(rectTrans, new UIDefaultSelStateRepo());
			result.SetIsShownOnActivation(true);
			return result;
		}
		public ISlotSystemManager CreateEquipToolSSM(RectTransformFake rectTrans){
			ISlotSystemManager result = new SlotSystemManager(rectTrans, new SSMSelStateRepo(), new EquipToolInventoryManager());
			result.SetIsShownOnActivation(true);
			return result;
		}
	}
	public interface IUIElementFactory{
		IUIElement CreateUIElement(RectTransformFake rectTrans);
		ISlotSystemManager CreateEquipToolSSM(RectTransformFake rectTrans);
	}
}
